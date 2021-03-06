using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAnimScript : MonoBehaviour
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
        float speed = rb.velocity.magnitude;
        bool forwardPressed = false;
        bool runPressed = false;
        bool isWalking = anim.GetBool(isWalkingID);
        bool isRunning = anim.GetBool(isRunningID);

        if (speed > 0)
        {
            forwardPressed = true;

        }
        if (speed > 7)
        {
            runPressed = true;
        }

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

    private void FixedUpdate()
    {
        bool jumpPressed = Input.GetKey(KeyCode.Space);
        bool isJumping = anim.GetBool(isJumpingID);

        if (jumpPressed)
        {
            anim.SetBool(isJumpingID, true);
            //Jump
            rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
        }
        else
        {
            anim.SetBool(isJumpingID, false);
        }
    }
}
