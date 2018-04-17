using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    //Make sure that the trigger collider is big enough to catch the character the frame after a jump so that it can add corrective movement

    //We're moving all the additive move into a playermove function. All velocity will be calculated outside in the rotation and velocity functions and then the outcome will be passed to the fpscontrollers with a "moveplayer" function




    public Vector3 velocity, previousPos, target, prevRot = Vector3.zero;
    public List<FPSController> playerControllers;
    public List<GameObject> children;
    private FPSController removeController;
    private bool movingUp;
    float rotationDelta;
    public bool moving = false;


    virtual public void Start()
    {
        playerControllers = new List<FPSController>();
        children = new List<GameObject>();
        previousPos = transform.position;
        prevRot = transform.rotation.eulerAngles;
    }

    virtual public void Update()
    {

    }
    
    public void VelocityUpdate()
    {
        velocity = (transform.position - previousPos) / Time.deltaTime;

        if (velocity.y > 0)
            movingUp = true;
        else
            movingUp = false;

        previousPos = transform.position;
    }

    public void RotationUpdate()
    {
        rotationDelta = (transform.rotation.eulerAngles.y - prevRot.y) * Mathf.Deg2Rad;
        prevRot = transform.rotation.eulerAngles;
    }

    public void MovePlayer(bool rotating)
    {
        if (playerControllers.Count > 0)
        {
            foreach (FPSController controller in playerControllers)
            {                
                if (controller.m_IsGrounded)
                {
                    Vector3 playerMove = velocity;
                    if(movingUp)
                    {
                        //Add opposite player gravity to stop clipping against the downward movement of the player against the upward motion of the platform.
                        playerMove -= Physics.gravity * 2 * Time.deltaTime;
                    }
                    if(rotating)
                    {
                        playerMove += RotationalVelocityY(controller.transform.position);
                    }
                    controller.additiveMove += playerMove;
                }
            }
        }
    }

    public Vector3 RotationalVelocityY(Vector3 playerPos)
    {
        //use the cross product to calculate more complicated angles, this will give you the perpendicular line to work against. 
        Vector3 playerLocPos = playerPos - transform.position;
        Vector3 playerRotationalVelocity = new Vector3(playerLocPos.x * Mathf.Cos(rotationDelta) + playerLocPos.z * Mathf.Sin(rotationDelta),
            0, playerLocPos.x * -Mathf.Sin(rotationDelta) + playerLocPos.z * Mathf.Cos(rotationDelta));

        playerRotationalVelocity = (playerRotationalVelocity - playerLocPos) / Time.deltaTime;
        playerRotationalVelocity.y = 0;
        return playerRotationalVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerControllers.Add(other.GetComponent<FPSController>());
        }
        else if(!other.CompareTag("RagDoll"))
        {
            children.Add(other.transform.root.gameObject);
            other.transform.root.transform.SetParent(transform, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            removeController = other.GetComponent<FPSController>();
            playerControllers.Remove(removeController);
        }
        else
        {
            children.Remove(other.transform.root.gameObject);
            other.transform.root.transform.parent = null;
        }
    }

 



   

 
}