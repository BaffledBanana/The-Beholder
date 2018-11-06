using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitMenu : MonoBehaviour {

	public void Resume(){
		GameObject.Find ("GameMaster").GetComponent <GameMaster>().EnablePlayer ();
		gameObject.SetActive (false);
		GameObject.Find ("Main Camera").GetComponent <Interact>().openMainMenu = true;
	}

	public void Quit(){
		Application.Quit ();
	}

}
