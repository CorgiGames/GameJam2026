using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerCombatManager : MonoBehaviour
{
    [Header("Data Source")]
    public PlayerDeckData playerDeckData;

    [Header("UI References")]
    public Transform handPanel;
    public GameObject cardPrefab;

    [Header("Hand Settings")]
    public int maxHandSize = 4;

    [Header("Gameplay Mechanics")]
    public float cardPlayCooldown = 3f;
    private float lastCardPlayTime = -100f;

    // Temporary copy of the deck for single-use card logic
    private List<CardData> drawPile = new List<CardData>();

    void Start()
    {
        InitializeDeck();
        DrawInitialHand();
    }

    private void InitializeDeck()
    {
        if (playerDeckData != null && playerDeckData.savedDeck != null)
        {
            drawPile.AddRange(playerDeckData.savedDeck);
            Debug.Log("Loaded " + drawPile.Count + " cards to Tower Attack scene.");
        }
        else
        {
            Debug.LogError("PlayerDeckData reference is missing or empty!");
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

        Debug.Log("Played card: " + cardData.cardName);

        Destroy(cardObject);
        DrawCard();
    }
}