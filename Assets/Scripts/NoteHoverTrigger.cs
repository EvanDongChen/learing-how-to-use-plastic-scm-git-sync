using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(DraggableNote))]
public class NoteHoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private DraggableNote draggableNote;

    void Awake()
    {
        draggableNote = GetComponent<DraggableNote>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (draggableNote != null && draggableNote.noteData != null)
        {
            InfoPanelController.Instance.ShowInfo(draggableNote.noteData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanelController.Instance.HideInfo();
    }
}