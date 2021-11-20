using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class HandIK : MonoBehaviour
{
    [Range(0, 2)] public float distanceForIK = 2f;
    //[Range(0, 5)] public float originOffset = 1f;
    

    Animator anim;

    public bool ikActive = false;
    public Transform rightHandObj = null;
    public Transform lookObj = null;

    float HandIKWeight;
    float HeadIKWeight;

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

                    anim.SetLookAtWeight(calcWeightFromDistance(HumanBodyBones.Head, lookObj, distanceForIK));
                    anim.SetLookAtPosition(lookObj.position);
                }

                if (rightHandObj != null)
                {
                    float weight = calcWeightFromDistance(HumanBodyBones.RightHand, rightHandObj, distanceForIK);
                    anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
                    anim.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
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
    
    float calcWeightFromDistance(HumanBodyBones boneToWeigh, Transform IKTargetTrans, float maxDistance)
    {
        Vector3 ActorToObject = IKTargetTrans.position - anim.GetBoneTransform(boneToWeigh).position;
        float distance = ActorToObject.magnitude;
        float weight = maxDistance / distance;

        weight -= 0.2f;
        weight = weight / 0.7f;
        return weight;
    }
}

