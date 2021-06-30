using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class ShipsManagement : MonoBehaviour
{
    public GameObject Spaceship;
    public GameObject[] Menus;
    public GameObject WeaponsList;
    public List<GameObject> Weapons;
    public List<GameObject> AllWeaponsInGameList;
    Currency c;
    PlayerData d;

    private void Awake()
    {c = GetComponent<Currency>();
        if (PlayerPrefs.GetInt("NewGame")==0)
        {
            d = SaveSystem.Load(PlayerPrefs.GetInt("slot"));
            c.Money = d.Money; //set Money
            StartCoroutine(UpdateTheShop());
            Weapons = new List<GameObject>();
            for (int i = 0; i < d.Inventory.Length; i++) //Set Inventory
            {
                Weapons.Add(AllWeaponsInGameList[d.Inventory[i]]);
            }
            PlayerControl p = Spaceship.GetComponent<PlayerControl>();
            p.Cannons = new GameObject[d.ItemSlots.Length];
            for (int i = 0; i < d.ItemSlots.Length; i++) //Set Weapon Slots
            {
                p.Cannons[i] = AllWeaponsInGameList[d.ItemSlots[i]];
            }


        }
        else
        {
            GetComponent<Tutorial>().StartTutorial();
            StartCoroutine(FirstColor());
        }
        
    }

    IEnumerator FirstColor()
    {
        yield return new WaitForSeconds(0.01f);
        c.Textures[0].ChangeColor();
    }

    IEnumerator UpdateTheShop()
    {
        yield return new WaitForSeconds(0.01f);

        for (int i = 0; i < c.Items.Count; i++) //Set Shop
        {
            if (d.Shop[i])
            {
                c.Items[i].AlreadyBought();
            }
        }
        for (int i = 0; i < c.Upgrades.Count; i++) //Set Shop
        {
            int count = 0;
            while (c.Upgrades[i].Level < d.Upgrades[i])
            {
                c.Upgrades[i].Upgraded();
                count++;
                if (count > 100) break;
            }
        }
        c.UpgradeShop();
        c.Textures[d.Color].ChangeColor();
    }

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
    public void SaveAndExit()
    {
        SaveSystem.Save(gameObject, PlayerPrefs.GetInt("slot"));
        SceneManager.LoadScene("OpenScreen");
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
        foreach (Transform g in WeaponsList.transform)
        {
            Destroy(g.gameObject);
        }

        foreach (GameObject w in Weapons)
        {
            Weapon c = w.GetComponent<Weapon>();
            if (c != null)
            {
                GameObject i = Instantiate(c.WeaponIcon,WeaponsList.transform, false);
                DragAndDrop d = i.GetComponent<DragAndDrop>(); 
                d.TheWeapon = w;             
            }
        }
    }
}
