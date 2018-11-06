using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySetting : MonoBehaviour {

	public static Difficulty difficultySetting = Difficulty.Normal;
	public static bool inGame;
	public int NATimer, DATimer, HATimer, NsstMINAttack, NsstMAXAttack, NsstHealth, NstMINAttack, NstMAXAttack, NstHealth, NstAttSpd, NsstAttSpd, DsstMINAttack, DsstMAXAttack, DsstHealth, DstMINAttack, DstMAXAttack, DstHealth, DstAttSpd, DsstAttSpd, HsstMINAttack, HsstMAXAttack, HsstHealth, HstMINAttack, HstMAXAttack, HstHealth, HstAttSpd, HsstAttSpd;
	[HideInInspector]
	public enum Difficulty {Sandbox, Normal, Difficult, Hardcore};

	private GameObject enemyM;
	private GameObject[] SSTroops, STroops;
	private List<GameObject> SSEnemies, SEnemies;

	void Start () {

		if(inGame){
			enemyM = GameObject.Find ("EnemyHandler");
			SSTroops = GameObject.FindGameObjectsWithTag ("SSTroop");
			STroops = GameObject.FindGameObjectsWithTag ("ShieldTroop");
			SSEnemies = new List<GameObject> ();
			SEnemies = new List<GameObject> ();
			foreach(GameObject troop in SSTroops){
				if(troop.layer == 9){ //enemy
					SSEnemies.Add (troop);
				}
			}
			foreach(GameObject troop in STroops){
				if(troop.layer == 9){//enemy
					SEnemies.Add (troop);
				}
			}
			if(difficultySetting == Difficulty.Sandbox){
				Debug.Log ("Loaded sandbox mode");
				Destroy (enemyM);
				GameObject.Find("GameMaster").GetComponent<GameMaster>().PassErrorMessage("Hello, Beholder! All neighboring kingdoms are friendly. Build your kingdom in peace!");
			}else if(difficultySetting == Difficulty.Normal){
				enemyM.GetComponent <EnemyController>().SecondsFirst = NATimer;
				enemyM.GetComponent <EnemyController>().SecondsAdd = 600;
				foreach(GameObject troop in SEnemies){
					troop.GetComponent <Troop>().health = NstHealth;
					troop.GetComponent <Troop>().maxHealth = NstHealth;
					troop.GetComponent <Troop>().minStrength = NstMINAttack;
					troop.GetComponent <Troop>().maxStrength = NstMAXAttack;
					troop.GetComponent <Troop> ().attackTimer = NstAttSpd;
				}
				foreach(GameObject troop in SSEnemies){
					troop.GetComponent <Troop>().health = NsstHealth;
					troop.GetComponent <Troop>().maxHealth = NsstHealth;
					troop.GetComponent <Troop>().minStrength = NsstMINAttack;
					troop.GetComponent <Troop>().maxStrength = NsstMAXAttack;
					troop.GetComponent <Troop> ().attackTimer = NsstAttSpd;
				}
				GameObject.Find("GameMaster").GetComponent<GameMaster>().PassErrorMessage("Hello, Beholder! Enemy troops will attack your base in " + (int)(GameObject.Find("EnemyHandler").GetComponent <EnemyController>().SecondsFirst/60) + " minutes! Good Luck!");
				Debug.Log ("Loaded normal mode");
			}else if(difficultySetting == Difficulty.Difficult){
				enemyM.GetComponent <EnemyController>().SecondsFirst = DATimer;
				enemyM.GetComponent <EnemyController>().SecondsAdd = 300;
				foreach(GameObject troop in SEnemies){
					troop.GetComponent <Troop>().health = DstHealth;
					troop.GetComponent <Troop>().maxHealth = DstHealth;
					troop.GetComponent <Troop>().minStrength = DstMINAttack;
					troop.GetComponent <Troop>().maxStrength = DstMAXAttack;
					troop.GetComponent <Troop> ().attackTimer = DstAttSpd;
				}
				foreach(GameObject troop in SSEnemies){
					troop.GetComponent <Troop>().health = DsstHealth;
					troop.GetComponent <Troop>().maxHealth = DsstHealth;
					troop.GetComponent <Troop>().minStrength = DsstMINAttack;
					troop.GetComponent <Troop>().maxStrength = DsstMAXAttack;
					troop.GetComponent <Troop> ().attackTimer = DsstAttSpd;
				}
				GameObject.Find("GameMaster").GetComponent<GameMaster>().PassErrorMessage("Hello, Beholder! Enemy troops will attack your base in " + (int)(GameObject.Find("EnemyHandler").GetComponent <EnemyController>().SecondsFirst/60) + " minutes! Good Luck!");
				Debug.Log ("Loaded difficult mode");
			}else if(difficultySetting == Difficulty.Hardcore){
				enemyM.GetComponent <EnemyController>().SecondsFirst = HATimer;
				enemyM.GetComponent <EnemyController>().SecondsAdd = 200;
				foreach(GameObject troop in SEnemies){
					troop.GetComponent <Troop>().health = HstHealth;
					troop.GetComponent <Troop>().maxHealth = HstHealth;
					troop.GetComponent <Troop>().minStrength = HstMINAttack;
					troop.GetComponent <Troop>().maxStrength = HstMAXAttack;
					troop.GetComponent <Troop> ().attackTimer = HstAttSpd;
				}
				foreach(GameObject troop in SSEnemies){
					troop.GetComponent <Troop>().health = HsstHealth;
					troop.GetComponent <Troop>().maxHealth = HsstHealth;
					troop.GetComponent <Troop>().minStrength = HsstMINAttack;
					troop.GetComponent <Troop>().maxStrength = HsstMAXAttack;
					troop.GetComponent <Troop> ().attackTimer = HsstAttSpd;
				}
				GameObject.Find("GameMaster").GetComponent<GameMaster>().PassErrorMessage("Hello, Beholder! Enemy troops will attack your base in " + (int)(GameObject.Find("EnemyHandler").GetComponent <EnemyController>().SecondsFirst/60) + " minutes! Good Luck!");
				Debug.Log ("Loaded hardcore mode");
			}
			Destroy (gameObject);
		}
	}

	public void Sandbox(){
		difficultySetting = Difficulty.Sandbox;
		inGame = true;
		Debug.Log ("Loaded sandbox");
		SceneManager.LoadScene ("Normal");
	}
	public void Normal(){
		difficultySetting = Difficulty.Normal;
		inGame = true;
		Debug.Log ("Loaded normal");
		SceneManager.LoadScene ("Normal");
	}

	public void Difficult(){
		difficultySetting = Difficulty.Difficult;
		inGame = true;
		Debug.Log ("Loaded difficult");
		SceneManager.LoadScene ("Normal");
	}

	public void Hardcore(){
		difficultySetting = Difficulty.Hardcore;
		inGame = true;
		Debug.Log ("Loaded hardcore");
		SceneManager.LoadScene ("Normal");
	}

}
