using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour, IDropHandler
{
    public GameObject draggableNotePrefab;
    public GameObject inventorySlotPrefab;
    public Transform slotContainer;

    void OnEnable()
    {
        InventoryManager.OnInventoryChanged += RedrawInventory;
        RedrawInventory();
    }

    void OnDisable()
    {
        InventoryManager.OnInventoryChanged -= RedrawInventory;
    }

    private void RedrawInventory()
    {
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var noteData in InventoryManager.Instance.ownedNotes)
        {
            GameObject slotGO = Instantiate(inventorySlotPrefab, slotContainer);
            GameObject noteGO = Instantiate(draggableNotePrefab, slotGO.transform);
            noteGO.transform.SetAsLastSibling();

            RectTransform noteRectTransform = noteGO.GetComponent<RectTransform>();
            noteRectTransform.anchoredPosition = Vector2.zero;

            // Get the DraggableNote script and give it the data
            DraggableNote note = noteGO.GetComponent<DraggableNote>();
            note.Initialize(noteData);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        PlacedNote placedNote = eventData.pointerDrag.GetComponent<PlacedNote>();
        if (placedNote != null)
        {
            InventoryManager.Instance.AddNote(placedNote.noteData);

            placedNote.currentBar.RemoveNote(placedNote);

            Destroy(placedNote.gameObject);
        }
    }

    public void TogglePanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}