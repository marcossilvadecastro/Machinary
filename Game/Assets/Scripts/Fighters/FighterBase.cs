using UnityEngine;
using System.Collections;
using System;

public abstract class FighterBase : MonoBehaviour {

    //=====================================================
    //STATES
    //====================================================
    public PlayerMovimentState MovimentState { get { return myMovimentState; } }
    public PlayerLookingState LookingState { get { return myLookingState; } }
    public PlayerShootingState ShootingState { get { return myShootingState; } }
    

    private PlayerShootingState myShootingState = PlayerShootingState.NONE;
    private PlayerMovimentState myMovimentState = PlayerMovimentState.IDLE;
    private PlayerLookingState myLookingState = PlayerLookingState.LEFT;
    public bool IsReady { get; set; }

    //=====================================================
    // PUBLIC PROPERTIES
    //====================================================
    public string playerName;
	public PlayerInput MyInput;
    public GameObject DiePrefab;
    private float health = 100f;

    public Component ProjectilPrefab;

    //=====================================================
    // PROTECTED PROPERTIES
    //====================================================
    protected Rigidbody2D myRigidbody;
    protected Animator myAnimator;
    protected Transform myTransform;
    protected HealthScript myHealthScript;
    protected Sprite myBgLive;
    protected Sprite myRankSprite;


    protected void Awake()
    {
        IsReady = false;
        myHealthScript = GetComponentInChildren<HealthScript>();
    }

    //=====================================================
    // STATUS
    //====================================================
    public float speedWalking = 5f;
    public float speedRunning = 7f;
    public bool IsAttaking { get; set; }
    public bool UsingWeapon = true;

    public void Die () {
        //Notify Manager
        if (OnDieHandler != null)
        {
            OnDieHandler(playerName);
        }
        myHealthScript.ApplyDamage(0);
        Instantiate(DiePrefab, myTransform.position, Quaternion.identity);
        Destroy(myTransform.gameObject);
    }

    public void ApplyDamage(float value)
    {
        health -= value;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            myHealthScript.ApplyDamage(health);
        }
    }

    public abstract void Attack();

    public void setMovimentState(PlayerMovimentState newMovimentState)
    {
        if (newMovimentState != MovimentState)
        {
            myMovimentState = newMovimentState;
        }
    }

    public void setShootingState(PlayerShootingState newShootingState)
    {
        if (newShootingState != ShootingState)
        {
            myShootingState = newShootingState;
        }
    }

    public void setLookState(PlayerLookingState newLookingState)
    {
        if (newLookingState != LookingState)
        {
            myLookingState = newLookingState;
            Vector3 newScale = myTransform.localScale;
            newScale.x *= -1;
            myTransform.localScale = newScale;
        }
    }

    public bool IsShooting
    {
        get
        {
            return ShootingState != PlayerShootingState.NONE;
        }
    }

    protected void HorizontalMove()
    {
        Vector3 moveAt = LookingState == PlayerLookingState.RIGHT ? Vector3.right : Vector3.left;
        float newSpeed = MovimentState == PlayerMovimentState.RUNNING ? speedRunning : speedWalking;
        myTransform.Translate(moveAt * newSpeed * Time.deltaTime);
    }

    public bool AnimationShootingIsPlaying { get; internal set; }

    public delegate void OnDie(string fighter);
    public static OnDie OnDieHandler;
}
