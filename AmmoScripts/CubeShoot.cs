using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeShoot : Projectile
{

    bool tf = true;
    public float growth = 0.005f;
    public float growthSpeed = 100f;
    public float cubeSlow = 0.5f;
    public Vector3 initialVelocity;
    // Use this for initialization
    void Start()
    {
        initialVelocity = Vector3.ProjectOnPlane(initialVelocity, Vector3.up);
        if (initialVelocity.z <= 0)
            initialVelocity.z = 0;
        StartCoroutine("Shoot");
    }
    
    private IEnumerator Shoot()
    {
        while (transform.localScale.x < 3)
        {         
            transform.localScale += new Vector3(growth, growth, growth) * Time.deltaTime * growthSpeed;
            transform.Translate((initialVelocity + Vector3.forward) * Time.deltaTime); 
            yield return null;
        }

        while (tf)
        {
            transform.Translate(((Vector3.forward*2)+initialVelocity) * Time.deltaTime * cubeSlow);
            yield return null;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Damage(other);
            Destroy(this.gameObject);
        }

        //if (other.CompareTag("Environment"))
        //{
         //   Destroy(this.gameObject);
       // }
    }
}
