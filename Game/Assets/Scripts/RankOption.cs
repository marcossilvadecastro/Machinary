using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RankOption : MonoBehaviour {

	public Image winner;
	public Image loser;
	public Text winnerText;
	public Text loserText;

	void Awake () {
		/*FighterBase P1 = SelectPlayersOption.selectedFighters[0].fighter.prefab.GetComponent<FighterBase>();
		if (P1 == null) Debug.LogError("Precisa de imagem p quando vencer");
		FighterBase P2 = SelectPlayersOption.selectedFighters[1].fighter.prefab.GetComponent<FighterBase>();
		if (P2 == null) Debug.LogError("Precisa de imagem p quando vencer");
		if(LivesManager.GetP1Live() == 0) {
			winner.sprite = P2.rankSprite;
			winnerText.text = P2.rankSprite.name;
			loser.sprite = P1.rankSprite;
			loserText.text = P1.rankSprite.name;
		} else {
			winner.sprite = P1.rankSprite;
			winnerText.text = P1.rankSprite.name;
			loser.sprite = P2.rankSprite;
			loserText.text = P2.rankSprite.name;
		}*/
	}

	void Update () {
		for (int i = 0; i < PlayerInput.playerInputs.Count; i++) {
			if (Input.GetButtonDown (PlayerInput.playerInputs["teclado"].fire1))
				Application.LoadLevel ("Level1Scene");
			else if (Input.GetButtonDown (PlayerInput.playerInputs["teclado"].fire2))
				Application.LoadLevel ("SelectPlayersScene");
			else if (Input.GetButtonDown (PlayerInput.playerInputs["teclado"].jump))
				Application.LoadLevel ("MainScene");
		}
	}
}
