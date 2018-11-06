using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpin : MonoBehaviour {

	public bool Rot_x, Rot_y, Rot_z;
	public float speed;

	private bool canSpin;
	private float x, y, z;

	void Start(){
		if(gameObject.transform.parent.gameObject.GetComponent <BuildHammer>().isBuilt){
			canSpin = true;
		}else{
			canSpin = false;
		}
	}

	// Update is called once per frame
	void Update () {
		if(canSpin == false){
			if(gameObject.transform.parent.gameObject.GetComponent <BuildHammer>().isBuilt){
				canSpin = true;
			}
		}
		else{
			if(Rot_x){
				x += speed;
			}else if(Rot_y){
				y += speed;
			}else if(Rot_z){
				z += speed;
			}
			gameObject.transform.Rotate (new Vector3(x, y, z));
			x = 0;
			y = 0;
			z = 0;
		}
		}
}
