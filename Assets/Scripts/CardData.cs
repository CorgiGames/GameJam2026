using UnityEngine;

// enums for card types
public enum CardType 
{ 
    Unit, 
    Buff, 
    Debuff,
    Special 
}

// ScriptableObject to hold card data
[CreateAssetMenu(fileName = "New Card", menuName = "Draft System/Card Data")]
public class CardData : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    public int cost;
    public Sprite cardIcon;
}