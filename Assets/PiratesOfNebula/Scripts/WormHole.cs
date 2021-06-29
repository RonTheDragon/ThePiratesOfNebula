using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHole : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 200*Time.deltaTime, 0);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 50);
        foreach(Collider c in hitColliders)
        {
            GameObject Victim = c.gameObject;
            if (Victim.tag == "Enemy")
            {
                float dist = Vector3.Distance(Victim.transform.position, transform.position);
                Hookable H = Victim.GetComponent<Hookable>();
                if (H == null) { Suck(Victim, dist, 200); } //if Meteor
                else if (H.hookable) { Suck(Victim, dist, 1000); H.UnHooked(); } //if Hookable Ship
                else { Suck(Victim, dist, 3); } //if a Normal Ship
                if (dist < 1) {
                    Health hp = Victim.GetComponent<Health>(); if (hp != null) hp.Hp = 0;
                    if (H != null)
                    {
                        if (H.Player != null)
                        {
                            if (Vector3.Distance(H.Player.transform.position, transform.position) < 50) //Gain Money From It if youre close enough
                            {
                                Currency C = H.Player.transform.parent.GetComponent<Currency>();
                                if (C != null)
                                {
                                    int r = Random.Range(H.Money, H.MaxStartMoney);
                                    C.PopupMoney(gameObject, r);
                                    C.Money += r;
                                }
                            }
                        }        
                    }
                }
            }
        }
    }
    void Suck(GameObject Victim, float dist,float SuckPower)
    {
        Victim.transform.position = Vector3.MoveTowards(Victim.transform.position, transform.position, (SuckPower * Time.deltaTime) / dist);
    }
}
