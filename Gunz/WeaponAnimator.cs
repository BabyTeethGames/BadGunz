using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour {

    public Transform m_IKLH, m_IKRH, m_IKHintRE, m_IKHintLE;

    public float m_LayerWeight;
    private Animator m_Anim;
    private PlayerAnimator m_PlayerAnim;
    private FPSController m_Controller;

    // Use this for initialization
    void Start() {
        m_Anim = GetComponent<Animator>();
        m_PlayerAnim = GetComponentInParent<PlayerAnimator>();
        m_Controller = GetComponentInParent<FPSController>();
        //Wait a frame to stop the overlap between IKFree on destroy and IKInit on start;
        StartCoroutine("Init");
    }

    // Update is called once per frame
    void Update()
    {
        m_Anim.SetFloat("Forward", m_PlayerAnim.m_Movement.x);
        m_Anim.SetFloat("Strafe", m_PlayerAnim.m_Movement.y);
    }

    public void Fire()
    {
        m_Anim.SetTrigger("Fire");
    }

    public void Reload()
    {
        m_Anim.SetTrigger("Reload");
    }

    public void Mobile()
    {
        m_Controller.mobile = !m_Controller.mobile;
    }

    public void RotationActive()
    {
        m_Controller.rotActive = !m_Controller.rotActive;
    }
 

    IEnumerator Init()
    {
        yield return null;
        m_PlayerAnim.IKInit(m_IKRH, m_IKLH, m_IKHintLE, m_IKHintRE, m_LayerWeight);
        m_PlayerAnim.ikActive = true;
    }

    private void OnDestroy()
    {
        m_PlayerAnim.IKFree();
        m_Controller.mobile = true;
        m_Controller.rotActive = true;
    }
}
