using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class HandIK : MonoBehaviour
{

    Animator anim;

    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform lookObj = null;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        if (anim)
        {
            if (ikActive)
            {
                if (lookObj != null)
                {
                    anim.SetLookAtWeight(1);
                    anim.SetLookAtPosition(lookObj.position);
                }

                if (rightHandObj != null)
                {
                    anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                    anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                }
            }

            else
            {
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                anim.SetLookAtWeight(0);
            }
        }
    }
}