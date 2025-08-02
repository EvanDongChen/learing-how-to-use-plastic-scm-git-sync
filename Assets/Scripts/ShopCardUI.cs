using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCardUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;

    public Image backgroundImage;

    private ShopCard cardData;

    public void Setup(ShopCard card)
    {
        cardData = card;
        nameText.text = card.cardName;
        iconImage.sprite = card.note.icon;
        iconImage.color = GetColorForElementNote(card.note.element);
        backgroundImage.color = GetColorForElement(card.note.element);
    }

    private Color GetColorForElement(NoteData.Elements element)
    {
        return element switch
        {
            NoteData.Elements.Fire => Color.red,
            NoteData.Elements.Water => Color.blue,
            NoteData.Elements.Lightining => Color.yellow,
            _ => Color.white
        };
    }
    private Color GetColorForElementNote(NoteData.Elements element)
    {
        return element switch
        {
            NoteData.Elements.Fire => Color.red,
            NoteData.Elements.Water => Color.blue,
            NoteData.Elements.Lightining => Color.yellow,
            _ => Color.white
        };
    }
    public ShopCard GetCardData()
    {
        return cardData;
    }
}
