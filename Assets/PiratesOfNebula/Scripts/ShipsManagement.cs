using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsManagement : MonoBehaviour
{
    public GameObject Spaceship;
    public GameObject[] Menus;
    public GameObject[] Weapons;

    void Start()
    {
        
    }
  
    void Update()
    {
        
    }

    public void SwitchWeapons()
    {
        ChangeMenu(1);
        Spaceship.SetActive(false);

    }
    public void FlyShip()
    {
        ChangeMenu(0);
        Spaceship.SetActive(true);
    }
    void ChangeMenu(int Menu)
    {
        foreach(GameObject m in Menus)
        {
            m.SetActive(false);
        }
        Menus[Menu].SetActive(true);
    }
}
