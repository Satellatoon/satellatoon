using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uTools;

public class GameMaster : MonoBehaviour {
	public enum STATE{
		START,
		PLAYING,
		END,
	}
	int playerid = 0;
	STATE state=STATE.START;

	public float restGameTime = 180;
	public Text textRestTime;
	public Text myCoverageText;
	public Text enemyCoverageText;

	public TweenPosition endTextTweenPos;

	float calculateAreasTime=0;
	public void StartGameEndProc(){
		endTextTweenPos.ResetToBeginning ();
		endTextTweenPos.gameObject.SetActive (true);
		endTextTweenPos.PlayForward ();
	}

	/// <summary>
	/// Gets the state of the game.
	/// </summary>
	/// <returns>The game state.(enum STATE)</returns>
	public STATE GetGameState(){
		return state;
	}


	void Update(){
		if (state == STATE.END) {
			return;
		}

		restGameTime -= Time.deltaTime;
		if (restGameTime<=0) {
			state = STATE.END;

			int winner = GetWinnerPlayerID ();
			if (winner == 0) {
				Debug.Log ("player 1 won!");
			} else {
				Debug.Log ("player 2 won!");
			}
			StartGameEndProc ();
		}
		textRestTime.text = ((int)restGameTime).ToString();


		switch (state) {
		case STATE.PLAYING:
		case STATE.START:
			//100msに1回
			calculateAreasTime+=Time.deltaTime;
			if (calculateAreasTime > 0.100) {
				calculateAreasTime = 0;

				Debug.Log("better= " + GetBetterPlayerID ());
			}
			break;
		}

	}

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

	void Start(){
		state = STATE.START;
		endTextTweenPos.gameObject.SetActive (false);
	}

	public void ResetGame(){
		playerid = 0;
	}

	public int AddPlayer(){
		return playerid++;
	}

	//形成判断する
	public int GetBetterPlayerID(){
		float myArea = earth.CalculateMyArea ();
		float enemyArea = earth.CalculateEnemyArea ();

		Debug.Log ("my Area coverage=" + myArea);
		Debug.Log ("enemy Area coverage=" + enemyArea);

		myCoverageText.text = (int)(myArea * 100) + "%";
		enemyCoverageText.text = (int)(enemyArea * 100) + "%";
		if (myArea > enemyArea) {
			return 0;
		} else {
			return 1;
		}
	}

	//勝った方のIDを返す
	public int GetWinnerPlayerID(){
		//0か1
		float myArea = earth.CalculateMyArea ();
		float enemyArea = earth.CalculateEnemyArea ();

		if (myArea > enemyArea) {
			return 0;
		} else {
			return 1;
		}
	}
}
