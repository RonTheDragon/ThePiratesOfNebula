﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsManagement : MonoBehaviour
{
    public GameObject Spaceship;
    public GameObject[] Menus;
    public GameObject[] Weapons;

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
    void ChangeMenu(int Menu)
    {
        foreach(GameObject m in Menus)
        {
            m.SetActive(false);
        }
        Menus[Menu].SetActive(true);
    }
    void setWeaponSwitching()
    {
        int x = -350;
        int y = 70;
        int count = 0;
        foreach (GameObject w in Weapons)
        {
            Cannon c = w.GetComponent<Cannon>();
            if (c != null)
            {
                GameObject i = Instantiate(c.WeaponIcon);
                // i.transform.parent = Menus[1].transform;
                i.transform.SetParent(Menus[1].transform);
                RectTransform r = i.GetComponent<RectTransform>();
                DragAndDrop d = i.GetComponent<DragAndDrop>();
                d.TheWeapon = w;
                r.anchoredPosition = new Vector3(x, y);
                count++;
                if (count >= 5) { x = -430; y -= 80; count = 0; }
                x += 80;
            }
        }
    }
}