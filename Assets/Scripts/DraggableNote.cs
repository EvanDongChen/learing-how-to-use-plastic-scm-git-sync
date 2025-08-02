using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableNote : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public NoteData noteData;
    public Image icon;


    private Transform originalParent;
    private Vector3 originalPosition;
    private CanvasGroup canvasGroup;

    [HideInInspector]
    public bool wasAcceptedIntoBar = false;
    private void Start()
    {
        SetupNoteVisual();
    }

    private void SetupNoteVisual()
    {
        if (icon != null && noteData != null)
        {
            icon.sprite = noteData.icon;
            icon.color = GetColorForElement(noteData.element);
        }
    }

    Color GetColorForElement(NoteData.Elements element)
    {
        return element switch
        {
            NoteData.Elements.Fire => Color.red,
            NoteData.Elements.Water => Color.blue,
            NoteData.Elements.Lightining => Color.yellow,
            _ => Color.white
        };
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) { canvasGroup = gameObject.AddComponent<CanvasGroup>(); }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.isEditable) return;

        originalParent = transform.parent;
        originalPosition = transform.position;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
        wasAcceptedIntoBar = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (!wasAcceptedIntoBar)
        {
            transform.SetParent(originalParent);
            transform.position = originalPosition;
        }

        // Check if we dropped it on something that wasn't a BarManager
        if (eventData.pointerEnter == null || eventData.pointerEnter.GetComponent<BarManager>() == null)
        {
            // If not, snap back to the inventory
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
        }
    }
}