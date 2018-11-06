using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour {

	public int upgrWood1, upgrRock1, upgrIron1, upgrGold1, upgrPoints1, upgrWood2, upgrRock2, upgrIron2, upgrGold2, upgrPoints2, upgrWood3, upgrRock3, upgrIron3, upgrGold3, upgrPoints3;

	private Text cost;
	private Vector3 place;
	private RaycastHit hit;
	private int buildingLevel;
	private GameObject Base, myCanvas, Master, buildingInfo, cam, constrBuilding, miscBuild;

	// Use this for initialization
	void Start () {
		Base = GameObject.Find ("BaseBuilding");
		miscBuild = GameObject.Find ("GUIHUD");
		myCanvas = gameObject.transform.Find ("Canvas").gameObject;
		cost = myCanvas.transform.Find ("Cost").GetComponent <Text> ();
		Master = GameObject.Find ("GameMaster");
		buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
		cam = GameObject.Find ("Main Camera");
		constrBuilding = GameObject.Find ("ConstructBuilding");
		buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Castle level: " + buildingLevel);
		Base.GetComponent <BaseBuilding>().townBuildDistanceLimit += 25;
		Base.GetComponent <BaseBuilding> ().FTaxHappynessDecrease -= 1;
		cam.GetComponent <PlayerStats> ().gatherSpeed -= 5;
		cam.GetComponent <PlayerStats> ().strength += 20;
		cam.GetComponent <PlayerStats> ().distance += 3;
		cam.GetComponent <PlayerStats> ().interactDistance += 3;
		constrBuilding.GetComponent <ConstructBuilding>().maxBanks += 1;
		constrBuilding.GetComponent <ConstructBuilding> ().maxFarms += 3;
		constrBuilding.GetComponent <ConstructBuilding> ().maxHouses += 10;
		constrBuilding.GetComponent <ConstructBuilding> ().maxMS += 3;
		constrBuilding.GetComponent <ConstructBuilding> ().maxOutposts += 5;
		constrBuilding.GetComponent <ConstructBuilding> ().maxStorages += 10;
		constrBuilding.GetComponent <ConstructBuilding> ().maxTaverns += 2;
		constrBuilding.GetComponent <ConstructBuilding> ().maxTP += 3;
		constrBuilding.GetComponent <ConstructBuilding> ().maxAC += 3;
		Master.GetComponent <GameMaster> ().storage += 1000;
		Master.GetComponent <GameMaster> ().recyclePercentage = 0.8f;
		Master.GetComponent <GameMaster> ().happyness += 10;
		Master.GetComponent <GameMaster> ().wFoodConsumption = 0;
		Master.GetComponent <GameMaster> ().pFoodConsuption -= 1;
		cam.GetComponent <Gather>().minXp += 7;
		cam.GetComponent <Gather>().maxXp  += 7;
		cam.GetComponent <Gather>().maxWood  += 7;
		cam.GetComponent <Gather>().minWood  += 7;
		cam.GetComponent <Gather>().maxRock  += 7;
		cam.GetComponent <Gather>().minRock  += 7;
		cam.GetComponent <Gather>().maxIron  += 7;
		cam.GetComponent <Gather>().minIron  += 7;
		cam.GetComponent <Gather>().maxFood  += 7;
		cam.GetComponent <Gather>().minFood  += 7;
		cam.GetComponent <Gather>().maxHide  += 7;
		cam.GetComponent <Gather>().minHide  += 7; 
		gameObject.GetComponent <BuildingHealth> ().health += 300;
		gameObject.GetComponent <BuildingHealth> ().maxHealth += 300;
		cost.text = "Cost: \r\n\t Wood: " + upgrWood1 + "\r\n\t Rock: " + upgrRock1 + "\r\n\t Iron: " + upgrIron1 + "\r\n\t Gold: " + upgrGold1 + "\r\n\t Xp points: " + upgrPoints1 + "\r\n Building level: "+ buildingLevel +"/3";
		myCanvas.SetActive (false);
	}

	public void UpgradeBuilding(){
		if(buildingLevel == 0){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood1 && Master.GetComponent <GameMaster>().gRock >= upgrRock1 && Master.GetComponent <GameMaster>().gIron >= upgrIron1 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints1 && Master.GetComponent <GameMaster>().gGold >= upgrGold1){
				cam.GetComponent <Gather>().minXp += 17;
				cam.GetComponent <Gather>().maxXp  += 17;
				cam.GetComponent <Gather>().maxWood  += 17;
				cam.GetComponent <Gather>().minWood  += 17;
				cam.GetComponent <Gather>().maxRock  += 17;
				cam.GetComponent <Gather>().minRock  += 17;
				cam.GetComponent <Gather>().maxIron  += 17;
				cam.GetComponent <Gather>().minIron  += 17;
				cam.GetComponent <Gather>().maxFood  += 17;
				cam.GetComponent <Gather>().minFood  += 17;
				cam.GetComponent <Gather>().maxHide  += 17;
				cam.GetComponent <Gather>().minHide  += 17; 
				Master.GetComponent <GameMaster> ().storage += 1000;
				Master.GetComponent <GameMaster> ().recyclePercentage = 1f;
				Master.GetComponent <GameMaster> ().happyness += 10;
				gameObject.GetComponent <BuildingHealth> ().health += 300;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 300;
				cam.GetComponent <PlayerStats> ().strength += 20;
				cam.GetComponent <PlayerStats> ().distance += 3;
				cam.GetComponent <PlayerStats> ().interactDistance += 3;
				constrBuilding.GetComponent <ConstructBuilding>().maxBanks += 1;
				constrBuilding.GetComponent <ConstructBuilding> ().maxFarms += 2;
				constrBuilding.GetComponent <ConstructBuilding> ().maxHouses += 10;
				constrBuilding.GetComponent <ConstructBuilding> ().maxMS += 2;
				constrBuilding.GetComponent <ConstructBuilding> ().maxOutposts += 15;
				constrBuilding.GetComponent <ConstructBuilding> ().maxStorages += 5;
				constrBuilding.GetComponent <ConstructBuilding> ().maxTaverns += 1;
				constrBuilding.GetComponent <ConstructBuilding> ().maxTP += 2;
				constrBuilding.GetComponent <ConstructBuilding> ().maxAC += 2;
				Base.GetComponent <BaseBuilding>().townBuildDistanceLimit += 25;
				Base.GetComponent <BaseBuilding> ().FTaxHappynessDecrease -= 1;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().castleQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood1);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold1);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints1;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Castle level: " + buildingLevel);
				cost.text = "Cost: \r\n\t Wood: " + upgrWood2 + "\r\n\t Rock: " + upgrRock2 + "\r\n\t Iron: " + upgrIron2 + "\r\n\t Gold: " + upgrGold2 + "\r\n\t Xp points: " + upgrPoints2 + "\r\n Building level: "+ buildingLevel +"/3";
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}	else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 1){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood2 && Master.GetComponent <GameMaster>().gRock >= upgrRock2 && Master.GetComponent <GameMaster>().gIron >= upgrIron2 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints2 && Master.GetComponent <GameMaster>().gGold >= upgrGold2){
				cam.GetComponent <Gather>().minXp += 17;
				cam.GetComponent <Gather>().maxXp  += 17;
				cam.GetComponent <Gather>().maxWood  += 17;
				cam.GetComponent <Gather>().minWood  += 17;
				cam.GetComponent <Gather>().maxRock  += 17;
				cam.GetComponent <Gather>().minRock  += 17;
				cam.GetComponent <Gather>().maxIron  += 17;
				cam.GetComponent <Gather>().minIron  += 17;
				cam.GetComponent <Gather>().maxFood  += 17;
				cam.GetComponent <Gather>().minFood  += 17;
				cam.GetComponent <Gather>().maxHide  += 17;
				cam.GetComponent <Gather>().minHide  += 17; 
				Master.GetComponent <GameMaster> ().storage += 1000;
				Master.GetComponent <GameMaster> ().recyclePercentage = 1f;
				Master.GetComponent <GameMaster> ().happyness += 10;
				gameObject.GetComponent <BuildingHealth> ().health += 300;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 300;
				cam.GetComponent <PlayerStats> ().strength += 15;
				cam.GetComponent <PlayerStats> ().distance += 3;
				cam.GetComponent <PlayerStats> ().interactDistance += 3;
				constrBuilding.GetComponent <ConstructBuilding>().maxBanks += 1;
				constrBuilding.GetComponent <ConstructBuilding> ().maxFarms += 2;
				constrBuilding.GetComponent <ConstructBuilding> ().maxHouses += 10;
				constrBuilding.GetComponent <ConstructBuilding> ().maxMS += 2;
				constrBuilding.GetComponent <ConstructBuilding> ().maxOutposts += 15;
				constrBuilding.GetComponent <ConstructBuilding> ().maxStorages += 5;
				constrBuilding.GetComponent <ConstructBuilding> ().maxTP += 2;
				constrBuilding.GetComponent <ConstructBuilding> ().maxAC += 2;
				Base.GetComponent <BaseBuilding>().townBuildDistanceLimit += 30;
				Base.GetComponent <BaseBuilding> ().FTaxHappynessDecrease -= 2;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().castleQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood2);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold2);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints2;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "Cost: \r\n\t Wood: " + upgrWood3 + "\r\n\t Rock: " + upgrRock3 + "\r\n\t Iron: " + upgrIron3 + "\r\n\t Gold: " + upgrGold3 + "\r\n\t Xp points: " + upgrPoints3 + "\r\n Building level: "+ buildingLevel +"/3";
				buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Castle level: " + buildingLevel);
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 2){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood3 && Master.GetComponent <GameMaster>().gRock >= upgrRock3 && Master.GetComponent <GameMaster>().gIron >= upgrIron3 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints3 && Master.GetComponent <GameMaster>().gGold >= upgrGold3){
				cam.GetComponent <Gather>().minXp += 17;
				cam.GetComponent <Gather>().maxXp  += 17;
				cam.GetComponent <Gather>().maxWood  += 17;
				cam.GetComponent <Gather>().minWood  += 17;
				cam.GetComponent <Gather>().maxRock  += 17;
				cam.GetComponent <Gather>().minRock  += 17;
				cam.GetComponent <Gather>().maxIron  += 17;
				cam.GetComponent <Gather>().minIron  += 17;
				cam.GetComponent <Gather>().maxFood  += 17;
				cam.GetComponent <Gather>().minFood  += 17;
				cam.GetComponent <Gather>().maxHide  += 17;
				cam.GetComponent <Gather>().minHide  += 17; 
				Master.GetComponent <GameMaster> ().storage += 1000;
				Master.GetComponent <GameMaster> ().happyness += 10;
				gameObject.GetComponent <BuildingHealth> ().health += 500;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 500;
				cam.GetComponent <PlayerStats> ().strength += 40;
				cam.GetComponent <PlayerStats> ().distance += 3;
				cam.GetComponent <PlayerStats> ().interactDistance += 3;
				constrBuilding.GetComponent <ConstructBuilding> ().maxFarms += 2;
				constrBuilding.GetComponent <ConstructBuilding> ().maxHouses += 10;
				constrBuilding.GetComponent <ConstructBuilding> ().maxMS += 2;
				constrBuilding.GetComponent <ConstructBuilding> ().maxOutposts += 15;
				constrBuilding.GetComponent <ConstructBuilding> ().maxStorages += 5;
				constrBuilding.GetComponent <ConstructBuilding> ().maxTP += 2;
				constrBuilding.GetComponent <ConstructBuilding> ().maxAC += 2;
				Base.GetComponent <BaseBuilding>().townBuildDistanceLimit += 50;
				Base.GetComponent <BaseBuilding> ().FTaxHappynessDecrease -= 2;
				//TODO animal coop max needs to be upgraded here
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().castleQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood3);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold3);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints3;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "You have upgraded your castle to it's maximum. "+ "\r\n Castle level: " + buildingLevel + "/3";
				buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Castle level: " + buildingLevel);
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else{
			Debug.Log ("Upgraded to the max!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("You have upgraded your castle to it's maximum.");
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
		Master.GetComponent <GameMaster>().EnablePlayer ();
		GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (false);
	}

}
