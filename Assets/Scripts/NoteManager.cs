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

        Dictionary<Rarity, int> rarityLevels = new Dictionary<Rarity, int>
    {
        { Rarity.Common, 1 }, { Rarity.Rare, 2 }, { Rarity.Epic, 3 }, { Rarity.Legendary, 4 }
    };

        List<(Sprite sprite, float duration, string name)> spriteDurationList = new List<(Sprite, float, string)>
    {
        (WholeNote, 4f, "Whole Note"),
        (HalfNote, 2f, "Half Note"),
        (QuarterNote, 1f, "Quarter Note"),
        (EighthNote, 0.5f, "Eighth Note"),
        (Rest, 1f, "Rest"),
        (TwoSixteenthNote, 0.25f, "Sixteenth Note")
    };

        var elements = new List<NoteData.Elements> { NoteData.Elements.Fire, NoteData.Elements.Water, NoteData.Elements.Lightining };
        var allAttributes = new List<NoteData.AttributeType>((NoteData.AttributeType[])System.Enum.GetValues(typeof(NoteData.AttributeType)));
        System.Random rand = new System.Random();

        foreach (var rarity in rarityLevels.Keys)
        {
            for (int i = 0; i < 9; i++)
            {
                NoteData note = ScriptableObject.CreateInstance<NoteData>();
                note.rarity = (int)rarity + 1;
                note.level = rarityLevels[rarity];
                note.element = elements[i % elements.Count];

                var spriteDur = spriteDurationList[i % spriteDurationList.Count];
                note.icon = spriteDur.sprite;
                note.noteDuration = spriteDur.duration;

                note.noteName = note.element.ToString() + " " + spriteDur.name; // Assign the proper name from the list

                note.attributes = new List<NoteData.AttributeType>();
                if (rarity == Rarity.Common || rarity == Rarity.Rare)
                {
                    note.attributes.Add(allAttributes[rand.Next(allAttributes.Count)]);
                }
                else
                {
                    var shuffled = new List<NoteData.AttributeType>(allAttributes);
                    for (int j = 0; j < 3; j++)
                    {
                        int idx = rand.Next(shuffled.Count);
                        note.attributes.Add(shuffled[idx]);
                        shuffled.RemoveAt(idx);
                    }
                }

                noteData.Add(note);
            }
        }
    }
}