using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookable : MonoBehaviour
{
    public float HookableHealth;
    public float HookingRange;
    public GameObject Player;
    PlayerControl Pc;
    bool hookable;
    Health hp;
    public GameObject Button;
    // Start is called before the first frame update
    void Start()
    {
        hp = gameObject.GetComponent<Health>();
        Pc = Player.GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
     if (!hookable)
        {
            if (HookableHealth >= hp.Hp)
            {
                hookable = true;
                Button.SetActive(true);
            }
        }
    }
    public void Hooked()
    {
        Pc.Hook(gameObject,HookingRange);
    }
    
}
