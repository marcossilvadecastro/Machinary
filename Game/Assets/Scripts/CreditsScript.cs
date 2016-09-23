using UnityEngine;
using System.Collections;

public class CreditsScript : MonoBehaviour {

	void Update () {
		if (Input.GetButtonDown ("Fire1P1") || Input.GetButtonDown ("Fire1P2"))
				Application.LoadLevel ("MainScene");

	}
}
