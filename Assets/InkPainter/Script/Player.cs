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

	void Start(){
		this.playerColor = GameMaster.instance.AddPlayer ();
    this.audio = GetComponent<AudioSource>();
    initKey ();
	}

	public void RideSat(Satellite selectedNewSat){
		//ここで飛び移るアニメーションとか
		satellite = selectedNewSat;
	}

  void initKey(){
    if (this.playerColor == 0) { // player1
      this.paintKeyCode = KeyCode.L;
    } else { // player2
      this.paintKeyCode = KeyCode.A;
    }

    if (this.playerColor == 0) { // player1
      this.recoveryKeyCode = KeyCode.P;
    } else { // player2
      this.recoveryKeyCode = KeyCode.Q;
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
      //Satellite value = other.GetComponents();
      //
      //value.Paint ();
		}
	}
}
