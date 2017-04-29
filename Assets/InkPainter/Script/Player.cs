using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public int energy;
	public int energyMax;
	public int playerColor;		//色(RGB32? 今はID)

	public Satellite satellite;	//乗ってる衛星

	void Start(){
		playerColor = GameMaster.instance.AddPlayer ();
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

	//なく
	public void Shout(){
	}
		
	void OnTriggerStay(Collider other){
		if (other.tag.Equals ("Satellite")) {
			//ほかの衛星と当たればここにくる
			//自分以外を排除することに注意
		}
	}
}
