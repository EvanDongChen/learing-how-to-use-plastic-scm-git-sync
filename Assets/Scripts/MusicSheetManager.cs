using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;


public class MusicSheetManager : MonoBehaviour
{
    public List<BarManager> bars = new List<BarManager>();
    public ProjectileManagerScript projectileManager;
    public GameObject playhead;
    public float beatsPerMin = 120f;

    // A public class to hold event information
    public class PlaybackEvent
    {
        public NoteData noteData;
        public PlacedNote sourceNote; // Reference to the actual note object
        public float startTimeInTicks;
        public float durationInTicks;
    }

    public List<PlaybackEvent> fullSequence;
    private bool isPlaying = false;
    private bool hasFiredForCurrentNote = false;
    private Coroutine playbackCoroutine;
    private NoteData activeNoteData;

    private void Start()
    {
        if (playhead != null)
        {
            playhead.SetActive(false);
        }
    }

    public void TogglePlayback()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            // When play starts, build the sequence and start the playback loop
            CompilePlaybackSequence();
            if (fullSequence.Count > 0)
            {
                playbackCoroutine = StartCoroutine(PlaybackLoop());
            }
            else
            {
                isPlaying = false; // Nothing to play
            }
        }
        else
        {
            // When play stops, stop the loop and hide the playhead
            if (playbackCoroutine != null)
            {
                StopCoroutine(playbackCoroutine);
            }
            if (playhead != null)
            {
                playhead.SetActive(false);
            }
            activeNoteData = null;
        }
    }

    private void Update()
    {
        // Player input is still checked every frame
        if (activeNoteData != null && Mouse.current.rightButton.wasPressedThisFrame && !hasFiredForCurrentNote)
        {
            hasFiredForCurrentNote = true;
            projectileManager.ShootNoteAtCursor(activeNoteData);
        }
    }

    void CompilePlaybackSequence()
    {
        fullSequence = new List<PlaybackEvent>();
        int ticksSoFar = 0;
        foreach (var bar in bars)
        {
            int tickInCurrentBar = 0;
            foreach (var note in bar.GetPlacedNotes())
            {
                var noteDuration = note.noteData.noteDuration * bar.scalingFactor;
                fullSequence.Add(new PlaybackEvent
                {
                    noteData = note.noteData,
                    sourceNote = note,
                    startTimeInTicks = ticksSoFar + tickInCurrentBar,
                    durationInTicks = noteDuration
                });
                tickInCurrentBar += (int)noteDuration;
            }
            ticksSoFar += bar.totalTicksInBar;
        }
    }

    private IEnumerator PlaybackLoop()
    {
        int currentEventIndex = 0;
        float timePerTick = (60f / beatsPerMin) / bars[0].scalingFactor;

        while (true)
        {
            // Get the current note event
            PlaybackEvent currentEvent = fullSequence[currentEventIndex];

            // --- Show the playhead on the note ---
            var currentBar = currentEvent.sourceNote.currentBar;
            playhead.transform.SetParent(currentBar.transform, false);
            float noteCenterX = currentEvent.sourceNote.localXPos;
            playhead.transform.localPosition = new Vector3(noteCenterX, 0, 0);
            playhead.SetActive(true);

            // Set this note as "active" for shooting
            activeNoteData = currentEvent.noteData;
            hasFiredForCurrentNote = false;

            float noteDurationInSeconds = currentEvent.durationInTicks * timePerTick;
            yield return new WaitForSeconds(noteDurationInSeconds);

            // Deactivate the note and hide the playhead
            activeNoteData = null;
            playhead.SetActive(false);

            // --- Calculate the rest time until the next note ---
            currentEventIndex = (currentEventIndex + 1) % fullSequence.Count;
            PlaybackEvent nextEvent = fullSequence[currentEventIndex];

            float endOfCurrentNoteTicks = currentEvent.startTimeInTicks + currentEvent.durationInTicks;
            float startOfNextNoteTicks = nextEvent.startTimeInTicks;

            // If the next note is on a new loop, calculate accordingly
            if (startOfNextNoteTicks < endOfCurrentNoteTicks)
            {
                float totalSheetDurationInTicks = bars.Sum(b => b.totalTicksInBar);
                startOfNextNoteTicks += totalSheetDurationInTicks;
            }

            float restDurationInTicks = startOfNextNoteTicks - endOfCurrentNoteTicks;
            float restDurationInSeconds = restDurationInTicks * timePerTick;

            if (restDurationInSeconds > 0)
            {
                yield return new WaitForSeconds(restDurationInSeconds);
            }
        }
    }
}