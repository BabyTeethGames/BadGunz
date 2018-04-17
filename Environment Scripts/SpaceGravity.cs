
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGravity : MonoBehaviour {
    FPSController controller;
    private List<FPSController> playerControllers;
    //private List<Rigidbody> children; Uncomment all of the children properties after you figure out how to fix all that shiiiiiit with the meteors and destroyed rigidbodies(aka: guns that are picked up)

    private void Start()
    {
        playerControllers = new List<FPSController>();
        //children = new List<Rigidbody>();
    }

    private void Update()
    {
        if (playerControllers.Count > 0)
        {
            foreach (FPSController controller in playerControllers)
            {
                controller.additiveMove += Physics.gravity * Time.deltaTime * 2;
            }
        }

       /* if (children.Count > 0)
        {
            foreach (Rigidbody rb in children)
            {
                rb.AddForce(Physics.gravity * Time.deltaTime);
            }
        }
        */

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerControllers.Add(other.GetComponent<FPSController>());
        }
        /*
        else if(other.GetComponent<Rigidbody>())
        {
            children.Add(other.GetComponent<Rigidbody>());
        }
        */
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FPSController removeController = other.GetComponent<FPSController>();
            playerControllers.Remove(removeController);
        }/*
        else if(other.GetComponent<Rigidbody>())
        {
            Rigidbody rbRemove = other.GetComponent<Rigidbody>(); 
            children.Remove(rbRemove);
        }*/
    }

}
