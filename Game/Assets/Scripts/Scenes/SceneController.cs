using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class SceneController : MonoBehaviour {

    LevelScript levelController = new LevelScript();
    Transform myTransform;
    public Component[] readyKOComponets;
    private bool GameReady { get; set; }
    private GameObject[] PlayersCached = new GameObject[2];
    private PlayerInput inputP1, inputP2;
    private GameManager gameManager;

    void Awake()
    {
        myTransform = transform;
        gameManager = GameManager.instance;
        GameReady = false;
        Transform[] spawnPoints = SearchForSpawnPints();
        levelController.Init(spawnPoints);


        ReadyGO.OnReadySceneHandler += ChangeGameToReady;
        ReadyGO.OnKOSceneHandler += ChangeGameToFinish;
        FighterBase.OnDieHandler += OnPlayerWin;

        inputP1 = gameManager.Controls.Values.ElementAt(0);
        inputP2 = gameManager.Controls.Values.ElementAt(1);
    }

    void OnPlayerWin(string Name)
    {
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.enabled = false;
        Vector3 cameraPosition = Camera.main.transform.position;
        cameraPosition.z = 0;
        gameManager.setWinnername(Name);
        levelController.Instanciate(readyKOComponets[1], cameraPosition);
    }

    void OnDisable()
    {
        ReadyGO.OnReadySceneHandler -= ChangeGameToReady;
        ReadyGO.OnKOSceneHandler -= ChangeGameToFinish;
        FighterBase.OnDieHandler += OnPlayerWin;
    }

	// Use this for initialization
	void Start () {
        //====================== TEXTE
        //TextPLayers();
        //==========================
        Loadplayers();
       
        Vector3 cameraPosition = Camera.main.transform.position;
        cameraPosition.z = 0;
        levelController.Instanciate(readyKOComponets[0], cameraPosition);
    }

    private void Loadplayers()
    {

        PlayersCached[0] = levelController.InstanciateFighter(gameManager.NameOfSelectedFighters[0], inputP1);
        EnablePlayer(ref PlayersCached[0], false);
        PlayersCached[1] = levelController.InstanciateFighter(gameManager.NameOfSelectedFighters[1], inputP2);
        EnablePlayer(ref PlayersCached[1], false);
    }

    private void TextPLayers()
    {
        //====================  TESTE ====================
        PlayersCached[0] = levelController.InstanciateFighter("Creator", PlayerInput.playerInputs["player1C1"]);
        EnablePlayer(ref PlayersCached[0], false);
        PlayersCached[1] = levelController.InstanciateFighter("Bubbles", PlayerInput.playerInputs["player2T2"]);
        EnablePlayer(ref PlayersCached[1], false);
        //=================================================
    }

    void EnablePlayer(ref GameObject player, bool enabled)
    {
        FighterBase fighter = player.GetComponent<FighterBase>();
        fighter.IsReady = enabled;
    }

    public void ChangeGameToFinish(ReadyGO rgComponent)
    {
        GameReady = false;
        Destroy(rgComponent.gameObject);

        //Lançar o menu
        gameManager.LoadScene("WinScene");

    }
    public void ChangeGameToReady(ReadyGO rgComponent)
    {
        GameReady = true;
        Destroy(rgComponent.gameObject);
        EnablePlayer(ref PlayersCached[0], true);
        EnablePlayer(ref PlayersCached[1], true);
    }

    Transform[] SearchForSpawnPints()
    {
        Transform fatherSpawnPonits  = myTransform.FindChild("SpawnPoints");
        Transform[] spawnPoints = fatherSpawnPonits.GetComponentsInChildren<Transform>();
        return spawnPoints.Where(s => s != fatherSpawnPonits).ToArray();
    }
}
