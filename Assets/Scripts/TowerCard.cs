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
        // Only process left clicks
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (combatManager != null && cardData != null)
            {
                combatManager.PlayCard(this.gameObject, cardData);
                if (cardData.cardType == CardType.Unit)
                {
                    if (cardData.characterPrefab != null)
                    {
                        // Trigger the spawner we created earlier
                        CharacterSpawner.main.SpawnCharacter(cardData.characterPrefab);
                    }
                    else
                    {
                        Debug.LogWarning($"Card {cardData.cardName} is a Unit but has no Prefab assigned!");
                    }
                }
            }
            else
            {
                Debug.LogError("TowerCard: Missing reference to CardData or TowerCombatManager.");
            }
        }
    }
   
}