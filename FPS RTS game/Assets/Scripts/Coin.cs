using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	public int rotateAmount = 1, moveAmount = 1;
	public float  maxHeight = 0f, minHeight = 0.3f;

	void Update () {
		gameObject.transform.Rotate(new Vector3(0, rotateAmount, 0), Space.Self);
		/*if(transform.position.y >= maxHeight){
			gameObject.transform.Translate (Vector3.down * moveAmount, Space.Self);	
		}else if(transform.position.y <= minHeight){
			gameObject.transform.Translate (Vector3.up * moveAmount, Space.Self);	
		} */
	}
}
