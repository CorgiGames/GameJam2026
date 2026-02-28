using UnityEngine;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerClickHandler
{
    public CardData cardData;
    private DraftManager draftManager;

    void Start()
    {
        draftManager = FindObjectOfType<DraftManager>();
    }

    public void SetupCard(CardData data)
    {
        cardData = data;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Yalnızca sağ tık (Right-Click) algılandığında çalışır
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (cardData != null && draftManager != null)
            {
                draftManager.OpenCardDetailModal(cardData);
            }
        }
    }
}