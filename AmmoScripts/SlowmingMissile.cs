using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowmingMissile : Projectile {

    public Transform target;

    public float thrust = 1;
    public float turnSpeed = 0.01f;


    // Update is called once per frame
    void Update () {
        if (target != null)
        {
            Vector3 relativePos = target.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), Time.deltaTime * turnSpeed);          
        }
        transform.Translate(Vector3.forward * thrust * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Damage(collision.gameObject.GetComponent<Collider>());
        }
        //instantiate explosion;
        Destroy(this.gameObject);
        Destroy(target.gameObject);
    }
}
