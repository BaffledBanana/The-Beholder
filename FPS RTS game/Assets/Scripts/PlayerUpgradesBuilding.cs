using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradesBuilding : MonoBehaviour {
		
	private GameObject cam, PlayerUpgradesBuildingUI, Master, gatherStats, buildingInfo;

	//gs = GatherSpeed, mw = MaxWood, mr = MaxRock...

	public Text gsText, mwText, mrText, miText, mfText, mgText, mhText, mmWood, mmRock, mmIron, mmFood, mmHide;
	public int gsWood, gsRock, gsIron, gsGold, gswIncrease, gsrIncrease, gsiIncrease, gsgIncrease, maxGSLevel, gsLevel = 0; // also add leather as a requirement here
	public int mwWood, mwRock, mwIron, mwGold, mwwIncrease, mwrIncrease, mwiIncrease, mwgIncrease;
	public int mrWood, mrRock, mrIron, mrGold, mrwIncrease, mrrIncrease, mriIncrease, mrgIncrease;
	public int miWood, miRock, miIron, miGold, miwIncrease, mirIncrease, miiIncrease, migIncrease;
	public int mfWood, mfRock, mfIron, mfGold, mfwIncrease, mfrIncrease, mfiIncrease, mfgIncrease;
	public int mgWood, mgIron, mgGold, mgHide, mgwIncrease, mgiIncrease, mggIncrease, mghIncrease;
	public int mhWood, mhIron, mhGold, mhHide, mhwIncrease, mhiIncrease, mhgIncrease, mhhIncrease;
	public int woodChanceIncrease, rockChanceIncrease, ironChanceIncrease, foodChanceIncrease, hideChanceIncrease;
	public int WCIGoldCost, RCIGoldCost, ICIGoldCost, FCIGoldCost, HCIGoldCost, WCIGCostIncrease, RCIGCostIncrease, ICIGCostIncrease, FCIGCostIncrease, HCIGCostIncrease;

	void Start () {
		cam = GameObject.Find ("Main Camera");
		PlayerUpgradesBuildingUI = GameObject.Find ("PlayerUpgradesBuildingUI");
		Master = GameObject.Find ("GameMaster");
		buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
		buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Gathering speed level: " + gsLevel + "\r\n min/max wood: " + cam.GetComponent <Gather>().minWood + "/" + cam.GetComponent <Gather>().maxWood + "\r\n min/max rock: " + cam.GetComponent <Gather>().minRock + "/" + cam.GetComponent <Gather>().maxRock + "\r\n min/max iron: " + cam.GetComponent <Gather>().minIron + "/" + cam.GetComponent <Gather>().maxIron + "\r\n min/max food: " + cam.GetComponent <Gather>().minFood + "/" + cam.GetComponent <Gather>().maxFood + "\r\n min/max hide: " + cam.GetComponent <Gather>().minHide + "/" + cam.GetComponent <Gather>().maxHide);
		PlayerUpgradesBuildingUI.SetActive (false);
		CloseMenu ();
		mmHide.text = "Cost: \r\n\t Gold: " + HCIGoldCost;
		mmIron.text = "Cost: \r\n\t Gold: " + ICIGoldCost;
		mmFood.text = "Cost: \r\n\t Gold: " + FCIGoldCost;
		mmRock.text = "Cost: \r\n\t Gold: " + RCIGoldCost;
		mmWood.text = "Cost: \r\n\t Gold: " + WCIGoldCost;
		mrText.text = "Cost: \r\n\t Wood: " + mrWood + "\r\n\t Rock: " + mrRock + "\r\n\t Iron: " + mrIron + "\r\n\t Gold: " + mrGold;
		miText.text = "Cost: \r\n\t Wood: " + miWood + "\r\n\t Rock: " + miRock + "\r\n\t Iron: " + miIron + "\r\n\t Gold: " + miGold;
		mfText.text = "Cost: \r\n\t Wood: " + mfWood + "\r\n\t Rock: " + mfRock + "\r\n\t Iron: " + mfIron + "\r\n\t Gold: " + mfGold;
		mgText.text = "Cost: \r\n\t Wood: " + mgWood + "\r\n\t Hide: " + mgHide + "\r\n\t Iron: " + mgIron + "\r\n\t Gold: " + mgGold;
		mhText.text = "Cost: \r\n\t Wood: " + mhWood + "\r\n\t Hide: " + mhHide + "\r\n\t Iron: " + mhIron + "\r\n\t Gold: " + mhGold;
		mwText.text = "Cost: \r\n\t Wood: " + mwWood + "\r\n\t Rock: " + mwRock + "\r\n\t Iron: " + mwIron + "\r\n\t Gold: " + mwGold;
		gsText.text = "Cost: \r\n\t Wood: " + gsWood + "\r\n\t Rock: " + gsRock + "\r\n\t Iron " + gsIron + "\r\n\t Gold: " + gsGold + "\r\n\t Level: " + gsLevel + "/" + maxGSLevel;
	}
	
	public void IncreaseGatherSpeed(){
		if(cam.GetComponent <PlayerStats>().IncreaseGatherSpeed (gsWood, gsRock, gsIron, gsGold)){
			gsWood += gswIncrease;
			gsRock += gsrIncrease;
			gsIron += gsiIncrease;
			gsGold += gsgIncrease;
			gsLevel += 1;
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Gathering speed level: " + gsLevel + "\r\n min/max wood: " + cam.GetComponent <Gather>().minWood + "/" + cam.GetComponent <Gather>().maxWood + "\r\n min/max rock: " + cam.GetComponent <Gather>().minRock + "/" + cam.GetComponent <Gather>().maxRock + "\r\n min/max iron: " + cam.GetComponent <Gather>().minIron + "/" + cam.GetComponent <Gather>().maxIron + "\r\n min/max food: " + cam.GetComponent <Gather>().minFood + "/" + cam.GetComponent <Gather>().maxFood + "\r\n min/max hide: " + cam.GetComponent <Gather>().minHide + "/" + cam.GetComponent <Gather>().maxHide);
			gsText.text = "Cost: \r\n\t Wood: " + gsWood + "\r\n\t Rock: " + gsRock + "\r\n\t Iron: " + gsIron + "\r\n\t Gold: " + gsGold + "\r\n\t Level: " + gsLevel + "/" + maxGSLevel;
		}
	}
	public void IncreaseMaxWood(){
		if(cam.GetComponent <PlayerStats>().IncreaseMaxWood (mwWood, mwRock, mwIron, mwGold)){
			mwWood += mwwIncrease;
			mwRock += mwrIncrease;
			mwIron += mwiIncrease;
			mwGold += mwgIncrease;
			mwText.text = "Cost: \r\n\t Wood: " + mwWood + "\r\n\t Rock: " + mwRock + "\r\n\t Iron: " + mwIron + "\r\n\t Gold: " + mwGold;
		}
	}
	public void IncreaseMaxRock(){
		if(cam.GetComponent <PlayerStats>().IncreaseMaxRock (mrWood, mrRock, mrIron, mrGold)){
			mrWood += mrwIncrease;
			mrRock += mrrIncrease;
			mrIron += mriIncrease;
			mrGold += mrgIncrease;
			mrText.text = "Cost: \r\n\t Wood: " + mrWood + "\r\n\t Rock: " + mrRock + "\r\n\t Iron: " + mrIron + "\r\n\t Gold: " + mrGold;
		}
	}
	public void IncreaseMaxIron(){
		if(cam.GetComponent <PlayerStats>().IncreaseMaxIron (miWood, miRock, miIron, miGold)){
			miWood += miwIncrease;
			miRock += mirIncrease;
			miIron += miiIncrease;
			miGold += migIncrease;
			miText.text = "Cost: \r\n\t Wood: " + miWood + "\r\n\t Rock: " + miRock + "\r\n\t Iron: " + miIron + "\r\n\t Gold: " + miGold;
		}
	}
	public void IncreaseMaxFood(){
		if(cam.GetComponent <PlayerStats>().IncreaseMaxFood (mfWood, mfRock, mfIron, mfGold)){
			mfWood += mfwIncrease;
			mfRock += mfrIncrease;
			mfIron += mfiIncrease;
			mfGold += mfgIncrease;
			mfText.text = "Cost: \r\n\t Wood: " + mfWood + "\r\n\t Rock: " + mfRock + "\r\n\t Iron: " + mfIron + "\r\n\t Gold: " + mfGold;
		}
	}
	public void IncreaseMaxGold(){ //change the params in the method thats being passed
		if(cam.GetComponent <PlayerStats>().IncreaseMaxGold (mgWood, mgHide, mgIron, mgGold)){
			mgWood += mgwIncrease;
			mgHide += mghIncrease;
			mgIron += mgiIncrease;
			mgGold += mggIncrease;
			mgText.text = "Cost: \r\n\t Wood: " + mgWood + "\r\n\t Hide: " + mgHide + "\r\n\t Iron: " + mgIron + "\r\n\t Gold: " + mgGold;
		}
	}
	public void IncreaseMaxHide(){//change the params in the method thats being passed
		if(cam.GetComponent <PlayerStats>().IncreaseMaxHide (mhWood, mhHide, mhIron, mhGold)){
			mhWood += mhwIncrease;
			mhHide += mhhIncrease;
			mhIron += mhiIncrease;
			mhGold += mhgIncrease;
			mhText.text = "Cost: \r\n\t Wood: " + mhWood + "\r\n\t Hide: " + mhHide + "\r\n\t Iron: " + mhIron + "\r\n\t Gold: " + mhGold;
		}
	}
	public void UpgradeMinMaxWood(){
		if(Master.GetComponent <GameMaster>().gGold >= WCIGoldCost){
			cam.GetComponent <Gather>().minWood += woodChanceIncrease;
			cam.GetComponent <Gather>().maxWood += woodChanceIncrease;
			Master.GetComponent <GameMaster>().Subtract ("Gold", WCIGoldCost);
			WCIGoldCost += WCIGCostIncrease;
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Gathering speed level: " + gsLevel + "\r\n min/max wood: " + cam.GetComponent <Gather>().minWood + "/" + cam.GetComponent <Gather>().maxWood + "\r\n min/max rock: " + cam.GetComponent <Gather>().minRock + "/" + cam.GetComponent <Gather>().maxRock + "\r\n min/max iron: " + cam.GetComponent <Gather>().minIron + "/" + cam.GetComponent <Gather>().maxIron + "\r\n min/max food: " + cam.GetComponent <Gather>().minFood + "/" + cam.GetComponent <Gather>().maxFood + "\r\n min/max hide: " + cam.GetComponent <Gather>().minHide + "/" + cam.GetComponent <Gather>().maxHide);

			mmWood.text = "Cost: \r\n\t Gold: " + WCIGoldCost;
		}else{
			Debug.Log ("PlayerUpgradesBuilding: you dont have enough gold for this action");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you do not have enough gold for this upgrade.");
		}
	}
	public void UpgradeMinMaxRock(){
		if(Master.GetComponent <GameMaster>().gGold >= RCIGoldCost){
			cam.GetComponent <Gather>().minRock += rockChanceIncrease;
			cam.GetComponent <Gather>().maxRock += rockChanceIncrease;
			Master.GetComponent <GameMaster>().Subtract ("Gold", RCIGoldCost);
			RCIGoldCost += RCIGCostIncrease;
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Gathering speed level: " + gsLevel + "\r\n min/max wood: " + cam.GetComponent <Gather>().minWood + "/" + cam.GetComponent <Gather>().maxWood + "\r\n min/max rock: " + cam.GetComponent <Gather>().minRock + "/" + cam.GetComponent <Gather>().maxRock + "\r\n min/max iron: " + cam.GetComponent <Gather>().minIron + "/" + cam.GetComponent <Gather>().maxIron + "\r\n min/max food: " + cam.GetComponent <Gather>().minFood + "/" + cam.GetComponent <Gather>().maxFood + "\r\n min/max hide: " + cam.GetComponent <Gather>().minHide + "/" + cam.GetComponent <Gather>().maxHide);

			mmRock.text = "Cost: \r\n\t Gold: " + RCIGoldCost;
		}else{
			Debug.Log ("PlayerUpgradesBuilding: you dont have enough gold for this action");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you do not have enough gold for this upgrade.");
		}
	}
	public void UpgradeMinMaxFood(){
		if(Master.GetComponent <GameMaster>().gGold >= FCIGoldCost){
			cam.GetComponent <Gather>().minFood += foodChanceIncrease;
			cam.GetComponent <Gather>().maxFood += foodChanceIncrease;
			Master.GetComponent <GameMaster>().Subtract ("Gold", FCIGoldCost);
			FCIGoldCost += FCIGCostIncrease;
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Gathering speed level: " + gsLevel + "\r\n min/max wood: " + cam.GetComponent <Gather>().minWood + "/" + cam.GetComponent <Gather>().maxWood + "\r\n min/max rock: " + cam.GetComponent <Gather>().minRock + "/" + cam.GetComponent <Gather>().maxRock + "\r\n min/max iron: " + cam.GetComponent <Gather>().minIron + "/" + cam.GetComponent <Gather>().maxIron + "\r\n min/max food: " + cam.GetComponent <Gather>().minFood + "/" + cam.GetComponent <Gather>().maxFood + "\r\n min/max hide: " + cam.GetComponent <Gather>().minHide + "/" + cam.GetComponent <Gather>().maxHide);

			mmFood.text = "Cost: \r\n\t Gold: " + FCIGoldCost;
		}else{
			Debug.Log ("PlayerUpgradesBuilding: you dont have enough gold for this action");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you do not have enough gold for this upgrade.");
		}
	}
	public void UpgradeMinMaxIron(){
		if(Master.GetComponent <GameMaster>().gGold >= ICIGoldCost){
			cam.GetComponent <Gather>().minIron += ironChanceIncrease;
			cam.GetComponent <Gather>().maxIron += ironChanceIncrease;
			Master.GetComponent <GameMaster>().Subtract ("Gold", ICIGoldCost);
			ICIGoldCost += ICIGCostIncrease;
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Gathering speed level: " + gsLevel + "\r\n min/max wood: " + cam.GetComponent <Gather>().minWood + "/" + cam.GetComponent <Gather>().maxWood + "\r\n min/max rock: " + cam.GetComponent <Gather>().minRock + "/" + cam.GetComponent <Gather>().maxRock + "\r\n min/max iron: " + cam.GetComponent <Gather>().minIron + "/" + cam.GetComponent <Gather>().maxIron + "\r\n min/max food: " + cam.GetComponent <Gather>().minFood + "/" + cam.GetComponent <Gather>().maxFood + "\r\n min/max hide: " + cam.GetComponent <Gather>().minHide + "/" + cam.GetComponent <Gather>().maxHide);

			mmIron.text = "Cost: \r\n\t Gold: " + ICIGoldCost;
		}else{
			Debug.Log ("PlayerUpgradesBuilding: you dont have enough gold for this action");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you do not have enough gold for this upgrade.");
		}
	}
	public void UpgradeMinMaxHide(){
		if(Master.GetComponent <GameMaster>().gGold >= HCIGoldCost){
			cam.GetComponent <Gather>().minHide += hideChanceIncrease;
			cam.GetComponent <Gather>().maxHide += hideChanceIncrease;
			Master.GetComponent <GameMaster>().Subtract ("Gold", HCIGoldCost);
			HCIGoldCost += HCIGCostIncrease;
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Gathering speed level: " + gsLevel + "\r\n min/max wood: " + cam.GetComponent <Gather>().minWood + "/" + cam.GetComponent <Gather>().maxWood + "\r\n min/max rock: " + cam.GetComponent <Gather>().minRock + "/" + cam.GetComponent <Gather>().maxRock + "\r\n min/max iron: " + cam.GetComponent <Gather>().minIron + "/" + cam.GetComponent <Gather>().maxIron + "\r\n min/max food: " + cam.GetComponent <Gather>().minFood + "/" + cam.GetComponent <Gather>().maxFood + "\r\n min/max hide: " + cam.GetComponent <Gather>().minHide + "/" + cam.GetComponent <Gather>().maxHide);

			mmHide.text = "Cost: \r\n\t Gold: " + HCIGoldCost;
		}else{
			Debug.Log ("PlayerUpgradesBuilding: you dont have enough gold for this action");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you do not have enough gold for this upgrade.");
		}
	}

	public void OpenMenu(){
		if(gameObject.GetComponent <BuildHammer>().isBuilt){
			Master.GetComponent <GameMaster> ().DisablePlayer ();
			PlayerUpgradesBuildingUI.SetActive (true);	
		}
	}
	public void CloseMenu(){
		PlayerUpgradesBuildingUI.SetActive (false);
		Master.GetComponent <GameMaster> ().EnablePlayer ();
	}

	public void DestroyBuilding(){
		CloseMenu ();
		//BuildingDiedProtocol ();
		Destroy (gameObject, 0.1f);
	}

}
