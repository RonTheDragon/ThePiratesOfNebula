using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : Damager
{
    public bool DeclaredByShooter;
    public bool CantHitSelf;
    public GameObject ShootBy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (damagecooldown > 0) { damagecooldown -= Time.deltaTime; }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (damagecooldown <= 0)
        {
            string HitableTag = null;
            string HitableTag2 = null;

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

            if (other.gameObject.tag == HitableTag || other.gameObject.tag == HitableTag2)
            {
                //Debug.Log("Boom! You Shot: " + collision.gameObject.name);
                if (CantHitSelf == false || other.gameObject != ShootBy)
                {
                    Health hp = other.gameObject.GetComponent<Health>();
                    if (hp != null)
                    {
                        hp.Damage(AttackDamage, Knockback, gameObject,TempetureEffect);
                        damagecooldown = DamageCooldown;
                    }
                }
            }
        }
    }
}
