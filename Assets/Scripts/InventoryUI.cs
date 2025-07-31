using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject draggableNotePrefab;
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
            // Instantiate the new prefab
            GameObject noteGO = Instantiate(draggableNotePrefab, slotContainer);

            // Get the DraggableNote script and give it the data
            DraggableNote note = noteGO.GetComponent<DraggableNote>();
            note.noteData = noteData;
        }
    }

    public void TogglePanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}