using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHealth : MonoBehaviour {

	public int health, maxHealth;
	private GameObject cam;

	public bool TakeDmg(int amount){
		if(health - amount > 0){
			health -= amount;
			return false;
		}
		else{
			Debug.Log ("Building destroyed! : " + gameObject.name);
			if(gameObject.layer == 10){//10 is enemy building
				cam = GameObject.Find ("Main Camera");
				cam.GetComponent <PlayerStats>().AddXp (250);
			}
			if(gameObject.GetComponent <House>() != null){
				gameObject.GetComponent <House> ().BuildingDiedProtocol ();
			}else if(gameObject.GetComponent <StorageBuilding>() != null){
				gameObject.GetComponent <StorageBuilding> ().BuildingDiedProtocol ();
			}else if(gameObject.GetComponent <BaseBuilding>() != null){
				gameObject.GetComponent <BaseBuilding> ().Master.GetComponent <GameMaster> ().GameOver ("Your base building has been destroyed! Game over!");
			}
			else if(gameObject.GetComponent <MineShaft>() != null){
				gameObject.GetComponent <MineShaft> ().BuildingDiedProtocol ();
			}
			else if(gameObject.GetComponent <TreePlantation>() != null){
				gameObject.GetComponent <TreePlantation> ().BuildingDiedProtocol ();
			}
			else if(gameObject.GetComponent <AnimalCoop>() != null){
				gameObject.GetComponent <AnimalCoop> ().BuildingDiedProtocol ();
			}
			else if(gameObject.GetComponent <Farm>() != null){
				gameObject.GetComponent <Farm> ().BuildingDiedProtocol ();
			}
			Destroy (gameObject, 0.3f);
			return true;
		}
	}

}
