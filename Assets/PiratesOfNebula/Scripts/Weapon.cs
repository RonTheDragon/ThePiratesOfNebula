using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject GunPoint;
    public float Cooldown = 0.2f;
    protected float cooldown;
    public Cannon[] ExtraCannons;
    public GameObject WeaponIcon;
   

    public void Shoot(GameObject Owner)
    {
        if (cooldown <= 0)
        {
            cooldown = Cooldown;
            Shooting(Owner);

        }
        if (ExtraCannons.Length > 0)
        {
            foreach (Cannon c in ExtraCannons)
            {
                c.Shoot(Owner);
            }
        }
    }
    public abstract void Shooting(GameObject Owner);
}
