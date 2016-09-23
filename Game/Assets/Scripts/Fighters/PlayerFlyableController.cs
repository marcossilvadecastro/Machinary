using UnityEngine;
using System.Collections;
using System;

public class PlayerFlyableController : FighterBase
{
    protected CalculeDirection SpawnsPoint;

    public override void Attack()
    {

        Component projectil = Instantiate(ProjectilPrefab, SpawnsPoint.transform.position, SpawnsPoint.transform.rotation) as Component;
        BaseShot baseShot = projectil.GetComponent<BaseShot>();
        baseShot.MyOwnInstance = myTransform;
        baseShot.enabled = true;
        Vector3 lookingFace = LookingState == PlayerLookingState.RIGHT ? Vector3.right : Vector3.left;
        Vector3 direction = SpawnsPoint.GetDirection();
        baseShot.Orientation = new Vector3(direction.x * (-lookingFace.x), direction.y, 0);
    }

    void Start()
    {
        SpawnsPoint = myTransform.GetComponentInChildren<CalculeDirection>();
    }

    new void Awake()
    {
        base.Awake();
        myTransform = transform;
        myAnimator = GetComponentInChildren<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (IsReady)
        {
            UpdateState();
        }
        else
        {
            myAnimator.SetFloat("Speed", 0);
        }
    }

    private void UpdateState()
    {
        //AXIS
        float horizontalAxis = Input.GetAxis(MyInput.hAxis);
        float verticalAxis = Input.GetAxis(MyInput.vAxis);

        //LOCK
        float controlLocked = Input.GetAxis(MyInput.lock_control);

        //EVENTS
        bool shifting = Input.GetAxis(MyInput.shift) != 0;
        bool shooting = Input.GetButtonDown(MyInput.fire1);

        //DIRECTIONS
        bool diagonalUp = Mathf.Abs(horizontalAxis) > 0 && verticalAxis < 0;
        bool diagonalDown = Mathf.Abs(horizontalAxis) > 0 && verticalAxis > 0;

        //LOOKING STATE
        PlayerLookingState newLookingState = horizontalAxis > 0 ? PlayerLookingState.RIGHT : horizontalAxis < 0 ? PlayerLookingState.LEFT : LookingState;
        setLookState(newLookingState);

        PlayerLookingState flyLookingState = verticalAxis > 0 ? PlayerLookingState.DOWN : verticalAxis < 0 ? PlayerLookingState.UP : LookingState;

        if (shifting && horizontalAxis != 0 && controlLocked == 0)
        {
            setMovimentState(PlayerMovimentState.RUNNING);
            HorizontalMove();
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


        if (shifting && verticalAxis != 0 && controlLocked == 0)
        {
            setMovimentState(PlayerMovimentState.RUNNING);
            VerticalMove(flyLookingState);
        }
        else if (verticalAxis != 0 && controlLocked == 0)
        {
            setMovimentState(PlayerMovimentState.WALKING);
            VerticalMove(flyLookingState);
        }
        else
        {
            setMovimentState(PlayerMovimentState.IDLE);
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
        myAnimator.SetBool("Shifting", MovimentState == PlayerMovimentState.RUNNING);
        myAnimator.SetBool("Diagonal_up", diagonalUp);
        myAnimator.SetBool("Diagonal_down", diagonalDown);
        myAnimator.SetBool("Top", verticalAxis < 0 && horizontalAxis == 0);
        myAnimator.SetBool("Down", verticalAxis > 0 && horizontalAxis == 0);
    }

    private void VerticalMove(PlayerLookingState flyLookingState)
    {
        Vector3 moveAt = flyLookingState == PlayerLookingState.UP ? Vector3.up : Vector3.down;
        float newSpeed = MovimentState == PlayerMovimentState.RUNNING ? speedRunning : speedWalking;
        myTransform.Translate(moveAt * newSpeed * Time.deltaTime);
    }
}
