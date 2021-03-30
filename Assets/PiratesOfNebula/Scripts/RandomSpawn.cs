using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public float MinX;
    public float MaxX;
    public float MinZ;
    public float MaxZ;


    // Start is called before the first frame update
    void Start()
    {
        float x = Random.Range(MinX, MaxX + 1);
        float z = Random.Range(MinZ, MaxZ + 1);
        transform.position = new Vector3(x,transform.position.y,z);
        float r = Random.Range(0, 361);
        transform.Rotate(0, r, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
