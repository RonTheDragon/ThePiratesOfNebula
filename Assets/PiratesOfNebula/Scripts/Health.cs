using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IpooledObject
{
    public float MaxHp;
    public float Hp;
    public Image Healthbar;
    float knockback;
    GameObject Attacker;
    Vector3 V3Knockback;
    Vector3 no;
    public GameObject[] TurnOffWhenDeath;
    SpaceshipsAI SAI;
    // Start is called before the first frame update
    void IpooledObject.OnObjectSpawn()
    {
        Hp = MaxHp;
    }
    void Start()
    {    
        SAI = gameObject.GetComponent<SpaceshipsAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0) { Death(); }

        if (knockback > 0) { Knockback(); }

        if (Healthbar != null) {
            if (Hp>MaxHp/2)
            Healthbar.color = Color.Lerp(Color.yellow, Color.green, Hp/MaxHp*2-1);
            else
            Healthbar.color = Color.Lerp(Color.red, Color.yellow, Hp/MaxHp*2);

            Healthbar.fillAmount = Hp / MaxHp;
        }
        
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
            SAI?.Scan(100);
        }
    }

    void Death()
    {
        foreach(GameObject g in TurnOffWhenDeath)
        g.SetActive(false);
    }

    
}
