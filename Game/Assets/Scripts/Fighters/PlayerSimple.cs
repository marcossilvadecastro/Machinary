using UnityEngine;
using System.Collections;

public class PlayerSimple : MonoBehaviour {

    // Use this for initialization
    public string Name;
    private Animator myAnimator;
    public PlayerInput MyInput;
    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (MyInput == null)
        {
            return;
        }

        if (Input.GetButtonDown(MyInput.fire1))
        {
            myAnimator.SetTrigger("fire");
        }
    }






}
