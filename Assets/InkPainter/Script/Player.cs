using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public int energy;
	public int energyMax;
	public int playerColor;		//色(RGB32? 今はID)
  private AudioSource audio = null;

	public Satellite satellite;	//乗ってる衛星

	void Start(){
		playerColor = GameMaster.instance.AddPlayer ();
    audio = GetComponent<AudioSource>();
	}

	public void RideSat(Satellite selectedNewSat){
		//ここで飛び移るアニメーションとか
		satellite = selectedNewSat;
	}

	void Update(){
		//spaceキーでぬる
		//time.deltaで要調整？
		if (playerColor == 0) {
			if (energy > satellite.energyComsumption) {
				if (Input.GetKey (KeyCode.Space)) {
					energy -= satellite.Paint (playerColor);
				}
			}
		} else {
			if (energy > satellite.energyComsumption) {
				if (Input.GetKey (KeyCode.A)) {
					energy -= satellite.Paint (playerColor);
				}
			}
		}
	}
  // 体力まんたん
  public void lifeRecovery()
  {
    energy = energyMax;
  }
  // 体力取得
  public void GetEnergy()
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
		}
	}
}
