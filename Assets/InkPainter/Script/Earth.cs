using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour {
	public float myArea;
	public float yourArea;

	public float CalculateMyArea(){
		return myArea;
	}
	public float CalculateEnemyArea(){
		return yourArea;
	}
}
