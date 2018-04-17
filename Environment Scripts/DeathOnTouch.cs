using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnTouch : MonoBehaviour {
    private Player player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<Player>();
            player.Damaged(1000, player.m_PlayerNum);
        }

        else
        {
            Destroy(other.gameObject);
        }
    }
}
