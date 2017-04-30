﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uTools;

public class Player : MonoBehaviour {
	public int energy;
	public int energyMax;
	public int playerColor;		//色(RGB32? 今はID)
	public Satellite satellite;	//乗ってる衛星

  public AudioClip audioPaint;
  public AudioClip audioPaintError;
  public AudioClip audioRide;
  public AudioClip audioRideError;
  public AudioClip audioRecovery;
  public Image energyGuid;

  private string inspectorName;
  private AudioSource audio;
  private KeyCode paintKeyCode = KeyCode.Space;
  private KeyCode recoveryKeyCode = KeyCode.Space;
  private KeyCode moveKeyCode = KeyCode.Space;

	void Start(){
		this.playerColor = GameMaster.instance.AddPlayer ();
    this.audio = GetComponent<AudioSource>();
    initKeyMap ();
    initSatellite ();
    this.energyGuid.fillAmount = 1;
	}

  private void rideSatelliteWithMotion(string strValue){
    //ここで飛び移るアニメーション TODO
    Shout (this.audioRide);
    rideSatellite (strValue);
  }
  public void rideSatellite(string str_value){
    this.inspectorName = str_value;
    this.transform.parent = GameObject.Find(str_value).transform;   
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

  void energyGageDown(int val){
    //var gage = this.energyGuid.Find ("Image").transform;
    //Image gages = gage.GetComponent (typeof (Image)) as Image;
    //gages.fillAmount = val / 100;
    //this.energyGuid.fillAmount = 1.0f/this.energy;
    this.energyGuid.fillAmount = ((float)this.energy/100);
    Debug.Log ("energyGuid :" + this.energyGuid.fillAmount.ToString());
  }
  void energyGageRecovery(){
    //var gage = this.energyGuid.Find ("Image").transform;
    //Image gages = gage.GetComponent (typeof (Image)) as Image;
    //gages.fillAmount = 1;
    this.energyGuid.fillAmount = 1;
    Debug.Log ("energyGuid :" + this.energyGuid.fillAmount.ToString());
  }

	void Update(){
		//spaceキーでぬる
		//time.deltaで要調整？

    if (energy > satellite.energyComsumption) {
      if (Input.GetKey (this.paintKeyCode)) {
        Shout (this.audioPaint);
        this.energy -= satellite.Paint (playerColor);
        // TODO
        energyGageDown(this.energy);
      }
    } else {
      if (Input.GetKey (this.paintKeyCode)) {
        Shout (this.audioPaintError);
      }
    }

    if (energy <= satellite.energyComsumption) {
      if (Input.GetKey (this.recoveryKeyCode)) {
        Shout (this.audioRecovery);
        this.energy = this.energyMax;
        // TODO
        energyGageRecovery();
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
  public void Shout(AudioClip audioval){
    audio.clip = audioval;
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
          // 相手がのっているかチェック
          if (true == satellite_val.isUserRiding) {
            Debug.Log ("satellite:"+strValue+" is user riding........");
            Shout (this.audioRideError);
          } else {
            Debug.Log ("satellite:"+strValue+" is not user riding!!");
            // のっていなければのりかえ
            rideSatelliteWithMotion (strValue);
          }
        }
      }
		}
	}
}
