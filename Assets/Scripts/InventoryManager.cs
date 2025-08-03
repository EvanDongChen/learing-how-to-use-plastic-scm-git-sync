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
            if (note.noteName == newNote.noteName && note.level == newNote.level)
            {
                matchCount++;
            }
        }
        ownedNotes.Add(newNote);

        if (matchCount + 1 >= 3)
        {
            int removed = 0;
            for (int i = ownedNotes.Count - 1; i >= 0 && removed < 3; i--)
            {
                if (ownedNotes[i].noteName == newNote.noteName && ownedNotes[i].level == newNote.level)
                {
                    ownedNotes.RemoveAt(i);
                    removed++;
                }
            }

            NoteData upgradedNote = LoadUpgradedNote(newNote);
            if (upgradedNote != null)
            {
                ownedNotes.Add(upgradedNote);
                Debug.Log($"Note upgraded to: {upgradedNote.noteName} Level {upgradedNote.level}");
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

    public void ClearInventory()
    {
        ownedNotes.Clear();
        OnInventoryChanged?.Invoke();
    }
}
