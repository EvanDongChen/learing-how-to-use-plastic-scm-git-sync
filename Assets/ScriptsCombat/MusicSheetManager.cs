using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MusicSheetManager : MonoBehaviour
{
    //Music Sheet Manager

    public List<MusicSlot> musicSlots = new List<MusicSlot>();
    public Button playPauseButton;

    private Color activateColor = Color.green;
    private Color restColor = Color.red;
    private Color defaultColor = Color.white;

    //temp fixed time per slot
    private const float TIME_PER_SLOT = 1.0f;

    private bool isPlaying = false;
    private int currentSlotIndex = 0;
    private float slotTimer = 0f;

    private void Start()
    {
        if (musicSlots.Count == 0)
        {
            musicSlots.AddRange(GetComponentsInChildren<MusicSlot>());
            musicSlots.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        }

        if(playPauseButton != null)
        {
            playPauseButton.onClick.AddListener(TogglePlayback);
        }

        //initialize to default color
        foreach (var slot in musicSlots)
        {
            slot.SetVisualState(defaultColor);
        }
    }

    private void Update()
    {
        if (isPlaying)
        {
            slotTimer += Time.deltaTime;

            if (slotTimer >= TIME_PER_SLOT)
            {
                slotTimer -= TIME_PER_SLOT;
                AdvanceToNextSlot();
            }
        }
    }


    public void TogglePlayback()
    {
        isPlaying = !isPlaying;
        if (isPlaying)
        {
            Debug.Log("Playback started");
            slotTimer = 0f;
            currentSlotIndex = 0;
            AdvanceToNextSlot(true);
        }
        else
        {
            Debug.Log("Playback stopped");
            foreach (var slot in musicSlots)
            {
                slot.SetVisualState(defaultColor);
            }
        }
    }

    private void AdvanceToNextSlot(bool forceImmediate = false)
    {
        //if not force immediate reset color of the prev active slot
        if (!forceImmediate && musicSlots.Count > 0)
        {
            int previousSlotIndex = currentSlotIndex - 1;
            if (previousSlotIndex < 0)
            {
                previousSlotIndex = musicSlots.Count - 1;
            }
            musicSlots[previousSlotIndex].SetVisualState(defaultColor);
        }

        //now advance to next slot
        if (currentSlotIndex >= musicSlots.Count)
        {
            currentSlotIndex = 0; //loop back if it's at t eh end
        }

        //figure out which slot we are and change collor
        MusicSlot currentSlot = musicSlots[currentSlotIndex];
        //check if it's a rest or not
        bool hasNote = (currentSlot.currentNote != null);

        if (hasNote)
        {
            currentSlot.SetVisualState(activateColor);
        }
        else
        {
            //rest note
            currentSlot.SetVisualState(restColor);
        }

        currentSlotIndex++;
    }

    //see when a note is dropped in
    public void RegisterNotePlacement(MusicSlot slot, bool placed)
    {
        Debug.Log($"Slot {musicSlots.IndexOf(slot)} has a note: {placed}");
    }
}
