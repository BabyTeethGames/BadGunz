using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649 //collision flags not assigned warning. They are assigned at collisions. Remove this for final build

public class FPSController : MonoBehaviour
{


    //movement variables
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float airSpeed = 16.0f;
    public float gravityMultiplier = 20.0f;
    public float drag = 0.5f;
  
    private float jumpX;
    private float jumpZ;
    public float jumpMaxSpeed;
    public Vector3 velocity;
    private float planarSpeed;
    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;
    private Vector3 moveDir = Vector3.zero;
    private CollisionFlags m_CollisionFlags;

    //Grounded Variables
    public bool m_IsGrounded;
    public bool m_WasGrounded;
    public float groundCheckDist = 0.01f;
    public  float m_StickCheck = 0.5f;
    public bool m_IsJumping = false;
    private Vector3 m_GroundCheckNormal;

    [HideInInspector] public Vector3 additiveMove = Vector3.zero;
    //state variables
    public bool mobile = true;
    public bool rotActive = true;
    public bool m_Flying = false;
    private bool m_Dead = false;

    //input variables
    private string[] m_Inputs = { "Jump", "Reload", "X", "Switch", "Grenade", "Fire", "Horizontal", "Vertical", "LHorizontal", "LVertical" };
    public string[] Inputs
    {
        get
        {
            return m_Inputs;
        }
    }


    //look variables 
    public float xSensitivity = 2f;
    public float ySensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float minimumX = -90F;
    public float maximumX = 90F;
    public Quaternion characterTargetRot;
    private Quaternion cameraTargetRot;

    //backup variables for respawns
    public float speedBU;
    public float jumpSpeedBU;
    public float gravityBU;

    public Camera cam;
    public CharacterController controller;
    private Inventory inventory;
    private PlayerAnimator playerAnimator;


    // Use this for initialization
    public void Init(int controllerNumber)
    {
        //inititalize controls to this player
        for (int i = 0; i < m_Inputs.Length; i++)
        {   
            m_Inputs[i] = string.Concat("P", controllerNumber.ToString(), "_", m_Inputs[i]);
        }

        inventory = GetComponent<Inventory>();
        cam = GetComponentInChildren<Camera>();
        characterTargetRot = transform.localRotation;
        cameraTargetRot = cam.transform.localRotation;
        controller = GetComponent<CharacterController>();
        speedBU = speed;
        jumpSpeedBU = jumpSpeed;
        jumpMaxSpeed = speed;
        playerAnimator = GetComponentInChildren<PlayerAnimator>();

        //transfer the input key to the Inventory system
        inventory.Init(Inputs[3]);
    }

    void Update()
    {
        if (!m_Dead)
        {
            GetInput();
            RotateView(lookInput);
            inventory.Pickup();
            CalculateMove();
            Move(moveDir);
            UpdateAnimator(velocity);
        }
    }

    private void FixedUpdate()
    {
        if (!m_Dead)
            GroundCheck();
    }

    void GetInput()
    {
        lookInput = new Vector2(Input.GetAxis(Inputs[8]) * xSensitivity, Input.GetAxis(Inputs[9]) * ySensitivity);
        moveInput = new Vector2(Input.GetAxis(Inputs[6]), Input.GetAxis(Inputs[7]));
        if (moveInput.sqrMagnitude > 1)
            moveInput.Normalize();

        if (Input.GetButton(Inputs[5]))
        {
            inventory.Fire();
        }

        //right now grenades aren't really in the game, this is unecessary code. 
        if (Input.GetButtonDown(Inputs[4]))
        {
            inventory.Grenade();
        }

        if (Input.GetButtonDown(Inputs[1]))
        {
            inventory.Reload();
        }
    }



    void CalculateMove()
    {
        if (mobile)
        {
            Vector3 pendingMove = transform.forward * moveInput.y + transform.right * moveInput.x;
        
            if (m_IsGrounded && !m_Flying)
            {
                if (!m_WasGrounded)
                {
                    //moveDir.y = 0;
                    m_IsJumping = false;
                }
                pendingMove = Vector3.ProjectOnPlane(pendingMove, m_GroundCheckNormal);
                moveDir = pendingMove * speed;
                jumpMaxSpeed = speed;
                if (Input.GetButtonDown(Inputs[0]))
                {
                    m_IsJumping = true;
                    moveDir.y = jumpSpeed;
                }
            }

            else
            {
               if(m_WasGrounded && !m_IsJumping && !m_Flying)
                {
                    Debug.Log("push");
                    moveDir.y -= StickToGround();
                }
               
                planarSpeed = Vector3.ProjectOnPlane(velocity, Vector3.up).magnitude;
                if (m_Flying)
                    jumpMaxSpeed = planarSpeed;

                if (!m_Flying)
                {
                    if (planarSpeed < jumpMaxSpeed)
                        jumpMaxSpeed = planarSpeed;

                    if (jumpMaxSpeed < speed)
                    {
                        jumpMaxSpeed = speed;
                    }

                    moveDir.x += pendingMove.x * airSpeed * Time.deltaTime;
                    moveDir.z += pendingMove.z * airSpeed * Time.deltaTime;
                    Vector2 jumpMove = Vector2.ClampMagnitude(new Vector2(moveDir.x, moveDir.z), jumpMaxSpeed);
                    moveDir.x = jumpMove.x;
                    moveDir.z = jumpMove.y;

                    if (planarSpeed > speed)
                    {
                        moveDir += Vector3.ProjectOnPlane(-velocity, Vector3.up).normalized * drag * Time.deltaTime;
                    }
                }
            }
        }

        if (!mobile)
        {
            moveDir.x = 0;
            moveDir.z = 0;
        }
        moveDir += Physics.gravity * gravityMultiplier * Time.deltaTime;
        moveDir += additiveMove;
        additiveMove = Vector3.zero;


    }

    public void Move(Vector3 dir)
    {
        controller.Move(dir * Time.deltaTime);
        velocity = controller.velocity;

    }

    void RotateView(Vector2 input)
    {
        if (rotActive)
        {
            characterTargetRot *= Quaternion.Euler(0f, input.y, 0f);
            cameraTargetRot *= Quaternion.Euler(-input.x, 0f, 0f);
            cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);
            transform.localRotation = characterTargetRot;
            cam.transform.localRotation = cameraTargetRot;
        }
    }

    void UpdateAnimator(Vector3 vel)
    {
        //Make sure velocity is local and normalized
        vel = transform.InverseTransformVector(vel) / speed;
        if (!m_IsJumping)
            playerAnimator.GetInput(vel.z, vel.x);
        else
            playerAnimator.GetInput(0, 0);
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, minimumX, maximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    public void Reset()
    {
        m_Dead = false;
        speed = speedBU;
        jumpSpeed = jumpSpeedBU;
        controller.enabled = true;
        mobile = true;
    }

    private void GroundCheck()
    {
        m_WasGrounded = m_IsGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position + controller.center, controller.radius * (1.0f - controller.skinWidth), Vector3.down, out hitInfo,
            ((controller.height / 2f) - controller.radius) + groundCheckDist, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            m_IsGrounded = true;
            m_GroundCheckNormal = hitInfo.normal;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundCheckNormal = Vector3.up;
        }
        
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(controller.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }

    private float StickToGround()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position + controller.center, controller.radius * (1.0f - controller.skinWidth), Vector3.down, out hitInfo,
            ((controller.height / 2f) - controller.radius) + m_StickCheck, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
                return hitInfo.distance;
        }
        else
        {
            return 0;
        }
    }

    public void Death()
    {
        m_Dead = true;
        inventory.DropGun();
        inventory.gunzNAmmo.Clear();
        controller.enabled = false;
    }
}
   