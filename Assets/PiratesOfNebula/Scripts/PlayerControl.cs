using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject TheHook;
    private bool hooking;
    private bool catched;
    private GameObject TheHooked;
    private float TheHookedRange;
    public GameObject[] Cannons; //0 Front , 1 Back , 2 Right , 3 Left
    Cannon[] CS;
    bool[] ShootingSide;
    public Camera Cam;
    public Joystick joystick;
    public Transform joystickH;
    public float jsd;
    private float RotateShip;
    private float RememberRotation;
    private float ClosestRotation;
    public float MovementSpeed;
    float MoveAxl;
    public float RotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        CS = new Cannon[Cannons.Length];
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
        foreach(bool s in ShootingSide)
        {
            if (s) { Attack(count); }
            count++;
        }


        if (hooking)
        {
            if (TheHook.activeSelf == false || TheHooked.activeSelf == false || gameObject.activeSelf == false) { StopHooking(); }
            else
            {
                TheHook.transform.LookAt(TheHooked.transform.position);
                TheHook.transform.position = Vector3.MoveTowards(TheHook.transform.position, TheHooked.transform.position, 10 * Time.deltaTime);
                float dist = Vector3.Distance(TheHook.transform.position, TheHooked.transform.position);
                if (dist < 1)
                {
                    catched = true;
                }

                if (catched)
                {
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, TheHooked.transform.position, 5 * Time.deltaTime);
                    TheHooked.transform.position = Vector3.MoveTowards(TheHooked.transform.position, gameObject.transform.position, 5 * Time.deltaTime);
                    float dist2 = Vector3.Distance(gameObject.transform.position, TheHooked.transform.position);
                    if (dist2 > 30)
                    {
                        StopHooking();
                    }
                    else if (dist2 < TheHookedRange)
                    {
                        StopHooking();
                        //Boarding The Ship
                    }
                }
            }
        }

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
        if (!hooking)
        {
            hooking = true;
            catched = false;
            TheHook.SetActive(true);
            TheHook.transform.position = gameObject.transform.position;
            TheHooked = Thehooked;
            TheHookedRange = THR;
        }
    }
    void StopHooking()
    {
        catched = false; hooking = false; TheHook.SetActive(false); TheHooked = null; TheHookedRange = 0;
    }
}
