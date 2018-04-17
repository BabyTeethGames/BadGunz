using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScan : Gun {

    
    public float shotForce;
    public ParticleSystem hitParticle;
    public int damage;

    public override void Shoot()
    {
       
        RaycastHit hit;
        if (Physics.Raycast(cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)), cam.transform.forward, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (hit.collider.GetComponent<Player>() != null)
                {
                    hit.collider.GetComponent<Player>().Damaged(damage, playerShot);
                }
                else if (hit.collider.GetComponent<AIPlayerWalk>() != null)
                {
                    hit.collider.GetComponent<AIPlayerWalk>().Damaged(damage, playerShot);
                }
                else
                {
                    Debug.Log("Player is lacking 'player' script");
                }
            }

            else if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * shotForce);
            }

            Instantiate(hitParticle, hit.point, Quaternion.identity);
        }
        
    }
}
