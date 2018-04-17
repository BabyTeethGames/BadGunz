using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : Projectile {

    Rigidbody rb;
    public float shotForce = 90;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * shotForce);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.relativeVelocity.magnitude > 3)
        {
            Damage(collision.gameObject.GetComponent<Collider>());
        }

    }
}
