using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public int energy;
	public int energyMax;
	public int playerID;

	public Satellite ridingSat;

	void Start(){
		playerID = GameMan.instance.AddPlayer ();
	}

	public void RideSat(Satellite selectedNewSat){
		//ここで飛び移るアニメーションとか
		ridingSat = selectedNewSat;
	}

	void Update(){
		if (energy > ridingSat.energyComsumption) {
			if (Input.GetKey (KeyCode.Space)) {
				energy -= ridingSat.Paint (playerID);
			}
		}
	}
}
