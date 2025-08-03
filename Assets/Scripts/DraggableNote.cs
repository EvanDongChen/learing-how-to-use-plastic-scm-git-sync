using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableNote : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public NoteData noteData { get; private set; }
    public Image icon;


    private Transform originalParent;
    private Vector3 originalPosition;
    private CanvasGroup canvasGroup;

    [HideInInspector]
    public bool wasAcceptedIntoBar = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) { canvasGroup = gameObject.AddComponent<CanvasGroup>(); }

    }

    public void Initialize(NoteData data)
    {
        noteData = data;
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;

        if (!GameManager.Instance.isEditable)
        {
            eventData.pointerDrag = null;
            return;
        }

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
        wasAcceptedIntoBar = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag == gameObject)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (wasAcceptedIntoBar)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.SetParent(originalParent);
            transform.position = originalPosition;
        }
    }
}