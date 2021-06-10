using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Damager
{
    public float MovementSpeed;
    public float RotationSpeed;
    public float MaxScale;
    public float MinScale;
    public float Speed;
    public Vector3 Rotation;
    public bool DestroyOnImpact;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = gameObject.transform.position + gameObject.transform.forward * Speed * Time.deltaTime;
        transform.position = new Vector3() { x = transform.position.x, y = 0, z = transform.position.z };
        transform.GetChild(0).Rotate(Rotation*Time.deltaTime);
        if (damagecooldown > 0) { damagecooldown -= Time.deltaTime; }
    }
    private void OnCollisionEnter(Collision collision)
    {
        string HitableTag = null;
        string HitableTag2 = null;
        if (damagecooldown <= 0)
        {
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
                    hp.Damage(AttackDamage+Speed+transform.localScale.x, Knockback + Speed + transform.localScale.x, gameObject);
                    if (DestroyOnImpact) {
                        Health h = GetComponent<Health>();
                        if (h != null)
                        {
                            h.Hp = 0;
                        }
                        else gameObject.SetActive(false);
                             }
                    damagecooldown = DamageCooldown;
                }

            }
        }
    }
}
