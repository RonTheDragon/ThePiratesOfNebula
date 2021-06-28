using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour , IDropHandler
{
    public int SlotNumber;
    public GameObject Player;
    PlayerControl Pc;
    GameObject Icon;
    public void OnDrop(PointerEventData eventData)
    {
        DragAndDrop d = eventData.pointerDrag.GetComponent<DragAndDrop>();
        Pc.ChangeWeapon(d.TheWeapon, SlotNumber);
        SetIcon();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetShip();
        SetIcon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SetShip()
    {
        ShipsManagement s = Player.GetComponent<ShipsManagement>();
        Pc = s.Spaceship.GetComponent<PlayerControl>();
    }
    void SetIcon()
    {
        Destroy(Icon);
        Weapon c = Pc.Cannons[SlotNumber].GetComponent<Weapon>();
        if (c != null)
        {
            GameObject i = Instantiate(c.WeaponIcon, transform, false);
            RectTransform r = i.GetComponent<RectTransform>();
            RectTransform rr = gameObject.GetComponent<RectTransform>();
            r.anchoredPosition = rr.anchoredPosition;
            Icon = i;
            CanvasGroup cg = i.GetComponent<CanvasGroup>();
            cg.blocksRaycasts = false;
        }
    }
}
