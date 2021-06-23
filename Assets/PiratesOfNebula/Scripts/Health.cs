using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float MaxHp;
    public float Hp;
    public float HpRegan;
    public Image Healthbar;
    float knockback;
    GameObject Attacker;
    Vector3 V3Knockback;
    Vector3 no = new Vector3();
    public string[] SpawnOnDeath;
    public GameObject[] TurnOffWhenDeath;
    public GameObject DamageIndicator;
    SpaceshipsAI SAI;
    ObjectPooler objectPooler;
    // Start is called before the first frame update

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        Spawn();
        SAI = gameObject.GetComponent<SpaceshipsAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0) { Death(); }
        else if (Hp > MaxHp) { Hp = MaxHp; }
        else if (HpRegan > 0 && Hp < MaxHp) { Hp += HpRegan * Time.deltaTime; }


        if (knockback > 0) { Knockback(); }

        if (Healthbar != null)
        {
            if (Hp > MaxHp / 2)
                Healthbar.color = Color.Lerp(Color.yellow, Color.green, Hp / MaxHp * 2 - 1);
            else
                Healthbar.color = Color.Lerp(Color.red, Color.yellow, Hp / MaxHp * 2);

            Healthbar.fillAmount = Hp / MaxHp;
        }

    }

    public void Spawn()
    {
        Hp = MaxHp;
    }

    void Knockback()
    {
        if (Attacker != null)
        {
            if (V3Knockback == no) { V3Knockback = new Vector3(Attacker.transform.position.x, gameObject.transform.position.y, Attacker.transform.position.z); }
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, V3Knockback, -knockback * Time.deltaTime);
        }
        knockback -= Time.deltaTime * knockback;
        knockback -= Time.deltaTime;
    }


    public void Damage(float damage, float kb, GameObject attacker)
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
            if (DamageIndicator!=null) StartCoroutine(OuchInducator());
            
        }
    }

    void Death()
    {
        foreach (string s in SpawnOnDeath)
        {
            objectPooler.SpawnFromPool(s, transform.position, transform.rotation, transform.localScale);
        }
        foreach (GameObject g in TurnOffWhenDeath)
            g.SetActive(false);
    }

    IEnumerator OuchInducator()
    {
        GetComponent<AudioManager>()?.PlaySound(Sound.Activation.Custom, "Ouch");
        DamageIndicator.SetActive(true);
        yield return new WaitForSeconds(0.02f);
        DamageIndicator.SetActive(false);
        yield return new WaitForSeconds(0.02f);
        DamageIndicator.SetActive(true);
        yield return new WaitForSeconds(0.02f);
        DamageIndicator.SetActive(false);
        yield return new WaitForSeconds(0.02f);
        DamageIndicator.SetActive(true);
        yield return new WaitForSeconds(0.02f);
        DamageIndicator.SetActive(false);
    }
}
