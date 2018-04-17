using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereShoot : Projectile {
    Rigidbody rb;
    public float shotForce = 1;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * shotForce *Random.Range(0.75f, 1.5f));
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Damage(other);
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Environment"))
        {
            Destroy(this.gameObject);
        }
    }

}
