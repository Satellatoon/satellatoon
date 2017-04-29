using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour {
	enum STATE{
		START,
		PLAYING,
		END,
	}
	int playerid = 0;
	//
	//singleton game state manager
	//
	public static GameMan instance = null;
	void Awake(){
		
		if (instance == null) {
			DontDestroyOnLoad (this.gameObject);
			instance = this;
		} else {
			DestroyObject (this.gameObject);
		}
	}

	public void ResetGame(){
		playerid = 0;
	}

	public int AddPlayer(){
		return playerid++;
	}
}
