using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimManager : MonoBehaviour {
	Animation anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation> ();
		anim.Play ("FloatAnim");

	}

	public bool jump=false;
	void Update(){
		if (jump) {
			jump = false;
			JumpToNextSat ();
		}
	}
	
	public void JumpToNextSat(){
		anim.Play ("Jump");
		anim.PlayQueued ("FloatAnim");
	}
}
