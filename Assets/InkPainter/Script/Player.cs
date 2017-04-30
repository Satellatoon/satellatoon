using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	public int energy;
	public int energyMax;
	public int playerColor;		//色(RGB32? 今はID)
	public Satellite satellite = null;	//乗ってる衛星

  public AudioClip audioPaint;
  public AudioClip audioPaintError;
  public AudioClip audioRide;
  public AudioClip audioRideError;
  public AudioClip audioRecovery;
  public Image energyGuid;

  private string inspectorName;
  private AudioSource audio = null;
  private KeyCode paintKeyCode = KeyCode.Space;
  private KeyCode recoveryKeyCode = KeyCode.Space;
  private KeyCode moveKeyCode = KeyCode.Space;
  public PlayerAnimManager playerAnimMan;

	public float waitNextJumpTimer=5f;
	float waitNextJumpTimerCnt=0f;

	enum STATE{
		WAITING,
		JUMP,
		JUMP_WAIT,	//ジャンプ硬直
	}
	STATE state = STATE.WAITING;

	public Text satelliteInfoText;
	public Image satelliteInfoImg;
	void Start(){
		this.playerColor = GameMaster.instance.AddPlayer ();
    this.audio = GetComponent<AudioSource>();
    initKeyMap ();
    initSatellite ();
		satelliteInfoImg.enabled = false;
		satelliteInfoText.enabled = false;
    this.energyGuid.fillAmount = 1;
	}

  private void rideSatelliteWithMotion(string strValue){
    //ここで飛び移るアニメーション TODO
    Shout (this.audioRide);
    rideSatellite (strValue);
  }
  public void rideSatellite(string str_value){
    if(null != this.satellite){
      this.satellite.isUserRiding = false; // 外す
    }

		str_value = str_value.Replace (" (Satellite)", "");
    this.inspectorName = str_value;
		Debug.Log ("going to " + str_value);
		Transform tmpTransform = GameObject.Find(str_value).transform;
		this.transform.parent = tmpTransform;


		Vector3 parentRight = tmpTransform.right * 10;
		this.transform.localPosition = parentRight + new Vector3 (Random.Range(-3,3),Random.Range(-3,3),Random.Range(-3,3));

		//random position
		//this.transform.localPosition = new Vector3 (Random.Range(-10,10),Random.Range(-10,10),Random.Range(-10,10));

    this.satellite = this.transform.parent.GetComponent(typeof(Satellite)) as Satellite;
    this.satellite.isUserRiding = true; // setting
  }

  void initSatellite(){

    string strValue;
    if (this.playerColor == 0) { // player1
      strValue = "isscombined"; // default
    } else { // player2
			strValue = "Aura_27"; // default
    }

    this.rideSatellite (strValue);
  }

  void initKeyMap(){
    if (this.playerColor == 0) { // player1
      this.paintKeyCode = KeyCode.A;
      this.recoveryKeyCode = KeyCode.Q;
      this.moveKeyCode = KeyCode.Z;
   } else { // player2
      this.paintKeyCode = KeyCode.L;
      this.recoveryKeyCode = KeyCode.P;
      this.moveKeyCode = KeyCode.Comma;

    }
  }
  void energyGageDown(int val){
    this.energyGuid.fillAmount = ((float)this.energy/100);
    Debug.Log ("energyGuid :" + this.energyGuid.fillAmount.ToString());
  }
  void energyGageRecovery(){
    this.energyGuid.fillAmount = 1;
    Debug.Log ("energyGuid :" + this.energyGuid.fillAmount.ToString());
  }

	public float jumpWaitTime=0.5f;
	public float jumpWaitCnt=0f;
	string strValueHolder="";
	Transform nextSatTransform;

	void Update(){
		Debug.Log (state);

		//追加してほしい
		if(null != this.satellite){
			this.satellite.isUserRiding = true; // 外す
		}

		//Debug.Log ("energyGuid :" + this.energy);
		switch(state){
		case STATE.WAITING:
			waitNextJumpTimerCnt -= Time.deltaTime;

			//spaceキーでぬる
			//time.deltaで要調整？
			this.transform.LookAt (satellite.EarthObject.transform.position);
			if (energy > satellite.energyComsumption) {
				if (Input.GetKey (this.paintKeyCode)) {
					Shout (this.audioPaint);
					energy -= satellite.Paint (playerColor);
          energyGageDown(this.energy);
				}
			} else {
				if (Input.GetKey (this.paintKeyCode)) {
					Shout (this.audioPaintError);
				}
			}

			if (energy <= satellite.energyComsumption) {
				if (Input.GetKey (this.recoveryKeyCode)) {
					Shout (this.audioRecovery);
					this.energy = this.energyMax;
          energyGageRecovery();
				}
			}
			break;
		case STATE.JUMP:
			//this.transform.parent = null;

			playerAnimMan.jump = true;
			state = STATE.JUMP_WAIT;

			waitNextJumpTimerCnt = waitNextJumpTimer;

			break;
		case STATE.JUMP_WAIT:
			jumpWaitCnt += Time.deltaTime;
			if (jumpWaitCnt > jumpWaitTime) {
				state = STATE.WAITING;
				rideSatelliteWithMotion (strValueHolder);
				jumpWaitCnt = 0;
			}
			//this.transform.position = Vector3.Lerp (this.transform.position, nextSatTransform.position, 3f);

			break;
		}

		if (GameMaster.instance.GetGameState () == GameMaster.STATE.END) {
			satelliteInfoImg.enabled = false;
			satelliteInfoText.enabled = false;
		}
	}
  // 体力まんたん
  public void lifeRecovery()
  {
    energy = energyMax;
  }
  // 体力取得
  public int GetEnergy()
  {
    return energy;
  }
	//なく
  public void Shout(AudioClip audioval){
    audio.clip = audioval;
    audio.Play();
	}
		

	void OnTriggerLeave(Collider other){
		satelliteInfoImg.enabled = false;
		satelliteInfoText.enabled = false;
	}


	void OnTriggerStay(Collider other){
		if (GameMaster.instance.GetGameState () != GameMaster.STATE.PLAYING) {
			return;
		}

		if (other.tag.Equals ("Satellite")) {
		//ほかの衛星と当たればここにくる`
		//自分以外を排除することに注意

		Satellite satellite_val = other.GetComponent(typeof(Satellite)) as Satellite;
		var strValue = satellite_val.ToString ();
		Debug.Log ("OnTriggerStay :" + strValue);


			//value.GetInstanceID
			if(this.inspectorName == strValue){
			// 自分なので何もしない
			}else{
				switch (state) {
				case STATE.WAITING:
					if (waitNextJumpTimerCnt > 0) {
						return;
					}
					break;
				default:
					return;
				}
				// 相手がのっているかチェック
				if (true == satellite_val.isUserRiding) {
					Debug.Log ("satellite:"+strValue+" is user riding........");
					if (Input.GetKey (this.moveKeyCode)) {
						Shout (this.audioRideError);
					}

					//追加してほしい
					satelliteInfoImg.enabled = false;
					satelliteInfoText.enabled = false;
				} else {
					strValue = strValue.Replace (" (Satellite)", "");
					satelliteInfoImg.enabled = true;
					satelliteInfoText.enabled = true;
					satelliteInfoText.text=strValue+"接近中!";
					if (Input.GetKey (this.moveKeyCode)) {
						Debug.Log ("moveKey push!!");
						Debug.Log ("satellite:"+strValue+" is not user riding!!");
						// のっていなければのりかえ

						state = STATE.JUMP;
						satellite_val.isUserRiding = true; // keep

						satelliteInfoImg.enabled = false;
						satelliteInfoText.enabled = false;

						//rideSatelliteWithMotion (strValue);
						strValueHolder = strValue;
						nextSatTransform = GameObject.Find(strValue).transform;  
						//乗り換えたフラグon
						state = STATE.JUMP;
					}
				}
			}
		}
	}
}
