using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlacedNote : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //Allows note to be dragged

    public NoteData noteData;
    public BarManager currentBar { get; set; }

    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPos = rectTransform.anchoredPosition;
    }

    //Start dragging object
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPos = GetComponent<RectTransform>().anchoredPosition;

        if (currentBar != null)
        {
            currentBar.RemoveNote(this);
            currentBar = null;
        }

        //reparent the main canvas
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();

        canvasGroup.alpha = .7f;
        canvasGroup.blocksRaycasts = false;
    }


    //Allows drag on every frame
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    //Let go of click
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (currentBar == null)
        {
            rectTransform.anchoredPosition = originalPos;
            Debug.Log($"Note dropped off slot. Returned to original position: {originalPos}");
        }
        else
        {
            Debug.Log($"Note successfully placed in bar {currentBar.name}.");
        }
    }
}
