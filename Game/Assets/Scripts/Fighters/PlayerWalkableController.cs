using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class PlayerWalkableController : FighterBase {
    
    //=====================================================
    // PUBLIC PROPERTIES
    //====================================================
    public float jumpForceWalking = 75f;
    public float jumpForceRunning = 90f;
    public float horizontalFoce = 30f;
    public LayerMask groundMask;
    public BoxCollider2D myCollider;
    

    //=====================================================
    // PRIVATE PROPERTIES
    //====================================================
    protected CalculeDirection SpawnsPoint;
    protected float directorFactor = 2f;

    //=====================================================
    // STATUS
    //====================================================
    protected bool Grounded
    {
        get
        {
            return Physics2D.OverlapArea(myCollider.bounds.min, myCollider.bounds.max, groundMask);
        }
    }

    new void Awake()
    {
        base.Awake();
        myTransform = transform;
        myAnimator = GetComponentInChildren<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        InitSpawnPoints();
    }

    private void InitSpawnPoints()
    {
        SpawnsPoint = myTransform.GetComponentInChildren<CalculeDirection>();
    }

    public override void Attack()
    {
        if (UsingWeapon)
        {
            
            Component projectil = Instantiate(ProjectilPrefab, SpawnsPoint.transform.position, SpawnsPoint.transform.rotation) as Component;
            BaseShot baseShot = projectil.GetComponent<BaseShot>();
            baseShot.MyOwnInstance = myTransform;
            baseShot.enabled = true;
            Vector3 lookingFace = LookingState == PlayerLookingState.RIGHT ? Vector3.right : Vector3.left;
            Vector3 direction = SpawnsPoint.GetDirection();
            baseShot.Orientation = new Vector3(direction.x * (-lookingFace.x), direction.y, 0);
        }
    }

    protected void Update()
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

    protected void UpdateState()
    {
        //AXIS
        float horizontalAxis = Input.GetAxis(MyInput.hAxis);
        float verticalAxis = Input.GetAxis(MyInput.vAxis);
        //LOCK
        float controlLocked = Input.GetAxis(MyInput.lock_control);
        //EVENTS
        bool shifting = Input.GetAxis(MyInput.shift) != 0;
        bool jumping = Input.GetButtonDown(MyInput.jump);
        bool shooting = Input.GetButtonDown(MyInput.fire1);

        //DIRECTIONS
        bool diagonalUp = Mathf.Abs(horizontalAxis) > 0 && verticalAxis < 0;
        bool diagonalDown = Mathf.Abs(horizontalAxis) > 0 && verticalAxis > 0;

        PlayerLookingState newLookingState = horizontalAxis > 0 ? PlayerLookingState.RIGHT : horizontalAxis < 0 ? PlayerLookingState.LEFT : LookingState;
        setLookState(newLookingState);

        if (Grounded)
        {
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
        }
        else
        {
            setMovimentState(PlayerMovimentState.JUMPING);
        }

        if(jumping) 
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

        myAnimator.SetFloat("Speed", controlLocked > 0 ? 0: Mathf.Abs(horizontalAxis));
        myAnimator.SetBool("Shifting", MovimentState == PlayerMovimentState.RUNNING);
        myAnimator.SetBool("Ground", MovimentState != PlayerMovimentState.JUMPING);
        myAnimator.SetBool("Diagonal_up", diagonalUp);
        myAnimator.SetBool("Diagonal_down", diagonalDown);
        myAnimator.SetBool("Top", verticalAxis < 0 && horizontalAxis == 0);
        myAnimator.SetBool("Down", verticalAxis > 0 && horizontalAxis == 0);


    }
    

    protected void JumpPlayer()
    {
        Vector2 orientation = LookingState == PlayerLookingState.RIGHT ? Vector2.right : Vector2.left;
        float newJumpForce = MovimentState == PlayerMovimentState.RUNNING ? jumpForceRunning : jumpForceWalking;

        if (Grounded)
        {
            Vector2 totalForce = Vector2.zero;
            if ( MovimentState == PlayerMovimentState.WALKING ||
                 MovimentState == PlayerMovimentState.RUNNING  )
            {
                totalForce = orientation * horizontalFoce;
            }
            setMovimentState(PlayerMovimentState.JUMPING);
            totalForce += Vector2.up * newJumpForce;
            JumpMove(totalForce);
        }
    }

    private void JumpMove(Vector2 totalForce)
    {
        myRigidbody.AddForce(totalForce, ForceMode2D.Impulse);
        setMovimentState(PlayerMovimentState.JUMPING);

    }
}
 