using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCardUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;
    public Image backgroundImage;
    public TMP_Text rarityText;
    public Transform attributesContainer;
    public GameObject attributeIconPrefab;

    private ShopCard cardData;

    public void Setup(ShopCard card)
    {
        cardData = card;

        nameText.text = card.cardName;

        if (card.note != null)
        {
            iconImage.sprite = card.note.icon;
            iconImage.color = GetColorForElementNote(card.note.element);
            backgroundImage.color = GetColorForElement(card.note.element);
        }

        rarityText.text = new string('*', card.note.rarity);
        
        foreach (Transform child in attributesContainer)
        {
            Destroy(child.gameObject);
        }

        if (attributeIconPrefab != null)
        {
            foreach (var attribute in card.note.attributes)
            {
                string attributeName = attribute.ToString();
                string resourcePath = "AttributeIcons/" + attributeName;
                Sprite attributeIcon = Resources.Load<Sprite>(resourcePath);
                
                if (attributeIcon != null)
                {
                    GameObject iconGO = Instantiate(attributeIconPrefab, attributesContainer);
                    iconGO.GetComponent<Image>().sprite = attributeIcon;
                }
            }
        }
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
