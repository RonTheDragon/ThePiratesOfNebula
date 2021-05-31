using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipsAI : SpaceShips
{
    public float Speed;
    public float RotationSpeed;
    public float DetectionRange;
    public GameObject[] Weapons;
  //  public GameObject[] Cannons;
    float RandomMovement;
    int RandomDirection;
    float ScanCooldown;
  //  Cannon[] CS;
    public GameObject Target;
    Hookable hookAble;
    Health hp;

    // Start is called before the first frame update
    void Start()
    {
        CS = new Weapon[Cannons.Length];
        hp = gameObject.GetComponent<Health>();
        hookAble = gameObject.GetComponent<Hookable>();       
    }
        // Update is called once per frame
        new void Update()
    {
        base.Update();
        if (!hookAble.hookable)
        {
            Movement();
            Combat();
        }
    }
    public void Scan(float Range)
    {
        if (Target == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, Range);
            foreach (Collider hit in colliders)
            {
                if (hit.transform.tag == "Player") { Target = hit.gameObject; hookAble.Player = Target; break; }
            }
        }
    }
    void CheckForDodge()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.right * -1.5f, transform.forward, out hit, 20))
        {
            //Debug.DrawRay(transform.position + transform.right * -1.5f, transform.forward * hit.distance, Color.yellow);
            Dodge(hit, 40);
        }
        else
        {
            RaycastHit hit2;
            if (Physics.Raycast(transform.position + transform.right * 1.5f, transform.forward, out hit2, 20))
            {
                // Debug.DrawRay(transform.position + transform.right * 1.5f, transform.forward * hit2.distance, Color.yellow);
                Dodge(hit2, -40);
            }
        }
    }
    void Dodge(RaycastHit hit,float turn)
    {
        if (Target == null)
        {
            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < 20) { transform.Rotate(0, RotationSpeed * turn * Time.deltaTime, 0); }
        }
        else if (hit.transform != Target.transform)
        {
            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < 20) { transform.Rotate(0, RotationSpeed * turn * Time.deltaTime, 0); }
        }
    }

    protected override void Movement()
    {
        CheckForDodge();

        gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * Speed * Time.deltaTime; //Go Forward

        if (Target != null) //Rotate Towards Target
        {
            var targetRotation = Quaternion.LookRotation(Target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }
        else //Rotate Randomly
        {
            if (RandomMovement > 0) { RandomMovement -= Time.deltaTime; }
            else
            {
                RandomMovement = Random.Range(1, 3);
                RandomDirection = Random.Range(0, 4);
            }
            if (RandomDirection ==2) { transform.Rotate(0, RotationSpeed * 10 * Time.deltaTime, 0);  }
            else if (RandomDirection == 3) { transform.Rotate(0, -RotationSpeed *10* Time.deltaTime, 0); }
        }
    }
    protected override void Combat()
    {
        if (Target == null) //Scanning For Target
        {
            if (ScanCooldown > 0) { ScanCooldown -= Time.deltaTime; }
            else
            {
                ScanCooldown = 1;
                Scan(DetectionRange);
            }
        }
        else //Attacking Target
        {
            int count = 0;
            foreach (GameObject C in Cannons)
            {
                if (CS[count] == null)
                {
                    CS[count] = C.GetComponent<Weapon>();
                    
                }

                RaycastHit hit;
                if (Physics.Raycast(CS[count].GunPoint.transform.position, CS[count].GunPoint.transform.forward, out hit, Mathf.Infinity))
                {
                    //Debug.DrawRay(C.transform.position, C.transform.forward * hit.distance, Color.yellow);
                    if (hit.transform == Target.transform && OverHeated==false)
                    {
                        CS[count].Shoot(gameObject);       
                    }
                }
                count++;
            }

            float dist = Vector3.Distance(Target.transform.position, transform.position);
            if (dist > DetectionRange * 4) { Target = null; } //Losing Target
        }
    }

    public void SwitchWeapons()
    {
        int n = 0;
        foreach(GameObject c in Cannons)
        {
            ChangeWeapon(Weapons[Random.Range(0, Weapons.Length)], n);
            n++;
        }
    }
   
}
