using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {
    float[] positionsX, positionsY;
	public float camSpeed = 3;
	private Vector3  finalLookAt;
	public Vector2 cameraBuffer = new Vector2(5f,5f);
    public float maxCameraSize = 32f;

	void Start(){
		finalLookAt = transform.position;
	}

	void LateUpdate() {
		CalculateNewCameraPosition();
	}
	
	void CalculateNewCameraPosition(){
		Component[] players = getPlyers();
        if (players.Length == 0)
        {
            return;
        }
        else {
            positionsX = new float[players.Length];
            positionsY = new float[players.Length];
        }

		Vector3 cameraCenter = new Vector3 (0f, 0f, 0f);
		for (int i = 0; i < players.Length;i++) {
			cameraCenter += players[i].transform.position;
            positionsX[i] = players[i].transform.position.x;
            positionsY[i] = players[i].transform.position.y;
        }
		Vector3 finalCameraCenter = cameraCenter;
		finalCameraCenter = cameraCenter/players.Length;
		
		var rot  = Quaternion.Euler (new Vector3(0f,0f,0f));
		Vector3 pos = rot * new Vector3(0f, 0f, -10) + finalCameraCenter;
		transform.rotation = rot;
		transform.position = Vector3.Lerp(transform.position, pos, camSpeed * Time.deltaTime);
		finalLookAt = Vector3.Lerp (finalLookAt, finalCameraCenter, camSpeed * Time.deltaTime);
		transform.LookAt(finalLookAt);

		float sizeX = Mathf.Max(positionsX) - Mathf.Min(positionsX) + cameraBuffer.x;
		float sizeY = Mathf.Max(positionsY) - Mathf.Min(positionsY) + cameraBuffer.y;

		float camSize = (sizeX > sizeY ? sizeX : sizeY);
        Camera camera = GetComponent<Camera>();
        camera.orthographicSize = Mathf.Clamp(camSize * 0.5f, 8, maxCameraSize);
    }

	Component[] getPlyers(){
		return (Component[])GameObject.FindObjectsOfType(typeof(FighterBase));
	}
}