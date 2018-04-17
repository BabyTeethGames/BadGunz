using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform m_Target;
    public Vector3 m_Offset;

    void Start()
    {
        m_Offset = new Vector3(0, 1.79f, 5);
    }

    void LateUpdate()
    {
        transform.position = m_Target.position + m_Offset;
        transform.LookAt(m_Target.position);
    }
}