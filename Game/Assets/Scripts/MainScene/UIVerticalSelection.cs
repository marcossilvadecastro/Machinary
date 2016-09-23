using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIVerticalSelection : MonoBehaviour
{
    public Component[] options;
    public List<Sprite> sprites;
    private Transform arrow;
    int currentPosition = 0;
	private float TimeToResponseCounterKeyBoard = 0.2f;
    private bool _canMove = true;
    GameManager gameManager { get; set; }

    void Start ()
	{
        gameManager = GameManager.instance;
        arrow = GameObject.Find("arrow").transform;
        currentPosition = 0;
        MoveArrowTo();
	}

	void Update(){
		MoveArrow ();
		LoadScene ();
	}

    private void CanMove()
    {
        _canMove = true;
    }


	void LoadScene()
    {
        if (Input.GetButtonDown(gameManager.Controls.Values.ElementAt(0).fire1) ||
            Input.GetButtonDown(gameManager.Controls.Values.ElementAt(1).fire1))
        {
            switch (currentPosition)
            {
                case 0 :
                    gameManager.LoadScene("SelectPlayersScene");
                    break;
                case 1 :
                    Debug.Log("Not implemented yet");
                    break;
                case 2:
                    gameManager.LoadScene("Credits");
                    break;
                case 3:
                    gameManager.FinishGame();
                    break;
            }
        }
	}

	void MoveArrow ()
	{
		if ((Input.GetAxis(gameManager.Controls.Values.ElementAt(0).vAxis) < 0 || Input.GetAxis(gameManager.Controls.Values.ElementAt(1).vAxis) > 0) && _canMove)
        {
            _canMove = false;
            Invoke("CanMove", TimeToResponseCounterKeyBoard + 0.1f);
            currentPosition--;
            MoveArrowTo ();
		}
        else if((Input.GetAxis(gameManager.Controls.Values.ElementAt(0).vAxis) > 0 || Input.GetAxis(gameManager.Controls.Values.ElementAt(1).vAxis) < 0) && _canMove)
        {
            _canMove = false;
           Invoke("CanMove", TimeToResponseCounterKeyBoard + 0.1f);
           currentPosition++;
           MoveArrowTo ();
        }
	}

    void MoveArrowTo()
    {
        currentPosition = Mathf.Clamp(currentPosition, 0, options.Length - 1);
        arrow.transform.position = options[currentPosition].transform.position;
        //yield return StartCoroutine(arrow.transform.MoveInTime(options[currentPosition].transform.position,TimeToResponseCounterKeyBoard));
        arrow.GetComponent<SpriteRenderer>().sprite = sprites[currentPosition];
    }
}
