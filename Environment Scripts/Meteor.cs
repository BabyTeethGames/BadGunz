using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Projectile {

    public float multiplier = 1;
    public float explosionForce = 1;
    public float startForce = 10;
    public float angle = 0;
    public float particles = 10;
    Vector3 oppositeCollision;
    Quaternion randomAngle; 
    public GameObject meteorParticle;
    public ParticleSystem dust;

    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * startForce, ForceMode.Impulse);
	}
	

    private void OnCollisionEnter(Collision collision)
    {
        
        oppositeCollision = collision.contacts[0].point - transform.position;
        oppositeCollision = -oppositeCollision.normalized;
        Instantiate(dust, transform.position, Quaternion.LookRotation(oppositeCollision));
        float r = 10 * multiplier;
        var cols = Physics.OverlapSphere(transform.position, r);
        var rigidbodies = new List<Rigidbody>();
        foreach (var col in cols)
        {
            if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
            {
                rigidbodies.Add(col.attachedRigidbody);
            }      
        }
        foreach (var rb in rigidbodies)
        {
            rb.AddExplosionForce(explosionForce * multiplier, transform.position, r, 1 * multiplier, ForceMode.Impulse);
        }

        if (collision.gameObject.CompareTag("Player") && collision.relativeVelocity.magnitude > 3)
        {
            Damage(collision.gameObject.GetComponent<Collider>());
        }
        
        for (int i = 0; i < particles; i++)
        {
            ShootMeteor();
        }

        Destroy(this.gameObject);
        //Change this to object pooling eventually this.gameObject.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Environment"))
        {
            Destroy(this.gameObject);
        }
    }

    void ShootMeteor()
    {
        randomAngle = Quaternion.Euler(RandomVector(angle));
        Instantiate(meteorParticle, transform.position, Quaternion.LookRotation(oppositeCollision) * randomAngle);
    }

    private Vector3 RandomVector(float x)
    {
        return new Vector3(Random.Range(-x, x), Random.Range(-x, x), Random.Range(-x, x));
    }

}
