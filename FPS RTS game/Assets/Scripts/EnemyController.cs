using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public List<GameObject> squads, buildings;
	public int SecondsFirst, SecondsAdd;
	public GameObject friendlyBase;
	public bool shouldAttack = true;

	private Transform[] inSquad, temp;
	private int i = 0, x = 0;

	// Use this for initialization
	void Start () {
		//squads = new List<GameObject> ();
		//buildings = new List<GameObject> ();
		//temp = GameObject.FindObjectsOfType <GameObject> ();
		/*foreach(GameObject building in temp){
			if(building.layer == 11){//friendly building
				buildings.Add (building);
			}
		} */
		friendlyBase = GameObject.Find ("BaseBuilding");
		foreach(GameObject squad in squads){
			squad.SetActive (false);
		}
	}

	// Update is called once per frame
	void Update () {
		if(shouldAttack){
			if(Time.time >= SecondsFirst){
				if(friendlyBase != null){
					if(squads[x] != null){
						squads [x].SetActive (true);
						Debug.Log("Enemies attacking!");
						x++;
					}
					/*if(i < buildings.Count){
						foreach(GameObject enemy in inSquad){
							enemy.GetComponent <Troop>().Charge (buildings[i]);
						}
					} */
					/*if(squads [i].GetComponentsInChildren <Transform> ().Length == 0){
						previousDead = true;
						Debug.Log ("Previous was dead");
						i++;
					}
					if(i < squads.Count && previousDead){
						inSquad = squads [i].GetComponentsInChildren <Transform> ();
						previousDead = false;
					}
					if(i > squads.Count && previousDead == false){
						Master.GetComponent <GameMaster>().PassErrorMessage ("Congratulations! You have defeated all of the enemy troops and your base is still standing! You have won the game.");
					} */
				inSquad = squads [i].GetComponentsInChildren <Transform> ();
					foreach(Transform enemy in inSquad){
					if(enemy.gameObject.GetComponent <Troop>() != null){
						enemy.gameObject.GetComponent <Troop>().Charge (friendlyBase);
							enemy.gameObject.GetComponent <Troop> ().wasAttackingBase = true;
						Debug.Log ("Told enemies to attack the friendly base.");

					}
					}
					SecondsFirst += SecondsAdd;
				}else{
					shouldAttack = false;
				}
			}
		}
	}
}
