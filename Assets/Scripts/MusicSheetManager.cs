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
        Debug.Log($"TogglePlayback called at time {Time.time}. 'isPlaying' is currently: {isPlaying}");

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
        // Make the playhead visible and set it to the start.
        playhead.SetActive(true);
        playhead.transform.SetParent(bars[0].transform, false);

        // Calculate total duration of the entire sheet in seconds.
        float totalSheetTicks = bars.Sum(b => b.totalTicksInBar);
        float timePerTick = (60f / beatsPerMin) / bars[0].scalingFactor; // Assumes all bars have same scaling
        float totalSheetDurationInSeconds = totalSheetTicks * timePerTick;

        float playbackTimer = 0f;
        NoteData previouslyActiveNote = null;

        // Loop as long as playback is active
        while (isPlaying) 
        {
            // Increment timer and loop it if it exceeds the total duration
            playbackTimer += Time.deltaTime;
            if (playbackTimer >= totalSheetDurationInSeconds)
            {
                playbackTimer -= totalSheetDurationInSeconds; // Loop back to the beginning
            }

            // Calculate total progress in ticks
            float currentProgressPercent = playbackTimer / totalSheetDurationInSeconds;
            float currentTotalTicks = currentProgressPercent * totalSheetTicks;

            // Figure out which bar the playhead is currently on
            BarManager currentBar = null;
            int ticksSoFar = 0;
            foreach (var bar in bars)
            {
                if (currentTotalTicks >= ticksSoFar && currentTotalTicks < ticksSoFar + bar.totalTicksInBar)
                {
                    currentBar = bar;
                    break;
                }
                ticksSoFar += bar.totalTicksInBar;
            }

            // Position the playhead within the current bar
            if (currentBar != null)
            {
                playhead.transform.SetParent(currentBar.transform, false);
                playhead.transform.SetAsLastSibling();

                float ticksInCurrentBar = currentTotalTicks - ticksSoFar;
                float percentInCurrentBar = ticksInCurrentBar / currentBar.totalTicksInBar;
                float barWidth = currentBar.GetComponent<RectTransform>().rect.width;

                playhead.GetComponent<RectTransform>().anchoredPosition = new Vector2(percentInCurrentBar * barWidth, 0);
            }

            PlaybackEvent activeEvent = fullSequence.FirstOrDefault(e => currentTotalTicks >= e.startTimeInTicks && currentTotalTicks < e.startTimeInTicks + e.durationInTicks);

            activeNoteData = activeEvent?.noteData; 

            if (activeNoteData != previouslyActiveNote)
            {
                hasFiredForCurrentNote = false;
                previouslyActiveNote = activeNoteData;
            }

            yield return null; // Wait for the next frame
        }
    }
}