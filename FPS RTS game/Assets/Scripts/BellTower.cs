using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class BellTower : MonoBehaviour {

	public AudioClip alarm;
	public AudioSource sound;

	private GameObject Master, myCanvas;
	private bool hasEntered;

	void Start(){
		myCanvas = gameObject.transform.parent.transform.Find ("Canvas").gameObject;
		myCanvas.SetActive (false);
		Master = GameObject.Find ("GameMaster");
		 sound = GetComponent <AudioSource> ();
		sound.clip = alarm;
		sound.loop = true;
	}

	void OnTriggerEnter(Collider other){
		if(hasEntered == false && sound.isPlaying == false){
			if(other.gameObject.tag == "SSTroop" || other.gameObject.tag == "ShieldTroop"){
				if(other.gameObject.layer == 9 && gameObject.layer == 11){//enemy layer and friendly building
					sound.Play ();
					sound.loop = true;
					hasEntered = true;
				}else if(other.gameObject.layer == 8 && gameObject.layer == 10){//friendly troops but enemy building
					sound.Play ();
					sound.loop = true;
					hasEntered = true;
				}
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "SSTroop" || other.gameObject.tag == "ShieldTroop"){
			if(other.gameObject.layer == 9 && gameObject.layer == 11){//enemy layer and friendly building
				sound.loop = false;
				sound.Stop ();
				hasEntered = false;
			}else if(other.gameObject.layer == 8 && gameObject.layer == 10){//friendly troops but enemy building
				sound.loop = false;
				sound.Stop ();
				hasEntered = false;
			}
		}
	}

	public void OpenMenu(){
		Master.GetComponent <GameMaster>().DisablePlayer ();
		myCanvas.SetActive (true);
	}

	public void CloseMenu(){
		Master.GetComponent <GameMaster> ().EnablePlayer ();
		myCanvas.SetActive (false);
	}

	public void DestroyBuilding(){
		CloseMenu ();
		Destroy (gameObject.transform.parent.gameObject);
	}

}
