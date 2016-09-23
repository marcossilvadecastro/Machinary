using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadBackgroundController : MonoBehaviour {

    GameManager gameManager;
    Transform myTtansform;
    public GameObject Background;



    void Awake()
    {
        gameManager = GameManager.instance;
    }


    void Start()
    {
        SetBackground();
        Invoke("ShowNextScene", 1f);
    }

    void ShowNextScene()
    {
        gameManager.LoadNextScene();
    }

    private void SetBackground()
    {
        string[] names = gameManager.NameOfSelectedFighters;
        string name = string.Format("{0}vs{1}", names[0].Replace(" ", "").ToLower(), names[1].Replace(" ", "").ToLower());
        Sprite selectedSprite = Resources.Load<Sprite>("Sprites/LoadingScreen/" + name);
        Image renderBackground = Background.GetComponent<Image>();
        renderBackground.sprite = selectedSprite;
    }
}
