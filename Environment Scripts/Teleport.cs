using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    public Transform teleLoc;
    public float turnAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.position = teleLoc.position;
            other.GetComponent<FPSController>().characterTargetRot *= Quaternion.Euler(0, turnAmount, 0);
        }
    }

}
