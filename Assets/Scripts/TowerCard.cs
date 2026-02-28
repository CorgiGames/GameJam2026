using UnityEngine;
using UnityEngine.EventSystems;

public class TowerCard : MonoBehaviour, IPointerClickHandler
{
    private CardData cardData;
    private TowerCombatManager combatManager;

    // Initializes the card with its data and manager reference
    public void SetupCard(CardData data, TowerCombatManager manager)
    {
        cardData = data;
        combatManager = manager;
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
            }
            else
            {
                Debug.LogError("TowerCard: Missing reference to CardData or TowerCombatManager.");
            }
        }
    }
}