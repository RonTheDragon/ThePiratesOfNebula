using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject Spaceship;
    public Camera Cam;
    public Joystick joystick;
    public float RotateShip;
    private float RememberRotation;
    private float ClosestRotation;
    public float MovementSpeed;
    public float RotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        
    }
    void Movement()
    {
        Spaceship.transform.position = Spaceship.transform.position + Spaceship.transform.up * MovementSpeed * Time.deltaTime;
        Vector3 Camdist = new Vector3(Spaceship.transform.position.x, Cam.transform.position.y, Spaceship.transform.position.z);
        float dist = Vector3.Distance(Camdist, Cam.transform.position);
        if (dist > 2) { Cam.transform.position = Vector3.MoveTowards(Cam.transform.position, Camdist, MovementSpeed * Time.deltaTime); }

        RotateShip = Mathf.Atan2(joystick.Horizontal, joystick.Vertical) * -180 / Mathf.PI * -1;
        if (RotateShip < 0) { RotateShip *= -1; RotateShip -= 180; RotateShip *= -1; RotateShip += 180; }
        if (RotateShip == 0) RotateShip = RememberRotation;
        RememberRotation = RotateShip;
        
        

        //rotationDistCheck (Math = EVIL)
        float RotationDist1 = RotateShip - Spaceship.transform.rotation.eulerAngles.y;
        if (RotationDist1 < 0) RotationDist1 *= -1;
        float RotationDist2 = 360 - Spaceship.transform.rotation.eulerAngles.y + RotateShip;
        float RotationDist3 = 360 - RotateShip + Spaceship.transform.rotation.eulerAngles.y;
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
        float RS = RotationSpeed * (ClosestRotation/20);
        if (right)
        {
            if (RotateShip > Spaceship.transform.rotation.eulerAngles.y) { Spaceship.transform.Rotate(RS, 0, 0); }
            else if (RotateShip < Spaceship.transform.rotation.eulerAngles.y) { Spaceship.transform.Rotate(-RS, 0, 0); }
        }
        else
        {
            if (RotateShip < Spaceship.transform.rotation.eulerAngles.y) { Spaceship.transform.Rotate(RS, 0, 0); }
            else if (RotateShip > Spaceship.transform.rotation.eulerAngles.y) { Spaceship.transform.Rotate(-RS, 0, 0); }
        }
    }
}
