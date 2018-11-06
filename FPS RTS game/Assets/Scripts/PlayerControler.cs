using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CharacterController))]
public class PlayerControler : MonoBehaviour {

	public static bool invertY;
	public float movementSpeed = 8.0f;
	public float mouseSpeed = 3.0f;
	public float upDownRange = 60.0f;
	public float verticalRotation = 0;
	float verticalVelocity = 0;
	public float jumpSpeed =10.0f;
	CharacterController characterController;
		// Use this for initialization
	void Start () {
		//Screen.lookCursor = true;
		characterController = GetComponent<CharacterController> ();

	}

	// Update is called once per frame
	void Update () {
		//Mouse

		float rotLeftRight = Input.GetAxis ("Mouse X") * mouseSpeed;
		transform.Rotate (0, rotLeftRight, 0);

		if(invertY){
			verticalRotation -= -(Input.GetAxis("Mouse Y")) * mouseSpeed;
		}else{
			verticalRotation -= Input.GetAxis("Mouse Y") * mouseSpeed;
		}
		verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

		//Movement

		float forwardSpeed = Input.GetAxis ("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis ("Horizontal") * movementSpeed;

		verticalVelocity += Physics.gravity.y * Time.deltaTime;
		if (characterController.isGrounded && Input.GetButtonDown("Jump")) {
			verticalVelocity = jumpSpeed;
		}
		Vector3 speed = new Vector3 (sideSpeed, verticalVelocity, forwardSpeed);

		speed = transform.rotation * speed;
		characterController.Move (speed * Time.deltaTime);
	}

	public void InvertY(){
		invertY = !invertY;
	}

}
