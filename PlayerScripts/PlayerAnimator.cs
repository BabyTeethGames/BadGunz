using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    private GameObject [] m_Cull;
    public Camera cam;
    Animator anim;
    Transform ikTargLH;
    Transform ikTargRH;
    Transform ikHintLE;
    Transform ikHintRE;
    public bool ikActive = false;
    public Vector2 m_Movement;


    public void Awake()
    {
        anim = GetComponentInParent<Animator>();
    }


    public void GetInput(float forward, float strafe)
    {
        anim.SetFloat("Forward", forward);
        anim.SetFloat("Strafe", strafe);
        m_Movement = new Vector2(forward, strafe);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (ikActive)
        {
            if (ikTargRH != null)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                if (ikHintRE != null)
                {
                    anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 1);
                    anim.SetIKHintPosition(AvatarIKHint.RightElbow, ikHintRE.position);
                }
                anim.SetIKPosition(AvatarIKGoal.RightHand, ikTargRH.position);
                anim.SetIKRotation(AvatarIKGoal.RightHand, ikTargRH.rotation);
            }

            else if (ikTargRH == null)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                anim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, 0);
            }

            if (ikTargLH != null)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                if (ikHintLE != null)
                {
                    anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1);
                    anim.SetIKHintPosition(AvatarIKHint.LeftElbow, ikHintLE.position);        
                }
                anim.SetIKPosition(AvatarIKGoal.LeftHand, ikTargLH.position);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, ikTargLH.rotation);
                
            }

            else if (ikTargLH == null)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                anim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 0);
            }
        }
    }

    public void IKInit(Transform ikRH, Transform ikLH, Transform hintLE, Transform hintRE, float layerWeight)
    {
        anim.SetLayerWeight(1, layerWeight);
        ikTargRH = ikRH;
        ikTargLH = ikLH;
        ikHintLE = hintLE;
        ikHintRE = hintRE;
    }

    public void IKFree()
    {
        ikActive = false;
        anim.SetLayerWeight(1, 0);
    }
}
