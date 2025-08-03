using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour
    
{
    public List<NoteData> noteData;

    //temp prefab
    public GameObject notePrefab;

    // Sprites for different note icons
    public Sprite WholeNote;
    public Sprite HalfNote;
    public Sprite Rest;
    public Sprite QuarterNote;
    public Sprite EighthNote;
    public Sprite TwoSixteenthNote;

    [SerializeField] private Canvas mainCanvas;

    private void Start()
    {
        ClearNotes();
        InitializeNotes();
    }

    public enum Rarity { Common, Rare, Epic, Legendary }
    
    public void ClearNotes()
    {
        if (noteData != null)
        {
            noteData.Clear();
        }
    }
    public void InitializeNotes()
    {
        noteData = new List<NoteData>();

        // Rarity settings
        Dictionary<Rarity, float> rarityDurations = new Dictionary<Rarity, float>
        {
            { Rarity.Common, 60f },
            { Rarity.Rare, 25f },
            { Rarity.Epic, 10f },
            { Rarity.Legendary, 5f }
        };

        // Sprites and durations
        List<(Sprite sprite, float duration)> spriteDurationList = new List<(Sprite, float)>
        {
            (WholeNote, 1f),
            (HalfNote, 0.5f),
            (QuarterNote, 1f),
            (EighthNote, 0.5f),
            (Rest,1f),
            (TwoSixteenthNote, 0.5f)
        };

        // All elements
        var elements = new List<NoteData.Elements> {
            NoteData.Elements.Fire,
            NoteData.Elements.Water,
            NoteData.Elements.Lightining
        };

        // All attributes
        var allAttributes = new List<NoteData.AttributeType>((NoteData.AttributeType[])System.Enum.GetValues(typeof(NoteData.AttributeType)));

        System.Random rand = new System.Random();

        foreach (var rarity in rarityDurations.Keys)
        {
            for (int i = 0; i < 9; i++)
            {
                NoteData note = ScriptableObject.CreateInstance<NoteData>();
                note.rarity = (int)rarity + 1;
                note.level = rarity == Rarity.Common ? 1 : 2;

                // Pick element in round-robin for balance
                note.element = elements[i % elements.Count];

                // Pick icon and duration
                var spriteDur = spriteDurationList[i % spriteDurationList.Count];
                note.icon = spriteDur.sprite;
                note.noteDuration = rarityDurations[rarity];
                // Override duration to match icon if needed
                note.noteDuration = spriteDur.duration;

                // Pick attributes
                note.attributes = new List<NoteData.AttributeType>();
                if (rarity == Rarity.Common || rarity == Rarity.Rare)
                {
                    // 1 random attribute
                    note.attributes.Add(allAttributes[rand.Next(allAttributes.Count)]);
                }
                else
                {
                    // 3 unique random attributes
                    var shuffled = new List<NoteData.AttributeType>(allAttributes);
                    for (int j = 0; j < 3; j++)
                    {
                        int idx = rand.Next(shuffled.Count);
                        note.attributes.Add(shuffled[idx]);
                        shuffled.RemoveAt(idx);
                    }
                }

                note.noteName = $"{rarity} Note {i + 1}";
                noteData.Add(note);
            }
        }
    }
}