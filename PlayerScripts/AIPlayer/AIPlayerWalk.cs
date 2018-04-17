using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIPlayerWalk : MonoBehaviour {

    NavMeshAgent agent;
    GameObject[] destinations;
    Transform destination;
    Animator anim;
    Rigidbody rb;
    public int health = 100;
    bool m_Dead = false;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        destinations = GameObject.FindGameObjectsWithTag("AmmoSpawn");
        destination = destinations[Random.Range(0, destinations.Length)].transform;
        agent.SetDestination(destination.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Dead)
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
            AnimationUpdate();
            if (health < 1)
            {
                Death();
            }
        }        
    }

    public void Damaged(int damage, int playerShot)
    {
        if (!m_Dead)
        {
            health -= damage;
            if (health > 100)
                health = 100;

            if (health <= 0)
            {
                //if (onKilled != null) These will be added later. 
                    //onKilled(m_PlayerNum, playerShot);
                Death();
            }
        }
    }

    void Death()
    {
        agent.enabled = false;
        rb.isKinematic = false;
        anim.SetFloat("Forward", 0);
        anim.SetFloat("Strafe", 0);
        m_Dead = true;
    }

    void AnimationUpdate()
    {
        if (!m_Dead)
        {
            Vector3 pVel = transform.InverseTransformDirection(agent.velocity);
            anim.SetFloat("Forward", pVel.z);
            anim.SetFloat("Strafe", pVel.x);
        }
      
    }
}
