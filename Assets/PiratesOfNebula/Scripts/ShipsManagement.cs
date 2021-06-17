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
    public AudioMixer audioMixer;
    public static AudioMixerGroup[] audiomixergroup;
    public AudioMixerGroup[] audioMixerGroup;

    private void Awake()
    {
        audiomixergroup = audioMixerGroup;

        audioMixer.SetFloat("Sound", PlayerPrefs.GetFloat("Sound"));
        audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("Music"));
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"));
        Resolution R = Screen.resolutions[PlayerPrefs.GetInt("Res")];
        Screen.SetResolution(R.width, R.height, Screen.fullScreen);


        if (PlayerPrefs.GetInt("NewGame")==0)
        {
            PlayerData d = SaveSystem.Load(PlayerPrefs.GetInt("slot"));
            Currency c = GetComponent<Currency>();
            c.Money = d.Money;
            for (int i = 0; i < c.Items.Count; i++)
            {
                if (d.Shop[i])
                {
                    c.Items[i].AlreadyBought();
                }
            }
            Weapons = new List<GameObject>();
            for (int i = 0; i < d.Inventory.Length; i++)
            {
                Weapons.Add(AllWeaponsInGameList[d.Inventory[i]]);
            }
            PlayerControl p = Spaceship.GetComponent<PlayerControl>();
            p.Cannons = new GameObject[d.ItemSlots.Length];
            for (int i = 0; i < d.ItemSlots.Length; i++)
            {
                p.Cannons[i] = AllWeaponsInGameList[d.ItemSlots[i]];
            }
        }
        
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
