using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DraftManager : MonoBehaviour
{
    [Header("Card Database")]
    public List<CardData> allCards;

    [Header("Data & Scene Management")]
    public PlayerDeckData playerDeckData;
    public string nextSceneName = "Tower";

    [Header("UI References")]
    public Image[] cardSlots;
    public TextMeshProUGUI coinText;
    public Button rerollButton;
    public TextMeshProUGUI rerollButtonText;
    
    [Header("Deck UI - Bottom Panel")]
    public Transform deckPanel;
    public GameObject deckCardPrefab;
    public int maxVisibleCards = 6; 
    private int currentVisibleCards = 0;
    public Button viewAllButton;

    [Header("Deck UI - Full Modal")]
    public GameObject fullDeckModal;
    public Transform fullDeckContent;
    public Button closeModalButton;

    [Header("Card Detail Modal")]
    public GameObject cardDetailModal;
    public Image enlargedCardImage;

    [Header("Deck Stats UI")]
    public TextMeshProUGUI unitCountText;
    public TextMeshProUGUI buffCountText;
    public TextMeshProUGUI sabotageCountText;

    [Header("Economy")]
    public int startingCoins = 100;
    public int currentRerollCost = 1;
    private int currentCoins;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioClip buyCardSfx;
    public AudioClip rerollSfx;
    public AudioClip viewAllSfx;

    public List<CardData> currentShopCards = new List<CardData>();
    public List<CardData> playerDeck = new List<CardData>(); 

    void Start()
    {
        currentCoins = startingCoins;
        currentVisibleCards = 0;

        if (fullDeckModal != null) fullDeckModal.SetActive(false);
        if (viewAllButton != null) viewAllButton.gameObject.SetActive(false);
        if (cardDetailModal != null) cardDetailModal.SetActive(false);

        UpdateUI();
        UpdateDeckStatsUI(); 
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
            if (sfxSource != null && rerollSfx != null)
                {
                    sfxSource.clip = rerollSfx;
                    sfxSource.PlayDelayed(0.1f);
                }
            currentRerollCost++;
            
            UpdateUI();
            RollCards();
        }
    }

    public void BuyCard(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= currentShopCards.Count) return;

        CardData cardToBuy = currentShopCards[slotIndex];
        if (cardToBuy == null) return;

        if (currentCoins >= cardToBuy.cost)
        {
            currentCoins -= cardToBuy.cost;
            if (sfxSource != null && buyCardSfx != null)
            sfxSource.PlayOneShot(buyCardSfx);

            if (cardToBuy.cardType == CardType.Special)
            {
                Debug.Log("Special card purchased: " + cardToBuy.cardName + ". Instant effect triggered.");
            }
            else
            {
                playerDeck.Add(cardToBuy);
                
                if (currentVisibleCards < maxVisibleCards)
                {
                    GameObject newDeckCard = Instantiate(deckCardPrefab, deckPanel);
                    Image cardImage = newDeckCard.GetComponent<Image>();
                    if (cardImage != null) cardImage.sprite = cardToBuy.cardIcon;

                    CardDisplay display = newDeckCard.GetComponent<CardDisplay>();
                    if (display != null) display.SetupCard(cardToBuy);

                    currentVisibleCards++;

                    if (viewAllButton != null)
                    {
                        viewAllButton.gameObject.SetActive(true);
                        viewAllButton.transform.SetAsLastSibling();
                    }
                }

                UpdateDeckStatsUI(); 
            }
            
            UpdateUI();
            RollCards();
        }
        else
        {
            Debug.LogWarning("Insufficient coins.");
        }
    }

public void OpenFullDeckModal()
{
    if (sfxSource != null && viewAllSfx != null)
        sfxSource.PlayOneShot(viewAllSfx);

    if (fullDeckModal == null || fullDeckContent == null) return;

    fullDeckModal.SetActive(true);

    foreach (Transform child in fullDeckContent)
    {
        Destroy(child.gameObject);
    }

    foreach (CardData card in playerDeck)
    {
        GameObject newCard = Instantiate(deckCardPrefab, fullDeckContent);
        Image cardImage = newCard.GetComponent<Image>();
        if (cardImage != null) cardImage.sprite = card.cardIcon;

        CardDisplay display = newCard.GetComponent<CardDisplay>();
        if (display != null) display.SetupCard(card);
    }
}

    public void CloseFullDeckModal()
    {
        if (fullDeckModal != null) fullDeckModal.SetActive(false);
    }

    public void OpenCardDetailModal(CardData cardData)
    {
        if (cardDetailModal == null || enlargedCardImage == null) return;
        
        enlargedCardImage.sprite = cardData.cardIcon;
        cardDetailModal.SetActive(true);
    }

    public void CloseCardDetailModal()
    {
        if (cardDetailModal != null) cardDetailModal.SetActive(false);
    }

    private void UpdateDeckStatsUI()
    {
        int unitCount = 0;
        int buffCount = 0;
        int sabotageCount = 0;

        foreach (CardData card in playerDeck)
        {
            if (card.cardType == CardType.Unit) unitCount++;
            else if (card.cardType == CardType.Buff) buffCount++;
            else if (card.cardType == CardType.Sabotage) sabotageCount++;
        }

        if (unitCountText != null) unitCountText.text = "Unit: " + unitCount;
        if (buffCountText != null) buffCountText.text = "Buff: " + buffCount;
        if (sabotageCountText != null) sabotageCountText.text = "Sabotage: " + sabotageCount;
    }

    public void RollCards()
    {
        if (allCards.Count == 0) return;

        currentShopCards.Clear();

        for (int i = 0; i < cardSlots.Length; i++)
        {
            CardData selectedCard = GetRandomCardWithWeights();
            currentShopCards.Add(selectedCard);
            
            cardSlots[i].sprite = selectedCard.cardIcon;

            CardDisplay display = cardSlots[i].GetComponent<CardDisplay>();
            if (display != null) display.SetupCard(selectedCard);
        }

        UpdateUI();
        CheckDraftState();
    }

    private void CheckDraftState()
    {
        bool canAffordAnyCard = false;

        for (int i = 0; i < cardSlots.Length; i++)
        {
            if (currentShopCards[i] != null)
            {
                Button slotButton = cardSlots[i].GetComponent<Button>();

                if (currentCoins >= currentShopCards[i].cost)
                {
                    cardSlots[i].color = new Color(1, 1, 1, 1); 
                    if (slotButton != null) slotButton.interactable = true;
                    canAffordAnyCard = true;
                }
                else
                {
                    cardSlots[i].color = new Color(0.6f, 0.6f, 0.6f, 1f); 
                    if (slotButton != null) slotButton.interactable = false;
                }
            }
        }

        if (!canAffordAnyCard && currentCoins < currentRerollCost)
        {
            EndDraftPhase();
        }
    }

    private void EndDraftPhase()
    {
        Debug.Log("Draft Phase Complete. Saving deck and loading next scene.");

        if (playerDeckData != null)
        {
            // Clears data from the previous round
            playerDeckData.ClearDeck(); 
            // Saves the current deck
            playerDeckData.savedDeck.AddRange(playerDeck); 
        }
        else
        {
            Debug.LogError("PlayerDeckData is not assigned in the Inspector!");
            return;
        }

        // Loads the next scene
        SceneManager.LoadScene(nextSceneName);
    }

    private CardData GetRandomCardWithWeights()
    {
        int roll = Random.Range(0, 100);
        CardType selectedType;

        if (roll < 40) selectedType = CardType.Unit;
        else if (roll < 65) selectedType = CardType.Buff;
        else if (roll < 90) selectedType = CardType.Sabotage;
        else selectedType = CardType.Special;

        List<CardData> filteredCards = new List<CardData>();
        foreach (CardData card in allCards)
        {
            if (card.cardType == selectedType)
            {
                filteredCards.Add(card);
            }
        }

        if (filteredCards.Count == 0) return allCards[Random.Range(0, allCards.Count)];

        return filteredCards[Random.Range(0, filteredCards.Count)];
    }
}