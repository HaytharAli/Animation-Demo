using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator anim;
    int isWalkingID;
    int isRunningID;
    int isJumpingID;
    int isAttackingID;
    int isDyingID;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isWalkingID = Animator.StringToHash("isWalking");
        isRunningID = Animator.StringToHash("isRunning");
        isJumpingID = Animator.StringToHash("isJumping");
        isAttackingID = Animator.StringToHash("isAttacking");
        isDyingID = Animator.StringToHash("isDying");

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        bool jumpPressed = Input.GetKey(KeyCode.Space);
        bool isWalking = anim.GetBool(isWalkingID);
        bool isRunning = anim.GetBool(isRunningID);
        bool isJumping = anim.GetBool(isJumpingID);


        if (!isWalking && forwardPressed)
        {
            anim.SetBool(isWalkingID, true);
        }
        if (isWalking && !forwardPressed)
        {
            anim.SetBool(isWalkingID, false);
        }

        if (!isRunning && (forwardPressed && runPressed))
        {
            anim.SetBool(isRunningID, true);
        }

        if (isRunning && (!forwardPressed || !runPressed))
        {
            anim.SetBool(isRunningID, false);
        }

        //if(jumpPressed && isJumping)
        //{
        //    anim.SetBool(isJumpingID, false);
        //}
        //else
        if (jumpPressed)
        {
            anim.SetBool(isJumpingID, true);
            //Jump
            rb.AddForce(Vector3.up * 0.5f, ForceMode.Impulse);
        }
        else
        {
            anim.SetBool(isJumpingID, false);   
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            anim.SetBool(isAttackingID, true);
        }
        else
        {
            anim.SetBool(isAttackingID, false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetBool(isDyingID, true);
        }
    }
}
