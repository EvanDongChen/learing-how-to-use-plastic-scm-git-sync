using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

public class BarManager : MonoBehaviour, IDropHandler
{
    //Handles logic for adding/arranging notes

    //to make all the note duration values whole numbers
    private int scalingFactor = 2;
    private int totalTicksInBar = 4;

    private List<PlacedNote> notesInBar = new List<PlacedNote>();
    private RectTransform barRect;

    public float fixedNoteWidth = 30f;

    private void Awake()
    {
        barRect = GetComponent<RectTransform>();
        totalTicksInBar *= scalingFactor;
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
        //metrics
        float barWidth = barRect.rect.width;
        float currentX = 0;

        foreach (PlacedNote note in notesInBar)
        {
            //calculate how much space there is for each of the notes + the scaling
            RectTransform noteRect = note.GetComponent<RectTransform>();
            float timeSlotWidthPercentage = (float)note.noteData.noteDuration * scalingFactor / totalTicksInBar;
            float timeSlotWidth = barWidth * timeSlotWidthPercentage;

            //set the anchor and pivot
            noteRect.anchorMin = new Vector2(0, 0.5f);
            noteRect.anchorMax = new Vector2(0, 0.5f);
            noteRect.pivot = new Vector2(0, 0.5f);

            noteRect.anchoredPosition = new Vector2(currentX, 0);
            noteRect.sizeDelta =  new Vector2(fixedNoteWidth, noteRect.sizeDelta.y);

            currentX += timeSlotWidth;
        }
    }

    //get the sequence for the musicsheetmanager and projectiles
    public List<NoteData> GetNoteSequence()
    {
        return notesInBar.Select(note => note.noteData).ToList();
    }

}
