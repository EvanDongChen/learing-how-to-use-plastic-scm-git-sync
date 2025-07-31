using UnityEngine;

public enum Rarity { Common, Rare, Epic, Legendary }

[System.Serializable]

public class ShopCard
{
    public string cardName;
    public Rarity rarity;
    public Sprite icon;

    public NoteData note;
}