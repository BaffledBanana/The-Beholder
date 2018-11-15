using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
	public float distance = 3f, interactDistance = 4f, sprintMax, sprintSpeed, sprintRegen, walkSpeed;
	public int maxWood, maxRock, maxIron, maxFood, maxGold, maxHide, gatherSpeed = 100, gatherSpeedIncrease = 100, maxWoodIncrease, maxRockIncrease, maxIronIncrease, maxFoodIncrease, maxGoldIncrease, maxHideIncrease;//the smaller the gatherSpeed number the faster it goes
	public int pWood, pRock, pIron, pFood, pGold, pHide, armor, maxArmor, xp, nextLevelXp, level, xpPoints, strength, hammerStrenght;
	public bool isBlocking;

	private GameObject Master;


	/*void Update(){
		if(Input.GetKeyDown (KeyCode.T)){
			AddXp (30);
		}
	} */

	void Start(){
		walkSpeed = gameObject.transform.parent.GetComponent <PlayerControler> ().movementSpeed;
		level = 0;
		xp = 0;
		nextLevelXp = 60;
		Master = GameObject.Find ("GameMaster");
		gameObject.GetComponent <PLayerUIHandler> ().UpdateText ("Armor", armor, maxArmor);
		AddXp (xp); //easy way to update the text, cos i was lazy
		gameObject.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", xpPoints, 0);
	}

	public int Add(string name, int amount){
		if(name == "Wood"){
			if(pWood + amount < maxWood){
				pWood += amount;
				gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Wood", pWood, maxWood);
				return pWood;
			}
			else if(pWood + amount >= maxWood){
				pWood = maxWood;
				gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Wood", pWood, maxWood);
				return pWood;
			}
			else{ return 0;}
		}
		else if(name == "Rock"){
			if(pRock + amount < maxRock){
				pRock += amount;
				gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Rock", pRock, maxRock);
				return pRock;
			}
			else if(pRock + amount >= maxRock){
				pRock = maxRock;
				gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Rock", pRock, maxRock);
				return pRock;
			}
			else{ return 0;}
		}
		else if(name == "Iron"){
			if(pIron + amount < maxIron){
				pIron += amount;
				gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Iron", pIron, maxIron);
				return pIron;
			}
			else if(pIron + amount >= maxIron){
				pIron = maxIron;
				gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Iron", pIron, maxIron);
				return pIron;
			}
			else{ return 0;}
		}
		else if(name == "Food"){
			if(pFood + amount < maxFood){
				pFood += amount;
				gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Food", pFood, maxFood);
				return pFood;
			}
			else if(pFood + amount >= maxFood){
				pFood = maxFood;
				gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Food", pFood, maxFood);
				return pFood;
			}
			else{ return 0;}
		}
		else if(name == "Hide"){
			if(pHide + amount < maxHide){
				pHide += amount;
				gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Hide", pHide, maxHide);
				return pHide;
			}
			else if(pHide + amount >= maxHide){
				pHide = maxHide;
				gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Hide", pHide, maxHide);
				return pHide;
			}
			else{ return 0;}
		}
		else if(name == "Gold"){
			if(pGold + amount < maxGold){
				pGold += amount;
				gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Gold", pGold, maxGold);
				return pGold;
			}
			else if(pGold + amount >= maxGold){
				pGold = maxGold;
				gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Gold", pGold, maxGold);
				return pGold;
			}
			else{ return 0;}
		}
		else{ return 0;}
	}

	public bool IncreaseGatherSpeed(int wood, int rock, int iron, int gold){
		if(gatherSpeed > 19 && Master.GetComponent <GameMaster>().gWood >= wood && Master.GetComponent <GameMaster>().gRock >= rock && Master.GetComponent <GameMaster>().gIron >= iron && Master.GetComponent <GameMaster>().gGold >= gold){
			gatherSpeed -= gatherSpeedIncrease;
			Master.GetComponent <GameMaster> ().Subtract ("Wood", wood);
			Master.GetComponent <GameMaster> ().Subtract ("Rock", rock);
			Master.GetComponent <GameMaster> ().Subtract ("Iron", iron);
			Master.GetComponent <GameMaster> ().Subtract ("Gold", gold);
			gameObject.GetComponent <Gather>().UpdateGatherSpeed (gatherSpeed);
			return true;
		}else{
			Debug.Log ("PlayerStats: Gather speed is at it's lowest or not enough res.");
			Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, you don't have enough resources for that or gathering speed is at it's fastest.");
			return false;
		}

	}

	public bool IncreaseMaxWood(int wood, int rock, int iron, int gold){
		if(Master.GetComponent <GameMaster>().gWood >= wood && Master.GetComponent <GameMaster>().gRock >= rock && Master.GetComponent <GameMaster>().gIron >= iron && Master.GetComponent <GameMaster>().gGold >= gold){
			maxWood += maxWoodIncrease;
			Master.GetComponent <GameMaster> ().Subtract ("Wood", wood);
			Master.GetComponent <GameMaster> ().Subtract ("Rock", rock);
			Master.GetComponent <GameMaster> ().Subtract ("Iron", iron);
			Master.GetComponent <GameMaster> ().Subtract ("Gold", gold);
			gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Wood", pWood, maxWood);
			return true;
		}else{
			Debug.Log ("PlayerStats: not enough res for upgrade");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you don't have enough resources for that.");
			return false;
		}
	}

	public bool IncreaseMaxRock(int wood, int rock, int iron, int gold){
		if(Master.GetComponent <GameMaster>().gWood >= wood && Master.GetComponent <GameMaster>().gRock >= rock && Master.GetComponent <GameMaster>().gIron >= iron && Master.GetComponent <GameMaster>().gGold >= gold){
			maxRock += maxRockIncrease;
			Master.GetComponent <GameMaster> ().Subtract ("Wood", wood);
			Master.GetComponent <GameMaster> ().Subtract ("Rock", rock);
			Master.GetComponent <GameMaster> ().Subtract ("Iron", iron);
			Master.GetComponent <GameMaster> ().Subtract ("Gold", gold);
			gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Rock", pRock, maxRock);
			return true;
		}else{
			Debug.Log ("PlayerStats: not enough res for upgrade");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you don't have enough resources for that.");
			return false;
		}
	}
	public bool IncreaseMaxIron(int wood, int rock, int iron, int gold){
		if(Master.GetComponent <GameMaster>().gWood >= wood && Master.GetComponent <GameMaster>().gRock >= rock && Master.GetComponent <GameMaster>().gIron >= iron && Master.GetComponent <GameMaster>().gGold >= gold){
			maxIron += maxIronIncrease;
			Master.GetComponent <GameMaster> ().Subtract ("Wood", wood);
			Master.GetComponent <GameMaster> ().Subtract ("Rock", rock);
			Master.GetComponent <GameMaster> ().Subtract ("Iron", iron);
			Master.GetComponent <GameMaster> ().Subtract ("Gold", gold);
			gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Iron", pIron, maxIron);
			return true;
		}else{
			Debug.Log ("PlayerStats: not enough res for upgrade");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you don't have enough resources for that.");
			return false;
		}
	}

	public bool IncreaseMaxFood(int wood, int rock, int iron, int gold){
		if(Master.GetComponent <GameMaster>().gWood >= wood && Master.GetComponent <GameMaster>().gRock >= rock && Master.GetComponent <GameMaster>().gIron >= iron && Master.GetComponent <GameMaster>().gGold >= gold){
			maxFood += maxFoodIncrease;
			Master.GetComponent <GameMaster> ().Subtract ("Wood", wood);
			Master.GetComponent <GameMaster> ().Subtract ("Rock", rock);
			Master.GetComponent <GameMaster> ().Subtract ("Iron", iron);
			Master.GetComponent <GameMaster> ().Subtract ("Gold", gold);
			gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Food", pFood, maxFood);
			return true;
		}else{
			Debug.Log ("PlayerStats: not enough res for upgrade");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you don't have enough resources for that.");
			return false;
		}
	}
	public bool IncreaseMaxGold(int wood, int hide, int iron, int gold){
		if(Master.GetComponent <GameMaster>().gWood >= wood && Master.GetComponent <GameMaster>().gHide >= hide && Master.GetComponent <GameMaster>().gIron >= iron && Master.GetComponent <GameMaster>().gGold >= gold){
			maxGold += maxGoldIncrease;
			Master.GetComponent <GameMaster> ().Subtract ("Wood", wood);
			Master.GetComponent <GameMaster> ().Subtract ("Hide", hide);
			Master.GetComponent <GameMaster> ().Subtract ("Iron", iron);
			Master.GetComponent <GameMaster> ().Subtract ("Gold", gold);
			gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Gold", pGold, maxGold);
			return true;
		}else{
			Debug.Log ("PlayerStats: not enough res for upgrade");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you don't have enough resources for that.");
			return false;
		}
	}
	public bool IncreaseMaxHide(int wood, int hide, int iron, int gold){
		if(Master.GetComponent <GameMaster>().gWood >= wood && Master.GetComponent <GameMaster>().gHide >= hide && Master.GetComponent <GameMaster>().gIron >= iron && Master.GetComponent <GameMaster>().gGold >= gold){
			maxHide += maxHideIncrease;
			Master.GetComponent <GameMaster> ().Subtract ("Wood", wood);
			Master.GetComponent <GameMaster> ().Subtract ("Hide", hide);
			Master.GetComponent <GameMaster> ().Subtract ("Iron", iron);
			Master.GetComponent <GameMaster> ().Subtract ("Gold", gold);
			gameObject.GetComponent <PLayerUIHandler>().UpdateText ("Hide", pHide, maxHide);
			return true;
		}else{
			Debug.Log ("PlayerStats: not enough res for upgrade");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you don't have enough resources for that.");
			return false;
		}
	}

	public bool TakeDamage(int amount){
		Debug.Log ("Player was attacked!");
		if(isBlocking){
			amount = (int)(amount * 0.25f);
			if(armor - amount >= 0){
				armor -= amount;
				gameObject.GetComponent <PLayerUIHandler> ().UpdateText ("Armor", armor, maxArmor);
				return false;
				//update the UI
			} else {
				Debug.Log ("You died");
				Master.GetComponent <GameMaster> ().GameOver ("You have died! Game over!");
				return true;
				//pass death error
			}
		}else{
			if(armor - amount >= 0){
				armor -= amount;
				gameObject.GetComponent <PLayerUIHandler> ().UpdateText ("Armor", armor, maxArmor);
				return false;
			} else {
				Debug.Log ("You died");
				Master.GetComponent <GameMaster> ().GameOver ("You have died! Game over!");
				return true;
			}
		}
	}

	public void AddXp(int amount){
		if (xp + amount < nextLevelXp){
			xp += amount;
			gameObject.GetComponent <PLayerUIHandler>().UpdateXp (xp, nextLevelXp, level);
		}else if(xp + amount == nextLevelXp){
			nextLevelXp += (int)((nextLevelXp * 1.2020f)/ 1.6180f);//Apéry's constant ζ(3), The golden ratio φ
			strength += 5;
			level++;
			interactDistance++;
			gameObject.GetComponent <Gather>().minXp++;
			gameObject.GetComponent <Gather>().maxXp++;
			gameObject.GetComponent <Gather>().maxWood++;
			gameObject.GetComponent <Gather>().minWood++;
			gameObject.GetComponent <Gather>().maxRock++;
			gameObject.GetComponent <Gather>().minRock++;
			gameObject.GetComponent <Gather>().maxIron++;
			gameObject.GetComponent <Gather>().minIron++;
			gameObject.GetComponent <Gather>().maxFood++;
			gameObject.GetComponent <Gather>().minFood++;
			gameObject.GetComponent <Gather>().maxHide++;
			gameObject.GetComponent <Gather>().minHide++;
			hammerStrenght += 10;
			xpPoints += Random.Range (8, 15);
			xp = 0;
			gameObject.GetComponent <PLayerUIHandler>().UpdateXp (xp, nextLevelXp, level);
			gameObject.GetComponent <PLayerUIHandler>().UpdateText ("XpPoints", xpPoints, 0);
		} else if(xp + amount > nextLevelXp){
			xp = (xp + amount) - nextLevelXp;
			nextLevelXp += (int)((nextLevelXp * 1.2020f)/ 1.6180f);//Apéry's constant ζ(3), The golden ratio φ
			level++;
			strength += 5;
			interactDistance++;
			gameObject.GetComponent <Gather>().minXp++;
			gameObject.GetComponent <Gather>().maxXp++;
			gameObject.GetComponent <Gather>().maxWood++;
			gameObject.GetComponent <Gather>().minWood++;
			gameObject.GetComponent <Gather>().maxRock++;
			gameObject.GetComponent <Gather>().minRock++;
			gameObject.GetComponent <Gather>().maxIron++;
			gameObject.GetComponent <Gather>().minIron++;
			gameObject.GetComponent <Gather>().maxFood++;
			gameObject.GetComponent <Gather>().minFood++;
			gameObject.GetComponent <Gather>().maxHide++;
			gameObject.GetComponent <Gather>().minHide++;
			hammerStrenght += 10;
			xpPoints += Random.Range (8, 15);
			gameObject.GetComponent <PLayerUIHandler>().UpdateXp (xp, nextLevelXp, level);
			gameObject.GetComponent <PLayerUIHandler>().UpdateText ("XpPoints", xpPoints, 0);
			if(xp >= nextLevelXp){
				AddXp (0);
			}
		}
	}

}
