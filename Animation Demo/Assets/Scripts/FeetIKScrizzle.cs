using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetIKScrizzle : MonoBehaviour
{
    public Animator anim = null;

    private Vector3 rightFootPosition, leftFootPosition, rightFootIKPosition, leftFootIKPosition;
    private Quaternion leftFootIKRotation, rightFootIKRotation;
    private float lastPelvisPositionY, lastRightFootPositionY, lastLeftFootPositionY;

    [Header("IK Stuff")]
    public bool enableFeetIK = true;
    [Range(0, 2)] [SerializeField] private float heightFromGroundRaycast = 1.14f;
    [Range(0, 2)] [SerializeField] private float raycastDownDistance = 1.5f;
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private float pelvisOffset = 0f;
    [Range(0, 1)] [SerializeField] private float pelvisUpAndDownSpeed = 0.28f;
    [Range(0, 1)] [SerializeField] private float feetToIKPositionSpeed = 0.5f;

    public bool useProIKFeature = false;
    public bool showSolverDebug = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        if (enableFeetIK && anim != null)
        {
            adjustFeetTarget(ref rightFootPosition, HumanBodyBones.RightFoot);
            adjustFeetTarget(ref leftFootPosition, HumanBodyBones.LeftFoot);

            feetPositionSolver(rightFootPosition, ref rightFootIKPosition, ref rightFootIKRotation);
            feetPositionSolver(leftFootPosition, ref leftFootIKPosition, ref leftFootIKRotation);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (enableFeetIK && anim != null)
        {
            movePelvisHeight();

            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);

            moveFeetToIKPoint(AvatarIKGoal.RightFoot, rightFootIKPosition, rightFootIKRotation, ref lastRightFootPositionY);


            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);

            moveFeetToIKPoint(AvatarIKGoal.LeftFoot, rightFootIKPosition, leftFootIKRotation, ref lastLeftFootPositionY);

        }
    }

    void moveFeetToIKPoint(AvatarIKGoal foot, Vector3 PositionIKHolder, Quaternion rotationIKHolder, ref float lastFootPositionY)
    {
        Vector3 targetIKPosition = anim.GetIKPosition(foot);

        if (PositionIKHolder != Vector3.zero)
        {
            targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
            PositionIKHolder = transform.InverseTransformPoint(PositionIKHolder);

            float yVar = Mathf.Lerp(lastFootPositionY, PositionIKHolder.y, feetToIKPositionSpeed);
            targetIKPosition.y += yVar;

            lastFootPositionY = yVar;

            targetIKPosition = transform.TransformPoint(targetIKPosition);

            anim.SetIKRotation(foot, rotationIKHolder);
        }

        anim.SetIKPosition(foot, targetIKPosition);
    }

    private void movePelvisHeight()
    {
        if (rightFootIKPosition == Vector3.zero || leftFootIKPosition == Vector3.zero || lastPelvisPositionY == 0)
        {
            lastPelvisPositionY = anim.bodyPosition.y;
            return;
        }
        float leftOffsetPosition = leftFootIKPosition.y - transform.position.y;
        float rightOffsetPosition = rightFootIKPosition.y - transform.position.y;

        float totalOffset = (leftOffsetPosition < rightOffsetPosition) ? leftOffsetPosition : rightOffsetPosition;

        Vector3 newPelvisPosition = anim.bodyPosition + Vector3.up * totalOffset;

        newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);

        anim.bodyPosition = newPelvisPosition;

        lastPelvisPositionY = anim.bodyPosition.y;
    }


    private void feetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIKPositions, ref Quaternion feetIKRotations)
    {
        RaycastHit feetOutHit;
        if (showSolverDebug)
        {
            Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.yellow);
        }
        if (Physics.Raycast(fromSkyPosition, Vector3.down, out feetOutHit, raycastDownDistance + heightFromGroundRaycast, environmentLayer))
        {
            feetIKPositions = fromSkyPosition;
            feetIKPositions.y = feetOutHit.point.y + pelvisOffset;
            feetIKRotations = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;

            return;
        }

        feetIKPositions = Vector3.zero;
    }

    private void adjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot)
    {
        feetPositions = anim.GetBoneTransform(foot).position;
        feetPositions.y = transform.position.y + heightFromGroundRaycast;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
