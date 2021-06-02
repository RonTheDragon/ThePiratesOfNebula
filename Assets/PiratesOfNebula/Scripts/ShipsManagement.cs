using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsManagement : MonoBehaviour
{
    public GameObject Spaceship;
    public GameObject[] Menus;
    public GameObject WeaponsList;
    public List<GameObject> Weapons;

    void Start()
    {
        setWeaponSwitching();
  
    }
  
    void Update()
    {
   
    }

    public void SwitchWeapons()
    {
        ChangeMenu(1);
        Time.timeScale = 0;
    }
    public void FlyShip()
    {
        ChangeMenu(0);   
        Time.timeScale = 1;
    }
    public void VisitShop()
    {
        GetComponent<Currency>().Shop();
        ChangeMenu(2);
        Time.timeScale = 0;
    }

    void ChangeMenu(int Menu)
    {
        foreach(GameObject m in Menus)
        {
            m.SetActive(false);
        }
        Menus[Menu].SetActive(true);
    }
    public void setWeaponSwitching()
    {
        foreach(Transform g in WeaponsList.transform)
        {
            Destroy(g.gameObject);
        }
        int x = -350;
        int y = 150;
        int count = 0;
        foreach (GameObject w in Weapons)
        {
            Weapon c = w.GetComponent<Weapon>();
            if (c != null)
            {
                GameObject i = Instantiate(c.WeaponIcon);
                // i.transform.parent = Menus[1].transform;
                i.transform.SetParent(WeaponsList.transform,false);
                RectTransform r = i.GetComponent<RectTransform>();
                DragAndDrop d = i.GetComponent<DragAndDrop>();
                
                d.TheWeapon = w;
                r.anchoredPosition = new Vector3(x, y);
                count++;
                if (count >= 5) { x = -450; y -= 70; count = 0; }
                x += 100;
            }
        }
    }
}
