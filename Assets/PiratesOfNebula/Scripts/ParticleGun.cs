using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGun : Weapon
{
    ParticleSystem Particle;
    ParticleDamage PD;
    public int ParticleAmount;

    // Start is called before the first frame update
    void Start()
    {
        Particle = GunPoint.GetComponent<ParticleSystem>();
        PD = Particle.gameObject.GetComponent<ParticleDamage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0) { cooldown -= Time.deltaTime; }
    }

    public override void Shooting(GameObject Owner)
    {
        if (PD.ShootBy == null) PD.ShootBy = Owner;
        Particle.Emit(ParticleAmount);
    }
}
