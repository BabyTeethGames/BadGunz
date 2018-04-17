using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGrenade : Projectile
{
    public ParticleSystem explosion;
    public Color start;
    public Color end;
    public float throwForce = 300;
    Renderer rend;
    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        start = rend.material.color;
        StartCoroutine("Grenade");
        rb.AddTorque(Vector3.right * 300);
        rb.AddForce((transform.forward + Vector3.up) * throwForce);
        
    }

    IEnumerator Grenade ()
    {
        for (float i = 0; i < 1; i += 0.004f)
        {
            rend.material.color = Color.Lerp(start, end, i);
            yield return null;
        }
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.localRotation);
        }
        float r = 6;
        var cols = Physics.OverlapSphere(transform.position, r);
        foreach(var col in cols)
        {
            if(col.CompareTag("Player"))
            {
                base.Damage(col);
            }
        }
        Destroy(this.gameObject);
    }
}
