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

    protected abstract void Movement();
    protected abstract void Combat();

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
        if (Heatbar != null)
        {
            if (OverHeated) Heatbar.color = Color.red;
            else Heatbar.color = Color.Lerp(Color.yellow, Color.red, Heat / (MaxHeat*1.5f));
            Heatbar.fillAmount = Heat / MaxHeat;
        }
        if (Heat> 100) { OverHeated = true; Heat = 100; }
        else if (Heat > 0) { Heat -= CoolingHeat * Time.deltaTime; }
        else if (Heat < 0) { OverHeated = false; Heat = 0;}
    }
}
