using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlacedNote : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //Allows note to be dragged

    public NoteData noteData;
    public BarManager currentBar { get; set; }
    public float localXPos;

    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPos;

    [SerializeField] private Image icon;

    public void SetupVisual(NoteData note)
    {
        icon.sprite = note.icon;
        switch (note.element)
        {
            case NoteData.Elements.Fire:
                icon.color = Color.red;
                break;
            case NoteData.Elements.Water:
                icon.color = Color.blue;
                break;
            case NoteData.Elements.Lightining:
                icon.color = Color.yellow;
                break;
            default:
                icon.color = Color.white;
                break;
        }
    }

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
