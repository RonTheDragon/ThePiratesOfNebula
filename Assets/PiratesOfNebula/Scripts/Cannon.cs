using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    ObjectPooler objectPooler;
    public GameObject GunPoint;
    public string PrefabShot;
    public bool ParticleShot;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot()
    {
        if (ParticleShot)
        {

        }
        else
        {
            objectPooler.SpawnFromPool(PrefabShot, GunPoint.transform.position, GunPoint.transform.rotation);
        }
    }
}
