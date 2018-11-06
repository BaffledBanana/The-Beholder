using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildHammer : MonoBehaviour {

	public int buildAmount;
	public float buildSpeed;
	public bool isBuilt, constructStructures;

	private GameObject constructB, consStruct, cam;
	private float minB, maxB, height, minH, maxH, builtAmount;

	// Use this for initialization
	void Start () {
		cam = GameObject.Find ("Main Camera");
		constructStructures = GameObject.Find ("GameMaster").GetComponent <GameMaster> ().constructStructures;
		constructB = GameObject.Find ("ConstructBuilding");
		gameObject.GetComponent <BuildingHealth> ().health = 0;
		maxH = gameObject.GetComponent <BuildingHealth> ().maxHealth;
		//get all the upOffsets of all the buildings for later use, hopefully ive made tags for all buildings
		if (gameObject.tag == "Outpost") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetOutpost;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().OutpostStruct, gameObject.transform.position, Quaternion.identity, null);
				//Debug.Log ("Spawned outpost struct");
			}
		} else if (gameObject.tag == "Storage") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetStorage;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().StorageStruct, gameObject.transform.position, Quaternion.identity, null);
				consStruct.transform.Rotate (0, 90, 0);
			}
		} else if (gameObject.tag == "House") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetHouse;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().HouseStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		} else if (gameObject.tag == "Bank") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetBank;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().BankStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}else if (gameObject.tag == "Farm") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetFarm;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().FarmStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}else if (gameObject.tag == "Tavern") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetTavern;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().TavernStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}else if (gameObject.tag == "PlayerUpgradesBuilding") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetPUB;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().PUBStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}
		else if (gameObject.tag == "Market") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetMarket;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().MarketStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}else if (gameObject.tag == "Blacksmith") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetBlacksmith;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().BlacksmithStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}else if (gameObject.tag == "MineShaft") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetMS;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().MSStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}else if (gameObject.tag == "TreePlantation") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetTP;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().TPStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}else if (gameObject.layer == 15) {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetWWall;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().WallStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}else if (gameObject.layer == 14) {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetRWall;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().WallStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}else if (gameObject.tag == "Castle") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetCastle;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().CastleStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}else if (gameObject.tag == "Barracks") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetBarracks;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().BarracksStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}else if (gameObject.tag == "AnimalCoop") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetAC;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().ACStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}else if (gameObject.tag == "BellTower") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetBT;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().BTStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}else if (gameObject.tag == "TrainingCamp") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetTC;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().TCStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}else if (gameObject.tag == "DefenceTower") {
			height = constructB.GetComponent <ConstructBuilding> ().upOffsetDT;
			if (constructStructures) {
				consStruct = Instantiate (constructB.GetComponent <ConstructBuilding> ().DTStruct, gameObject.transform.position, Quaternion.identity, null);
			}
		}
		if(constructStructures){
			consStruct.transform.Translate (0, -height, 0);//make a bunch of floats and declare minHeight as one of those floats according to the building (and those should be set as how far down you have to go for the building to be underground), like you find the heights now, then make the build happen from minHeight to height
			consStruct.transform.Rotate (0, GameObject.Find ("ConstructBuilding").GetComponent <ConstructBuilding> ().buildingDegrees * GameObject.Find ("ConstructBuilding").GetComponent <ConstructBuilding> ().degreesOfRotation, 0);
		}
		if(gameObject.tag == "House"){
			consStruct.transform.Rotate (0, 90, 0);
		}
		//initialize the building to be under the ground, so it looks like its being built from the ground up and not look weird, popping down once you hit it :)
		gameObject.transform.localPosition = new Vector3(gameObject.transform.position.x, 0,gameObject.transform.position.z);
	}

	void Update(){
		if(AutomaticBuild(buildSpeed)){
			gameObject.GetComponent<BuildHammer>().enabled = false;
		}
	}

	public bool Build(float amount = 0.01f){
		if(builtAmount + amount < buildAmount){
			builtAmount += amount;
			minB = builtAmount;
			maxB = buildAmount;
			minH = (minB / maxB)*maxH;
			gameObject.GetComponent <BuildingHealth> ().health = (int)minH;
			gameObject.transform.localPosition = new Vector3(gameObject.transform.position.x, (minB/maxB)*height,gameObject.transform.position.z);
			cam.GetComponent <PlayerStats>().AddXp (3);
			return false;
		}else{
			isBuilt = true;
			BuildingBuiltProtocol();
			gameObject.GetComponent <BuildingHealth> ().health = (int)maxH;
			gameObject.transform.localPosition = new Vector3(gameObject.transform.position.x, height,gameObject.transform.position.z);
			if(constructStructures){
				Destroy (consStruct);
			}
			cam.GetComponent <PlayerStats>().AddXp (7);
			return true;
		}
	}

	public bool AutomaticBuild(float amount = 0.01f){
		if(builtAmount + amount < buildAmount){
			builtAmount += amount;
			minB = builtAmount;
			maxB = buildAmount;
			minH = (minB / maxB)*maxH;
			gameObject.GetComponent <BuildingHealth> ().health = (int)minH;
			gameObject.transform.localPosition = new Vector3(gameObject.transform.position.x, (minB/maxB)*height,gameObject.transform.position.z);
			return false;
		}else{
			isBuilt = true;
			BuildingBuiltProtocol();
			gameObject.GetComponent <BuildingHealth> ().health = (int)maxH;
			gameObject.transform.localPosition = new Vector3(gameObject.transform.position.x, height,gameObject.transform.position.z);
			if(constructStructures){
				try{
					Destroy (consStruct);
				}catch{
				}
			}
			return true;
		}
	}

	void BuildingBuiltProtocol(){
		if(gameObject.tag == "Storage" && isBuilt == true){
			GetComponent<StorageBuilding>().Built();
			gameObject.GetComponent<BuildHammer>().enabled = false;
		}
		if(gameObject.tag == "House" && isBuilt == true){
			GetComponent<House>().Built();
			gameObject.GetComponent<BuildHammer>().enabled = false;
		}
	}

}
