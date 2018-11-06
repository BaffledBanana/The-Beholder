using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

	public float speed;

	private Transform res;
	// Use this for initialization
	void Start () {
		res = GameObject.Find("Recources").transform;
	}

	// Update is called once per frame
	void Update () {
		transform.RotateAround(Vector3.zero, Vector3.down, speed);
		transform.LookAt(res);
	}
}
