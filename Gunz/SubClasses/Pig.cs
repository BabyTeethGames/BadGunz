using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pig : MonoBehaviour {


    NavMeshAgent agent;
    GameObject[] destinations;
    Transform destination;
	// Use this for initialization
	void Start ()
    {
        agent = GetComponent<NavMeshAgent>();
        destinations = GameObject.FindGameObjectsWithTag("AmmoSpawn");
        destination = destinations[Random.Range(0, destinations.Length)].transform;
        agent.SetDestination(destination.position);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    destination = destinations[Random.Range(0, destinations.Length)].transform;
                    agent.SetDestination(destination.position);
                }
            }
        }
    }



}
