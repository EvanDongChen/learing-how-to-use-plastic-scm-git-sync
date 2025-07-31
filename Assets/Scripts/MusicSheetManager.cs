using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class MusicSheetManager : MonoBehaviour
{
    public List<BarManager> bars = new List<BarManager>();

    public float beatsPerMin = 120f;

    private bool isPlaying = false;
    private int currentBeat = 0;
    private float beatTimer = 0f;

    private void Update()
    {
        if (!isPlaying) return;

        beatTimer += Time.deltaTime;
        float timePerBeat = 60f;

        if(beatTimer >= timePerBeat)
        {
            beatTimer -= timePerBeat;
            PlayBar(currentBeat);
            currentBeat = (currentBeat + 1) % (bars.Count * 4);
        }
    }

    private void PlayBar(int beatIndex)
    {
        Debug.Log($"Playing beat {beatIndex + 1}");

        //find which note is active later on

        //future: figure out which bar the beat is in -> get the note sequence from that bar -> determine which note starts on -> play the note sound
    }

    public void TogglePlayback()
    {
        isPlaying = !isPlaying;
        if (isPlaying)
        {
            currentBeat = 0;
            beatTimer = 0;
            Debug.Log("Playback Started");
        }
        else
        {
            Debug.Log("Playback Stopped");
        }
    }

}