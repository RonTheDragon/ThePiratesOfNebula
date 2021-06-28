using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]

public abstract class Product
{
    public string Name;
    protected Currency c;
    public int Cost;
    public abstract void Buy();
    
    public void SetCurrency(Currency C)
    {
        c = C;
    }
}
[System.Serializable]
public class Upgrade : Product
{
    [HideInInspector]
    public int Level;
    public int MaxLevel;
    public float CostMultiplier = .5f;
    public Sprite Icon;
    [HideInInspector]
    public List<int> Costs;
    public UnityEvent OnUpgrade;
    public override void Buy()
    {
        if (c.Money >= Costs[Level] && Level < Costs.Count-1)
        {
            c.Money -= Costs[Level];
            OnUpgrade?.Invoke();
            Level++;
            c.UpgradeShop();
        }
    }
    public void Upgraded()
    {
        OnUpgrade?.Invoke();
        Level++;
    }
    public void SetCosts()
    {
        float CurrentCost = Cost;
        for (int i = 0; i < MaxLevel; i++)
        {
            Costs.Add((int)CurrentCost);
            CurrentCost *= 1+ CostMultiplier;
        }
    }
}
[System.Serializable]
public class Item : Product
{
    public GameObject Weapon;
    bool Bought;
    

    public override void Buy()
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
    ShipsManagement SM;
    Health Hp;
    PlayerControl PC;
    ObjectPooler objectPooler;
    public List<Item> Items;
    public List<Upgrade> Upgrades;
    public int Money;
    private int money=-1;
    public TMP_Text MoneyUI;
    public GameObject button;
    public GameObject Upgradebutton;
    public GameObject WeaponsList;
    public GameObject UpgradesList;
    public RectTransform WeaponShopContent;
    public RectTransform UpgradesShopContent;
    public GameObject Wormhole;
    public GameObject WormholeCompus;
    bool FixMoneySet;
    Vector3 prev;
    bool alreadyInShop;
    float distFromWormhole;

    // Start is called before the first frame update
    private void Awake()
    {
        prev = MoneyUI.gameObject.transform.localScale;
        SM = GetComponent<ShipsManagement>();
        Hp = SM.Spaceship.GetComponent<Health>();
        PC = SM.Spaceship.GetComponent<PlayerControl>();

    }

    void Start()
    {       
        objectPooler = ObjectPooler.Instance;
        foreach(Item i in Items)
        {
            i.SetCurrency(this);
        }
        foreach(Upgrade i in Upgrades)
        {
            i.SetCurrency(this);
            i.SetCosts();
        }
        Shop();
        UpgradeShop();
        SpawnWormHole();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoney();
        if (WormholeCompus.activeSelf)
        {
            WormholeCompus.transform.LookAt(Wormhole.transform.position);
        }
        distFromWormhole = Vector3.Distance(SM.Spaceship.transform.position, Wormhole.transform.position);
        if (distFromWormhole < 4 && alreadyInShop == false) { alreadyInShop = true;  SM.VisitShop(); }
        else if (distFromWormhole >= 4) { alreadyInShop = false; }
        
    }

    public void UpdateMoney()
    {
        if (Money != money)
        {
            money = Money;
            MoneyUI.text = string.Format("{0:#,#}$", money);
            if (FixMoneySet) StartCoroutine(MoneyEffect());
            
        }
        FixMoneySet = true;
    }

    IEnumerator MoneyEffect()
    {
        if (Time.timeScale > 0)
        {
            MoneyUI.gameObject.transform.localScale = new Vector3() { x = prev.x * 2, y = prev.y * 2, z = prev.z * 2 };
            MoneyUI.color = Color.yellow;
            yield return new WaitForSeconds(0.5f);
            MoneyUI.gameObject.transform.localScale = prev;
            MoneyUI.color = Color.white;
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


        int count = 0;
        //yes
        foreach (Item i in Items)
        {
            if (!i.checkifOwned())
            {
                count++;
                GameObject g = Instantiate(button, WeaponsList.transform,false);
                TMP_Text t = g.transform.GetChild(0).GetComponent<TMP_Text>();
                t.text = $"{i.Name} {i.Cost}";
                Button b = g.GetComponent<Button>();
                b.onClick.AddListener(i.Buy);
                Image Im = g.transform.GetChild(1).GetComponent<Image>();
                Im.sprite = i.Weapon.GetComponent<Weapon>().WeaponIcon.GetComponent<Image>().sprite;
            }
        }
        WeaponShopContent.sizeDelta = new Vector2(WeaponShopContent.sizeDelta.x, count * 100);
    }
    public void UpgradeShop()
    {
        foreach (Transform g in UpgradesList.transform)
        {
            Destroy(g.gameObject);
        }

        //yes
        foreach (Upgrade i in Upgrades)
        {
            if (i.Level < i.Costs.Count - 1)
            {
                
                GameObject g = Instantiate(Upgradebutton, UpgradesList.transform, false);
                TMP_Text t = g.transform.GetChild(0).GetComponent<TMP_Text>();
                t.text = $"{i.Name} {i.Costs[i.Level]}$ <{i.Level+1}>";
                Button b = g.GetComponent<Button>();
                b.onClick.AddListener(i.Buy);
                Image Im = g.transform.GetChild(1).GetComponent<Image>();
                if (i.Icon!=null) Im.sprite = i.Icon;
                
            }
            else
            {
                GameObject g = Instantiate(Upgradebutton, UpgradesList.transform, false);
                TMP_Text t = g.transform.GetChild(0).GetComponent<TMP_Text>();
                t.text = $"{i.Name} MAX <{i.Level + 1}>";
                Image Im = g.transform.GetChild(1).GetComponent<Image>();
                if (i.Icon != null) Im.sprite = i.Icon;
            }
        }
        UpgradesShopContent.sizeDelta = new Vector2(UpgradesShopContent.sizeDelta.x, Upgrades.Count * 100);
    }

    public void SpawnWormHole()
    {
        float x = Random.Range(50, 100 + 1);
        if (Random.Range(0, 2) == 0) { x *= -1; }
        float z = Random.Range(50, 100 + 1);
        if (Random.Range(0, 2) == 0) { z *= -1; }
        float r = Random.Range(0, 361);
        Wormhole.transform.position = new Vector3(SM.Spaceship.transform.position.x + x, SM.Spaceship.transform.position.y, SM.Spaceship.transform.position.z + z);
    }
    
    public void TurnCompus()
    {
        if (WormholeCompus.activeSelf) { WormholeCompus.SetActive(false); }
        else { if (distFromWormhole > 250) SpawnWormHole(); WormholeCompus.SetActive(true); } 
    }
   
    


    public void UpgradeHealth()
    {
        
        Hp.MaxHp += 50;
        Hp.Hp = Hp.MaxHp;
    }
    public void UpgradeSpeed()
    {
       
        PC.MovementSpeed += 2;
    }
    public void UpgradeRaids()
    {
        
        PC.MilkTime -= 0.4f;
        PC.PiratesAmount++;
    }
    public void UpgradeHealing()
    {
        
        Hp.HpRegan++;
    }
    public void UpgradeMotor()
    {
        
        PC.MaxHeat += 25;
    }
}
