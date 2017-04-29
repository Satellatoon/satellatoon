using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public int energy;
	public int energyMax;

	public Satellite ridingSat;

	public void RideSat(Satellite selectedNewSat){
		//ここで飛び移るアニメーションとか
		ridingSat = selectedNewSat;
	}

	void Update(){
		if (energy > ridingSat.energyComsumption) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				energy -= ridingSat.Paint ();
			}
		}
	}
}
