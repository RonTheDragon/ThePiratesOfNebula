using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHp;
    public float Hp;
    float knockback;
    GameObject Attacker;
    Vector3 V3Knockback;
    Vector3 no;
    public GameObject[] TurnOffWhenDeath;
    // Start is called before the first frame update
    void Start()
    {
        Hp = MaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0) { Death(); }
        if (knockback > 0) { Knockback(); }
    }

    void Knockback()
    {    
        if (Attacker != null)
        {
            if (V3Knockback == no) { V3Knockback = new Vector3(Attacker.transform.position.x, gameObject.transform.position.y, Attacker.transform.position.z); }
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, V3Knockback, -knockback * Time.deltaTime);
        }
        knockback -= Time.deltaTime*knockback;
        knockback -= Time.deltaTime;
    }


    public void Damage(float damage,float kb,GameObject attacker)
    {
        if (Hp > 0)
        {
            Hp -= damage;
            Attacker = attacker;
            if (knockback < kb)
            {
                knockback = kb;
                V3Knockback = no;
            }
        }
    }

    void Death()
    {
        foreach(GameObject g in TurnOffWhenDeath)
        g.SetActive(false);
    }
}
