using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookable : MonoBehaviour
{
    public float HookableHealth;
    public float HookingRange;
    public GameObject Player;
    PlayerControl Pc;
    public bool hookable;
    Health hp;
    public GameObject Canvas;
    // Start is called before the first frame update
    void Start()
    {
        hp = gameObject.GetComponent<Health>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null && Pc == null)
        {
            Pc = Player.GetComponent<PlayerControl>();
        }

        if (hp.Hp < 0) { Canvas.SetActive(false); }
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
        Pc.Hook(gameObject,HookingRange);
    }
    IEnumerator SpawnHookable()
    {
        yield return new WaitForSeconds(1);
        hookable = true;
        Canvas.SetActive(true);
    }
    
}
