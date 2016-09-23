using UnityEngine;
using System.Collections;
using System;

public class PlayerDashableController : PlayerWalkableController
{

    public float DashForce;

    new void Update()
    {
        if (IsReady)
        {
            UpdateState();
        }
        else
        {
            myAnimator.SetBool("Ground", true);
        }
    }

    new void UpdateState()
    {
        //AXIS
        float horizontalAxis = Input.GetAxis(MyInput.hAxis);
        float verticalAxis = Input.GetAxis(MyInput.vAxis);

        //EVENTS
        bool dash = Input.GetButtonDown(MyInput.shift);
        bool jumping = Input.GetButtonDown(MyInput.jump);
        bool shooting = Input.GetButtonDown(MyInput.fire1);

        //LOCK
        float controlLocked = Input.GetAxis(MyInput.lock_control);

        //DIRECTIONS
        bool diagonalUp = Mathf.Abs(horizontalAxis) > 0 && verticalAxis < 0;
        bool diagonalDown = Mathf.Abs(horizontalAxis) > 0 && verticalAxis > 0;

        PlayerLookingState newLookingState = horizontalAxis > 0 ? PlayerLookingState.RIGHT : horizontalAxis < 0 ? PlayerLookingState.LEFT : LookingState;
        setLookState(newLookingState);

        if (Grounded)
        {
            if (dash && controlLocked == 0)
            {
                setMovimentState(PlayerMovimentState.DASHING);
                DashMoviment();
            }
            else if (horizontalAxis != 0 && controlLocked == 0)
            {
                setMovimentState(PlayerMovimentState.WALKING);
                HorizontalMove();
            }
            else
            {
                setMovimentState(PlayerMovimentState.IDLE);
            }
        }
        else
        {
            setMovimentState(PlayerMovimentState.JUMPING);
        }

        if (jumping)
        {
            JumpPlayer();
        }

        if (!AnimationShootingIsPlaying)
        {
            if (shooting)
            {
                if (horizontalAxis == 0 && verticalAxis < 0)
                {
                    setShootingState(PlayerShootingState.SHOOTING_UP);
                }
                else if (verticalAxis > 0 && horizontalAxis == 0)
                {
                    setShootingState(PlayerShootingState.SHOOTING_DOWN);
                }
                else if (diagonalUp)
                {
                    setShootingState(PlayerShootingState.SHOOTING_DIAGONAL_UP);
                }
                else if (diagonalDown)
                {
                    setShootingState(PlayerShootingState.SHOOTING_DIAGONAL_DOWN);
                }
                else
                {
                    setShootingState(PlayerShootingState.SHOOTING);
                }

                myAnimator.SetTrigger("Shooting");
            }
            else
            {
                setShootingState(PlayerShootingState.NONE);
            }
        }

        myAnimator.SetFloat("Speed", controlLocked > 0 ? 0 : Mathf.Abs(horizontalAxis));
        myAnimator.SetBool("Ground", MovimentState != PlayerMovimentState.JUMPING);

        myAnimator.SetBool("Diagonal_up", diagonalUp);
        myAnimator.SetBool("Diagonal_down", diagonalDown);
        myAnimator.SetBool("Top", verticalAxis < 0 && horizontalAxis == 0);
        myAnimator.SetBool("Down", verticalAxis > 0 && horizontalAxis == 0);
    }



    private void DashMoviment()
    {
        myAnimator.SetTrigger("Dash");
        Vector3 moveAt = LookingState == PlayerLookingState.RIGHT ? Vector3.right : Vector3.left;
        myRigidbody.AddForce(moveAt * DashForce,ForceMode2D.Impulse);
    }
}
