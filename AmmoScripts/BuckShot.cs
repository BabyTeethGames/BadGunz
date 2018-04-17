using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuckShot : Projectile {

    Rigidbody rb;
    public float shotForce = 2000;
    public float lifeTime = 20;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(this.gameObject, lifeTime);
        rb.AddForce(transform.forward * shotForce);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.relativeVelocity.magnitude > 1)
        {
            Damage(collision.gameObject.GetComponent<Collider>());
        }
    }
}
