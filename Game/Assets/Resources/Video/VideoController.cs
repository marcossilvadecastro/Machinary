using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class VideoController : MonoBehaviour {

    public MovieTexture IntroVideo;
    MeshRenderer render;
    AudioSource myAudio;
    GameManager gameManager;
    public float AudioDelay;

    void Start()
    {
        gameManager = GameManager.instance;
        myAudio = GetComponent<AudioSource>();
        render = GetComponent<MeshRenderer>();
        render.material.mainTexture = IntroVideo as MovieTexture;
        IntroVideo.Play();
        myAudio.clip = IntroVideo.audioClip;
        myAudio.Play();
        //Load with Delay
        Invoke("NextScene", IntroVideo.duration - AudioDelay);
    }
    
    void NextScene()
    {
        gameManager.LoadNextScene();
    }

    void Update()
    {
        if (Input.anyKey)
        {
            NextScene();
        }
    }
}
