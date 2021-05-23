﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedMechanic : MonoBehaviour, IpooledObject
{
    public GameObject SummonedByThePlayer;
    public GameObject DistantBody;
    public Health health;
    public Hookable hookable;
    public SpaceshipsAI spaceshipsAI;

    public void OnObjectSpawn() //When Spawned
    {
        if (DistantBody != null)
        {
            DistantBody.transform.position = new Vector3(transform.position.x, DistantBody.transform.position.y, transform.position.z);
        }
        //Debug.Log("works");
        if (health != null)
        {
            health.Hp = health.MaxHp;
            //Debug.Log(health.gameObject.name + " " +health.Hp);
            foreach(GameObject g in health.TurnOffWhenDeath)
            {
                g.SetActive(true);
            }
        }
        if (hookable != null)
        {
            hookable.Money = hookable.StartMoney;
            hookable.hookable = false;
        }
        if (spaceshipsAI != null)
        {
            spaceshipsAI.SwitchWeapons();
        }      

    }
    

    // Update is called once per frame
    void Update()
    {
        if (DistantBody != null && SummonedByThePlayer != null )
        {
            float dist = Vector3.Distance(DistantBody.transform.position, SummonedByThePlayer.transform.position);
            if (dist > 100) { gameObject.SetActive(false); }
        }
    }
   
}
