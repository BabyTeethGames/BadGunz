using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Player player;
    public int damage;
    public int playerShot;


    virtual public void Damage(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>() != null)
            {
                other.GetComponent<Player>().Damaged(damage, playerShot);
            }
            else if(other.GetComponent<AIPlayerWalk>() !=null)
            {
                other.GetComponent<AIPlayerWalk>().Damaged(damage, playerShot);
            }
            else
            {
                Debug.Log("Player is lacking 'player' script");
            }
        }

    }
}

