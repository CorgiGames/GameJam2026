using UnityEngine;
using System.Collections.Generic;

public class DeckLoader : MonoBehaviour
{
    [Header("Deck Data Source")]
    public PlayerDeckData playerDeckData;

    public List<CardData> activeDeck = new List<CardData>();

    void Start()
    {
        LoadDeck();
    }

    public void LoadDeck()
    {
        if (playerDeckData != null)
        {
            activeDeck.Clear();
            activeDeck.AddRange(playerDeckData.savedDeck);
            Debug.Log("Loaded " + activeDeck.Count + " cards into the Tower Attack scene.");
        }
        else
        {
            Debug.LogError("PlayerDeckData reference is missing!");
        }
    }
}