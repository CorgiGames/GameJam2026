using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class TowerCombatManager : MonoBehaviour
{
    [Header("Data Source")]
    public PlayerDeckData playerDeckData;

    [Header("UI References")]
    public Transform handPanel;
    public GameObject cardPrefab;
    public TMPro.TextMeshProUGUI cardsLeftText;

    [Header("Scene Management")]
    public string lostSceneName = "LostScreen";

    [Header("Hand Settings")]
    public int maxHandSize = 4;

    [Header("Gameplay Mechanics")]
    public float cardPlayCooldown = 2f;
    private float lastCardPlayTime = -100f;
    private int totalPlayableCards;

    [Header("VFX")]
    public GameObject turretDestroyVfxPrefab;

    private List<CardData> drawPile = new List<CardData>();
    
    private bool isGameOver = false;

    void Start()
    {
        InitializeDeck();
        DrawInitialHand();
    }

    void Update()
    {
        if (!isGameOver)
        {
            CheckGameOverCondition();
        }
    }

    private void CheckGameOverCondition()
    {
        bool isDeckEmpty = drawPile.Count == 0;
        bool isHandEmpty = handPanel.childCount == 0;
        bool noActiveUnits = CharacterSpawner.main.activeUnitCount == 0;

        if (isDeckEmpty && isHandEmpty && noActiveUnits)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over! No playable cards and no active units remain. Loading LostScreen.");
        SceneManager.LoadScene(lostSceneName);
    }

    private System.Collections.IEnumerator FreezeTurrets(float seconds)
    {
        Turret.IsFrozen = true;
        yield return new WaitForSeconds(seconds);
        Turret.IsFrozen = false;
    }

    private void InitializeDeck()
    {
        if (playerDeckData != null && playerDeckData.savedDeck != null)
        {
            drawPile.AddRange(playerDeckData.savedDeck);
            totalPlayableCards = playerDeckData.savedDeck.Count;
            UpdateCardsLeftUI(); 
            Debug.Log("Loaded " + drawPile.Count + " cards to Tower Attack scene.");
        }
        else
        {
            Debug.LogError("PlayerDeckData reference is missing or empty!");
        }
    }

    private void UpdateCardsLeftUI()
    {
        if (cardsLeftText != null)
        {
            cardsLeftText.text = "Cards Left: " + totalPlayableCards;
        }
    }

    private void DrawInitialHand()
    {
        for (int i = 0; i < maxHandSize; i++)
        {
            DrawCard();
        }
    }

    public void DrawCard()
    {
        if (drawPile.Count == 0)
        {
            Debug.Log("The deck is empty! No more cards to draw.");
            return;
        }

        int randomIndex = Random.Range(0, drawPile.Count);
        CardData drawnCard = drawPile[randomIndex];
        drawPile.RemoveAt(randomIndex);

        GameObject newCardUI = Instantiate(cardPrefab, handPanel);
        Image cardImage = newCardUI.GetComponent<Image>();
        
        if (cardImage != null)
        {
            cardImage.sprite = drawnCard.cardIcon;
        }

        TowerCard towerCard = newCardUI.GetComponent<TowerCard>();
        if (towerCard == null)
        {
            towerCard = newCardUI.AddComponent<TowerCard>();
        }
        
        towerCard.SetupCard(drawnCard, this);
        Debug.Log("Drawn card: " + drawnCard.cardName + ". Remaining cards: " + drawPile.Count);
    }

    private void DestroyOneTurret()
    {
        Turret[] turrets = FindObjectsByType<Turret>(FindObjectsSortMode.None);

        if (turrets.Length == 0)
        {
            Debug.LogWarning("No turrets found to destroy (Turret component not found in scene).");
            return;
        }

        Turret chosen = turrets[Random.Range(0, turrets.Length)];
        Vector3 pos = chosen.transform.position;

        if (turretDestroyVfxPrefab != null)
        {
            GameObject vfx = Instantiate(turretDestroyVfxPrefab, pos, Quaternion.identity);
            Destroy(vfx, 2f);
        }

        Destroy(chosen.gameObject);
        Debug.Log("Destroyed 1 turret!");
    }

    public bool CanPlayCard()
    {
        return Time.time >= lastCardPlayTime + cardPlayCooldown;
    }

    public void PlayCard(GameObject cardObject, CardData cardData)
    {
        if (!CanPlayCard())
        {
            Debug.LogWarning("Cooldown is active. Cannot play card yet.");
            return;
        }

        lastCardPlayTime = Time.time;
        totalPlayableCards--;
        UpdateCardsLeftUI(); 

        Debug.Log("Played card: " + cardData.cardName);

        if (cardData.cardName == "Heal")  
        {
            Health[] allHealth = FindObjectsOfType<Health>();

            foreach (var h in allHealth)
                h.FullHeal();

            Debug.Log("Heal card effect applied: all alive characters healed to full.");
        }

        if (cardData.cardName == "Freeze") 
        {
            StartCoroutine(FreezeTurrets(5f));
            Debug.Log("Freeze card played: Turrets frozen for 5 seconds.");
        }

        if (cardData.cardName == "Destruction")
        {
            DestroyOneTurret();
            Debug.Log("Destruction card played: destroyed one turret.");
        }

        if (cardData.cardName == "Speed")  
        {
            CharacterMovement[] allCharacters = FindObjectsOfType<CharacterMovement>();

            foreach (var ch in allCharacters)
            {
                ch.AddMoveSpeed(1f); 
            }

            Debug.Log("Speed card effect applied: all characters speed +1");
        }

        Destroy(cardObject);
        DrawCard();
    }

    public float GetRemainingCooldown()
    {
        float timeSinceLastPlay = Time.time - lastCardPlayTime;
        float remaining = cardPlayCooldown - timeSinceLastPlay;
        return Mathf.Max(0, remaining);
    }
}