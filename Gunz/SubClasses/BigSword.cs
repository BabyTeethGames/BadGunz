using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSword : Melee {

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().m_PlayerNum != playerShot)
                Damage(other);
        }
    }

}
