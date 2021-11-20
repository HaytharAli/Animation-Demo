using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaxFace : MonoBehaviour
{
    public bool enable = false;
    public SkinnedMeshRenderer smr;
    float weight;
    public Transform DaxHead;
    public Animator anim;
    public float maxDistance = 2f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            Debug.DrawLine(anim.GetBoneTransform(HumanBodyBones.RightHand).position, DaxHead.position, Color.yellow);
            Vector3 ActorToObject = anim.GetBoneTransform(HumanBodyBones.RightHand).position - DaxHead.position;
            float distance = ActorToObject.magnitude;
            weight = maxDistance / distance;

            if (weight > 1)
            {
                weight = 1;
            }

            weight *= 100;
            weight -= 33;

            weight /= 67;
            weight *= 100;

            smr.SetBlendShapeWeight(0, weight);
        }
    }


}
