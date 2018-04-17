using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamBat : Melee {
    public float swingForce = 25;
    public Transform hitter;

 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Debug.Log("hit");
            if (other.isTrigger)
            {
                Debug.Log("istrigger");
                other.transform.forward = transform.up;
                other.GetComponent<Projectile>().playerShot = playerShot;
                
            }
            if (!other.isTrigger)
            {
                Debug.Log("is not trigger");
                Rigidbody rb = other.GetComponent<Rigidbody>();
                other.GetComponent<Projectile>().playerShot = playerShot;
                rb.AddForce(transform.up * swingForce, ForceMode.VelocityChange);
            }
        }
        if (other.CompareTag("Player"))
        {
            if(other.GetComponent<Player>().m_PlayerNum != playerShot)
            Damage(other);
        }
    }
}
