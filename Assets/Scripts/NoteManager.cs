using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class NoteManager : MonoBehaviour
{
    public List<NoteData> noteData;

    //temp prefab
    public GameObject notePrefab;

    [SerializeField] private Canvas mainCanvas;

    public PlacedNote SpawnTestNote(NoteData dataToSpawn, Vector2 position)
    {
        if (notePrefab == null || mainCanvas == null)
        {
            Debug.LogError("Cannot spawn note: PlacedNotePrefab or MainCanvas is null.");
            return null;
        }

        // Instantiate the visual note prefab
        GameObject noteGO = Instantiate(notePrefab, position, Quaternion.identity, mainCanvas.transform);
        PlacedNote placedNoteComponent = noteGO.GetComponent<PlacedNote>();

        if (placedNoteComponent != null)
        {
            // Assign the correct NoteData to this spawned visual note
            placedNoteComponent.noteData = dataToSpawn;

            // Set the RectTransform's anchored position
            RectTransform noteRect = noteGO.GetComponent<RectTransform>();
            if (noteRect != null)
            {
                noteRect.anchoredPosition = position;
            }

            Debug.Log($"Spawned note '{dataToSpawn.noteName}' for testing.");
            return placedNoteComponent;
        }
        else
        {
            Debug.LogError("notePrefab does not have a PlacedNote component!");
            Destroy(noteGO);
            return null;
        }
    }

    private void Start()
    {
        if (noteData != null && noteData.Count > 0)
        {
            float startX = -150f;
            float spacing = 100f;

            // Spawn notes in a horizontal line below the sheet, just for testing
            for (int i = 0; i < noteData.Count; i++)
            {
                // Y position is arbitrary for testing, adjust as needed
                SpawnTestNote(noteData[i], new Vector2(startX + i * spacing, -200f));
            }
        }
        else
        {
            Debug.LogWarning("NoteManager: No NoteData assets assigned to 'All Available Note Data' list. No notes will be spawned for testing.");
        }
    }
}