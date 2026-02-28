using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DraftManager : MonoBehaviour
{
    [Header("Card Database")]
    public List<CardData> allCards;

    [Header("UI References")]
    public Image[] cardSlots;
    public TextMeshProUGUI coinText;
    public Button rerollButton;
    public TextMeshProUGUI rerollButtonText;

    [Header("Economy")]
    public int startingCoins = 100;
    public int currentRerollCost = 1;
    private int currentCoins;

    public List<CardData> currentShopCards = new List<CardData>();

    void Start()
    {
        currentCoins = startingCoins;
        UpdateUI();
        
        // Initialize first set of cards without charging coins
        RollCards(); 
    }

    void UpdateUI()
    {
        coinText.text = "Coins: " + currentCoins;
        rerollButtonText.text = "Reroll (" + currentRerollCost + ")";
        rerollButton.interactable = currentCoins >= currentRerollCost;
    }

    public void OnRerollButtonClicked()
    {
        if (currentCoins >= currentRerollCost)
        {
            currentCoins -= currentRerollCost;
            currentRerollCost++;
            
            UpdateUI();
            RollCards();
        }
    }

    public void RollCards()
    {
        if (allCards.Count == 0)
        {
            Debug.LogError("All Cards list is empty. Please assign CardData objects in the Inspector.");
            return;
        }

        currentShopCards.Clear();

        for (int i = 0; i < cardSlots.Length; i++)
        {
            CardData selectedCard = GetRandomCardWithWeights();
            currentShopCards.Add(selectedCard);
            cardSlots[i].sprite = selectedCard.cardIcon;
        }

        Debug.Log("Cards have been successfully rolled. Next reroll cost: " + currentRerollCost);
    }

    private CardData GetRandomCardWithWeights()
    {
        int roll = Random.Range(0, 100);
        CardType selectedType;

        if (roll < 40) 
        {
            selectedType = CardType.Unit;
        }
        else if (roll < 65) 
        {
            selectedType = CardType.Buff;
        }
        else if (roll < 90) 
        {
            selectedType = CardType.Debuff;
        }
        else 
        {
            selectedType = CardType.Special;
        }

        List<CardData> filteredCards = new List<CardData>();
        foreach (CardData card in allCards)
        {
            if (card.cardType == selectedType)
            {
                filteredCards.Add(card);
            }
        }

        // Fallback mechanism if no card of the selected type exists in the pool
        if (filteredCards.Count == 0)
        {
            Debug.LogWarning("No cards found for type: " + selectedType + ". Returning a random card from the general pool.");
            return allCards[Random.Range(0, allCards.Count)];
        }

        return filteredCards[Random.Range(0, filteredCards.Count)];
    }
}