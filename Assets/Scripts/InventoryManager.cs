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

    public void AddNote(NoteData note)
    {
        ownedNotes.Add(note);
        OnInventoryChanged?.Invoke();
    }

    public void RemoveNote(NoteData note)
    {
        if (ownedNotes.Remove(note))
        {
            OnInventoryChanged?.Invoke();
        }
    }
}
