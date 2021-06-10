using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public string Name;
    public GameObject Weapon;
    public int Cost;
    bool Bought;
    Currency c;
    public void Buy()
    {
        
        if (!Bought && c.Money>=Cost)
        {
            Bought = true;
            c.Money -= Cost;
            ShipsManagement s = c.gameObject.GetComponent<ShipsManagement>();
            s.Weapons.Add(Weapon);
            s.setWeaponSwitching();
            c.Shop();
        }
    }
    public void SetCurrency(Currency C)
    {
        c = C;
    }
    public bool checkifOwned()
    {
        return Bought;
    }
    public void AlreadyBought()
    {
        Bought = true;
    }
}

public class Currency : MonoBehaviour
{
    ObjectPooler objectPooler;
    public List<Item> Items;
    public int Money;
    private int money=-1;
    public TMP_Text MoneyUI;
    public GameObject button;
    public GameObject WeaponsList;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        foreach(Item i in Items)
        {
            i.SetCurrency(this);
        }
        Shop();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoney();
    }

    public void UpdateMoney()
    {
        if (Money != money)
        {
            money = Money;
            MoneyUI.text = $"Units: {Money}";
        }
    }
    public void PopupMoney(GameObject position, int amount)
    {
      objectPooler.SpawnFromPool("MoneyPopup", position.transform.position, Quaternion.Euler(new Vector3() {x=90,y=0,z=0 })).transform.GetChild(0).GetComponent<TMP_Text>().text = $"+{amount.ToString()}"; 
    }
    
    public void Shop()
    {
        foreach (Transform g in WeaponsList.transform)
        {
            Destroy(g.gameObject);
        }
        int x = -250;
        int y = 150;
        int count = 0;
        //yes
        foreach (Item i in Items)
        {
            if (!i.checkifOwned())
            {
                GameObject g = Instantiate(button);
                g.transform.SetParent(WeaponsList.transform);
                TMP_Text t = g.transform.GetChild(0).GetComponent<TMP_Text>();
                t.text = $"{i.Name} {i.Cost}";
                Button b = g.GetComponent<Button>();
                b.onClick.AddListener(i.Buy);
                g.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y);
                count++;
                if (count >= 5) { x = -450; y -= 70; count = 0; }
                y -= 100;
            }
        }

    }
}
