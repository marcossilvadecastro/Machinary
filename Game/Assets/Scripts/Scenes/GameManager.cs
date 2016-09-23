using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[RequireComponent(typeof(LoadingManager))]
public class GameManager : MonoBehaviour {

    string CurrentScene = null;
    LoadingManager loadingManager;
    public static GameManager instance = null;
    private bool GameUping = true;
    private LevelScript levelManager;
    private Transform myTransform;
    public bool IsDebugging = false;
    public string[] NameOfSelectedFighters { get; set; }
    public string WinnerName { get; private set; }

    //============================
    // CONTROLS
    //===========================
    public bool ControllerConnected { private set; get; }
    public Dictionary<string, PlayerInput> Controls { private set; get; }
    public string SelectedScene { get; set; }

    void Awake()
    {
        NameOfSelectedFighters = new string[2];
        Controls = new Dictionary<string, PlayerInput>();
        //Check if instance already exists
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        InitGame();
    }

    internal void setWinnername(string nameOfPlayerDeath)
    {
        foreach (string playerName in NameOfSelectedFighters)
        {
            if (!playerName.Equals(nameOfPlayerDeath))
            {
                WinnerName = playerName;
                break;
            }
        }
    }

    internal void CreateDebugControl()
    {
        Controls["tecladoP1"] = PlayerInput.playerInputs["player2T2"];
    }

    void Start()
    {
        if (GameUping)
        {
            GameUping = false;
            CurrentScene = "IntroScene";
        }
        StartCoroutine(loadingManager.LoadAsyncScene(CurrentScene));
    }

    internal void FinishGame()
    {
        Application.Quit();
    }

    internal void LoadScene(string newScene)
    {
        CurrentScene = newScene;
        loadingManager.LoadScene("GeneraLoading");
        StartCoroutine(loadingManager.LoadAsyncScene(newScene));
    }

    internal void LoadVSScene()
    {
        CurrentScene = "Loading";
        loadingManager.LoadScene(CurrentScene);
    }



    void InitGame()
    {
        myTransform = transform;
        levelManager = new LevelScript();
        loadingManager = GetComponent<LoadingManager>();
    }

    internal void LoadNextScene()
    {
        if (CurrentScene == "SelectControlScene")
        {
            CurrentScene = "MainScene";
            loadingManager.LoadScene("GeneraLoading");
            StartCoroutine(loadingManager.LoadAsyncScene(CurrentScene));
        }
        else if (CurrentScene == "Loading")
        {
            CurrentScene = SelectedScene;
            StartCoroutine(loadingManager.LoadAsyncScene(CurrentScene));
        } else if (CurrentScene == "IntroScene")
        {
            CurrentScene = "SelectControlScene";
            loadingManager.LoadScene(CurrentScene);
        }
    }

    public void DetectController()
    {
        try
        {
            if (Input.GetJoystickNames().Count() > 0)
            {
                ControllerConnected = true;
                IdentifyController();
            }
        }
        catch
        {
            ControllerConnected = false;
        }
    }

    void IdentifyController()
    {   
        string joystickNameSecond = Input.GetJoystickNames().Count() > 1 ?  Input.GetJoystickNames()[1] : null;

        if (string.IsNullOrEmpty(joystickNameSecond))
        {
            Controls["tecladoP1"] = PlayerInput.playerInputs["player2T2"];
            Controls["controleP1"] = PlayerInput.playerInputs["player1C1"];
        }
        else
        {
            Controls["controleP1"] = PlayerInput.playerInputs["player1C1"];
            Controls["controleP1"] = PlayerInput.playerInputs["player2C2"];
        }
    }

}
