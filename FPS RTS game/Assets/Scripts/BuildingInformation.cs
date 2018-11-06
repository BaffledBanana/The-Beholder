using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInformation : MonoBehaviour {

	public GameObject cam;
	public Text theText;

	// Use this for initialization
	void Awake () {
		cam = GameObject.Find ("Main Camera");
		theText = gameObject.transform.Find ("Text").GetComponent <Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (cam.transform);
		transform.Rotate (0f, 180f, 0f, Space.Self);
	}

	public void UpdateText(string msg){
		theText.text = msg;
	}
}
