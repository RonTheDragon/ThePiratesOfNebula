using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Damager , IpooledObject
{
    public float Speed;
    public bool DestroyOnImpact;
    public bool DeclaredByShooter;
    public bool CantHitSelf;
    public GameObject ShootBy;
    // Start is called before the Active
    public void OnObjectSpawn()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * Speed * Time.deltaTime;
        if (damagecooldown > 0) { damagecooldown -= Time.deltaTime; }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (damagecooldown <= 0)
        {
            damagecooldown = DamageCooldown;
            string HitableTag=null;
            string HitableTag2=null;
            
                if (DeclaredByShooter)
                {
                    if (ShootBy.gameObject.tag == "Player")
                    {
                        CantHitEnemy = false;
                        CantHitPlayer = true;
                    }
                    else if (ShootBy.gameObject.tag == "Enemy")
                    {
                        CantHitEnemy = true;
                        CantHitPlayer = false;
                    }
                }
            
            if (!CantHitPlayer)
            {
                HitableTag = "Player";
            }
            if (!CantHitEnemy)
            {
                HitableTag2 = "Enemy";
                
            }

            if (collision.gameObject.tag == HitableTag|| collision.gameObject.tag == HitableTag2)
            {
                //Debug.Log("Boom! You Shot: " + collision.gameObject.name);
                if (CantHitSelf == false || collision.gameObject != ShootBy) 
                {
                    Health hp = collision.gameObject.GetComponent<Health>();
                    if (hp != null)
                    {
                        hp.Damage(AttackDamage, Knockback, gameObject);
                        if (DestroyOnImpact) gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
