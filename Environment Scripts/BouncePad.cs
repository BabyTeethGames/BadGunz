using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour {
    FPSController fps;
    float oldJump;
    public float jumpBoost;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            fps = other.GetComponent<FPSController>();
            oldJump = fps.jumpSpeed;
            fps.jumpSpeed += jumpBoost; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fps = other.GetComponent<FPSController>();
            fps.jumpSpeed = oldJump;
        }
    }
}
