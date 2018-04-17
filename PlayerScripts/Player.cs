using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerUI))]
[RequireComponent(typeof(FPSController))]
[RequireComponent(typeof(Inventory))]

public class Player : MonoBehaviour {


    public GameObject m_Ragdoll;
    public RedDeadEffect redDead;
    public bool m_Dead = false;
    private bool m_GameEnd = false;
    private FPSController m_FpsController;
    public int m_PlayerNum = 1, m_ControllerNum = 1, m_Health = 100, m_Score;
    public Camera[] m_Cams;
    public SkinnedMeshRenderer[] m_Meshes;
    public PlayerUI m_UI;

    //EventSenders
    public delegate void KilledAction(int playerDead, int playerKill);
    public static event KilledAction onKilled;
    public delegate void RespawnAction(Player player);
    public static event RespawnAction onRespawn;

    
    public void Init(int playerNumber, int controllerNum, Vector2 camPos, Vector2 camSize)
    { 
        m_PlayerNum = playerNumber;
        m_ControllerNum = controllerNum;
        m_Meshes = GetComponentsInChildren<SkinnedMeshRenderer>(true);
        redDead = GetComponentInChildren<RedDeadEffect>();
        m_FpsController = GetComponent<FPSController>();
        m_UI = GetComponent<PlayerUI>();
        m_Cams = GetComponentsInChildren<Camera>();      
        m_FpsController.Init(m_ControllerNum);
        m_FpsController.enabled = true;
        CameraInit(camPos, camSize);
        m_GameEnd = false;
        m_UI.HealthUpdate(m_Health);
    }

    public void Reset()
    {
        foreach (SkinnedMeshRenderer mesh in m_Meshes)
        {
            mesh.enabled = true;
        }
        
        m_Health = 100;
        m_UI.HealthUpdate(m_Health);
        m_Dead = false;
        redDead.enabled = false;
        m_FpsController.Reset();
    }

    private void Update()
    {
        if (m_Health < 1 && m_Dead == false)
        {
            Death();          
        }

        if (m_Dead == true)
        {
            if (Input.GetButtonDown(m_FpsController.Inputs[0]) && onRespawn != null)
            {
                Respawn();
            }
        }        

    }

    private void CameraInit(Vector2 position, Vector2 size)
    {
        string cullName = "P" + m_ControllerNum + "Cull";
        Transform[] cullObjs = GetComponentsInChildren<Transform>(true);
        foreach (Transform cullObj in cullObjs)
        {
            if(cullObj.CompareTag("Cull"))
            {
               cullObj.gameObject.layer = LayerMask.NameToLayer(cullName);
            }
        }

        foreach (Camera cam in m_Cams)
        {
            cam.rect = new Rect(position, size);
            cam.cullingMask &= ~(1 << LayerMask.NameToLayer(cullName));
        }

    }


    void Death()
    {
        Instantiate(m_Ragdoll, transform.position, transform.rotation);
        foreach (SkinnedMeshRenderer mesh in m_Meshes)
        {
            mesh.enabled = false;
        }
        m_Dead = true;
        m_FpsController.Death();
    }

    void Respawn()
    {      
        onRespawn(this);
    }

    public void Damaged(int damage, int playerShot)
    {
        if (!m_Dead && ! m_GameEnd)
        {
            m_Health -= damage;
            if (m_Health > 100)
                m_Health = 100;
            m_UI.HealthUpdate(m_Health);

            if (m_Health <= 0)
            {
                if (onKilled != null)
                {
                    Debug.Log(m_PlayerNum + " Just got killed. Event sent");
                    onKilled(m_PlayerNum, playerShot);
                }
                Death();
            }
            StartCoroutine("RedDead");
        }
    }

    public void GameEnd(bool winner)
    {
        foreach (Camera cam in m_Cams)
        {
            cam.enabled = false;
        }
        if (!winner)
        {
            m_FpsController.mobile = false;
            m_GameEnd = true;
        }
    }


    IEnumerator RedDead() {
        Color colorStart = Color.white;
        Color colorEnd = Color.red;
        redDead.enabled = true;
        
        yield return new WaitForSeconds(0.1f);
        if (!m_Dead)
        {
            redDead.enabled = false;
        }
    }
}
