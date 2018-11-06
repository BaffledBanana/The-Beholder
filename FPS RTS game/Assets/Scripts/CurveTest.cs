using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveTest : MonoBehaviour {

	public AnimationCurve testCurve;

	// Use this for initialization
	void Start () {
		gameObject.isStatic = true;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (testCurve.Evaluate (Time.time));
	}
}
