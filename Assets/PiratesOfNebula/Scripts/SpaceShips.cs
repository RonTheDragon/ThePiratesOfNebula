using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SpaceShips : MonoBehaviour
{
    public GameObject[] Cannons; //0 Front , 1 Back , 2 Right , 3 Left
    public GameObject[] CannonHolders;
    protected Weapon[] CS;
    public float MaxHeat=100;
    public float Heat;
    public float CoolingHeat=30;
    protected bool OverHeated;
    public Image Heatbar;
    protected Health health;

    protected abstract void Movement();
    protected abstract void Combat();

    void Awake()
    {
        health = GetComponent<Health>();
    }

    protected void Update()
    {
        transform.position = new Vector3() { x = transform.position.x, y = 0, z = transform.position.z };
        HeatSystem();
    }

    public void ChangeWeapon(GameObject Weapon, int WhichCannon)
    {
        if (Cannons[WhichCannon] != null)
        {
            if (Cannons[WhichCannon].activeInHierarchy)
                Destroy(Cannons[WhichCannon]);
        }
        Cannons[WhichCannon] = Instantiate(Weapon);
        Cannons[WhichCannon].transform.position = CannonHolders[WhichCannon].transform.position;
        Cannons[WhichCannon].transform.rotation = CannonHolders[WhichCannon].transform.rotation;
        Cannons[WhichCannon].transform.SetParent(CannonHolders[WhichCannon].transform);
        CS = new Weapon[Cannons.Length];
        CS[WhichCannon] = Cannons[WhichCannon].GetComponent<Weapon>();
        
    }
    public void HeatSystem()
    {
        if (health.Froze) { Heatbar.color = Color.blue; Heatbar.fillAmount = 1; }
        else
        {
            float Temp = health.Tempeture * -0.1f;
            if (health.Burn) Temp = -10;
            if (Heatbar != null)
            {
                if (OverHeated) Heatbar.color = Color.red;
                else Heatbar.color = Color.Lerp(Color.yellow, Color.red, Heat / (MaxHeat * 1.5f));
                Heatbar.fillAmount = Heat / MaxHeat;
            }
            if (Heat > MaxHeat) { OverHeated = true; Heat = MaxHeat; GetComponent<AudioManager>()?.PlaySound(Sound.Activation.Custom, "OverHeat"); StartCoroutine(FlashHeat()); }
            else if (Heat > 0) { Heat -= (CoolingHeat + Temp) * Time.deltaTime; }
            else if (Heat < 0) { OverHeated = false; Heat = 0; }
        }
    }

    IEnumerator FlashHeat()
    {
        Heatbar.color = Color.yellow;
        yield return new WaitForSeconds(0.01f);
        Heatbar.color = Color.red;
        yield return new WaitForSeconds(0.01f);
        Heatbar.color = Color.yellow;
        yield return new WaitForSeconds(0.01f);
        Heatbar.color = Color.red;
        yield return new WaitForSeconds(0.01f);
        Heatbar.color = Color.yellow;
        yield return new WaitForSeconds(0.01f);
        Heatbar.color = Color.red;
    }
}
