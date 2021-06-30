using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShoot : Shoot, IpooledObject
{
    public float TimerReset = 0.5f;
    public float SpinSpeed = 360;
    float NowSpeed;
    float HalfSpeed;
    float NowSpin;
    float HalfSpin;
    float Timer;
    int Direction = 1;
    bool FirstSpin = true;

    public void OnObjectSpawn()
    {
        FirstSpin = true;
        Timer = TimerReset * 0.5f;
        Direction = Random.Range(0, 2);
        if (Direction == 0) Direction = -1;
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        NowSpeed = Speed;
        HalfSpeed = Speed / 4;
       // NowSpin = SpinSpeed;
       // HalfSpin = SpinSpeed* 4;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        Timer -= Time.deltaTime;
        transform.Rotate(0, SpinSpeed * Time.deltaTime * Direction, 0);
        if (FirstSpin)
        {
            Speed = NowSpeed;
           // SpinSpeed = NowSpin;
        }
        else
        {
            Speed = HalfSpeed;
           // SpinSpeed = HalfSpin;
        }
        if (Timer <= 0 ) { if (FirstSpin) { FirstSpin = false; Timer = TimerReset; } else { FirstSpin = true; Timer = TimerReset; } } 
    }
}
