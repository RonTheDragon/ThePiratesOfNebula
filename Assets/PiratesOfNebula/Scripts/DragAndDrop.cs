using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour , IPointerDownHandler , IBeginDragHandler , IEndDragHandler ,  IDragHandler, IDropHandler
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
      //  icon.localScale = new Vector3() { x = canvas.scaleFactor, y = canvas.scaleFactor, z = canvas.scaleFactor };
        }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = icon.anchoredPosition;
        canvas = transform.parent.parent.parent.GetComponent<Canvas>(); // oh great father
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

    public void OnDrop(PointerEventData eventData)
    {
        int a = 0;
        int b = 0;
        DragAndDrop d = eventData.pointerDrag.GetComponent<DragAndDrop>();
        if (d&& d!=this)
        {
            ShipsManagement s = transform.parent.parent.parent.parent.GetComponent<ShipsManagement>();
            for (int i = 0; i < s.Weapons.Count; i++)
            {
                if (s.Weapons[i] == TheWeapon) a = i;
            }
            for (int i = 0; i < s.Weapons.Count; i++)
            {
                if (s.Weapons[i] == d.TheWeapon) b = i;
            }
            GameObject g = s.Weapons[b];
            s.Weapons[b] = s.Weapons[a];
            s.Weapons[a] = g;
            s.setWeaponSwitching();
        }
    }
}
