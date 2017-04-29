using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {
	enum STATE{
		START,
		PLAYING,
		END,
	}
	int playerid = 0;
	//
	//singleton game state manager
	//
	public Earth earth;

	public static GameMaster instance = null;
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

	//形成判断する
	public int GetBetterPlayerID(){
		return 0;
	}

	//勝った方のIDを返す
	public int GetWinnerPlayerID(){
		//0か1
		return 0;
	}
}
