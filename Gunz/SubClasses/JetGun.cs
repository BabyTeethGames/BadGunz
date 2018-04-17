using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Effects;
using UnityEngine;

public class JetGun : Gun
{
    public Transform shotSpawn;
    public ParticleSystem jetParticle;
    public AfterburnerPhysicsForce afterBurner;
    public Transform jetCenter;
    public float thrustForce = 3f;
    public float initialThrust = 10f;
    public float effectRadius;
    public int damage = 1;
    private Vector3 thrustDir;
    private Collider[] colliders;
   
    public override void Start()
    {
        
        base.Start();
    }

    public override void Shoot()
    {

        thrustDir += -transform.forward * thrustForce * Time.deltaTime;
        //you can add a magnitude clamp on thrust now to cap its speed
        
        controller.additiveMove += thrustDir;
        JetDamage();          
    }

    public override void Update()
    {
        if (Input.GetButtonUp(controller.Inputs[5]) || reloading)
        {
            jetParticle.Stop();
            afterBurner.enabled = false;
            sound.Stop();
            controller.m_Flying = false;
            thrustDir = Vector3.zero;
        }

        if (Input.GetButtonDown(controller.Inputs[5]))
        {
            controller.m_Flying = true;
            thrustDir += -transform.forward * initialThrust;
            jetParticle.Play();
            afterBurner.enabled = true;
            sound.Play(); 
        }

        base.Update();
    
    }

    private void JetDamage()
    {    
        colliders = Physics.OverlapSphere(jetCenter.position, effectRadius);
        for (int n = 0; n < colliders.Length; ++n)
        {
            if (colliders[n].CompareTag("Player"))
                if (colliders[n].GetComponent<Player>() !=null)
                {
                    if (playerShot != colliders[n].GetComponent<Player>().m_PlayerNum)
                    {
                        colliders[n].GetComponent<Player>().Damaged(damage, playerShot);
                    }
                }
                else if (colliders[n].GetComponent<AIPlayerWalk>() != null)
                {
                    colliders[n].GetComponent<AIPlayerWalk>().Damaged(damage, playerShot);
                }
        }
    }
}
