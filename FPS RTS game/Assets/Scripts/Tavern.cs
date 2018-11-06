using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tavern : MonoBehaviour {

	private GameObject myCanvas, Master, cam, loadingBar, buildingInfo, mug, miscBuild;
	private Text loadingText, cost;
	private int x;
	private bool brew, readyToPour;
	private RaycastHit hit;
	private Vector3 place;

	public int cycleLength, townsWoodCost, townsFoodCost, happynessToGive = 3, loadAmount = 1, scaleX = 0, buildingLevel;
	public int upgrWood1, upgrRock1, upgrIron1, upgrGold1, upgrPoints1, upgrWood2, upgrRock2, upgrIron2, upgrGold2, upgrPoints2, upgrWood3, upgrRock3, upgrIron3, upgrGold3, upgrPoints3;

	// Use this for initialization
	void Start () {
		myCanvas = gameObject.transform.Find ("Canvas").gameObject;
		Master = GameObject.Find ("GameMaster");
		cam = GameObject.Find ("Main Camera");
		loadingBar = myCanvas.transform.Find ("LoadingBar").gameObject;
		miscBuild = GameObject.Find ("GUIHUD");
		loadingBar.transform.localScale = new Vector3(0, 1, 1);
		loadingText = myCanvas.transform.Find ("LoadingText").GetComponent<Text>();
		loadingText.text = scaleX + " %";
		buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
		buildingInfo.GetComponent <BuildingInformation>().UpdateText (scaleX + "% done brewing \r\n Building level: " + buildingLevel);
		Master.GetComponent <GameMaster>().Add ("Happyness", 1);
		//Master.GetComponent <GameMaster> ().AddXpToPlayer (15);
		myCanvas.SetActive (false);
		mug = gameObject.transform.Find ("Mug").gameObject;
		mug.SetActive (false);
		cost = myCanvas.transform.Find ("Cost").GetComponent <Text>();
		cost.text = "Cost: \r\n\t Wood: " + upgrWood1 + "\r\n\t Rock: " + upgrRock1 + "\r\n\t Iron: " + upgrIron1 + "\r\n\t Gold: " + upgrGold1 + "\r\n\t Xp points: " + upgrPoints1 + "\r\n Building level: "+ buildingLevel +"/3";

	}
	
	// Update is called once per frame
	void Update () {
		if(brew){
			if(x >= cycleLength){
				if(loadingBar.transform.localScale.x + loadAmount/100f < 1){
					scaleX += loadAmount;
					loadingBar.transform.localScale = new Vector3 (scaleX/100f, 1, 1);
					loadingText.text = scaleX + " % ...";
					buildingInfo.GetComponent <BuildingInformation>().UpdateText (scaleX + "% done brewing \r\n Building level: " + buildingLevel);
					x = 0;
				}else if(loadingBar.transform.localScale.x + loadAmount/100f >= 1){
					scaleX = 1;
					loadingBar.transform.localScale = new Vector3 (1, 1, 1);
					loadingText.text = "100 % Press \"Pour\" button";
					readyToPour = true;
					brew = false;
					mug.SetActive (true);
					buildingInfo.GetComponent <BuildingInformation>().UpdateText ("100 % done brewing \r\n Please press \"Pour\" button \r\n Building level: " + buildingLevel);
				}
			}else{
				x++;
			}
		}
	}

	public void InputRes(){//THE BREW BUTTON CALLS THIS FUCTION
		if(cam.GetComponent <PlayerStats>().pWood >= townsWoodCost && cam.GetComponent <PlayerStats>().pFood >= townsFoodCost){
			brew = true;
			x = 0;
			scaleX = 0;
			loadingBar.transform.localScale = new Vector3(0, 1, 1);
			cam.GetComponent <PlayerStats> ().pWood -= townsWoodCost;
			cam.GetComponent <PlayerStats> ().pFood -= townsFoodCost;
			cam.GetComponent <PLayerUIHandler> ().UpdateText ("Food", cam.GetComponent <PlayerStats> ().pFood, cam.GetComponent <PlayerStats> ().maxFood);
			cam.GetComponent <PLayerUIHandler> ().UpdateText ("Wood", cam.GetComponent <PlayerStats> ().pWood, cam.GetComponent <PlayerStats> ().maxWood);
			buildingInfo.GetComponent <BuildingInformation>().UpdateText (scaleX + "% done brewing \r\n Building level: " + buildingLevel);
		}else{
			Master.GetComponent <GameMaster>().PassErrorMessage ("You dont have enough resources for this action.");
		}
	}

	void GiveOutAlcohol(){
		Master.GetComponent <GameMaster> ().Add ("Happyness", happynessToGive);
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
		Master.GetComponent <GameMaster> ().EnablePlayer ();
		GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (true);
	}

	public void PourForTheTown(){
		if(readyToPour){
			GiveOutAlcohol ();
			readyToPour = false;
			buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Ready to brew");
			loadingText.text = "0 %";
			x = 0;
			scaleX = 0;
			loadingBar.transform.localScale = new Vector3(0, 1, 1);
			mug.SetActive (false);
		}
	}

	public void PourForSelf(){
		if(readyToPour){
			cam.GetComponent <PlayerStats>().strength += 10;
			Master.GetComponent <GameMaster> ().Subtract ("Happyness", 1);
			readyToPour = false;
			buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Ready to brew \r\n Building level: " + buildingLevel);
			loadingText.text = "0 %";
			x = 0;
			scaleX = 0;
			loadingBar.transform.localScale = new Vector3(0, 1, 1);
			mug.SetActive (false);
		}
	}
	public void UpgradeBuilding(){
		if(buildingLevel == 0){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood1 && Master.GetComponent <GameMaster>().gRock >= upgrRock1 && Master.GetComponent <GameMaster>().gIron >= upgrIron1 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints1 && Master.GetComponent <GameMaster>().gGold >= upgrGold1){
				cycleLength -= 10;
				happynessToGive += 1;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().tavernQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood1);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold1);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints1;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				buildingInfo.GetComponent <BuildingInformation>().UpdateText (scaleX + " % done brewing" + "\r\n Building level: " + buildingLevel);				
				cost.text = "Cost: \r\n\t Wood: " + upgrWood2 + "\r\n\t Rock: " + upgrRock2 + "\r\n\t Iron: " + upgrIron2 + "\r\n\t Gold: " + upgrGold2 + "\r\n\t Xp points: " + upgrPoints2 + "\r\n Building level: "+ buildingLevel +"/3";
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}	else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 1){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood2 && Master.GetComponent <GameMaster>().gRock >= upgrRock2 && Master.GetComponent <GameMaster>().gIron >= upgrIron2 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints2 && Master.GetComponent <GameMaster>().gGold >= upgrGold2){
				cycleLength -= 10;
				happynessToGive += 1;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().tavernQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood2);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold2);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints2;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "Cost: \r\n\t Wood: " + upgrWood3 + "\r\n\t Rock: " + upgrRock3 + "\r\n\t Iron: " + upgrIron3 + "\r\n\t Gold: " + upgrGold3 + "\r\n\t Xp points: " + upgrPoints3 + "\r\n Building level: "+ buildingLevel +"/3";
				buildingInfo.GetComponent <BuildingInformation>().UpdateText (scaleX + " % done brewing" + "\r\n Building level: " + buildingLevel);			
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 2){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood3 && Master.GetComponent <GameMaster>().gRock >= upgrRock3 && Master.GetComponent <GameMaster>().gIron >= upgrIron3 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints3 && Master.GetComponent <GameMaster>().gGold >= upgrGold3){
				cycleLength -= 10;
				happynessToGive += 1;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().tavernQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood3);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold3);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints3;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "You have upgraded this building to it's maximum. "+ "\r\n Building level: " + buildingLevel + "/3";
				buildingInfo.GetComponent <BuildingInformation>().UpdateText (scaleX + " % done brewing" + "\r\n Building level: " + buildingLevel);			
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else{
			Debug.Log ("Upgraded to the max!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("You have upgraded this building to it's maximum.");
		}
	}

	public void BuildingDiedProtocol(){
		brew = false;
	}

	public void DestroyBuilding(){
		CloseMenu ();
		BuildingDiedProtocol ();
		Destroy (gameObject, 0.1f);
	}

}
