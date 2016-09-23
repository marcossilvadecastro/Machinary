using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class WinController : MonoBehaviour {

    GameManager gameManager;
    Transform myTtansform;
    public GameObject Background;
    PlayerInput inputP1, inputP2;
    bool _canMove;
    public float TimeToResponseCounterKeyBoard;
    public Transform[] options;
    private int CurrentIndex = 0;

    void Awake()
    {
        gameManager = GameManager.instance;
        _canMove = true;
        CurrentIndex = 0;
    }

    private void CanMove()
    {
        _canMove = true;
    }


    void Start () {
        SetBackground();

        inputP1 = gameManager.Controls.Values.ElementAt(0);
        inputP2 = gameManager.Controls.Values.ElementAt(1);
    }

    private void SetBackground()
    {
        string name = gameManager.WinnerName.Replace(" ","").ToLower();
        GameObject winnerObject = GameObject.Find(name);
        SpriteRenderer renderBackground = Background.GetComponent<SpriteRenderer>();
        SpriteRenderer renderWinner = winnerObject.GetComponent<SpriteRenderer>();
        renderBackground.sprite = renderWinner.sprite;
    }


    void Update () {
        if ((Input.GetAxis(inputP1.vAxis) < 0 || Input.GetAxis(inputP2.vAxis) > 0) && _canMove)
        {
            Invoke("CanMove", TimeToResponseCounterKeyBoard + 0.1f);
            CurrentIndex++;
            SwapOption();
        }
        else if ((Input.GetAxis(inputP1.vAxis) > 0 || Input.GetAxis(inputP2.vAxis) < 0) && _canMove)
        {
            Invoke("CanMove", TimeToResponseCounterKeyBoard + 0.1f);
            CurrentIndex--;
            SwapOption();
        }

        LoadScene();
    }

    void LoadScene()
    {
        if (Input.GetButtonDown(inputP1.fire1) ||
            Input.GetButtonDown(inputP2.fire1))
        {
            gameManager.LoadScene("SelectPlayersScene");
        }
    }

    private void SwapOption()
    {
        CurrentIndex = Mathf.Clamp(CurrentIndex, 0, options.Length - 1);
        options[CurrentIndex].gameObject.SetActive(true);
        for (int i = 0; i < options.Length ; i++)
        {
            if (i != CurrentIndex)
            {
                options[i].gameObject.SetActive(false);
            }
        }
    }
}
