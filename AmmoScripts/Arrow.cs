using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile {

    private Rigidbody rb;
    private bool m_Flying = true;
    CapsuleCollider arrowCol;
	void Start () {
        rb = GetComponent<Rigidbody>();
        arrowCol = GetComponent<CapsuleCollider>();
	}
	
	void FixedUpdate () {
		if(m_Flying)
        {
            transform.LookAt(transform.position + rb.velocity);
        }
	}


    private void OnCollisionEnter(Collision collision)
    {
        //Makes sure the arrow head is facing the direction it's flying
        if (m_Flying == true)
        {
            m_Flying = false;
            rb.isKinematic = true;
            rb.useGravity = false;
            arrowCol.enabled = false;
            transform.parent = collision.gameObject.transform;
            if (collision.gameObject.CompareTag("Player"))
            {
                Damage(collision.gameObject.GetComponent<Collider>());
            }

        }
    }
}
