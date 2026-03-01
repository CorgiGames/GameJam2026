using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerDeckData", menuName = "Card Game/Player Deck Data")]
public class PlayerDeckData : ScriptableObject
{
    public List<CardData> savedDeck = new List<CardData>();
    public int currentRound = 1;

    public void ClearDeck()
    {
        savedDeck.Clear();
    }

    public void ResetGameData()
    {
        savedDeck.Clear();
        currentRound = 1;
    }
}