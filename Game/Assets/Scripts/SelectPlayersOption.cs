using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class SelectPlayersOption : MonoBehaviour
{
    private GameManager gameManager;
    public static string[] players = { "ArrowP1", "ArrowP2" };
    public Component[] options;
    private float timeToMove = 0.2f;
    private bool _canMove = true;
    private Dictionary<string, Sprite> dicSpriteNames = new Dictionary<string, Sprite>();
    private List<FightersManager.Fighter> simpleFighters;
    public Transform[] Names;
    public Transform[] Players;

    public GameObject[] InstanciatedPLayers; 

    private int positionHeroP1 = 0;
    private int positionHeroP2 = 6;


    private bool lockP2 = false;
    private bool lockP1 = false;

    IEnumerator Start()
    {
        InstanciatedPLayers = new GameObject[2];
        gameManager = GameManager.instance;
        simpleFighters = FightersManager.SimpleFighters;
        LoadSpritesName();

        positionHeroP1 = 0;
        positionHeroP2 = options.Length -1;
       yield return StartCoroutine(MoveArrow(1, positionHeroP1));
       yield return StartCoroutine(MoveArrow(2, positionHeroP2));
    }

    private void LoadSpritesName()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/SelectPlayersScreen/Names");

        foreach (Sprite s in sprites)
        {
           dicSpriteNames.Add(s.name,s);
        }
    }

    void Update()
    {
        UpdatePlayer1();
        if (!gameManager.IsDebugging)
        {
            UpdatePlayer2();
        }
        SaveHero();
    }

    void CanMove()
    {
        _canMove = true;
    }

    void UpdatePlayer1()
    {
        string vAxis = gameManager.Controls.Values.ElementAt(0).vAxis;
        if (Input.GetAxis(vAxis) < 0  && _canMove)
        {
            positionHeroP1--;
            _canMove = false;
            Invoke("CanMove", timeToMove + 0.1f);
            positionHeroP1 = Mathf.Clamp(positionHeroP1, 0, options.Length - 1);
            StartCoroutine(MoveArrow(1,positionHeroP1));

        }
        else if (Input.GetAxis(vAxis) > 0 && _canMove)
        {
            positionHeroP1++;
            _canMove = false;
            Invoke("CanMove", timeToMove + 0.1f);
            positionHeroP1 = Mathf.Clamp(positionHeroP1, 0, options.Length - 1);
            StartCoroutine(MoveArrow(1,positionHeroP1));
        }
    }

    void UpdatePlayer2()
    {
        string vAxis = gameManager.Controls.Values.ElementAt(1).vAxis;
        if (Input.GetAxis(vAxis) < 0 && _canMove)
        {
            positionHeroP2++;
            _canMove = false;
            Invoke("CanMove", timeToMove + 0.1f);
            positionHeroP2 = Mathf.Clamp(positionHeroP2, 0, options.Length - 1);
            StartCoroutine(MoveArrow(2,positionHeroP2));
        }
        else if (Input.GetAxis(vAxis) > 0 && _canMove)
        {
            positionHeroP2--;
            _canMove = false;
            Invoke("CanMove", timeToMove + 0.1f);
            positionHeroP2 = Mathf.Clamp(positionHeroP2, 0, options.Length - 1);
            StartCoroutine(MoveArrow(2,positionHeroP2));
        }
    }

    IEnumerator MoveArrow(int player,int position)
    {
        int index = player - 1;
        Transform arrow = GameObject.Find(players[index]).transform;
        StartCoroutine(arrow.MoveInTime(options[position].transform.position,timeToMove));
        string name = options[position].name;
        string spriteName = string.Format("p{0}_{1}",player,name.Replace(" ", "").ToLower());

        //Change sprite name
        Sprite newSprite = dicSpriteNames[spriteName];
        Names[index].GetComponent<SpriteRenderer>().sprite = newSprite;

        //change sprite prefab
        FightersManager.Fighter playerPref = simpleFighters.Where(f => f.name == name).First();
        Destroy(InstanciatedPLayers[index]);
        GameObject newPlayer = Instantiate(playerPref.prefab);
        newPlayer.transform.parent = transform;
        InstanciatedPLayers[index] = newPlayer;
        newPlayer.transform.position = Players[index].position;
        if (player == 1)
        {
            Vector3 scale = newPlayer.transform.localScale;
            scale.x *= -1;
            newPlayer.transform.localScale = scale;
        }
        yield break;
    }

    void SaveHero()
    {
        if (Input.GetButtonDown(gameManager.Controls.Values.ElementAt(0).fire1))
        {
            gameManager.NameOfSelectedFighters[0] = InstanciatedPLayers[0].GetComponent<PlayerSimple>().Name;
            Animator anim =  InstanciatedPLayers[0].GetComponentInChildren<Animator>();
            anim.SetTrigger("fire");
            
            StartCoroutine(LockPLayer(InstanciatedPLayers[0], true));
        }

        if (!gameManager.IsDebugging && Input.GetButtonDown(gameManager.Controls.Values.ElementAt(1).fire1))
        {
            gameManager.NameOfSelectedFighters[1] = InstanciatedPLayers[1].GetComponent<PlayerSimple>().Name;
            Animator anim = InstanciatedPLayers[1].GetComponentInChildren<Animator>();
            anim.SetTrigger("fire");
            StartCoroutine(LockPLayer(InstanciatedPLayers[1], true));
        }
    }

    private IEnumerator LockPLayer(GameObject player, bool locked)
    {
        if (player.GetInstanceID().Equals(InstanciatedPLayers[0].GetInstanceID()))
        {
            lockP1 = locked;
        } else if (player.GetInstanceID().Equals(InstanciatedPLayers[1].GetInstanceID()))
        {
            lockP2 = locked;
        }
        if ((lockP2 == true && lockP1 == true) || (lockP1 && gameManager.IsDebugging))
        {
            yield return new WaitForSeconds(2.5f);
            gameManager.LoadScene("SelectStageScene");
        }
    }
}
