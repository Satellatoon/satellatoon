using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Es.InkPainter;

public class Earth : MonoBehaviour {
	public float myArea;
	public float yourArea;

	InkCanvas paintObject;

	void Start(){
		paintObject = GetComponent<InkCanvas> ();
	}


	public float CalculateMyArea(){
		int point = 0;
		int[,] matrix = paintObject.GetColorMatrix ();
		for (int i = 0; i < 1000; i++) {
			for (int j = 0; j < 1000; j++) {
				if (matrix [i, j] == 0) {
					point++;
				}
			}
		}

//		Debug.Log (" " + (float)point/(matrix.GetLength(0)*matrix.GetLength(1)) );

		return (float)point/(matrix.GetLength(0)*matrix.GetLength(1));
	}
	public float CalculateEnemyArea(){
		int point = 0;
		int[,] matrix = paintObject.GetColorMatrix ();
		for (int i = 0; i < 1000; i++) {
			for (int j = 0; j < 1000; j++) {
				if (matrix [i, j] == 1) {
					point++;
				}
			}
		}

//		Debug.Log (" " + (float)point/(matrix.GetLength(0)*matrix.GetLength(1)) );

		return (float)point/(matrix.GetLength(0)*matrix.GetLength(1));
	}
}
