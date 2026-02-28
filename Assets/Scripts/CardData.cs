using UnityEngine;

// enums for card types
public enum CardType 
{ 
    Unit, 
    Buff, 
    Sabotage,
    Special 
}

public enum SpecialEffect
{
    None,
    RandomCard
}


// ScriptableObject to hold card data
[CreateAssetMenu(fileName = "New Card", menuName = "Draft System/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    public SpecialEffect specialEffect;
    public int cost;
    public Sprite cardIcon;

    [Header("Unit Specific")]
    public GameObject characterPrefab;
}