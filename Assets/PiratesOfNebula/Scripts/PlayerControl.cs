using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : SpaceShips
{
    public GameObject TutorialGO;
    public GameObject TheHook;
    LineRenderer LR;
    Currency currency;
    Health health;
    float MilkingTime;
    Hookable pudge;
    public int hookingStep;
    private bool docked;
    public GameObject TheHooked;
    private float TheHookedRange;
   // public GameObject[] Cannons; //0 Front , 1 Back , 2 Right , 3 Left
   // public GameObject[] CannonHolders;
   // Cannon[] CS;
    bool[] ShootingSide;
    public Camera Cam;
    public Joystick joystick;
    public Transform joystickH;
    public Transform JoystickLimit;
    public float jsd;
    public float Maxjsd;
    bool NotMoving;
    private float RotateShip;
    private float RememberRotation;
    private float ClosestRotation;
    public float MovementSpeed;
    float MoveAxl;
    public float RotationSpeed;
    public GameObject[] SwitchJoysickToUndock;
    public float CameraDistFromShip;

    // Start is called before the first frame update
    void Start()
    {
        CS = new Weapon[Cannons.Length];
        LR = TheHook.GetComponent<LineRenderer>();
        ShootingSide = new bool[Cannons.Length];
        currency = transform.parent.GetComponent<Currency>();
        health = GetComponent<Health>();
        for (int i = 0; i < Cannons.Length; i++)
        {
            ChangeWeapon(Cannons[i],i);
        }
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        Movement();
        Combat();
        HookingMechanic();

    }
    protected override void Movement()
    {
        jsd = Vector3.Distance(joystick.transform.position, joystickH.position); //Joystick Distance
        Maxjsd = Vector3.Distance(joystick.transform.position, JoystickLimit.position);
        jsd = jsd / Maxjsd;
        if (jsd == 0 && docked == false) NotMoving = true;
        else
        {
            NotMoving = false;
            if (TutorialGO.GetComponent<Tutorial>().TutorialMain.activeInHierarchy)
            {
                TutorialGO.GetComponent<Tutorial>().ThirdTutorial();
            }
      
        }
        if (jsd < 0.4f) jsd = 0.4f;

        if (jsd > 0.9f) { MoveAxl += 0.1f * Time.deltaTime; }
        else { MoveAxl -= 0.3f * Time.deltaTime; }
        if (MoveAxl < 0) MoveAxl= 0; else if (MoveAxl > 1) MoveAxl = 1;
        float Speed;
        Speed = MovementSpeed * jsd  * (MoveAxl + 1);
        gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * Speed  * Time.deltaTime;
        Vector3 Camdist = new Vector3(gameObject.transform.position.x, Cam.transform.position.y, gameObject.transform.position.z- CameraDistFromShip); //THIS IS THE CAM LOCATION!!!!!
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
            if (RotateShip > gameObject.transform.rotation.eulerAngles.y) { gameObject.transform.Rotate(0, RS, 0); }
            else if (RotateShip < gameObject.transform.rotation.eulerAngles.y) { gameObject.transform.Rotate(0, -RS, 0); }
        }
        else
        {
            if (RotateShip < gameObject.transform.rotation.eulerAngles.y) { gameObject.transform.Rotate(0, RS, 0); }
            else if (RotateShip > gameObject.transform.rotation.eulerAngles.y) { gameObject.transform.Rotate(0, -RS, 0); }
        }
    }
    protected override void Combat()
    {
        int count = 0;
        foreach (bool s in ShootingSide) //Attack
        {
            if (s) { Attack(count); }
            count++;
        }
    }

    public void HookingMechanic()
    {

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
                TheHook.transform.rotation = Quaternion.Euler(new Vector3(){ x=TheHook.transform.rotation.x,y=TheHook.transform.rotation.y+180,z=TheHook.transform.rotation.z});
                
                TheHook.transform.position = Vector3.MoveTowards(TheHook.transform.position, transform.position, 20 * Time.deltaTime); //hook goes back to ship
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
                if (dist > 30) { StopHooking(); }
            }

            if (hookingStep == 2) //hook Connected
            {

                float dist = Vector3.Distance(transform.position, TheHooked.transform.position); // Enemy Distance 
                TheHook.transform.position = TheHooked.transform.position;

               

                if (dist < TheHookedRange && !docked && NotMoving)
                {
                    hookingStep = 3; docked = true; MilkingTime = 1;
                }
                else if (dist > 10)
                {
                    docked = false;
                }
                if (dist > 10 || NotMoving)
                {
                    float pullingSpeed = 5;
                    if (dist > pullingSpeed) { pullingSpeed = dist; }
                    TheHooked.transform.position = Vector3.MoveTowards(TheHooked.transform.position, gameObject.transform.position, pullingSpeed * 2.5f * Time.deltaTime);
                }
                else
                {
                    TheHooked.transform.position = Vector3.MoveTowards(TheHooked.transform.position, gameObject.transform.position, dist * Time.deltaTime);
                }

                if (dist > 30) { StopHooking(); }
            }

            if (hookingStep == 3) //Docked
            {
                if (pudge == null)
                {
                    pudge = TheHooked.GetComponent<Hookable>();
                }
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
                if (MilkingTime <= 0 && dist<5)
                {
                    MilkingTime = 1;
                    if (pudge != null)
                    {
                        
                        if (pudge.Money > 10)
                        {   int r = Random.Range(2, 5);
                            currency.PopupMoney(TheHooked, pudge.Money / r);
                            currency.Money += pudge.Money / r;
                            pudge.Money -= pudge.Money / r;  
                        }
                        else if (pudge.Money > 0)
                        {
                            currency.PopupMoney(TheHooked, pudge.Money);
                            currency.Money += pudge.Money;
                            pudge.Money = 0;
                        }
                        
                    }
                }
                else { MilkingTime -= Time.deltaTime; }

                if (dist > 20) { StopHooking(); }
            }

        }
    }


    public void StopDocking()
    {
        hookingStep = 2;
        SwitchJoysickToUndock[0].SetActive(true);
        SwitchJoysickToUndock[1].SetActive(false);
        if (pudge != null)
        {
            pudge.Buttons[1].SetActive(true);
            pudge = null;
        }
    }

    public void PressingFire(int Side)
    {
        ShootingSide[Side] = true;
        if (TutorialGO.GetComponent<Tutorial>().TutorialMain.activeInHierarchy)
        {
            TutorialGO.GetComponent<Tutorial>().FourthTutorial();
        }
    }
    public void StopPressingFire(int Side)
    {
        ShootingSide[Side] = false;
    }

    void Attack(int Side) //0 Front , 1 Back , 2 Right , 3 Left
    {
        if (CS[Side] == null)
        {
            CS[Side] = Cannons[Side].GetComponent<Weapon>();
            
        }
        if (OverHeated == false)
        CS[Side].Shoot(gameObject);
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
        TheHook.GetComponent<Hookable>()?.UnHooked();
        hookingStep = 0; TheHooked = null; TheHookedRange = 0;  
    }
    
}
