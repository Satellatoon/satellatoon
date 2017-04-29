using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public int energy;
	public int energyMax;
	public int playerColor;		//色(RGB32? 今はID)

	public Satellite satellite;	//乗ってる衛星
  private string inspectorName;
  private AudioSource audio = null;
  private KeyCode paintKeyCode = KeyCode.Space;
  private KeyCode recoveryKeyCode = KeyCode.Space;
  private KeyCode moveKeyCode = KeyCode.Space;

	void Start(){
		this.playerColor = GameMaster.instance.AddPlayer ();
    this.audio = GetComponent<AudioSource>();
    initKeyMap ();
    initSatellite ();
	}

  public void rideSatellite(string strValue){
		//ここで飛び移るアニメーション TODO
    //音とか TODO

    this.inspectorName = strValue;
    this.transform.parent = GameObject.Find(strValue).transform;   
    this.satellite = this.transform.parent.GetComponent(typeof(Satellite)) as Satellite;
  }

  void initSatellite(){

    string strValue;
    if (this.playerColor == 0) { // player1
      strValue = "Satellite1"; // default
    } else { // player2
      strValue = "Satellite2"; // default
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

	void Update(){
		//spaceキーでぬる
		//time.deltaで要調整？

    if (energy > satellite.energyComsumption) {
      if (Input.GetKey (this.paintKeyCode)) {
        Shout ();
        energy -= satellite.Paint (playerColor);
      }
    } else {
      // TODO 鳴きたい
    }

    if (energy <= satellite.energyComsumption) {
      if (Input.GetKey (this.recoveryKeyCode)) {
        // TODO 鳴きたい
        this.energy = this.energyMax;
      }
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
	public void Shout(){
    // playerごとの音を出すメソッド TODO
    audio.Play();
	}
		
	void OnTriggerStay(Collider other){
		if (other.tag.Equals ("Satellite")) {
			//ほかの衛星と当たればここにくる`
			//自分以外を排除することに注意

      Satellite satellite_val = other.GetComponent(typeof(Satellite)) as Satellite;
      var strValue = satellite_val.ToString ();
      Debug.Log ("OnTriggerStay :" + strValue);

      if (Input.GetKey (this.moveKeyCode)) {
        Debug.Log ("moveKey push!!");

        //value.GetInstanceID
        if(this.inspectorName == strValue){
          // 自分なので何もしない
        }else{
          // 相手がのっているかチェック TODO
          if (true == satellite_val.isUserRiding) {
            Debug.Log ("satellite:"+strValue+" is user riding........");
            // TODO 鳴く
          } else {
            Debug.Log ("satellite:"+strValue+" is not user riding!!");
            // のっていなければのりかえ
            rideSatellite (strValue);
          }
        }
      }
		}
	}
}
