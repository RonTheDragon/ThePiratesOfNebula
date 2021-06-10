using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollisionDamage : Damager
{
    public GameObject DontDamageIt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (damagecooldown > 0) { damagecooldown -= Time.deltaTime; }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (damagecooldown <= 0 && collision.gameObject != DontDamageIt)
        {
            damagecooldown = DamageCooldown;
            string HitableTag = null;
            string HitableTag2 = null;

            if (!CantHitPlayer)
            {
                HitableTag = "Player";
            }
            if (!CantHitEnemy)
            {
                HitableTag2 = "Enemy";

            }

            if (collision.gameObject.tag == HitableTag || collision.gameObject.tag == HitableTag2)
            {
                //Debug.Log("Boom! You Shot: " + collision.gameObject.name);
               
                    Health hp = collision.gameObject.GetComponent<Health>();
                    if (hp != null)
                    {
                        hp.Damage(AttackDamage, Knockback, gameObject);
                    }
                
            }
        }
    }
}
