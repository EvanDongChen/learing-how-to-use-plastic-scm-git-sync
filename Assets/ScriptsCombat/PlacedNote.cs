using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlacedNote : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    //Allows note to be dragged

    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private MusicSlot currentOccupiedSlot;
    private Vector2 originalPos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        currentOccupiedSlot = GetComponentInParent<MusicSlot>();
        if (currentOccupiedSlot == null)
        {
            originalPos = rectTransform.anchoredPosition;
        }
    }

    // Get user click on item
    public void OnPointerDown(PointerEventData eventData)
    {
        currentOccupiedSlot = GetComponentInParent<MusicSlot>();

        if (currentOccupiedSlot != null)
        {
            currentOccupiedSlot.ClearSlot();
        }
        else
        {
            //note isnt in a slot, store pos before moving
            originalPos = rectTransform.anchoredPosition;
        }

        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
    }

    //Start dragging object
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .7f;
        canvasGroup.blocksRaycasts = false;
    }


    //Allows drag on every frame
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    //Let go of click
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        //drag off? detection
        MusicSlot droppedOnSlot = null;
        if (eventData.pointerEnter != null)
        {
            droppedOnSlot = eventData.pointerEnter.GetComponent<MusicSlot>();
        }

        if (droppedOnSlot == null)
        {
            //go back to original non-slot pos
            transform.SetParent(canvas.transform);
            rectTransform.anchoredPosition = originalPos;
            currentOccupiedSlot = null;
            Debug.Log($"Note dropped off slot. Returned to original position: {originalPos}");
        }
    }

    //call from musicSlot when note is placed
    public void SetCurrentParent(MusicSlot slot)
    {
        currentOccupiedSlot = slot;
        transform.SetParent(slot.transform);
        //snap to center of slot
        rectTransform.anchoredPosition = Vector2.zero;
        Debug.Log($"Note '{name}' snapped into slot '{slot.name}'.");
    }
}
