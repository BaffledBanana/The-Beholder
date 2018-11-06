using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessageHandler : MonoBehaviour {

	public Text errorMsgText;
	public int messageDuration = 200;
	public AudioClip sound;
	public GameObject MessageHandler;

	private AudioSource source;
	private bool timer = false;
	private int x;

	void Awake(){
		if(GameObject.Find ("ErrorMessageHandlerUI") != null){
			MessageHandler = GameObject.Find ("ErrorMessageHandlerUI");
			MessageHandler.SetActive (false);
		}
		source = GameObject.Find("Main Camera").GetComponent<AudioSource>();
	}

	public void PostError(string msg){
		errorMsgText.text = msg;
		MessageHandler.SetActive (true);
		x = 0;
		timer = true;
		source.clip = sound;
		source.Play();
	}


	void Update(){
		if(timer){
			x++;
			if(x>=messageDuration){
				MessageHandler.SetActive (false);
				x = 0;
				timer = false;
			}
		}
	}
}
