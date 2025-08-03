using UnityEngine;
using System.Collections.Generic;
using System;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; set; }
    public List<NoteData> ownedNotes = new List<NoteData>();

    //tell ui to draw itself
    public static event Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddNote(NoteData newNote)
    {
        if (newNote.level >= 3)
        {
            ownedNotes.Add(newNote);
            OnInventoryChanged?.Invoke();
            return;
        }
        int matchCount = 0;

        foreach (var note in ownedNotes)
        {
            if (note.noteName == newNote.noteName && note.level == newNote.level && note.element == newNote.element)
            {
                matchCount++;
            }
        }
        ownedNotes.Add(newNote);

        if (matchCount + 1 >= 3)
        {
            int removed = 0;
            List<NoteData> removedNotes = new List<NoteData>();
            for (int i = ownedNotes.Count - 1; i >= 0 && removed < 3; i--)
            {
                if (ownedNotes[i].noteName == newNote.noteName && ownedNotes[i].level == newNote.level && ownedNotes[i].element == newNote.element)
                {
                    removedNotes.Add(ownedNotes[i]);
                    ownedNotes.RemoveAt(i);
                    removed++;
                }
            }

            NoteData upgradedNote = LoadUpgradedNote(newNote);
            if (upgradedNote != null)
            {
                // Collect attributes from removed notes
                HashSet<NoteData.AttributeType> attrsToAdd = new HashSet<NoteData.AttributeType>();
                foreach (var note in removedNotes)
                {
                    if (note.attributes != null)
                    {
                        foreach (var attr in note.attributes)
                        {
                            attrsToAdd.Add(attr);
                        }
                    }
                }
                // Add attributes to upgraded note if not already present
                if (upgradedNote.attributes == null)
                    upgradedNote.attributes = new List<NoteData.AttributeType>();
                foreach (var attr in attrsToAdd)
                {
                    if (!upgradedNote.attributes.Contains(attr))
                        upgradedNote.attributes.Add(attr);
                }
                ownedNotes.Add(upgradedNote);
                Debug.Log($"Note upgraded to: {upgradedNote.noteName} Level {upgradedNote.level} with attributes [{string.Join(", ", upgradedNote.attributes)}]");
            }
        }

        OnInventoryChanged?.Invoke();
    }

    public void RemoveNote(NoteData note)
    {
        if (ownedNotes.Remove(note))
        {
            OnInventoryChanged?.Invoke();
        }
    }

    private NoteData LoadUpgradedNote(NoteData baseNote)
    {
        int nextLevel = baseNote.level + 1;
        string path = $"NoteData/{baseNote.noteName}_Lv{nextLevel}";
        NoteData upgraded = Resources.Load<NoteData>(path);

        if (upgraded == null)
        {
            Debug.LogWarning($"Upgraded note not found at path: {path}");
        }

        return upgraded;
    }
}
