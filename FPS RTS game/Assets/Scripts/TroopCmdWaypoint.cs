using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopCmdWaypoint : MonoBehaviour {

	public int timer;
	public float rotateAmount, upAmount;

	private int x;

	void Start(){
		gameObject.transform.Rotate (0, 0, 180);
		gameObject.transform.position = new Vector3 (gameObject.transform.position.x, upAmount, gameObject.transform.position.z);
	}

	// Update is called once per frame
	void Update () {
		if(x >= timer){
			Destroy (gameObject);
		}else{
			gameObject.transform.Rotate (0, rotateAmount, 0);
			x++;
		}
	}
}
