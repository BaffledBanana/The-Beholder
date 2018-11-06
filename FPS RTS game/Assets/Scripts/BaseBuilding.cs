using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBuilding : MonoBehaviour {

		private GameObject Menu, Player, buildingInfo;
		private GameObject[] houses;

		public float townBuildDistanceLimit;//put this in upgrades building and make it get an array of all workers and make it call the ResLoc function in all of them
		public GameObject WoodWorker, RockWorker, IronWorker, FoodWorker, Master;
		public int FTaxHappynessDecrease;

		//If max radius gets bigger we need to run Test() again

	void Awake () {
		Master = GameObject.FindGameObjectWithTag ("GameMaster");
		if(GameObject.Find ("BaseBuildingMenu") != null){
			Menu = GameObject.Find ("BaseBuildingMenu");
		}
		Menu.SetActive (false);
		Player = GameObject.Find ("Main Camera");
	}

	void Start(){
		buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
		buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Workers at this building: " + Master.GetComponent<GameMaster> ().workers);
		buildingInfo.SetActive (false);//TODO figure out what to write in the building info, i really got nothing right now
	}

	public void OpenMenu(){
		Master.GetComponent <GameMaster>().DisablePlayer ();
		Menu.SetActive (true);
		GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (true);
	}
	public void CloseMenu(){
		Master.GetComponent <GameMaster> ().EnablePlayer ();
		Menu.SetActive (false);
		GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (false);
	}

	public void DropOffWood(string temp){
		if(Player.GetComponent <PlayerStats>().pWood - int.Parse (temp) >= 0 && Master.GetComponent <GameMaster>().CalculateStorage () + int.Parse (temp) <= Master.GetComponent <GameMaster>().storage){
			Master.GetComponent <GameMaster>().Add ("Wood", int.Parse (temp));
			Player.GetComponent <PlayerStats> ().pWood -= int.Parse (temp);
			if(int.Parse (temp) == Player.GetComponent <PlayerStats>().maxWood){
				Master.GetComponent <GameMaster>().Add ("Happyness", 1);
			}
			Player.GetComponent <PLayerUIHandler>().UpdateText ("Wood", Player.GetComponent <PlayerStats>().pWood, Player.GetComponent <PlayerStats>().maxWood);
		} else{Debug.Log ("BaseBuilding: You dont have that much res in your inventory! Or there is not enough storage space!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have that much wood in your inventory or there is not enough storage space!");
		}
	}
	public void DropOffFood(string temp){
		if(Player.GetComponent <PlayerStats>().pFood - int.Parse (temp) >= 0 && Master.GetComponent <GameMaster>().CalculateStorage () + int.Parse (temp) <= Master.GetComponent <GameMaster>().storage){
			Master.GetComponent <GameMaster>().Add ("Food", int.Parse (temp));
			Player.GetComponent <PlayerStats> ().pFood -= int.Parse (temp);
			if(int.Parse (temp) == Player.GetComponent <PlayerStats>().maxFood){
				Master.GetComponent <GameMaster>().Add ("Happyness", 1);
			}
			Player.GetComponent <PLayerUIHandler>().UpdateText ("Food", Player.GetComponent <PlayerStats>().pFood, Player.GetComponent <PlayerStats>().maxFood);
		}else{Debug.Log ("BaseBuilding: You dont have that much res in your inventory! Or there is not enough storage space!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have that much food in your inventory or there is not enough storage space!");
		}
	}
	public void DropOffRock(string temp){
		if(Player.GetComponent <PlayerStats>().pRock - int.Parse (temp) >= 0 && Master.GetComponent <GameMaster>().CalculateStorage () + int.Parse (temp) <= Master.GetComponent <GameMaster>().storage){
			Master.GetComponent <GameMaster>().Add ("Rock", int.Parse (temp));
			Player.GetComponent <PlayerStats> ().pRock -= int.Parse (temp);
			if(int.Parse (temp) == Player.GetComponent <PlayerStats>().maxRock){
				Master.GetComponent <GameMaster>().Add ("Happyness", 1);
			}
			Player.GetComponent <PLayerUIHandler>().UpdateText ("Rock", Player.GetComponent <PlayerStats>().pRock, Player.GetComponent <PlayerStats>().maxRock);
		}else{Debug.Log ("BaseBuilding: You dont have that much res in your inventory! Or there is not enough storage space!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have that much rock in your inventory or there is not enough storage space!");
		}
	}
	public void DropOffIron(string temp){
		if(Player.GetComponent <PlayerStats>().pIron - int.Parse (temp) >= 0 && Master.GetComponent <GameMaster>().CalculateStorage () + int.Parse (temp) <= Master.GetComponent <GameMaster>().storage){
			Master.GetComponent <GameMaster>().Add ("Iron", int.Parse (temp));
			Player.GetComponent <PlayerStats> ().pIron -= int.Parse (temp);
			if(int.Parse (temp) == Player.GetComponent <PlayerStats>().maxIron){
				Master.GetComponent <GameMaster>().Add ("Happyness", 1);
			}
			Player.GetComponent <PLayerUIHandler>().UpdateText ("Iron", Player.GetComponent <PlayerStats>().pIron, Player.GetComponent <PlayerStats>().maxIron);
		}else{Debug.Log ("BaseBuilding: You dont have that much res in your inventory! Or there is not enough storage space!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have that much iron in your inventory or there is not enough storage space!");
		}
	}
	public void DropOffGold(string temp){
		if(Player.GetComponent <PlayerStats>().pGold - int.Parse (temp) >= 0 && Master.GetComponent <GameMaster>().CalculateStorage () + int.Parse (temp) <= Master.GetComponent <GameMaster>().storage){
			Master.GetComponent <GameMaster>().Add ("Gold", int.Parse (temp));
			Player.GetComponent <PlayerStats> ().pGold -= int.Parse (temp);
			if(int.Parse (temp) == Player.GetComponent <PlayerStats>().maxGold){
				Master.GetComponent <GameMaster>().Add ("Happyness", 1);
			}
			Player.GetComponent <PLayerUIHandler>().UpdateText ("Gold", Player.GetComponent <PlayerStats>().pGold, Player.GetComponent <PlayerStats>().maxGold);
		}else{Debug.Log ("BaseBuilding: You dont have that much res in your inventory! Or there is not enough storage space!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have that much gold in your inventory or there is not enough storage space!");
		}
	}
	public void DropOffHide(string temp){
		if(Player.GetComponent <PlayerStats>().pHide - int.Parse (temp) >= 0 && Master.GetComponent <GameMaster>().CalculateStorage () + int.Parse (temp) <= Master.GetComponent <GameMaster>().storage){
			Master.GetComponent <GameMaster>().Add ("Hide", int.Parse (temp));
			Player.GetComponent <PlayerStats> ().pHide -= int.Parse (temp);
			if(int.Parse (temp) == Player.GetComponent <PlayerStats>().maxHide){
				Master.GetComponent <GameMaster>().Add ("Happyness", 1);
			}
			Player.GetComponent <PLayerUIHandler>().UpdateText ("Hide", Player.GetComponent <PlayerStats>().pHide, Player.GetComponent <PlayerStats>().maxHide);
		}else{Debug.Log ("BaseBuilding: You dont have that much res in your inventory! Or there is not enough storage space!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have that much hide in your inventory or there is not enough storage space!");
		}
	}
	public void DropOffAllWood(){
		if (Master.GetComponent <GameMaster> ().CalculateStorage () + Player.GetComponent <PlayerStats> ().pWood <= Master.GetComponent <GameMaster> ().storage) {
			Master.GetComponent <GameMaster> ().Add ("Wood", Player.GetComponent <PlayerStats> ().pWood);
			if(Player.GetComponent <PlayerStats>().pWood >= Player.GetComponent <PlayerStats>().maxWood){
				Master.GetComponent <GameMaster>().Add ("Happyness", 1);
			}
			Player.GetComponent <PlayerStats> ().pWood = 0;
			Player.GetComponent <PLayerUIHandler> ().UpdateText ("Wood", 0, Player.GetComponent <PlayerStats> ().maxWood);
		} else {
			Debug.Log ("BaseBuilding: There is not enough storage space!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("There is not enough storage space!");
		}
	}
	public void DropOffAllRock(){
		if(Master.GetComponent <GameMaster>().CalculateStorage () + Player.GetComponent <PlayerStats>().pRock <= Master.GetComponent <GameMaster>().storage){
			Master.GetComponent <GameMaster>().Add ("Rock", Player.GetComponent <PlayerStats>().pRock);
			Player.GetComponent <PlayerStats> ().pRock = 0;
			if(Player.GetComponent <PlayerStats>().pRock == Player.GetComponent <PlayerStats>().maxRock){
				Master.GetComponent <GameMaster>().Add ("Happyness", 1);
			}
			Player.GetComponent <PLayerUIHandler>().UpdateText ("Rock", 0, Player.GetComponent <PlayerStats>().maxRock);
		}else{Debug.Log ("BaseBuilding: There is not enough storage space!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("There is not enough storage space!");
		}
	}
	public void DropOffAllFood(){
		if(Master.GetComponent <GameMaster>().CalculateStorage () + Player.GetComponent <PlayerStats>().pFood <= Master.GetComponent <GameMaster>().storage){
			Master.GetComponent <GameMaster>().Add ("Food", Player.GetComponent <PlayerStats>().pFood);
			Player.GetComponent <PlayerStats> ().pFood = 0;
			if(Player.GetComponent <PlayerStats>().pFood == Player.GetComponent <PlayerStats>().maxFood){
				Master.GetComponent <GameMaster>().Add ("Happyness", 1);
			}
			Player.GetComponent <PLayerUIHandler>().UpdateText ("Food", 0, Player.GetComponent <PlayerStats>().maxFood);
		}else{Debug.Log ("BaseBuilding: There is not enough storage space!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("There is not enough storage space!");
		}
	}
	public void DropOffAllIron(){
		if(Master.GetComponent <GameMaster>().CalculateStorage ()  + Player.GetComponent <PlayerStats>().pIron <= Master.GetComponent <GameMaster>().storage){
			Master.GetComponent <GameMaster>().Add ("Iron", Player.GetComponent <PlayerStats>().pIron);
			Player.GetComponent <PlayerStats> ().pIron = 0;
			if(Player.GetComponent <PlayerStats>().pIron == Player.GetComponent <PlayerStats>().maxIron){
				Master.GetComponent <GameMaster>().Add ("Happyness", 1);
			}
			Player.GetComponent <PLayerUIHandler>().UpdateText ("Iron", 0, Player.GetComponent <PlayerStats>().maxIron);
		}else{Debug.Log ("BaseBuilding: There is not enough storage space!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("There is not enough storage space!");
		}
	}
	public void DropOffAllGold(){
		if(Player.GetComponent <PlayerStats>().pGold + Master.GetComponent <GameMaster>().CalculateStorage () <= Master.GetComponent <GameMaster>().storage){
			Master.GetComponent <GameMaster>().Add ("Gold", Player.GetComponent <PlayerStats>().pGold);
			Player.GetComponent <PlayerStats> ().pGold = 0;
			if(Player.GetComponent <PlayerStats>().pGold == Player.GetComponent <PlayerStats>().maxGold){
				Master.GetComponent <GameMaster>().Add ("Happyness", 1);
			}
			Player.GetComponent <PLayerUIHandler>().UpdateText ("Gold", 0, Player.GetComponent <PlayerStats>().maxGold);
		}else{Debug.Log ("BaseBuilding: There is not enough storage space!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("There is not enough storage space!");
		}
	}
	public void DropOffAllHide(){
		if(Player.GetComponent <PlayerStats>().pHide + Master.GetComponent <GameMaster>().CalculateStorage () <= Master.GetComponent <GameMaster>().storage){
			Master.GetComponent <GameMaster>().Add ("Hide", Player.GetComponent <PlayerStats>().pHide);
			Player.GetComponent <PlayerStats> ().pHide = 0;
			Player.GetComponent <PLayerUIHandler>().UpdateText ("Hide", 0, Player.GetComponent <PlayerStats>().maxHide);
			if(Player.GetComponent <PlayerStats>().pHide == Player.GetComponent <PlayerStats>().maxHide){
				Master.GetComponent <GameMaster>().Add ("Happyness", 1);
			}
		}else{Debug.Log ("BaseBuilding: There is not enough storage space!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("There is not enough storage space!");
			//Master.GetComponent <GameMaster>().PassErrorMessage ("");
		}
	}

	public void ForceTax(){
		if(Master.GetComponent <GameMaster>().happyness >= FTaxHappynessDecrease){
			houses = GameObject.FindGameObjectsWithTag ("House");
			Master.GetComponent <GameMaster> ().Subtract ("Happyness", FTaxHappynessDecrease);
			foreach(GameObject currHouse in houses){
				currHouse.GetComponent <House>().ForceTaxes ();
			}
		}else{
			Master.GetComponent <GameMaster>().PassErrorMessage ("Your towns happyness is too low to force tax collection.");
		}
	}
	public void TakeOutWood(string temp){
		int amount = int.Parse (temp);
		if(Master.GetComponent<GameMaster> ().gWood >= amount){
			if(Player.GetComponent <PlayerStats>().maxWood >= Player.GetComponent <PlayerStats>().pWood + amount){
				Player.GetComponent <PlayerStats> ().Add ("Wood", amount);
				Master.GetComponent<GameMaster> ().Subtract ("Wood", amount);
			}else{
				Debug.Log ("Your inventory is too full for this amount");
				Master.GetComponent<GameMaster> ().PassErrorMessage ("Your inventory is too small for this amount.");
			}
		}else{
			Debug.Log ("You dont have that much res");
			Master.GetComponent<GameMaster> ().PassErrorMessage ("You do not have that much of this resource in your towns storage.");
		}
	}
	public void TakeOutRock(string temp){
		int amount = int.Parse (temp);
		if(Master.GetComponent<GameMaster> ().gRock >= amount){
			if(Player.GetComponent <PlayerStats>().maxRock >= Player.GetComponent <PlayerStats>().pRock + amount){
				Player.GetComponent <PlayerStats> ().Add ("Rock", amount);
				Master.GetComponent<GameMaster> ().Subtract ("Rock", amount);
			}else{
				Debug.Log ("Your inventory is too full for this amount");
				Master.GetComponent<GameMaster> ().PassErrorMessage ("Your inventory is too small for this amount.");
			}
		}else{
			Debug.Log ("You dont have that much res");
			Master.GetComponent<GameMaster> ().PassErrorMessage ("You do not have that much of this resource in your towns storage.");
		}
	}
	public void TakeOutIron(string temp){
		int amount = int.Parse (temp);
		if(Master.GetComponent<GameMaster> ().gIron >= amount){
			if(Player.GetComponent <PlayerStats>().maxIron >= Player.GetComponent <PlayerStats>().pIron + amount){
				Player.GetComponent <PlayerStats> ().Add ("Iron", amount);
				Master.GetComponent<GameMaster> ().Subtract ("Iron", amount);
			}else{
				Debug.Log ("Your inventory is too full for this amount");
				Master.GetComponent<GameMaster> ().PassErrorMessage ("Your inventory is too small for this amount.");
			}
		}else{
			Debug.Log ("You dont have that much res");
			Master.GetComponent<GameMaster> ().PassErrorMessage ("You do not have that much of this resource in your towns storage.");
		}
	}
	public void TakeOutFood(string temp){
		int amount = int.Parse (temp);
		if(Master.GetComponent<GameMaster> ().gFood >= amount){
			if(Player.GetComponent <PlayerStats>().maxFood >= Player.GetComponent <PlayerStats>().pFood + amount){
				Player.GetComponent <PlayerStats> ().Add ("Food", amount);
				Master.GetComponent<GameMaster> ().Subtract ("Food", amount);
			}else{
				Debug.Log ("Your inventory is too full for this amount");
				Master.GetComponent<GameMaster> ().PassErrorMessage ("Your inventory is too small for this amount.");
			}
		}else{
			Debug.Log ("You dont have that much res");
			Master.GetComponent<GameMaster> ().PassErrorMessage ("You do not have that much of this resource in your towns storage.");
		}
	}
	public void TakeOutGold(string temp){
		int amount = int.Parse (temp);
		if(Master.GetComponent<GameMaster> ().gGold >= amount){
			if(Player.GetComponent <PlayerStats>().maxGold >= Player.GetComponent <PlayerStats>().pGold + amount){
				Player.GetComponent <PlayerStats> ().Add ("Gold", amount);
				Master.GetComponent<GameMaster> ().Subtract ("Gold", amount);
			}else{
				Debug.Log ("Your inventory is too full for this amount");
				Master.GetComponent<GameMaster> ().PassErrorMessage ("Your inventory is too small for this amount.");
			}
		}else{
			Debug.Log ("You dont have that much res");
			Master.GetComponent<GameMaster> ().PassErrorMessage ("You do not have that much of this resource in your towns storage.");
		}
	}
	public void TakeOuHide(string temp){
		int amount = int.Parse (temp);
		if(Master.GetComponent<GameMaster> ().gHide >= amount){
			if(Player.GetComponent <PlayerStats>().maxHide >= Player.GetComponent <PlayerStats>().pHide + amount){
				Player.GetComponent <PlayerStats> ().Add ("Hide", amount);
				Master.GetComponent<GameMaster> ().Subtract ("Hide", amount);
			}else{
				Debug.Log ("Your inventory is too full for this amount");
				Master.GetComponent<GameMaster> ().PassErrorMessage ("Your inventory is too small for this amount.");
			}
		}else{
			Debug.Log ("You dont have that much res");
			Master.GetComponent<GameMaster> ().PassErrorMessage ("You do not have that much of this resource in your towns storage.");
		}
	}

	public void DropOffAll(){
		DropOffAllFood();
		DropOffAllWood();
		DropOffAllRock();
		DropOffAllIron();
		DropOffAllGold();
		DropOffAllHide();
	}

}
