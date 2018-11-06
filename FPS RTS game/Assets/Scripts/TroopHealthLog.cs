using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopHealthLog : MonoBehaviour {

	public GameObject cam, healthBar;
	private float minF, maxF;

	// Use this for initialization
	void Start () {
		cam = GameObject.Find ("Main Camera");
		healthBar = gameObject.transform.Find ("FullHealth").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (cam.transform);
		//might have to rotate 180 here
	}

	public void UpdateHealth(int amount, int max){
		minF = amount;
		maxF = max;
		if(max != 0){
			healthBar.transform.localScale = new Vector3 (minF / maxF, 1, 1);
		}
	}

}
