using UnityEngine;
using System.Collections.Generic;

public class GuideManager : MonoBehaviour
{
    public GameObject attributeEntryPrefab;
    public Transform contentContainer;

    public class GuideInfo
    {
        public string title;
        public string description;
    }

    private List<GuideInfo> guideEntries = new List<GuideInfo>();

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        LoadGuideData();
        PopulateGuide();
    }

    private void LoadGuideData()
    {
        guideEntries.Add(new GuideInfo { title = "Reverb", description = "Creates an explosion on impact" });
        guideEntries.Add(new GuideInfo { title = "Harmonic", description = "Enhances the next note to deal double damage, stacks if next is harmonic" });
        guideEntries.Add(new GuideInfo { title = "Symponic", description = "Bonus damage per note of matching type" });
        guideEntries.Add(new GuideInfo { title = "Dissonance", description = "Traps enemies hit in place" });
        guideEntries.Add(new GuideInfo { title = "Syncopate", description = "Temporarily disrupts the enemy’s rhythm, slowing their actions in bursts" });
        guideEntries.Add(new GuideInfo { title = "Legato", description = "Increases damage by the number of legato notes in your music" });
        guideEntries.Add(new GuideInfo { title = "Forte", description = "This note deals double damage" });
        guideEntries.Add(new GuideInfo { title = "Staccato", description = "Shoots 2 notes instead of 1" });
        guideEntries.Add(new GuideInfo { title = "Trio", description = "Shoots 3 notes instead of 1" });
        guideEntries.Add(new GuideInfo { title = "Quartet", description = "Shoots 4 notes instead of 1" });
        guideEntries.Add(new GuideInfo { title = "Quintet", description = "Shoots 5 notes instead of 1" });
        guideEntries.Add(new GuideInfo { title = "Accelerando", description = "Speeds up the movement of the note, making it travel faster" });
        guideEntries.Add(new GuideInfo { title = "Melody", description = "Heals you for a portion of the damage dealt with this note" });
        guideEntries.Add(new GuideInfo { title = "Cadence", description = "Increases your movement speed temporarily after hitting an enemy" });
    }

    private void PopulateGuide()
    {
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var info in guideEntries)
        {
            GameObject entryGO = Instantiate(attributeEntryPrefab, contentContainer);
            AttributeEntryUI entryUI = entryGO.GetComponent<AttributeEntryUI>();

            entryUI.titleText.text = info.title;
            entryUI.descriptionText.text = info.description;
        }
    }

    public void ToggleGuide()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
