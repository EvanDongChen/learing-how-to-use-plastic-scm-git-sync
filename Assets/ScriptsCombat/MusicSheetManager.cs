using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MusicSheetManager : MonoBehaviour
{
    public List<MusicSlot> musicSlots = new List<MusicSlot>();
    public Button playPauseButton;

    private Color activeColor = Color.green;
    private Color restColor = Color.red;
    private Color defaultColor = Color.white;

    public float beatsPerMin = 120f;

    private bool isPlaying = false;
    private int currentBeatIndex = 0;
    private float beatTimer = 0f;

    private NoteData[] noteSequence;

    private void Start()
    {
        // --- Music Slots Initialization ---
        if (musicSlots.Count == 0)
        {
            musicSlots.AddRange(GetComponentsInChildren<MusicSlot>());
            musicSlots = musicSlots.OrderBy(s => s.transform.position.x).ToList();
        }

        // Assign indices and check for null slots
        for (int i = 0; i < musicSlots.Count; i++)
        {
            if (musicSlots[i] == null)
            {
                Debug.LogError($"MusicSheetManager: musicSlots[{i}] is NULL! Removing it from the list. Please ensure all slots are assigned or found correctly.", this);
                musicSlots.RemoveAt(i);
                i--;
            }
            else
            {
                musicSlots[i].slotIndex = i;
            }
        }

        // --- Button Setup ---
        if (playPauseButton != null)
        {
            playPauseButton.onClick.AddListener(TogglePlayback);
        }
        else
        {
            Debug.LogWarning("MusicSheetManager: Play/Pause Button not assigned! Playback can only be controlled programmatically.", this);
        }

        // --- Sequence Notes Array Initialization ---
        if (musicSlots.Count > 0)
        {
            noteSequence = new NoteData[musicSlots.Count];
        }
        else
        {
            Debug.LogError("MusicSheetManager: No MusicSlots found or assigned after cleanup. Cannot initialize noteSequence array. Disabling MusicSheetManager.", this);
            enabled = false;
            return;
        }

        // --- Initial Visual State ---
        foreach (var slot in musicSlots)
        {
            if (slot != null)
            {
                slot.SetVisualState(defaultColor);
            }
        }
    }

    private void Update()
    {
        if (isPlaying)
        {
            beatTimer += Time.deltaTime;
            float timePerBeat = 60f / beatsPerMin;

            if (beatTimer >= timePerBeat)
            {
                beatTimer -= timePerBeat;
                AdvanceToNextBeat();
            }
        }
    }

    public void TogglePlayback()
    {
        isPlaying = !isPlaying;
        if (isPlaying)
        {
            Debug.Log("Playback started");
            beatTimer = 0f;
            currentBeatIndex = 0;
            AdvanceToNextBeat(true); // Force immediate play of first beat
        }
        else
        {
            Debug.Log("Playback stopped");
            foreach (var slot in musicSlots)
            {
                if (slot != null)
                {
                    slot.SetVisualState(defaultColor);
                }
            }
        }
    }

    private void AdvanceToNextBeat(bool forceImmediate = false)
    {
        // Early exit if no slots or sequence for safety
        if (musicSlots.Count == 0 || noteSequence == null || noteSequence.Length == 0)
        {
            Debug.LogWarning("MusicSheetManager: Cannot advance beat, no slots or sequence initialized. Stopping playback.", this);
            isPlaying = false;
            return;
        }

        // Unhighlight slots from previous beat(s)
        if (!forceImmediate)
        {
            int previousActiveSlotIndex = (currentBeatIndex - 1 + musicSlots.Count) % musicSlots.Count;
            if (musicSlots[previousActiveSlotIndex] != null)
            {
                musicSlots[previousActiveSlotIndex].SetVisualState(defaultColor);
            }
        }

        // Advance to next slot index, looping if at end of sheet
        currentBeatIndex %= musicSlots.Count;

        // Get the current MusicSlot object
        MusicSlot currentSlot = musicSlots[currentBeatIndex];
        if (currentSlot == null)
        {
            Debug.LogError($"MusicSheetManager: currentSlot at index {currentBeatIndex} is NULL despite cleanup! Skipping beat. This indicates a deeper setup issue.", this);
            currentBeatIndex++;
            return;
        }

        // Get the NoteData for the current beat's slot
        NoteData noteInCurrentSlot = noteSequence[currentBeatIndex];

        // Determine how many beats this note spans
        int beatsToSpan = 1;
        if (noteInCurrentSlot != null)
        {
            beatsToSpan = Mathf.Max(1, (int)noteInCurrentSlot.noteDuration);
        }

        for (int i = 0; i < beatsToSpan; i++)
        {
            int slotToHighlightIndex = currentBeatIndex + i;

            if (slotToHighlightIndex < musicSlots.Count)
            {
                MusicSlot slotToHighlight = musicSlots[slotToHighlightIndex];
                if (slotToHighlight != null)
                {
                    if (noteInCurrentSlot != null)
                    {
                        slotToHighlight.SetVisualState(activeColor);
                    }
                    else
                    {
                        slotToHighlight.SetVisualState(restColor);
                    }
                }
            }
            else
            {
                break;
            }
        }

        // Execute the Note's Effect
        if (noteInCurrentSlot != null)
        {
            Debug.Log($"Playing note: {noteInCurrentSlot.noteName} (Duration: {noteInCurrentSlot.noteDuration}) with Attributes: {noteInCurrentSlot.attributes} at beat {currentBeatIndex + 1}");
        }
        else
        {
            Debug.Log($"Resting at beat {currentBeatIndex + 1}");
        }

        currentBeatIndex = (currentBeatIndex + beatsToSpan);
    }

    public void RegisterNotePlacement(MusicSlot slot, bool placed)
    {
        if (slot == null || noteSequence == null || slot.slotIndex < 0 || slot.slotIndex >= noteSequence.Length)
        {
            Debug.LogError($"MusicSheetManager: Invalid slot registration attempt. Slot: {slot?.name}, Index: {slot?.slotIndex}, noteSequence Initialized: {noteSequence != null}.", this);
            return;
        }

        noteSequence[slot.slotIndex] = placed ? slot.currentNoteData : null;
        Debug.Log($"Manager: Slot {slot.slotIndex} is now {(placed ? "occupied by " + slot.currentNoteData?.noteName + " (Duration: " + slot.currentNoteData.noteDuration + ")" : "empty")}.");

        if (placed && slot.currentNoteData != null)
        {
            int beatsToSpan = Mathf.Max(1, (int)slot.currentNoteData.noteDuration);
            for (int i = 0; i < beatsToSpan; i++)
            {
                int indexToHighlight = slot.slotIndex + i;
                if (indexToHighlight < musicSlots.Count)
                {
                    if (musicSlots[indexToHighlight] != null)
                    {
                        musicSlots[indexToHighlight].SetVisualState(activeColor);
                    }
                }
            }
        }
        else
        {
            if (slot != null)
            {
                slot.SetVisualState(defaultColor);
            }
        }
    }
}