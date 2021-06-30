using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasiteShoot : Shoot ,IpooledObject
{
    GameObject Target;
    public float ScanRange = 20;
    public float LifeTime = 5;
    string TargetableTag;
    string TargetableTag2;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();   
        {
            if (Target != null) { transform.LookAt(Target.transform.position);
                if (Vector3.Distance(Target.transform.position, transform.position)<1) { Target = null; }
            }
        }
    }
    IEnumerator Scan()
    {
        while (true)
        { 
            float SmallestDist = ScanRange + 200;
            GameObject PossibleTarget = null;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, ScanRange);
            foreach (Collider c in hitColliders)
            {

                if (c.gameObject.tag == TargetableTag || c.gameObject.tag == TargetableTag2)
                {
                    if (CantHitSelf == false || c.gameObject != ShootBy)
                    {

                        float distanceFromTarget = Vector3.Distance(c.transform.position, transform.position);
                        if (SmallestDist > distanceFromTarget)
                        {
                            SmallestDist = distanceFromTarget;
                            PossibleTarget = c.gameObject;
                        }
                    }
                }
            }
            Target = PossibleTarget;
            yield return new WaitForSeconds(0.3f);
        }
    }
    IEnumerator DieOfAge()
    {
        yield return new WaitForSeconds(LifeTime);
        gameObject.SetActive(false);
    }
    IEnumerator SetUpScanning()
    {
        yield return new WaitForSeconds(0.1f);
        TargetableTag = null;
        TargetableTag2 = null;

        if (DeclaredByShooter)
        {
            if (ShootBy.gameObject.tag == "Player")
            {
                CantHitEnemy = false;
                CantHitPlayer = true;
            }
            else if (ShootBy.gameObject.tag == "Enemy")
            {
                CantHitEnemy = true;
                CantHitPlayer = false;
            }
        }

        if (!CantHitPlayer)
        {
            TargetableTag = "Player";
        }
        if (!CantHitEnemy)
        {
            TargetableTag2 = "Enemy";

        }
    }

    public void OnObjectSpawn()
    {
        StartCoroutine(DieOfAge());
        StartCoroutine(SetUpScanning());
        StartCoroutine(Scan());

    }
}
