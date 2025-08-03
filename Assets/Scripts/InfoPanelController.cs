using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class InfoPanelController : MonoBehaviour
{
    public static InfoPanelController Instance { get; private set; }

    public TextMeshProUGUI nameAndLevelText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI noteDurationText;
    public Image noteIcon;
    public Transform attributesContainer;
    public GameObject attributeTagPrefab;

    public Canvas mainCanvas;

    private RectTransform panelRectTransform;

    Color GetColorForElement(NoteData.Elements element)
    {
        return element switch
        {
            NoteData.Elements.Fire => Color.red,
            NoteData.Elements.Water => Color.blue,
            NoteData.Elements.Lightining => Color.yellow,
            _ => Color.white
        };
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        panelRectTransform =  GetComponent<RectTransform>();
        HideInfo();
    }

    public void ShowInfo(NoteData data)
    {
        if (data == null) return;

        nameAndLevelText.text = $"{data.noteName} - Lvl {data.level}";
        noteIcon.sprite = data.icon;
        noteIcon.color = GetColorForElement(data.element);

        rarityText.text = new string('*', data.rarity);

        if (noteDurationText != null)
        {
            noteDurationText.text = $"Duration: {data.noteDuration} Beat(s)";
        }

        foreach (Transform child in attributesContainer) 
        {
            Destroy(child.gameObject);
        }

        if (attributeTagPrefab != null)
        {
            foreach (var attribute in data.attributes)
            {
                string attributeName = attribute.ToString();

                string resourcePath = "AttributeIcons/" + attributeName;

                Sprite attributeIcon = Resources.Load<Sprite>(resourcePath);

                GameObject tagGO = Instantiate(attributeTagPrefab, attributesContainer);
                Image tagImage = tagGO.GetComponent<Image>();

                if (tagImage != null && attributeIcon != null)
                {
                    tagImage.sprite = attributeIcon;
                }
                else
                {
                    Debug.LogError($"Could not load attribute icon from path: '{resourcePath}'");
                }
            }
        }

        //show the panel
        gameObject.SetActive(true);
    }

    public void HideInfo()
    {
        gameObject.SetActive(false);
    }

}
