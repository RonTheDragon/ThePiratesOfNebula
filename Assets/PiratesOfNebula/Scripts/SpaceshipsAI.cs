using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipsAI : MonoBehaviour
{
    public float Speed;
    public float RotationSpeed;
    public float DetectionRange;
    public GameObject[] Cannons;
    float RandomMovement;
    int RandomDirection;
    float ScanCooldown;
    Cannon[] CS;
    GameObject Target;
    Hookable hookAble;
    Health hp;

    // Start is called before the first frame update
    void Start()
    {
        CS = new Cannon[Cannons.Length];
        hp = gameObject.GetComponent<Health>();
        hookAble = gameObject.GetComponent<Hookable>();       
    }
        // Update is called once per frame
        void Update()
    {
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

    void Movement()
    {
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
    void Combat()
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
                    CS[count] = C.GetComponent<Cannon>();
                }

                RaycastHit hit;
                if (Physics.Raycast(CS[count].GunPoint.transform.position, CS[count].GunPoint.transform.forward, out hit, Mathf.Infinity))
                {
                    //Debug.DrawRay(C.transform.position, C.transform.forward * hit.distance, Color.yellow);
                    if (hit.transform == Target.transform)
                    {
                        CS[count].Shoot();
                    }
                }
                count++;
            }

            float dist = Vector3.Distance(Target.transform.position, transform.position);
            if (dist > DetectionRange * 4) { Target = null; } //Losing Target
        }
    }
}
