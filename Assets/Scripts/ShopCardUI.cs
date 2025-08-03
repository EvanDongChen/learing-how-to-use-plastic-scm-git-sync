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
    public AttributeDatabase attributeDB;

    private ShopCard cardData;

    public void Setup(ShopCard card)
    {
        cardData = card;
        nameText.text = card.cardName;
        rarityText.text = new string('*', card.note.rarity);

        foreach (Transform child in attributesContainer)
        {
            Destroy(child.gameObject);
        }

        if (card.note != null)
        {
            // Set the main note icon
            iconImage.sprite = card.note.icon;
            iconImage.color = GetColorForElementNote(card.note.element);
            backgroundImage.color = GetColorForElement(card.note.element);

            if (attributeIconPrefab != null && attributeDB != null)
            {
                foreach (var attribute in card.note.attributes)
                {
                    string attributeName = attribute.ToString();
                    Sprite attributeIcon = attributeDB.GetIconByName(attributeName);

                    // Only create an icon if it was found in the database
                    if (attributeIcon != null)
                    {
                        GameObject iconGO = Instantiate(attributeIconPrefab, attributesContainer);
                        iconGO.GetComponent<AttributeIconUI>().iconImage.sprite = attributeIcon;
                    }
                }
            }
        }
        else
        {
            iconImage.enabled = false;
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
