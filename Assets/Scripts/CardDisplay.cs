using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour, IPointerClickHandler
{
    [Header("Card Data")]
    public CardData cardData;
    private DraftManager draftManager;

    [Header("UI References")]
    public Image cardImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;

    void Start()
    {
        draftManager = FindObjectOfType<DraftManager>();
        
        if (draftManager == null)
        {
            Debug.LogError("CardDisplay: DraftManager not found in the scene.");
        }
    }

    public void SetupCard(CardData data)
    {
        if (data == null) return;

        cardData = data;

        if (cardImage != null && cardData.cardIcon != null)
        {
            cardImage.sprite = cardData.cardIcon;
        }

        if (nameText != null)
        {
            nameText.text = cardData.cardName;
        }

        if (costText != null)
        {
            costText.text = cardData.cost.ToString() + " <voffset=0.8em><sprite=0></voffset>";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (cardData != null && draftManager != null)
            {
                draftManager.OpenCardDetailModal(cardData);
            }
        }
    }
}