using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

public class BarManager : MonoBehaviour, IDropHandler
{
    //Handles logic for adding/arranging notes

    //to make all the note duration values whole numbers
    public int scalingFactor = 2;
    public int totalTicksInBar = 4;
    public GameObject noteOnBarPrefab;

    private List<PlacedNote> notesInBar = new List<PlacedNote>();
    private RectTransform barRect;

    public float fixedNoteWidth = 30f;

    private void Awake()
    {
        barRect = GetComponent<RectTransform>();
        totalTicksInBar *= scalingFactor;
    }

    public List<PlacedNote> GetPlacedNotes()
    {
        return notesInBar;
    }

    //notes being dropped on the bar
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DraggableNote draggedNote = eventData.pointerDrag.GetComponent<DraggableNote>();
            if (draggedNote != null && canNoteFit(draggedNote.noteData))
            {
                AddNote(draggedNote.noteData);

                InventoryManager.Instance.RemoveNote(draggedNote.noteData);

                Destroy(draggedNote.gameObject);
            }
        }
    }

    //check space in bar
    private bool canNoteFit(NoteData noteData)
    {
        //check if bar is full
        int currentTicks = (int)notesInBar.Sum(note => note.noteData.noteDuration * scalingFactor);
        return currentTicks + noteData.noteDuration * scalingFactor <= totalTicksInBar;
    }

    //Add note to list and redraw everything
    private void AddNote(NoteData newNoteData)
    {
        GameObject noteOnBarGO = Instantiate(noteOnBarPrefab, this.transform);
        noteOnBarGO.name = newNoteData.noteName;

        // Create and set up a PlacedNote component to hold the data on the bar
        PlacedNote placedNote = noteOnBarGO.GetComponent<PlacedNote>();
        placedNote.noteData = newNoteData;
        placedNote.currentBar = this;

        notesInBar.Add(placedNote);

        // Redraw all notes on the bar
        redrawNotes();
    }

    public void RemoveNote(PlacedNote noteToRemove)
    {
        notesInBar.Remove(noteToRemove);
        redrawNotes();
    }

    //arrange the notes visually
    private void redrawNotes()
    {
        float barWidth = barRect.rect.width;
        float runningTicks = 0;

        foreach (PlacedNote note in notesInBar)
        {
            RectTransform noteRect = note.GetComponent<RectTransform>();
            float noteDurationTicks = note.noteData.noteDuration * scalingFactor;

            float slotCenterInTicks = runningTicks + (noteDurationTicks / 2f);
            float slotCenterPercentage = slotCenterInTicks / totalTicksInBar;
            float noteCenterX = (slotCenterPercentage - barRect.pivot.x) * barWidth;

            noteRect.anchoredPosition = new Vector2(noteCenterX, 0);
            noteRect.sizeDelta = new Vector2(fixedNoteWidth, noteRect.sizeDelta.y);

            note.localXPos = noteCenterX;

            runningTicks += noteDurationTicks;
        }
    }

    //get the sequence for the musicsheetmanager and projectiles
    public List<NoteData> GetNoteSequence()
    {
        return notesInBar.Select(note => note.noteData).ToList();
    }

    public NoteData GetNoteAtTick(int targetTick)
    {
        int currentTick = 0;
        foreach (PlacedNote note in notesInBar)
        {
            int noteDuration = (int)(note.noteData.noteDuration * scalingFactor);
            //check if targettick is at the start or end of this note
            if (targetTick >= currentTick && targetTick < currentTick + noteDuration)
            {
                return note.noteData;
            }
            currentTick += noteDuration;
        }
        return null;
    }

    public int GetLastNoteEndTick()
    {
        if (notesInBar.Count == 0)
        {
            return 0;
        }

        int lastTick = (int)notesInBar.Sum(note => note.noteData.noteDuration * scalingFactor);
        Debug.Log($"Bar '{name}' is reporting its last note ends at tick: {lastTick}");

        return lastTick;
    }

}
