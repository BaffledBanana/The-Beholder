using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResMineLog : MonoBehaviour {

	public GameObject cam;
	private Text theText;
	private int x, delay, y, timer;

	// Use this for initialization
	void Awake () {
		cam = GameObject.Find ("Main Camera");
		theText = gameObject.transform.Find ("Text").GetComponent <Text> ();
		gameObject.transform.Find ("Text").GetComponent<Text>().enabled = false;
	}

	// Update is called once per frame
	void Update () {
		if(y >= delay){
			gameObject.transform.Find ("Text").GetComponent<Text>().enabled = true;
			transform.LookAt (cam.transform);
			transform.Rotate (0f, 180f, 0f, Space.Self);
			transform.Translate (0, 0.002f, 0);
			if(x >= timer){
				Destroy (gameObject);
			} else{
				x++;
			}
		}else{
			y++;
		}
	}

	public void Display(string name, int amount, int theDelay, int time){
			theText.text = "+" + amount + " " + name;
		delay = theDelay;
		timer = time;
	}
}
