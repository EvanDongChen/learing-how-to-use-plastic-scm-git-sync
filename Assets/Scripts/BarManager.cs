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
            PlacedNote droppedNote = eventData.pointerDrag.GetComponent<PlacedNote>();
            if (droppedNote != null && canNoteFit(droppedNote.noteData))
            {
                AddNote(droppedNote);
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
    private void AddNote(PlacedNote newNote)
    {
        notesInBar.Add(newNote);
        int currentTicks = (int)notesInBar.Sum(note => note.noteData.noteDuration * scalingFactor);
        //set note's new parent to the bar
        newNote.transform.SetParent(this.transform, false);
        newNote.currentBar = this;
        Debug.Log($"Space in bar: {totalTicksInBar / scalingFactor}. Space left in bar: {(totalTicksInBar - currentTicks) / scalingFactor}");
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
