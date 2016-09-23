using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlController : MonoBehaviour {
	
	public enum Position{
		Left,
		Right,
        Center
	};
    public Position[] Positions = { Position.Left, Position.Center, Position.Right };
    public int CurrentIndexPosition = 1;
    public Position CurrentPosition = Position.Center;

    public float DistanceToMove = 0f;
    public float SpeedMovement = 0.3f;
    public List<Sprite> sprites;
    private bool _canMove = true;

    private Transform myTransform;
    public PlayerInput MyInput { private get; set; }

	void Start(){
        myTransform = transform;
	}

    private void CanMove()
    {
        _canMove = true;
    }

    void Update()
    {
        if (MyInput != null)
        {
            // && Input.GetButtonDown(MyInput.hAxis) &&  _canMove
            if (Input.GetAxis(MyInput.hAxis) < 0 && _canMove)
            {
                CurrentIndexPosition--;
                MoveComponent();

            }
            //&& Input.GetButtonDown(MyInput.hAxis) && _canMove
            else if (Input.GetAxis(MyInput.hAxis) > 0 && _canMove)
            {
                CurrentIndexPosition++;
                MoveComponent();
            }

            if (Input.GetButtonDown(MyInput.fire1))
            {
                if (OnPlayerSelectedHandler != null)
                {
                    OnPlayerSelectedHandler(this);
                    _canMove = false;
                }
            }
        }
    }
	
	private void MoveComponent(){
        CurrentIndexPosition = Mathf.Clamp(CurrentIndexPosition, 0, Positions.Length - 1);
        _canMove = false;
        //Coloca um delay de 0.1 sec
        Invoke("CanMove", SpeedMovement + 0.1f);
        StartCoroutine(Move (Positions[CurrentIndexPosition]));
	}
	
	
	IEnumerator Move(Position position){
        if (CurrentPosition == position)
        {
            yield break;
        }

        Vector3 oldPosition = myTransform.position;
		switch (position) {
		    case Position.Left:
			    myTransform.GetComponent<SpriteRenderer>().sprite = sprites[0];
                CurrentPosition = position;
                yield return StartCoroutine(myTransform.MoveInTime(oldPosition + new Vector3(-DistanceToMove, 0, 0),SpeedMovement)); 
			    break;
		    case Position.Right:
			    myTransform.GetComponent<SpriteRenderer>().sprite = sprites[1];
                CurrentPosition = position;
                yield return StartCoroutine(myTransform.MoveInTime(oldPosition + new Vector3(DistanceToMove, 0, 0), SpeedMovement));
                break;
            case Position.Center:
                Vector3 distance = CurrentPosition == Position.Right ? new Vector3(-DistanceToMove, 0, 0) : new Vector3(DistanceToMove, 0, 0);
                CurrentPosition = position;
                yield return StartCoroutine(myTransform.MoveInTime(oldPosition + distance, SpeedMovement));
                break;
		}
        
	}

    public delegate void OnPlayerSelected(ControlController control);
    public static OnPlayerSelected OnPlayerSelectedHandler;
    
}
