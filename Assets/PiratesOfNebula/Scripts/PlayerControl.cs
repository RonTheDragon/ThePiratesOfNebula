﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject TheHook;
    LineRenderer LR;
    public int hookingStep;
    private bool docked;
    private GameObject TheHooked;
    private float TheHookedRange;
    public GameObject[] Cannons; //0 Front , 1 Back , 2 Right , 3 Left
    public GameObject[] CannonHolders;
    Cannon[] CS;
    bool[] ShootingSide;
    public Camera Cam;
    public Joystick joystick;
    public Transform joystickH;
    public float jsd;
    bool NotMoving;
    private float RotateShip;
    private float RememberRotation;
    private float ClosestRotation;
    public float MovementSpeed;
    float MoveAxl;
    public float RotationSpeed;
    public GameObject[] SwitchJoysickToUndock;
    // Start is called before the first frame update
    void Start()
    {
        CS = new Cannon[Cannons.Length];
        LR = TheHook.GetComponent<LineRenderer>();
        ShootingSide = new bool[Cannons.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
        Movement();
        Combat();
        
    }
    void Movement()
    {
        jsd = Vector3.Distance(joystick.transform.position, joystickH.position); //Joystick Distance
        if (jsd == 0 && docked==false) NotMoving = true;
        else NotMoving = false;
        if (jsd < 50) jsd = 50;

        if (jsd > 100) { MoveAxl += 0.1f * Time.deltaTime; }
        else { MoveAxl -= 0.3f * Time.deltaTime; }
        if (MoveAxl < 0) MoveAxl= 0; else if (MoveAxl > 1) MoveAxl = 1;
        float Speed;
        Speed = MovementSpeed * jsd / 100 * (MoveAxl + 1);
        gameObject.transform.position = gameObject.transform.position + gameObject.transform.up * Speed  * Time.deltaTime;
        Vector3 Camdist = new Vector3(gameObject.transform.position.x, Cam.transform.position.y, gameObject.transform.position.z);
        float dist = Vector3.Distance(Camdist, Cam.transform.position);
        if (dist > 2) { Cam.transform.position = Vector3.MoveTowards(Cam.transform.position, Camdist, Speed * 0.3f* dist * Time.deltaTime); }
        //Cam.transform.rotation = Quaternion.Euler(90,90,Spaceship.transform.rotation.eulerAngles.y-90);

        RotateShip = Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * -180 / Mathf.PI * -1;
        if (RotateShip < 0) { RotateShip += 360; }
        if (RotateShip == 0) RotateShip = RememberRotation;
        RememberRotation = RotateShip;
        
        

        //rotationDistCheck (Math = EVIL)
        float RotationDist1 = RotateShip - gameObject.transform.rotation.eulerAngles.y;
        if (RotationDist1 < 0) RotationDist1 *= -1;
        float RotationDist2 = 360 - gameObject.transform.rotation.eulerAngles.y + RotateShip;
        float RotationDist3 = 360 - RotateShip + gameObject.transform.rotation.eulerAngles.y;
        if (RotationDist3 < RotationDist2) { RotationDist2 = RotationDist3; }

        //Rotating The Ship
        if (RotationDist1 < RotationDist2)
        {
            ClosestRotation = RotationDist1;
            RotateTheShip(true);

        }
        else
        {
            ClosestRotation = RotationDist2;
            RotateTheShip(false);
        }
    }
    void RotateTheShip(bool right)
    {
        float RS = RotationSpeed * (ClosestRotation / 20);
        if (right)
        {
            if (RotateShip > gameObject.transform.rotation.eulerAngles.y) { gameObject.transform.Rotate(RS, 0, 0); }
            else if (RotateShip < gameObject.transform.rotation.eulerAngles.y) { gameObject.transform.Rotate(-RS, 0, 0); }
        }
        else
        {
            if (RotateShip < gameObject.transform.rotation.eulerAngles.y) { gameObject.transform.Rotate(RS, 0, 0); }
            else if (RotateShip > gameObject.transform.rotation.eulerAngles.y) { gameObject.transform.Rotate(-RS, 0, 0); }
        }
    }
    void Combat()
    {
        int count = 0;
        foreach (bool s in ShootingSide) //Attack
        {
            if (s) { Attack(count); }
            count++;
        }

        if (TheHook.activeSelf == true) //Set Rope
        {
            LR.SetPosition(0, transform.position);
            LR.SetPosition(1, TheHook.transform.position);
        }

        if (TheHooked != null)
        {
            if (TheHooked.activeSelf == false) //Target is dead... Stop Hooking
            {
                hookingStep = 0;
                TheHooked = null;
                SwitchJoysickToUndock[0].SetActive(true);
                SwitchJoysickToUndock[1].SetActive(false);
            }
        }

        if (hookingStep == 0) // Hook Back / Not Hooking
        {
            if (TheHook.activeSelf == true)
            {
                TheHook.transform.LookAt(transform.position);
                TheHook.transform.position = Vector3.MoveTowards(TheHook.transform.position, transform.position, 10 * Time.deltaTime);
                float dist = Vector3.Distance(TheHook.transform.position, transform.position);
                if (dist < 1)
                {
                    TheHook.SetActive(false);
                }
            }
        }
        else
        {
            
            if (hookingStep == 1) //hook Forward
            {

                float dist = Vector3.Distance(TheHook.transform.position, TheHooked.transform.position); // Hook Distance 

                TheHook.transform.LookAt(TheHooked.transform.position);
                TheHook.transform.position = Vector3.MoveTowards(TheHook.transform.position, TheHooked.transform.position, 10 * Time.deltaTime);
                if (dist < 1)
                {
                    hookingStep = 2;
                }
            }

            if (hookingStep == 2) //hook Connected
            {

                float dist = Vector3.Distance(transform.position, TheHooked.transform.position); // Enemy Distance 
                TheHook.transform.position = TheHooked.transform.position;  
                
                if (dist < TheHookedRange && !docked && NotMoving)
                {
                    hookingStep = 3; docked = true;
                }
                else if (dist > 10)
                {
                    docked = false;
                }
                if (dist > 10 || NotMoving)
                {
                    float pullingSpeed = 5;
                    if (dist > pullingSpeed) { pullingSpeed = dist; }
                    TheHooked.transform.position = Vector3.MoveTowards(TheHooked.transform.position, gameObject.transform.position, pullingSpeed * 1.5f * Time.deltaTime);
                }
                else 
                {
                    TheHooked.transform.position = Vector3.MoveTowards(TheHooked.transform.position, gameObject.transform.position, dist * Time.deltaTime);
                }


            }

            if (hookingStep == 3) //Docked
            {
                float dist = Vector3.Distance(transform.position, TheHooked.transform.position); // Enemy Distance 

                TheHook.transform.position = TheHooked.transform.position;
                SwitchJoysickToUndock[0].SetActive(false);
                SwitchJoysickToUndock[1].SetActive(true);
                if (dist > TheHookedRange)
                { TheHooked.transform.position = Vector3.MoveTowards(TheHooked.transform.position, gameObject.transform.position, dist * 2 * Time.deltaTime); }
                else
                {
                    TheHooked.transform.position = Vector3.MoveTowards(TheHooked.transform.position, gameObject.transform.position, dist * -0.1f * Time.deltaTime);
                }
            }

        }


    }
    public void StopDocking()
    {
        hookingStep = 2;
        SwitchJoysickToUndock[0].SetActive(true);
        SwitchJoysickToUndock[1].SetActive(false);
    }

    public void PressingFire(int Side)
    {
        ShootingSide[Side] = true;
    }
    public void StopPressingFire(int Side)
    {
        ShootingSide[Side] = false;
    }

    void Attack(int Side) //0 Front , 1 Back , 2 Right , 3 Left
    {
        if (CS[Side] == null)
        {
            CS[Side] = Cannons[Side].GetComponent<Cannon>();
        }
        CS[Side].Shoot();
    }

    public void Hook(GameObject Thehooked, float THR)
    {
        if (hookingStep==0)
        {
            hookingStep = 1;
            TheHook.SetActive(true);
            TheHook.transform.position = gameObject.transform.position;
            TheHooked = Thehooked;
            TheHookedRange = THR;
        }
    }
    void StopHooking()
    {
        hookingStep = 0; TheHooked = null; TheHookedRange = 0;  
    }
    public void ChangeWeapon(GameObject Weapon, int WhichCannon)
    {
        Destroy(Cannons[WhichCannon]);
        Cannons[WhichCannon] = Instantiate(Weapon);
        Cannons[WhichCannon].transform.position = CannonHolders[WhichCannon].transform.position;
        Cannons[WhichCannon].transform.rotation = CannonHolders[WhichCannon].transform.rotation;
        Cannons[WhichCannon].transform.SetParent(CannonHolders[WhichCannon].transform);
    }
}
