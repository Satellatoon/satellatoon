using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public int energy;
	public int energyMax;
	public int playerColor;		//色(RGB32? 今はID)

	public Satellite satellite;	//乗ってる衛星
  private AudioSource audio = null;
  private KeyCode paintKeyCode = KeyCode.Space;
  private KeyCode recoveryKeyCode = KeyCode.Space;
  private KeyCode moveKeyCode = KeyCode.Space;

	void Start(){
		this.playerColor = GameMaster.instance.AddPlayer ();
    this.audio = GetComponent<AudioSource>();
    initKey ();
	}

	public void RideSat(Satellite selectedNewSat){
		//ここで飛び移るアニメーションとか TODO

		satellite = selectedNewSat;
	}

  void initKey(){
    if (this.playerColor == 0) { // player1
      this.paintKeyCode = KeyCode.L;
      this.recoveryKeyCode = KeyCode.P;
      this.moveKeyCode = KeyCode.Comma;
    } else { // player2
      this.paintKeyCode = KeyCode.A;
      this.recoveryKeyCode = KeyCode.Q;
      this.moveKeyCode = KeyCode.Z;
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
			//ほかの衛星と当たればここにくる
			//自分以外を排除することに注意

      if (Input.GetKey (this.moveKeyCode)) {
        Satellite value = other.GetComponent(typeof(Satellite)) as Satellite;

        if(true == value.Equals(this.satellite)){
          // 自分なので何もしない
        }else{
          // 相手がのっているかチェック TODO
          //log
          // のっていなければのりかえ
          RideSat (value);
        }
      }

		}
	}
}
