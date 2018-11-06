using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiscBuildMenu : MonoBehaviour {
	[HideInInspector]
	public int storageQueue, houseQueue, marketQueue, ACQueue, bankQueue, barrQueue, BTQueue, BSQueue, castleQueue, DTQueue, MSQueue, outpostQueue, tavernQueue, TPQueue, TCQueue;

	public GameObject storageUpgrPH, storageUpgrP, houseUpgrP, houseUpgrPH, marketUpgrP, marketUpgrPH, ACUpgrP, ACUpgrPH, BankUpgrP, bankUpgrPH, barrUpgrP, barrUpgrPH, BTUpgrP, BTUpgrPH, BSUpgrP, BSUpgrPH, castleUpgrP, castleUpgrPH, DTUpgrP, DTUpgrPH, MSUpgrP, MSUpgrPH, outpostUpgrP, outpostUpgrPH, tavernUpgrP, tavernUpgrPH, TCUpgrP, TCUpgrPH, TPUpgrP, TPUpgrPH;
	public float upOffsetStorage, upOffsetHouse, upOffsetMarket, upOffsetAC, upOffsetBank, upOffsetBarr, upOffsetBS, upOffsetBT, upOffsetCastle, upOffsetDT, upOffsetMS, upOffsetOutpost, upOffsetTavern, upOffsetTC, upOffsetTP;

	private bool storageBool, houseBool, marketBool, ACBool, bankBool, barrBool, BTBool, BSBool, castleBool, DTBool, MSBool, outpostBool, tavernBool, TPBool, TCBool;
	private GameObject Master, constructBuilding;
	private Text storageText, houseText, marketText, ACText, bankText, barrText, BTText, BSText, castleText, DTText, MSText, outpostText, tavernText, TPText, TCText;
	private Transform miscBuild;

	void Start(){
		Master = GameObject.Find ("GameMaster");
		constructBuilding = GameObject.Find ("ConstructBuilding");
		miscBuild = gameObject.transform.Find ("BuildMenu").Find ("MiscBuildings");
		storageText = miscBuild.Find ("StorageUpgr").Find ("Queue").GetComponent <Text> ();
		houseText = miscBuild.Find ("HouseUpgr").Find ("Queue").GetComponent <Text> ();
		marketText = miscBuild.Find ("MarketUpgr").Find ("Queue").GetComponent <Text> ();
		//BTText = miscBuild.Find ("BTUpgr").Find ("Queue").GetComponent <Text> ();
		barrText = miscBuild.Find ("BarracksUpgr").Find ("Queue").GetComponent <Text> ();
		MSText = miscBuild.Find ("MSUpgr").Find ("Queue").GetComponent <Text> ();
		castleText = miscBuild.Find ("CastleUpgr").Find ("Queue").GetComponent <Text> ();
		ACText = miscBuild.Find ("ACUpgr").Find ("Queue").GetComponent <Text> ();
		//DTText = miscBuild.Find ("DTUpgr").Find ("Queue").GetComponent <Text> ();
		tavernText = miscBuild.Find ("TavernUpgr").Find ("Queue").GetComponent <Text> ();
		outpostText = miscBuild.Find ("OutpostUpgr").Find ("Queue").GetComponent <Text> ();
		bankText = miscBuild.Find ("BankUpgr").Find ("Queue").GetComponent <Text> ();
		TPText = miscBuild.Find ("TPUpgr").Find ("Queue").GetComponent <Text> ();
		//TCText = miscBuild.Find ("TCUpgr").Find ("Queue").GetComponent <Text> ();
		BSText = miscBuild.Find ("BlacksmithUpgr").Find ("Queue").GetComponent <Text> ();
		UpdateQueueTexts ();
	}

	bool ThisLogic(int queue, GameObject upgr, float offSet, bool solidness, int Rotation){
		if(queue > 0){
			constructBuilding.GetComponent <ConstructBuilding>().SpawnBuilding(upgr, offSet, solidness, Rotation);
			constructBuilding.GetComponent <ConstructBuilding> ().CloseMenu ();
			return true;
		}else{
			Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, you do not have this upgrade queued up");
			return false;
		}
	}

	public void UpdateQueueTexts(){
		storageText.text = "Queue: " + storageQueue;
		castleText.text = "Queue: " + castleQueue;
		houseText.text = "Queue: " + houseQueue;
		marketText.text = "Queue: " + marketQueue;
		//BTText.text = "Queue: " + BTQueue;
		barrText.text = "Queue: " + barrQueue;
		MSText.text = "Queue: " + MSQueue;
		ACText.text = "Queue: " + ACQueue;
		//DTText.text = "Queue: " + DTQueue;
		tavernText.text = "Queue: " + tavernQueue;
		outpostText.text = "Queue: " + outpostQueue;
		bankText.text = "Queue: " + bankQueue;
		TPText.text = "Queue: " + TPQueue;
		//TCText.text = "Queue: " + TCQueue;
		BSText.text = "Queue: " + BSQueue;
	}

	public void PlaceBankUpgr(){
		if(ThisLogic (bankQueue, bankUpgrPH, upOffsetBank, true, 0)){
			bankBool = true;
		}
	}
	public void PlaceBarrUpgr(){
		if(ThisLogic (barrQueue, barrUpgrPH, upOffsetBarr, true, 0)){
			barrBool = true;
		}
	}
	public void PlaceBTUpgr(){
		if(ThisLogic (BTQueue, BTUpgrPH, upOffsetBT, true, 0)){
			BTBool = true;
		}
	}
	public void PlaceBSUpgr(){
		if(ThisLogic (BSQueue, BSUpgrPH, upOffsetBS, true, 0)){
			BSBool = true;
		}
	}
	public void PlaceCastleUpgr(){
		if(ThisLogic (castleQueue, castleUpgrPH, upOffsetCastle, true, 0)){
			castleBool = true;
		}
	}
	public void PlaceDTUpgr(){
		if(ThisLogic (DTQueue, DTUpgrPH, upOffsetDT, true, 0)){
			DTBool = true;
		}
	}
	public void PlaceMSUpgr(){
		if(ThisLogic (MSQueue, MSUpgrPH, upOffsetMS, true, 0)){
			MSBool = true;
		}
	}
	public void PlaceOutpostUpgr(){
		if(ThisLogic (outpostQueue, outpostUpgrPH, upOffsetOutpost, true, 0)){
			outpostBool = true;
		}
	}
	public void PlaceTavernUpgr(){
		if(ThisLogic (tavernQueue, tavernUpgrPH, upOffsetTavern, true, 0)){
			tavernBool = true;
		}
	}
	public void PlaceTPUpgr(){
		if(ThisLogic (TPQueue, TPUpgrPH, upOffsetTP, true, 0)){
			TPBool = true;
		}
	}
	public void PlaceTCUpgr(){
		if(ThisLogic (TCQueue, TCUpgrPH, upOffsetTC, true, 0)){
			TCBool = true;
		}
	}
	public void PlaceStorageUpgr(){
		if(ThisLogic (storageQueue, storageUpgrPH, upOffsetStorage, true, -90)){
			storageBool = true;
		}
	}

	public void PlaceHouseUpgr(){
		if(ThisLogic (houseQueue, houseUpgrPH, upOffsetHouse, true, 0)){
			houseBool = true;
		}
	}
	public void PlaceMarketUpgr(){
		if(ThisLogic (marketQueue, marketUpgrPH, upOffsetMarket, true, 0)){
			marketBool = true;
		}
	}
	public void PlaceACUpgr(){
		if(ThisLogic (ACQueue, ACUpgrPH, upOffsetAC, true, 0)){
			ACBool = true;
		}
	}

	void Update(){
		if(storageBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (storageUpgrP, upOffsetStorage, true, 0, 0, 0, 0, 0, 0)){
				storageBool = false;
				storageQueue--;
				UpdateQueueTexts ();
			}
		}
		else if(houseBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (houseUpgrP, upOffsetHouse, true, 0, 0, 0, 0, 0, 0)){
				houseBool = false;
				houseQueue--;
				UpdateQueueTexts ();
			}
		}
		else if(marketBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (marketUpgrP, upOffsetMarket, false, 0, 0, 0, 0, 0, 0)){
				marketBool = false;
				marketQueue--;
				UpdateQueueTexts ();
			}
		}
		else if(ACBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (ACUpgrP, upOffsetAC, false, 0, 0, 0, 0, 0, 0)){
				ACBool = false;
				ACQueue--;
				UpdateQueueTexts ();
			}
		}
		else if(bankBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (BankUpgrP, upOffsetBank, true, 0, 0, 0, 0, 0, 0)){
				bankBool = false;
				bankQueue--;
				UpdateQueueTexts ();
			}
		}
		else if(barrBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (barrUpgrP, upOffsetBarr, true, 0, 0, 0, 0, 0, 0)){
				barrBool = false;
				barrQueue--;
				UpdateQueueTexts ();
			}
		}
		else if(BTBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (BTUpgrP, upOffsetBT, true, 0, 0, 0, 0, 0, 0)){
				BTBool = false;
				BTQueue--;
				UpdateQueueTexts ();
			}
		}
		else if(BSBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (BSUpgrP, upOffsetBS, true, 0, 0, 0, 0, 0, 0)){
				BSBool = false;
				BSQueue--;
				UpdateQueueTexts ();
			}
		}
		else if(DTBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (DTUpgrP, upOffsetDT, true, 0, 0, 0, 0, 0, 0)){
				DTBool = false;
				DTQueue--;
				UpdateQueueTexts ();
			}
		}
		else if(castleBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (castleUpgrP, upOffsetCastle, false, 0, 0, 0, 0, 0, 0)){
				castleBool = false;
				castleQueue--;
				UpdateQueueTexts ();
			}
		}
		else if(outpostBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (outpostUpgrP, upOffsetOutpost, false, 0, 0, 0, 0, 0, 0)){
				outpostBool = false;
				outpostQueue--;
				UpdateQueueTexts ();
			}
		}
		else if(tavernBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (tavernUpgrP, upOffsetTavern, true, 0, 0, 0, 0, 0, 0)){
				tavernBool = false;
				tavernQueue--;
				UpdateQueueTexts ();
			}
		}
		else if(TCBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (TCUpgrP, upOffsetTC, true, 0, 0, 0, 0, 0, 0)){
				TCBool = false;
				TCQueue--;
				UpdateQueueTexts ();
			}
		}
		else if(TPBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (TPUpgrP, upOffsetTP, true, 0, 0, 0, 0, 0, 0)){
				TPBool = false;
				TPQueue--;
				UpdateQueueTexts ();
			}
		}
		else if(MSBool){
			if(constructBuilding.GetComponent <ConstructBuilding>().UpgradesLogic (MSUpgrP, upOffsetMS, true, 0, 0, 0, 0, 0, 0)){
				MSBool = false;
				MSQueue--;
				UpdateQueueTexts ();
			}
		}
	}
}
