using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookable : MonoBehaviour
{
    public int MinStartMoney;
    public int MaxStartMoney;
    public int Money;
    public float HookableHealth;
    public float HookingRange;
    public GameObject Player;
    PlayerControl Pc;
    public bool hookable;
    Health hp;
    public GameObject Canvas;
    public GameObject[] Buttons;
    // Start is called before the first frame update
    void Start()
    {
        hp = gameObject.GetComponent<Health>(); 
    }
    
    // Update is called once per frame
    void Update() 
    {
        if (Pc == null)
        {
            Canvas.SetActive(false);
            if (Player != null)
            {
                
                Pc = Player.GetComponent<PlayerControl>();
            }
        }
        if (Pc != null)
        {
            if (hp.Hp < 0 || Pc.hookingStep == 3||Pc.TheHooked!=gameObject && Pc.hookingStep>0||hp.Hp==hp.MaxHp) //if Dead/docked/hooking step while not being hooked/full hp
            {
                Canvas.SetActive(false);
               // Buttons[0].SetActive(false);
              //  Buttons[1].SetActive(false);
            } 
            else
            {
                Canvas.SetActive(true);
            }
        }
        
        Canvas.transform.position = new Vector3(transform.position.x, Canvas.transform.position.y, transform.position.z);

        if (!hookable) // if not Hooked Yet
        {
            if (HookableHealth >= hp.Hp && Pc !=null)
            {
                StartCoroutine(SpawnHookable());
                         
            }
        }
    }
    public void Hooked()
    {
        ShipCollisionDamage SC1 = GetComponent<ShipCollisionDamage>();
        ShipCollisionDamage SC2 = Pc.gameObject.GetComponent<ShipCollisionDamage>();
        if (SC1 != null && SC2 != null)
        {
            SC1.DontDamageIt = Pc.gameObject;
            SC2.DontDamageIt = gameObject;
        }
        Pc.Hook(gameObject,HookingRange);
        Buttons[0].SetActive(false);
        Buttons[1].SetActive(true);
    }
    public void UnHooked()
    {
        ShipCollisionDamage SC1 = GetComponent<ShipCollisionDamage>();
        ShipCollisionDamage SC2 = Pc.gameObject.GetComponent<ShipCollisionDamage>();
        if (SC1 != null && SC2 != null)
        {
            SC1.DontDamageIt = null;
            SC2.DontDamageIt = null;
        }
        Pc.hookingStep = 0;
        Buttons[1].SetActive(false);
        Buttons[0].SetActive(true);
    }
    IEnumerator SpawnHookable()
    {
        yield return new WaitForSeconds(0.5f);
        hookable = true;
        Buttons[0].SetActive(true);
    }
    
}
