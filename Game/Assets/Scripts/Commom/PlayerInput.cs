using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInput {

	public string suffix;
	public string hAxis;
	public string vAxis;
	public string jump;
	public string fire1;
	public string fire2;
    public string shift;
    public string lock_control;

    public static Dictionary<string, PlayerInput> playerInputs =  new Dictionary<string, PlayerInput>(){
        {"player1T1", new PlayerInput("T1")},
        {"player1C1", new PlayerInput("C1")},
        {"player2T2", new PlayerInput("T2")},
        {"player2C2", new PlayerInput("C2")}
    };

	public PlayerInput(string p) {
		suffix = p;
		hAxis = "Horizontal" + p;
		vAxis = "Vertical" + p;
		jump = "Jump" + p;
		fire1 = "Fire1" + p;
		fire2 = "Fire2" + p;
        shift = "Shift" + p;
        lock_control = "Lock" + p;
    }

}
