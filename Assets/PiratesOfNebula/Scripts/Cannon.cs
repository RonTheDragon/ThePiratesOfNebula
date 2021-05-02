using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    ObjectPooler objectPooler;
    public GameObject GunPoint;
    public string PrefabShot;
    public bool ParticleShot;
    public float Cooldown=0.2f;
    float cooldown;
    public Cannon[] ExtraCannons;
    public GameObject WeaponIcon;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0) { cooldown -= Time.deltaTime; }
    }

    public void Shoot()
    {
        if (cooldown <= 0)
        {
            cooldown = Cooldown;
            if (ParticleShot)
            {

            }
            else
            {
                objectPooler.SpawnFromPool(PrefabShot, GunPoint.transform.position, GunPoint.transform.rotation);
            }
        }
        if (ExtraCannons.Length > 0)
        {
            foreach(Cannon c in ExtraCannons)
            {
                c.Shoot();
            }
        }
    }
}
