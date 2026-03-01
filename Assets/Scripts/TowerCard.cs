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

    private System.Collections.IEnumerator ApplyTemporaryRangeBuff(float buffedRange, float normalRange, float duration)
    {
        // 1. Set the range to the reduced value
        Turret.OnRangeBuffReceived?.Invoke(buffedRange);
        Debug.Log("Range Reduced!");

        // 2. Wait for the duration (e.g., 5 seconds)
        yield return new WaitForSeconds(duration);

        // 3. Set it back to normal
        Turret.OnRangeBuffReceived?.Invoke(normalRange);
        Debug.Log("Range Restored to normal.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (combatManager != null && cardData != null)
            {
                // BUG FIX: Check cooldown before starting the process.
                // If the card cannot be played, cut the method completely here.
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
                else if (cardData.cardName == "Protection")
                {
                    CharacterSpawner.main.ActivateInvincibility(5f);
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