using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uTools;

public class GameMaster : MonoBehaviour
{
	public enum STATE
	{
		START,
		READY,
		PLAYING,
		END,
	}
	int playerid = 0;
	public STATE state = STATE.START;

	public float restGameTime = 180;
	public Text textRestTime;
	public Text myCoverageText;
	public Text enemyCoverageText;

	public GameObject[] gamePlayingUI;
	public GameObject title;
	public GameObject ready;
	public GameObject last1MinDisp;
	public GameObject finishLines;
	public GameObject coutdown;

	public Image p1WinLoseInfo;
	public Image p2WinLoseInfo;
	public Sprite p1WinSprite;
	public Sprite p1LoseSprite;
	public Sprite p2WinSprite;
	public Sprite p2LoseSprite;

	public bool showAreaResultGauge = false;
	public void SetShowAreaResult(bool b)
	{
		if (b)
		{
			float myArea = earth.CalculateMyArea();
			float enemyArea = earth.CalculateEnemyArea();
			areaResultGauge.gameObject.SetActive(true);
			areaResultGauge.SetAreaResultParams(myArea, enemyArea);
			StartCoroutine("WaitForSeconds");
		}else
		{
			showAreaResultGauge = false;
		}
	}
	IEnumerator WaitForSeconds()
	{
		yield return new WaitForSeconds(1f);
		showAreaResultGauge = true;
	}

	public AreaResultGauge areaResultGauge;

	public TweenPosition endTextTweenPos;

	float calculateAreasTime = 0;
	public void StartGameEndProc()
	{
		//endTextTweenPos.ResetToBeginning();
		//endTextTweenPos.gameObject.SetActive(true);
		//endTextTweenPos.PlayForward();
	}

	public void StartGameStartProc()
	{
	}

	public void ShowRest1MinInfo()
	{
	}

	/// <summary>
	/// Gets the state of the game.
	/// </summary>
	/// <returns>The game state.(enum STATE)</returns>
	public STATE GetGameState()
	{
		return state;
	}


	void ReadyGame()
	{
		title.SetActive(false);
		ready.SetActive(true);
	}

	void EnableGamePlayingUI(bool b)
	{
		foreach (GameObject obj in gamePlayingUI)
		{
			obj.SetActive(b);
		}
	}

	public void StartPlayGame()
	{

		EnableGamePlayingUI(true);
		state = STATE.PLAYING;
	}

	void Update()
	{
		switch (state)
		{
			case STATE.START:
				if (Input.GetKey(KeyCode.Return))
				{
					state = STATE.READY;
					ReadyGame();
				}

				break;
			case STATE.END:
				finishLines.SetActive(true);
				if (showAreaResultGauge)
				{
					//300msに1回
					calculateAreasTime += Time.deltaTime;
					if (calculateAreasTime > 0.05)
					{
						calculateAreasTime = 0;
						AreaResultGauge.STATE state = areaResultGauge.UpdateAreaPercentage();
						if(state == AreaResultGauge.STATE.RESULT)
						{
							if (GetWinnerPlayerID() == 0)
							{
								p1WinLoseInfo.sprite = p1WinSprite;
								p2WinLoseInfo.sprite = p2LoseSprite;
							}else
							{
								p1WinLoseInfo.sprite = p1LoseSprite;
								p2WinLoseInfo.sprite = p2WinSprite;
							}
							p1WinLoseInfo.enabled = true;
							p2WinLoseInfo.enabled = true;
						}
					}
				}
				return;

			case STATE.PLAYING:
				restGameTime -= Time.deltaTime;
				if (restGameTime < 1)
				{
					state = STATE.END;

					int winner = GetWinnerPlayerID();
					if (winner == 0)
					{
						Debug.Log("player 1 won!");
					}
					else
					{
						Debug.Log("player 2 won!");
					}
					StartGameEndProc();
				}

				string sec = (((int)restGameTime) % 60).ToString();
				if(((int)restGameTime) % 60 < 10)
				{
					sec = "0" + sec;
				}
				textRestTime.text = "0" + ((int)(((int)restGameTime) / 60)) + ":" + sec;
				if (restGameTime <= 61)
				{
					last1MinDisp.SetActive(true);
				}

				if (restGameTime <= 11)
				{
					coutdown.SetActive(true);
				}

				//100msに1回
				calculateAreasTime += Time.deltaTime;
				if (calculateAreasTime > 0.100)
				{
					calculateAreasTime = 0;

					Debug.Log("better= " + GetBetterPlayerID());
				}
				break;
		}

	}

	//
	//singleton game state manager
	//
	public Earth earth;

	public static GameMaster instance = null;
	void Awake()
	{

		if (instance == null)
		{
			DontDestroyOnLoad(this.gameObject);
			instance = this;
		}
		else
		{
			DestroyObject(this.gameObject);
		}
	}

	void Start()
	{
		state = STATE.START;
		//endTextTweenPos.gameObject.SetActive(false);
		EnableGamePlayingUI(false);
		title.SetActive(true);

		p1WinLoseInfo.enabled = false;
		p2WinLoseInfo.enabled = false;
	}

	public void ResetGame()
	{
		playerid = 0;
	}

	public int AddPlayer()
	{
		return playerid++;
	}

	//形成判断する
	public int GetBetterPlayerID()
	{
		float myArea = earth.CalculateMyArea();
		float enemyArea = earth.CalculateEnemyArea();

		Debug.Log("my Area coverage=" + myArea);
		Debug.Log("enemy Area coverage=" + enemyArea);

		myCoverageText.text = (int)(myArea * 100) + "%";
		enemyCoverageText.text = (int)(enemyArea * 100) + "%";
		if (myArea > enemyArea)
		{
			return 0;
		}
		else
		{
			return 1;
		}
	}

	//勝った方のIDを返す
	public int GetWinnerPlayerID()
	{
		//0か1
		float myArea = earth.CalculateMyArea();
		float enemyArea = earth.CalculateEnemyArea();

		if (myArea > enemyArea)
		{
			return 0;
		}
		else
		{
			return 1;
		}
	}
}
