using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChariotWheel : MonoBehaviour
{
    public Transform m_Wheel, m_Tilt, m_Peg;
    public Vector3 m_PrevPos = Vector3.zero, m_Forces = Vector3.zero, m_Move = Vector3.zero ,m_GroundNormal, m_WallNormal;
    public Vector3 m_Velocity;
    private float m_CurrentSpeed, m_FluidDensity = 1.225f, m_ProjectedArea, m_Circumference, m_MaxSteerAngle, m_CurrentIncline;
    public float m_Radius = 0.5f, m_Height = 1f, m_DragCoefficient, m_Mass, m_MaxEngineTorque, m_MinLeanSpeed, m_MaxLeanAngle, m_LeanSpeed, m_MaxSlope, m_GroundCatch, m_MaxStepHeight;
    public bool m_IsGrounded;

    //Cast variables
    public Transform origin;
    private Vector3 p1, p2, capscastdir;
    private int m_LayerMask;
    public CapsuleCollider capsule;
    public float currentHit, castDist, castRad, capsuleDist, currentCapsuleDist, groundfixbuffer;
    private bool m_WasGrounded;

    private void Awake()
    {
        m_PrevPos = transform.position;
        m_Circumference = 2 * Mathf.PI * m_Radius;
        m_ProjectedArea = (m_Circumference * m_Height) / 2;
        m_MaxSteerAngle = CalculateMaxSteer(m_MinLeanSpeed, m_MaxLeanAngle);
        m_LayerMask = 1 << 2;
        m_LayerMask = ~m_LayerMask;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move();
        float turnAngle = TiltWheel();
        if (m_IsGrounded)
        {
            transform.Rotate(Vector3.up * turnAngle);
        }
        WheelRotate();
        
       // Debug.Log("ground: " + m_GroundNormal + " incline: " + m_CurrentIncline + " grounded?: " + m_IsGrounded + " velocity: " + m_Velocity);
    }


    private void Move()
    {
        m_CurrentSpeed += ForwardAcceleration();
        CollisionCheck();
        if (m_IsGrounded)
        {
            m_Move = Vector3.ProjectOnPlane(transform.forward, m_GroundNormal).normalized * m_CurrentSpeed;
        }
        else
        {
            m_Move = m_Velocity; 
            m_Move += GravityAcceleration() * Time.deltaTime;
        }

        Vector3 wallCheckDir = Vector3.ProjectOnPlane(m_Move.normalized, m_GroundNormal);
        if (WallCheck(wallCheckDir))
        {
            Vector3 wallAngle = Vector3.ProjectOnPlane(transform.forward, m_WallNormal).normalized;
            m_CurrentSpeed *= Vector3.Dot(transform.forward, wallAngle);
            m_Move = wallAngle * m_CurrentSpeed;   
        }

        m_Move = transform.InverseTransformDirection(m_Move); // aligning with local movement    
        transform.Translate(m_Move * Time.deltaTime); // this is the velocity

        m_Velocity = (transform.position - m_PrevPos) / Time.deltaTime;
        m_PrevPos = transform.position;

    }


    private void GroundCheck()
    {
        m_WasGrounded = m_IsGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(origin.position, castRad, Vector3.down, out hitInfo, castDist + m_GroundCatch, m_LayerMask, QueryTriggerInteraction.Ignore))
        {
            currentHit = hitInfo.distance;
            if (currentHit < castDist + groundfixbuffer)
            {
                m_IsGrounded = true;
                m_GroundNormal = hitInfo.normal;
                if (currentHit < castDist - groundfixbuffer)
                {
                   
                    float groundFix = (castDist - currentHit);
                    transform.Translate(Vector3.up * groundFix);
                    m_PrevPos.y -= groundFix;
                }
            }

            else if(m_WasGrounded)
            {
                float groundFix = castDist + m_GroundCatch - currentHit;
                m_GroundNormal = hitInfo.normal;
                transform.Translate(Vector3.up * -groundFix);
                m_PrevPos.y += groundFix;
                m_IsGrounded = true;
            }
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
            currentHit = castDist;
        }
        m_CurrentIncline = (Vector3.Angle(m_GroundNormal, transform.forward) - 90);

    }

    private bool WallCheck(Vector3 capsuleDir)
    {
        RaycastHit hitInfo;
        float radoffset = (capsule.height * 0.5f) - capsule.radius;
        capscastdir = capsuleDir;
        capsuleDist = m_Move.magnitude * Time.deltaTime;
        p1 = transform.position + capsule.center + Vector3.up * radoffset;
        p2 = transform.position + capsule.center + Vector3.down * radoffset;
        if (Physics.CapsuleCast(p1, p2, capsule.radius, capscastdir, out hitInfo, capsuleDist, m_LayerMask, QueryTriggerInteraction.Ignore))
        {
            float hitAngle = Vector3.Angle(hitInfo.normal, Vector3.up);
            currentCapsuleDist = hitInfo.distance;
            if (hitInfo.rigidbody == null && hitAngle > m_MaxSlope)
            {
                m_WallNormal = Vector3.ProjectOnPlane(hitInfo.normal, Vector3.up);
                return true;
            }
            return false;
        }
        else
        {
            currentCapsuleDist = capsuleDist;
            return false;
        }
    }


    private void CollisionCheck()
    {
        Collider[] cols = Physics.OverlapCapsule(p1, p2, capsule.radius, m_LayerMask, QueryTriggerInteraction.Ignore);
        if (cols != null)
        {
            foreach (Collider col in cols)
            {
                if (col.attachedRigidbody != null)
                {
                    float distance;
                    Vector3 direction;
                    if (Physics.ComputePenetration(col, col.attachedRigidbody.position, col.attachedRigidbody.rotation, capsule, transform.position, transform.rotation, out direction, out distance))
                    {
                        col.attachedRigidbody.position = (col.attachedRigidbody.position + (direction * distance));
                    }
                    RaycastHit hitInfo;
                    Ray ray = new Ray(transform.position, col.attachedRigidbody.position - transform.position);
                    if (col.Raycast(ray, out hitInfo, 10f))
                    {
                        Vector3 relativeVelocity = (Vector3.ProjectOnPlane((m_Velocity) - (col.attachedRigidbody.velocity), Vector3.up));
                        float hitDot = Vector3.Dot(Vector3.ProjectOnPlane(hitInfo.normal, Vector3.up).normalized, transform.forward);
                        Vector3 outImpulse = relativeVelocity * m_Mass;
                        col.attachedRigidbody.AddForceAtPosition((outImpulse + (-outImpulse * (1 - Mathf.Abs(hitDot)))), hitInfo.point, ForceMode.Impulse);
                        Vector3 backAcceleration = -relativeVelocity * col.attachedRigidbody.mass / m_Mass;
                        float addedAcceleration = (backAcceleration).magnitude * hitDot;

                        Vector3 correction = Vector3.ProjectOnPlane(transform.forward, m_GroundNormal).normalized * addedAcceleration;
                        transform.Translate(transform.InverseTransformDirection(correction) * Time.deltaTime);

                        m_CurrentSpeed += addedAcceleration;
                    }
                }
            }
        }

    }

    //collisions




    //forces  

    private float ForwardAcceleration()
    {
        Vector3 engine = EngineTorque();
        Vector3 drag = Drag((Vector3.forward * m_CurrentSpeed) + (engine / m_Mass)); // gross output of acceleration
        Vector3 inclineForce = new Vector3(0, 0, Physics.gravity.y * Mathf.Sin(m_CurrentIncline * Mathf.Deg2Rad) * Time.deltaTime);
        Vector3 forwardAcceleration;
        if (m_CurrentIncline < m_MaxSlope)
        {
            forwardAcceleration = ((engine + drag) / m_Mass) + inclineForce;
        }
        else
        {
            m_CurrentSpeed = 0;
            forwardAcceleration = inclineForce;
        }
          //net output of acceleration
        return forwardAcceleration.z;
    }


    private Vector3 EngineTorque()
    {
        float pedal = Input.GetAxis("Trigger");
        Vector3 engineTorque = Vector3.zero;
        if (m_IsGrounded)
        {
            engineTorque = m_MaxEngineTorque * pedal * Vector3.forward;
        }
        ThrottleVisual(pedal);
        return engineTorque;
    }

    private Vector3 Drag(Vector3 velocity)
    {
        float drag = m_DragCoefficient * m_FluidDensity * m_ProjectedArea * velocity.sqrMagnitude * 0.5f;
        Vector3 dragVector = drag * -velocity.normalized;
        return dragVector;
    }

    private Vector3 GravityAcceleration()
    {
        Vector3 gravityAcceleration = Physics.gravity + (Drag(new Vector3 (0,m_Velocity.y)/m_Mass));
        return gravityAcceleration;
    }


    //Visual applications
    private void ThrottleVisual(float pedal)
    {
        float pegTarget = pedal * 18;
        float pegTurn = (Mathf.LerpAngle(m_Peg.localEulerAngles.x, pegTarget, Time.deltaTime * 10));
        m_Peg.localEulerAngles = new Vector3(pegTurn, 0);
    }

    private void WheelRotate()
    {
        float rpm = m_CurrentSpeed / m_Circumference;
        float wheelRotDelta = rpm * 360;
        m_Wheel.Rotate(Vector3.right, wheelRotDelta * Time.deltaTime);
    }

     
    private float TiltWheel()
    {
        float steer = Input.GetAxis("Horizontal");
        float tiltTarget = -steer * m_MaxLeanAngle;
        float absSpeed = Mathf.Abs(m_CurrentSpeed);

        
        if (absSpeed < m_MinLeanSpeed)
        {
            tiltTarget *= absSpeed / 5;
        }
        

        float tilt = Mathf.LerpAngle(m_Tilt.localEulerAngles.z, tiltTarget, Time.deltaTime * m_LeanSpeed);
        m_Tilt.localEulerAngles = new Vector3(0, 0, tilt);

        float turnAngle;
        if (absSpeed > m_MinLeanSpeed)
        {
            float turnRad = (m_CurrentSpeed * m_CurrentSpeed) / Mathf.Tan(-tilt * Mathf.Deg2Rad); //velocity squared divided by the tilt angle
            turnAngle = m_CurrentSpeed / turnRad * Mathf.Rad2Deg;

        }
        else
        {
            turnAngle = Mathf.Clamp(m_CurrentSpeed * steer / 2, -m_MaxSteerAngle, m_MaxSteerAngle);// the minmax turn value will have to be changed to the turn value at the minimum speed needed to start leaning
        }
        return turnAngle;
    }

    private float CalculateMaxSteer(float velocity, float tilt)
    {
        float turnRad = (velocity * velocity) / Mathf.Tan(-tilt * Mathf.Deg2Rad); //velocity squared divided by the tilt angle
        return -velocity / turnRad * Mathf.Rad2Deg;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin.position, origin.position  + Vector3.down * currentHit);
        Gizmos.DrawWireSphere(origin.position + Vector3.down * currentHit, castRad);
        Gizmos.color = Color.yellow;
        Debug.DrawLine(p1, p1 + capscastdir * currentCapsuleDist *2);
        Debug.DrawLine(p2, p2 + capscastdir * currentCapsuleDist * 2);
        Gizmos.DrawWireSphere(p1 + capscastdir * currentCapsuleDist * 2, capsule.radius);
        Gizmos.DrawWireSphere(p2 + capscastdir * currentCapsuleDist * 2, capsule.radius);
    }
    /* 
 *   
*/
}


