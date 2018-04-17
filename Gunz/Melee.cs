using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{

    [HideInInspector] public bool swinging = false;
    public int damage;

    // Use this for initialization
    public override void Start()
    {

        base.Start();
    }

    public override void Fire()
    {
        if (swinging == false && Time.time > nextFire)
        {
            anim.Fire();
            nextFire = Time.time + fireRate;
        }
    }

    virtual public void Damage(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            other.GetComponent<Player>().Damaged(damage, playerShot);
        }

        else if (other.GetComponent<AIPlayerWalk>() != null)
        {
            other.GetComponent<AIPlayerWalk>().Damaged(damage, playerShot);
        }
        else
        {
            Debug.Log("Player is lacking 'player' script");
        }
    }

}
