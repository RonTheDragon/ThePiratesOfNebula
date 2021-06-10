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
    public string[] SpawnOnDeath;
    ObjectPooler objectPooler;
    // Start is called before the Active
    public void OnObjectSpawn()
    {
        
    }
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = gameObject.transform.position + gameObject.transform.forward * Speed * Time.deltaTime;
        transform.position = new Vector3() { x = transform.position.x, y = 0, z = transform.position.z };
        if (damagecooldown > 0) { damagecooldown -= Time.deltaTime; }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (damagecooldown <= 0)
        {
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
                        if (DestroyOnImpact) {
                            foreach (string s in SpawnOnDeath)
                            {
                                objectPooler.SpawnFromPool(s, transform.position, transform.rotation, transform.localScale);
                            }
                            gameObject.SetActive(false);
                        }
                        damagecooldown = DamageCooldown;
                    }
                }
            }
        }
    }
}
