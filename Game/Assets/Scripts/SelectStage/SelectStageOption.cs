using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class SelectStageOption : MonoBehaviour {


    GameManager gameManager;

    //Menu options
    public Transform[] options;
    public Component arrowPrefab;
    private Component arrowScene;
    private Transform myTransform;

    public float offSet = 1.8f;
    public float timeToMove = 0.3f;
    private bool _canMove = true;

    int currentPosition = 0;

    void Start()
    {
        gameManager = GameManager.instance;
        myTransform = transform;
        arrowScene = Instantiate(arrowPrefab) as Component;
        arrowScene.transform.parent = myTransform;
        StartCoroutine(moveArrowTo(0));
    }

    void CanMove()
    {
        _canMove = true;
    }

    void Update()
    {
        MoveArrow();
        LoadScene();
    }
    void LoadScene()
    {

        if (Input.GetButtonDown(gameManager.Controls.Values.ElementAt(0).fire1) ||
            Input.GetButtonDown(gameManager.Controls.Values.ElementAt(1).fire1))
        {
            Transform scene = getOptionByName(currentPosition);
            gameManager.SelectedScene = scene.name;
            gameManager.LoadVSScene();
        }
    }

    void MoveArrow()
    {
        if ( (Input.GetAxis(gameManager.Controls.Values.ElementAt(0).vAxis) < 0 ||
             Input.GetAxis(gameManager.Controls.Values.ElementAt(1).vAxis) > 0 ) && _canMove)
        {
            _canMove = false;
            currentPosition = Mathf.Clamp(--currentPosition, 0, options.Length - 1);
            StartCoroutine(moveArrowTo(currentPosition));
            Invoke("CanMove", timeToMove + 0.1f);
        }
        else if ( (Input.GetAxis(gameManager.Controls.Values.ElementAt(0).vAxis) > 0 ||
                  Input.GetAxis(gameManager.Controls.Values.ElementAt(1).vAxis) < 0) && _canMove)
        {
            _canMove = false;
            currentPosition = Mathf.Clamp(++currentPosition, 0, options.Length - 1);
            StartCoroutine(moveArrowTo(currentPosition));
            Invoke("CanMove", timeToMove + 0.1f);
        }
    }

    IEnumerator moveArrowTo(int position)
    {
        Transform option = getOptionByName(position);

        Vector3 newPosition = new Vector3(offSet, option.position.y, 0);
        arrowScene.transform.position = newPosition;
        //yield return StartCoroutine(arrowScene.transform.MoveInTime(newPosition, timeToMove));
        //Background
        GameObject.Find("Background").GetComponent<SpriteRenderer>().sprite = options[position].GetComponent<SpriteRenderer>().sprite;
        yield return null;
    }

    Transform getOptionByName(int position)
    {
       return options[position];
    }
}
