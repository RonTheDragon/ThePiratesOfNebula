using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour , IpooledObject
{
    public float Speed;
    public float AttackDamage;
    public float DamageCooldown;
    public float Knockback;
    float damagecooldown;
    public bool DestroyOnImpact;
    public bool CantHitPlayer;
    public bool CantHitEnemy;
    // Start is called before the first frame update
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
                Health hp = collision.gameObject.GetComponent<Health>();
                if (hp != null) { hp.Damage(AttackDamage,Knockback,gameObject); if (DestroyOnImpact) gameObject.SetActive(false); }
            }
        }
    }
}
