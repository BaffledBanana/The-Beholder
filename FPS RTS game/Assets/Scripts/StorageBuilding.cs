using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageBuilding : MonoBehaviour {

	public int storage = 300, addStorage = 500, upgrWood1, upgrRock1, upgrIron1, upgrPoints1, upgrGold1, upgrWood2, upgrRock2, upgrIron2, upgrPoints2, upgrGold2, upgrWood3, upgrRock3, upgrIron3, upgrPoints3, upgrGold3;
	public int lvl1Storage, lvl2Storage, lvl3Storage, buildingLevel = 0;
	public Text cost;
	public GameObject miscBuild;

	private int amount;
	private RaycastHit hit;
	private Vector3 place;
	private GameObject Master, buildingInfo, cam, myCanvas;

	void Start () {
		storage = addStorage;
		myCanvas = gameObject.transform.Find ("Canvas").gameObject;
		myCanvas.SetActive (false);
		Master = GameObject.Find ("GameMaster");
		cam = GameObject.Find ("Main Camera");
		miscBuild = GameObject.Find ("GUIHUD");
		buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
	}

	void Update(){
		buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Towns storage capacity: " + Master.GetComponent <GameMaster>().CalculateStorage () + "/" + Master.GetComponent <GameMaster>().storage + "\r\n Building level: " + buildingLevel);
	}

	public void UpgradeStorage () {
		if(buildingLevel == 0){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood1 && Master.GetComponent <GameMaster>().gRock >= upgrRock1 && Master.GetComponent <GameMaster>().gIron >= upgrIron1 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints1 && Master.GetComponent <GameMaster>().gGold >= upgrGold1){
				storage += lvl1Storage;
				buildingLevel++;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				Master.GetComponent <GameMaster>().storage += lvl1Storage;
				Master.GetComponent <GameMaster> ().UpdateValues ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood1);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold1);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints1;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Towns storage capacity: " + Master.GetComponent <GameMaster>().CalculateStorage () + "/" + Master.GetComponent <GameMaster>().storage + "\r\n Building level: " + buildingLevel);				cost.text = "Cost: \r\n\t Wood: " + upgrWood2 + "\r\n\t Rock: " + upgrRock2 + "\r\n\t Iron: " + upgrIron2 + "\r\n\t Gold: " + upgrGold2 + "\r\n\t Xp points: " + upgrPoints2 + "\r\n Building level: "+ buildingLevel +"/3";
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				miscBuild.GetComponent <MiscBuildMenu> ().storageQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				CloseMenu ();
			}	else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 1){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood2 && Master.GetComponent <GameMaster>().gRock >= upgrRock2 && Master.GetComponent <GameMaster>().gIron >= upgrIron2 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints2 && Master.GetComponent <GameMaster>().gGold >= upgrGold2){
				storage += lvl2Storage;
				buildingLevel++;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				Master.GetComponent <GameMaster>().storage += lvl2Storage;
				Master.GetComponent <GameMaster> ().UpdateValues ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood2);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold2);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints2;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "Cost: \r\n\t Wood: " + upgrWood3 + "\r\n\t Rock: " + upgrRock3 + "\r\n\t Iron: " + upgrIron3 + "\r\n\t Gold: " + upgrGold3 + "\r\n\t Xp points: " + upgrPoints3 + "\r\n Building level: "+ buildingLevel +"/3";
				buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Towns storage capacity: " + Master.GetComponent <GameMaster>().CalculateStorage () + "/" + Master.GetComponent <GameMaster>().storage + "\r\n Building level: " + buildingLevel);				CloseMenu ();
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				miscBuild.GetComponent <MiscBuildMenu> ().storageQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				CloseMenu ();
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 2){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood3 && Master.GetComponent <GameMaster>().gRock >= upgrRock3 && Master.GetComponent <GameMaster>().gIron >= upgrIron3 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints3 && Master.GetComponent <GameMaster>().gGold >= upgrGold3){
				storage += lvl3Storage;
				buildingLevel++;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				Master.GetComponent <GameMaster>().storage += lvl3Storage;
				Master.GetComponent <GameMaster> ().UpdateValues ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood3);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold3);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints3;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "You have upgraded this building to it's maximum. "+ "\r\n Building level: " + buildingLevel + "/3";
				buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Towns storage capacity: " + Master.GetComponent <GameMaster>().CalculateStorage () + "/" + Master.GetComponent <GameMaster>().storage + "\r\n Building level: " + buildingLevel);				CloseMenu ();
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				miscBuild.GetComponent <MiscBuildMenu> ().storageQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				CloseMenu ();
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else{
			Debug.Log ("Upgraded to the max!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("You have upgraded this building to it's maximum.");
		}
	}

	public void OpenMenu(){
		if(gameObject.GetComponent <BuildHammer>().isBuilt){
			Master.GetComponent <GameMaster> ().DisablePlayer ();
			myCanvas.SetActive (true);
			GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (true);
		}
	}
	public void CloseMenu(){
		myCanvas.SetActive (false);
		Master.GetComponent<GameMaster> ().EnablePlayer ();
		GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (false);
	}

	public void BuildingDiedProtocol(){
		Master.GetComponent <GameMaster> ().storage -= storage;
		storage = 0;
	}

	public void DumpWood(string number){
		int.TryParse (number, out amount);
		if(Master.GetComponent <GameMaster>().gWood >= amount){
			Master.GetComponent <GameMaster> ().Subtract ("Wood", amount);
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have that much wood in your town's storage.");
		}
	}
	public void DumpRock(string number){
		int.TryParse (number, out amount);
		if(Master.GetComponent <GameMaster>().gRock >= amount){
			Master.GetComponent <GameMaster> ().Subtract ("Rock", amount);
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have that much rock in your town's storage.");
		}
	}
	public void DumpIron(string number){
		int.TryParse (number, out amount);
		if(Master.GetComponent <GameMaster>().gIron >= amount){
			Master.GetComponent <GameMaster> ().Subtract ("Iron", amount);
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have that much iron in your town's storage.");
		}
	}
	public void DumpFood(string number){
		int.TryParse (number, out amount);
		if(Master.GetComponent <GameMaster>().gFood >= amount){
			Master.GetComponent <GameMaster> ().Subtract ("Food", amount);
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have that much food in your town's storage.");
		}
	}
	public void DumpGold(string number){
		int.TryParse (number, out amount);
		if(Master.GetComponent <GameMaster>().gGold >= amount){
			Master.GetComponent <GameMaster> ().Subtract ("Gold", amount);
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have that much gold in your town's storage.");
		}
	}
	public void DumpHide(string number){
		int.TryParse (number, out amount);
		if(Master.GetComponent <GameMaster>().gHide >= amount){
			Master.GetComponent <GameMaster> ().Subtract ("Hide", amount);
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have that much hide in your town's storage.");
		}
	}

	public void DestroyBuilding(){
		CloseMenu ();
		BuildingDiedProtocol ();
		Destroy (gameObject, 0.1f);
	}

	public void Built(){
		Master.GetComponent <GameMaster>().storage += storage;
		Master.GetComponent <GameMaster> ().UpdateValues ();
		Master.GetComponent <GameMaster>().Add ("Happyness", 1);
		buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Towns storage capacity: " + Master.GetComponent <GameMaster>().CalculateStorage () + "/" + Master.GetComponent <GameMaster>().storage + "\r\n Building level: " + buildingLevel);
		buildingInfo.SetActive (true);
		cost.text = "Cost: \r\n\t Wood: " + upgrWood1 + "\r\n\t Rock: " + upgrRock1 + "\r\n\t Iron: " + upgrIron1 + "\r\n\t Gold: " + upgrGold1 + "\r\n\t Xp points: " + upgrPoints1 + "\r\n Building level: "+ buildingLevel +"/3";
	}

}
