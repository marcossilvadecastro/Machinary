using UnityEngine;
using System.Collections;

public class OffsetScroller : MonoBehaviour {

    public float ScrollSpeed = 0.5f;
    public float Smoothing = 0.5f;
    public Camera myCamera;
    private Vector3 LastPosition;
    private Transform myTransform;

    void Start()
    {
        myTransform = transform;
        LastPosition = myCamera.transform.position;
    }


    void Update() {
        
        float parallax = (LastPosition.x - myCamera.transform.position.x) * ScrollSpeed;

        float positionX = myTransform.position.x + parallax;
        myTransform.position = Vector3.Lerp(
                                myTransform.position,
                                new Vector3(positionX, myTransform.position.y, myTransform.position.z),
                                Smoothing * Time.deltaTime
                               );
        LastPosition = myCamera.transform.position;
    }
}
