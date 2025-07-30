using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicSlot : MonoBehaviour, IDropHandler
{
    public PlacedNote currentNote {  get; private set; }

    private MusicSheetManager musicSheetManager;
    private Image slotImage;

    private void Awake()
    {
        //make sure it can find musicsheetmanager
        //check 1: check the parent
        if(musicSheetManager == null)
        {
            musicSheetManager = GetComponentInParent<MusicSheetManager>();
        }

        //check again
        if(musicSheetManager == null)
        {
            Debug.LogError("MusicSlot: Cannot find MusicSheetManager");
        }

        slotImage = GetComponent<Image>();
        if (slotImage == null)
        {
            Debug.LogError("MusicSlot: No Image component");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Allows note to be attached to a slot

        if (eventData.pointerDrag != null)
        {
            PlacedNote droppedNote = eventData.pointerDrag.GetComponent<PlacedNote>();
            if (droppedNote != null)
            {
                //If anotehr note is there, clear it
                if (currentNote != null && currentNote != droppedNote)
                {
                    Destroy(currentNote.gameObject);
                }

                currentNote = droppedNote;
                currentNote.SetCurrentParent(this);
                //tell the manager that note is placed
                musicSheetManager.RegisterNotePlacement(this, true);

            }
            
        }
    }

    //Check if the slot is cleared
    public void ClearSlot()
    {
        currentNote = null;
        if(musicSheetManager != null)
        {
            musicSheetManager.RegisterNotePlacement(this, false);
        }
        Debug.Log($"Slot {name} cleared.");
    }

    public void SetVisualState(Color color)
    {
        if (slotImage != null)
        {
            slotImage.color = color;
        }
    }

}
