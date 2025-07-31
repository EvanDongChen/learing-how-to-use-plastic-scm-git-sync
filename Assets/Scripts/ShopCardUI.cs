using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCardUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;

    private ShopCard cardData;

    public void Setup(ShopCard card)
    {
        cardData = card;
        iconImage.sprite = card.icon;
        nameText.text = card.cardName;
    }

    public ShopCard GetCardData()
    {
        return cardData;
    }
}
