using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour , IPointerDownHandler , IBeginDragHandler , IEndDragHandler ,  IDragHandler
{
    RectTransform icon;
    CanvasGroup cg;
    Vector3 startPosition;
    Canvas canvas;
    public GameObject TheWeapon;

    private void Awake()
    {
        icon = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = icon.anchoredPosition;
        canvas = gameObject.transform.parent.parent.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        cg.alpha = .6f;
        cg.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        icon.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.anchoredPosition = startPosition;
        cg.blocksRaycasts = true;
        cg.alpha = 1f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       // throw new System.NotImplementedException();
    }
    
}
