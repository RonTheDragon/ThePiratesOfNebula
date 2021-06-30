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
    public bool NoTempeture;
   [HideInInspector]
    public float Tempeture;
   [HideInInspector]
    public bool Burn;
    [HideInInspector]
    public bool Froze;
    SpaceshipsAI SAI;
    ObjectPooler objectPooler;
    GameObject OnFire;
    GameObject IceCube;
    
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

        if (!NoTempeture)
        {
            if (Tempeture > 0)
            {
                Froze = false; Tempeture -= 20 * Time.deltaTime; IceCube?.SetActive(false);
                if (Tempeture > 100) { Tempeture = 100; if (!Burn && Hp > 0) { OnFire = objectPooler.SpawnFromPool("OnFire", transform.position, Quaternion.Euler(new Vector3()), transform.localScale, true); Burn = true; } }
            }
            else if (Tempeture < 0)
            {
                Burn = false; Tempeture += 20 * Time.deltaTime; OnFire?.SetActive(false); 
                if (Tempeture < -100) { Tempeture = -100; if (!Froze&&Hp>0) { IceCube = objectPooler.SpawnFromPool("IceCube", transform.position, transform.rotation, transform.localScale, true); Froze = true; } } }

            else { Burn = false; Froze = false; OnFire?.SetActive(false); IceCube?.SetActive(false); }

            if (Burn)
            {
                OnFire.transform.position = transform.position;
                Hp -= 5*Time.deltaTime;
            }
            if (Froze)
            {
                IceCube.transform.position = transform.position;
                IceCube.transform.rotation = transform.rotation;
            }
        }

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


    public void Damage(float damage, float kb, GameObject attacker ,float TempChange = 0)
    {
        if (Hp > 0)
        {
            Hp -= damage;
            if (TempChange != 0) {if(TempChange>0||!Froze) Tempeture += TempChange; }
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
        OnFire?.SetActive(false);
        IceCube?.SetActive(false);
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
        Healthbar.color = Color.white;
        yield return new WaitForSeconds(0.02f);
        DamageIndicator.SetActive(false);
        Healthbar.color = Color.white;
        yield return new WaitForSeconds(0.02f);
        DamageIndicator.SetActive(true);
        Healthbar.color = Color.white;
        yield return new WaitForSeconds(0.02f);
        DamageIndicator.SetActive(false);
        Healthbar.color = Color.white;
        yield return new WaitForSeconds(0.02f);
        DamageIndicator.SetActive(true);
        Healthbar.color = Color.white;
        yield return new WaitForSeconds(0.02f);
        DamageIndicator.SetActive(false);
        Healthbar.color = Color.white;
    }
}
