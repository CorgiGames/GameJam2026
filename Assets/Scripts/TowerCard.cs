using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerCard : MonoBehaviour, IPointerClickHandler
{
    private CardData cardData;
    private TowerCombatManager combatManager;

    [Header("UI References")]
    public Image cooldownOverlay;

    // Initializes the card with its data and manager reference
    public void SetupCard(CardData data, TowerCombatManager manager)
    {
        cardData = data;
        combatManager = manager;
    }

    void Update()
    {
        if (combatManager != null && cooldownOverlay != null)
        {
            float remainingTime = combatManager.GetRemainingCooldown();
            
            if (remainingTime > 0)
            {
                // Calculates the fill fraction (0 to 1)
                cooldownOverlay.fillAmount = remainingTime / combatManager.cardPlayCooldown;
            }
            else
            {
                cooldownOverlay.fillAmount = 0f;
            }
        }
    }

    // Detects clicks on the UI element
public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (combatManager != null && cardData != null)
            {
                if (!combatManager.CanPlayCard()) return;

                if (cardData.cardType == CardType.Unit)
                {
                    if (cardData.characterPrefab != null)
                    {
                        CharacterSpawner.main.SpawnCharacter(cardData.characterPrefab);
                        combatManager.PlayCard(this.gameObject, cardData);
                    }
                    else
                    {
                        Debug.LogWarning($"Card {cardData.cardName} is a Unit but has no Prefab assigned!");
                    }
                }
                else if (cardData.cardName == "Decoy")
                {
                    if (cardData.characterPrefab != null)
                    {
                        CharacterSpawner.main.SpawnDecoyOnRandomPathPoint(cardData.characterPrefab);
                        combatManager.PlayCard(this.gameObject, cardData);
                    }
                    else
                    {
                        Debug.LogError("Decoy card is missing a prefab!");
                    }
                }
                else if (cardData.cardType == CardType.Buff && cardData.cardName == "Range")
                {
                    Turret.OnRangeBuffReceived?.Invoke(1.5f);
                    combatManager.PlayCard(this.gameObject, cardData);
                }
                else
                {
                    combatManager.PlayCard(this.gameObject, cardData);
                }
            }
            else
            {
                Debug.LogError("TowerCard: Missing reference to CardData or TowerCombatManager.");
            }
        }
    }
   
}