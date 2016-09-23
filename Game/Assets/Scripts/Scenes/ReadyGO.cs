using UnityEngine;
using System.Collections;

public class ReadyGO : MonoBehaviour {


    public Transform myTransform;

    void Awake()
    {
        myTransform = transform;
        float ortoSize = Camera.main.orthographicSize/5f;
        myTransform.localScale = new Vector3(ortoSize, ortoSize, 1);
    }

    void Update()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        cameraPosition.z = 0;
        myTransform.position = cameraPosition;
    }

    public void OnAnimationReadyGOFinish()
    {        
        if (OnReadySceneHandler != null)
        {
            OnReadySceneHandler(this);
        }
    }

    public void OnAnimationKOGOFinish()
    {

        if (OnKOSceneHandler != null)
        {
            OnKOSceneHandler(this);
        }
    }


    public delegate void OnReadyScene(ReadyGO rgComponent);
    public delegate void OnKOScene(ReadyGO rgComponent);
    public static OnReadyScene OnReadySceneHandler;
    public static OnReadyScene OnKOSceneHandler;
}
