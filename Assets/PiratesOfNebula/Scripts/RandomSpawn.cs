using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject Spaceship;
    public string[] Spawnables;
    ObjectPooler objectPooler;
    public float MinDist;
    public float MaxDist;


    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < 20)
        {          
            Spawn(Spawnables[Random.Range(0, Spawnables.Length)]);
        }
    }

    void Spawn(string prefab)
    {
        
        float x = Random.Range(MinDist, MaxDist + 1);
        if (Random.Range(0,2) == 0) { x *= -1; }
        float z = Random.Range(MinDist, MaxDist + 1);
        if (Random.Range(0, 2) == 0) { z *= -1; }
        float r = Random.Range(0, 361);
        Vector3 pos = new Vector3(Spaceship.transform.position.x+x, Spaceship.transform.position.y, Spaceship.transform.position.z+z);
        GameObject ob = objectPooler.SpawnFromPool(prefab, pos, transform.rotation,true);
        ob.GetComponent<SpawnedMechanic>().SummonedByThePlayer = Spaceship;
        
       // ob.transform.Rotate(0, r, 0);
        
    }
}
