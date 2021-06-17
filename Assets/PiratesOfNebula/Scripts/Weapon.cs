using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int Id;
    public GameObject GunPoint;
    public float Cooldown = 0.2f;
    public float HeatGain;
    protected float cooldown;
    public Weapon[] ExtraCannons;
    public GameObject WeaponIcon;
   

    public void Shoot(GameObject Owner)
    {
        if (cooldown <= 0)
        {
            cooldown = Cooldown;
            Shooting(Owner);
            GetComponent<AudioManager>()?.PlaySound(Sound.Activation.Shoot);
            

            if (HeatGain > 0)
            {
                SpaceShips ship = Owner.GetComponent<SpaceShips>();
                if (ship != null)
                {
                    ship.Heat += HeatGain;
                }
            }
        }
        if (ExtraCannons.Length > 0)
        {
            foreach (Weapon c in ExtraCannons)
            {
                c.Shoot(Owner);
            }
        }
    }
    public abstract void Shooting(GameObject Owner);
}
