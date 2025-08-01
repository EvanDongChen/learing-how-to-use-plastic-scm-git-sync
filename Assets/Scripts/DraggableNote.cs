using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableNote : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public NoteData noteData;
    public Image icon;

    private Transform originalParent;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) { canvasGroup = gameObject.AddComponent<CanvasGroup>(); }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Check if we dropped it on something that wasn't a BarManager
        if (eventData.pointerEnter == null || eventData.pointerEnter.GetComponent<BarManager>() == null)
        {
            // If not, snap back to the inventory
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
        }
    }
}