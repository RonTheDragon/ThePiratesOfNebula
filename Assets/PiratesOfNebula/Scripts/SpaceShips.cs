using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpaceShips : MonoBehaviour
{
    public GameObject[] Cannons; //0 Front , 1 Back , 2 Right , 3 Left
    public GameObject[] CannonHolders;
    protected Cannon[] CS;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected abstract void Movement();
    protected abstract void Combat();

    public void ChangeWeapon(GameObject Weapon, int WhichCannon)
    {
        Destroy(Cannons[WhichCannon]);
        Cannons[WhichCannon] = Instantiate(Weapon);
        Cannons[WhichCannon].transform.position = CannonHolders[WhichCannon].transform.position;
        Cannons[WhichCannon].transform.rotation = CannonHolders[WhichCannon].transform.rotation;
        Cannons[WhichCannon].transform.SetParent(CannonHolders[WhichCannon].transform);
        CS = new Cannon[Cannons.Length];
        CS[WhichCannon] = Cannons[WhichCannon].GetComponent<Cannon>();
        
    }
}
