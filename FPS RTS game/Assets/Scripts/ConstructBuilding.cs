using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructBuilding : MonoBehaviour {

	private GameObject Master, PlayerControl, BuildMenu, BuildingParent, mainCamera, TB, DB, MB, Menu, Base, currentPlaceholder;
	private bool canPlace, outpost = false, TC, barracks, AC, BT, DT,  storage = false, TP = false, mineshaft = false, blacksmith = false, house = false, market = false, PUB = false, farm = false, bank = false, tavern = false, WWall, RWall, castle, spikes;
	private RaycastHit hit;
	private Ray ray;
	private Vector3 place;
	private Renderer rend;
	private Renderer[] rendList;
	private LogicExitState state = LogicExitState.Loop;

	[HideInInspector]
	public enum LogicExitState{Loop, Placed, Cancled};
	public LayerMask resMask;
	public Material[] mat = new Material[2];
	public GameObject OutpostPlaceholder, OutpostPrefab, OutpostStruct, StoragePrefab, StoragePlaceholder, StorageStruct, HousePrefab, HousePlaceholder, HouseStruct, MarketPrefab, MarketPlaceholder, MarketStruct, PUBPrefab, PUBPlaceholder, PUBStruct, FarmPrefab, FarmPlaceholder, FarmStruct, BankPrefab, BankPlaceholder, BankStruct, TavernPrefab, TavernPlaceholder, TavernStruct, BlacksmithPrefab, BlacksmithPlaceholder, BlacksmithStruct, MSPrefab, MSPlaceholder, MSStruct, TPPrefab, TPPlaceholder, TPStruct, WWallPrefab, WallStruct, RWallPrefab, WWPlaceholder, RWPlaceholder, CastlePrefab, CastlePlaceholder, CastleStruct, BarracksPlaceholder, BarracksPrefab, BarracksStruct, ACPrefab, ACPlaceholder, ACStruct, BTPrefab, BTPlaceholder, BTStruct, TCPrefab, TCPlaceholder, TCStruct, DTPrefab, DTPlaceholder, DTStruct, Spikes, SpikesPlaceholder;
	public float distance = 1f, upOffsetOutpost, upOffsetBlacksmith, upOffsetTavern, upOffsetStorage, upOffsetHouse, upOffsetMarket, upOffsetPUB, upOffsetFarm, upOffsetBank, upOffsetMS, upOffsetTP, upOffsetWWall, upOffsetRWall, upOffsetCastle, upOffsetBarracks, upOffsetAC, upOffsetBT, upOffsetTC, upOffsetDT, upOffsetSpikes;
	public int outpostWoodCost, outpostRockCost, outpostIronCost, storageWoodCost, storageRockCost, houseWoodCost, houseRockCost, houseIronCost, houseFoodCost, marketWoodCost, marketRockCost, marketFoodCost, marketIronCost, PUBWoodCost, PUBRockCost, PUBIronCost, PUBGoldCost, FWCost, FRCost, FICost, FFCost, FHCost, BWCost, BRCost, BICost, BFCost, BGCost, tavernWoodCost, tavernRockCost, tavernIronCost, tavernFoodCost, tavernHideCost, tavernGoldCost, BSWoodCost, BSRockCost, BSIronCost, BSFoodCost, BSHideCost, BSGoldCost, MSWCost, MSICost, MSHCost, MSGCost, TPWoodCost, TPRockCost, TPIronCost, TPFoodCost, TPHideCost, TPGoldCost, WWallCost, RWallCostRock, RWallCostIron, CWCost, CRCost, CICost, CGCost, BarrWCost, BarrRCost, BarrICost, BarrHCost, BarrGCost, ACWCost, ACRCost, ACICost, ACGCost, ACFCost, BTWCost, BTICost, TCWCost, TCRCost, TCICost, TCGCost, DTWCost, DTRCost, DTICost, DTGCost, SpikesWoodCost;
	public int minXp, maxXp, maxOutposts, maxStorages, maxBarracks, maxHouses, maxBanks, maxFarms, maxTaverns, maxMS, maxTP, maxAC, maxBS, degreesOfRotation, buildingDegrees;
	public float costMultiplyer;

	void Awake(){
		rendList = new Renderer[100];
		PlayerControl = GameObject.Find ("Player");
		if(GameObject.Find ("BuildMenu") != null){
			BuildMenu = GameObject.Find ("BuildMenu");
			TB = BuildMenu.transform.Find ("TownBuildings").gameObject;
			DB = BuildMenu.transform.Find ("DefenceBuildings").gameObject;
			MB = BuildMenu.transform.Find ("MiscBuildings").gameObject;
			Menu = BuildMenu.transform.Find ("MainMenu").gameObject;
		}
		BuildingParent = GameObject.Find ("Buildings");
		mainCamera = GameObject.Find ("Main Camera");
		Master = GameObject.Find ("GameMaster");
		Base = GameObject.Find ("BaseBuilding");
		TB.SetActive (false);
		DB.SetActive (false);
		MB.SetActive (false);
		Menu.SetActive (false);
		CloseMenu ();
	}

	public LogicExitState Logic(GameObject building, float offSet, bool isSolid, int WCost, int RCost, int ICost, int FCost, int GCost, int HCost){//RETURNS TRUE IF THIS LOGIC HAS SERVED IT'S PURPOSE, AND SO THE CORRESPONGING BUILDING BOOL SHOULD BE TURNED OFF SO THE LOOP STOPS
		if(Physics.Raycast (mainCamera.transform.position, mainCamera.transform.forward, out hit, distance)){
			place = hit.point;
			place.y += offSet;
			currentPlaceholder.transform.position = place;
			if(isSolid){
				if(Vector3.Distance (place, Base.transform.position) > Base.GetComponent <BaseBuilding>().townBuildDistanceLimit){
					canPlace = false;
					rend.sharedMaterial = mat [1];
				}else{
					canPlace = true;
					rend.sharedMaterial = mat [0];
				}
			}else{
				if(Vector3.Distance (place, Base.transform.position) > Base.GetComponent <BaseBuilding>().townBuildDistanceLimit){
					canPlace = false;
					foreach(Renderer curr in rendList){
						if(curr.gameObject.name != "RadiusRing"){
							curr.sharedMaterial = mat [1];
						}
					}
				}else{
					canPlace = true;
					foreach(Renderer curr in rendList){
						if(curr.gameObject.name != "RadiusRing"){
							curr.sharedMaterial = mat [0];
						}
					}
				}
			}
		}
		if(Input.GetKeyDown (KeyCode.Mouse1)){
			if(canPlace){
				if(Master.GetComponent <GameMaster>().gWood >= WCost && Master.GetComponent <GameMaster>().gRock >= RCost && Master.GetComponent <GameMaster>().gIron >= ICost && Master.GetComponent <GameMaster>().gHide >= HCost && Master.GetComponent <GameMaster>().gGold >= GCost && Master.GetComponent <GameMaster>().gHide >= HCost){//CHANGE
					Destroy (BuildingParent.transform.Find ("Placeholder").gameObject);
					GameObject curr = Instantiate (building, place, Quaternion.Euler (currentPlaceholder.transform.localEulerAngles.x, buildingDegrees*degreesOfRotation,currentPlaceholder.transform.localEulerAngles.z), BuildingParent.transform.transform);
					Master.GetComponent <GameMaster> ().Subtract ("Wood", WCost);
					Master.GetComponent <GameMaster> ().Subtract ("Rock", RCost);
					Master.GetComponent <GameMaster> ().Subtract ("Iron", ICost);
					Master.GetComponent <GameMaster> ().Subtract ("Food", FCost);
					Master.GetComponent <GameMaster> ().Subtract ("Gold", GCost);
					Master.GetComponent <GameMaster> ().Subtract ("Hide", HCost);
					Collider[] hitRes = Physics.OverlapBox (curr.transform.position, curr.GetComponent <BoxCollider> ().bounds.extents, Quaternion.identity, resMask);
					foreach(Collider c in hitRes){
						Destroy (c.gameObject);
					}
					mainCamera.GetComponent <PlayerStats> ().AddXp (Random.Range (minXp, maxXp));
					canPlace = false;
					return LogicExitState.Placed;
				}else{
					Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, you do not have enough resources in your storage to spawn this building!");
					return LogicExitState.Loop;
				}
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you must build closer to your base or upgrade your castle.");
				return LogicExitState.Loop;
			}
		}else if(Input.GetKeyDown (KeyCode.Mouse2)){
			Destroy (BuildingParent.transform.Find ("Placeholder").gameObject);
			return LogicExitState.Cancled;
		}
		return LogicExitState.Loop;
	}

	public void SpawnBuilding(GameObject placeholder, float offSet, bool isSolid, int rotateX){
		buildingDegrees = 0;
		CloseMenu ();
		if(Physics.Raycast (mainCamera.transform.position, mainCamera.transform.forward, out hit, distance)){
			place = hit.point;
			place.y += offSet;
			currentPlaceholder = Instantiate (placeholder, place, Quaternion.identity, BuildingParent.transform.transform);
			currentPlaceholder.transform.Rotate (rotateX, 0, 0);
			currentPlaceholder.name = "Placeholder";
			if(isSolid){
				rend = currentPlaceholder.GetComponent <Renderer> ();
			}else{
				rendList = currentPlaceholder.GetComponentsInChildren <Renderer> ();
			}

			if(isSolid){
				if(Vector3.Distance (place, Base.transform.position) > Base.GetComponent <BaseBuilding>().townBuildDistanceLimit){
					canPlace = false;
					rend.sharedMaterial = mat [1];
				}else{
					canPlace = true;
					rend.sharedMaterial = mat [0];
				}
			}else{
				if(Vector3.Distance (place, Base.transform.position) > Base.GetComponent <BaseBuilding>().townBuildDistanceLimit){
					canPlace = false;
					foreach(Renderer curr in rendList){
						if(curr.gameObject.name != "RadiusRing"){
							curr.sharedMaterial = mat [1];
						}
					}
				}else{
					canPlace = true;
					foreach(Renderer curr in rendList){
						if(curr.gameObject.name != "RadiusRing"){
							curr.sharedMaterial = mat [0];
						}
					}
				}
			}
		}else{
			canPlace = false;
			currentPlaceholder = Instantiate (placeholder, mainCamera.transform.position + Vector3.forward*offSet, Quaternion.identity, BuildingParent.transform.transform);
			if(isSolid){
				rend = currentPlaceholder.GetComponent <Renderer> ();
			}else{
				rendList = currentPlaceholder.GetComponentsInChildren <Renderer> ();
			}
			currentPlaceholder.transform.Rotate (rotateX, 0, 0);
			currentPlaceholder.name = "Placeholder";
			if(isSolid){
				canPlace = false;
				rend.sharedMaterial = mat [1];
			}else{
				foreach(Renderer curr in rendList){
					if(curr.gameObject.name != "RadiusRing"){
						curr.sharedMaterial = mat [1];
					}
				}
			}
		}
	}

	public void OpenMenu(){
		BuildMenu.SetActive (true);
		TB.SetActive (false);
		DB.SetActive (false);
		MB.SetActive (false);
		Menu.SetActive (true);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		PlayerControl.GetComponent <PlayerControler>().mouseSpeed = 0;
	}
	public void CloseMenu(){
		BuildMenu.SetActive (false);
		Menu.SetActive (false);
		TB.SetActive (false);
		DB.SetActive (false);
		MB.SetActive (false);
		mainCamera.GetComponent <Interact>().showBuild = false;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		PlayerControl.GetComponent <PlayerControler> ().mouseSpeed = 3;
	}

	public void SpawnOutpost(){
			if(GameObject.FindGameObjectsWithTag ("Outpost").Length < maxOutposts){//CHANGE
			SpawnBuilding (OutpostPlaceholder, upOffsetOutpost, false, 0);
			outpost = true;
			}else{
				CloseMenu ();
				Debug.Log ("ConstructBuilding: attempted to spawn in an outpost building but there seems to be one in the map already!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, You have reached the maximum outpost limit in your town.");
			}
	}
	public void SpawnSpikes(){
		SpawnBuilding (SpikesPlaceholder, upOffsetSpikes, false, 0);
		spikes = true;
	}

	public void SpawnAC(){
		if(GameObject.FindGameObjectsWithTag ("Castle").Length > 0){
			if(GameObject.FindGameObjectsWithTag ("AnimalCoop").Length < maxAC){//CHANGE
				SpawnBuilding (ACPlaceholder, upOffsetAC, false, 0);
				AC = true;
			}else{
				CloseMenu ();
				Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, You have reached the maximum outpost limit in your town.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you must have a castle to build this building.");
		}

	}
	public void SpawnBT(){
		SpawnBuilding (BTPlaceholder, upOffsetBT, true, 0);
		BT = true;
	}
	public void SpawnDT(){
		SpawnBuilding (DTPlaceholder, upOffsetDT, true, 0);
		DT = true;
	}

	public void SpawnBarracks(){
			if(GameObject.FindGameObjectsWithTag ("Barracks").Length < maxBarracks){
			SpawnBuilding (BarracksPlaceholder, upOffsetBarracks, true, 0);
			barracks = true;
			}else{
				CloseMenu ();
				Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, You have reached the maximum barracks limit in your town.");
			}
	}

	public void SpawnStorage(){
		if(GameObject.FindGameObjectsWithTag ("Storage").Length < maxStorages){
			SpawnBuilding (StoragePlaceholder, upOffsetStorage, true, 0);
			storage = true;
		}else{
			CloseMenu ();
			Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, you have reached the maximum storage building limit in your town.");
		}
	}
	public void SpawnTavern(){
		if (GameObject.FindGameObjectsWithTag ("Castle").Length == 1) {
			if (GameObject.FindGameObjectsWithTag ("Tavern").Length < maxTaverns) {
				SpawnBuilding (TavernPlaceholder, upOffsetTavern, true, 0);
				tavern = true;
			} else {
				CloseMenu ();
				Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you have reached the maximum tavern limit in your town.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you must have a castle to build this building.");
		}
	}
	public void SpawnTreePlantation(){
		if (GameObject.FindGameObjectsWithTag ("Castle").Length == 1) {
			if (GameObject.FindGameObjectsWithTag ("TreePlantation").Length < maxTP) {
				SpawnBuilding (TPPlaceholder, upOffsetTP, true, 0);
				TP = true;
			} else {
				CloseMenu ();
				Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, You have reached the maximum tree plantation limit in your town.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you must have a castle to build this building.");
		}
	}
	public void SpawnHouse(){
		if(GameObject.FindGameObjectsWithTag ("House").Length < maxHouses){
			SpawnBuilding (HousePlaceholder, upOffsetHouse, true, 0);
			house = true;
		}else{
			CloseMenu ();
			Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, you have reached the maximum house limit in your town.");
		}
	}
	public void SpawnWoodWall(){
		SpawnBuilding (WWPlaceholder, upOffsetWWall, true, 0);
			WWall = true;
	}
	public void SpawnRockWall(){
		SpawnBuilding (RWPlaceholder, upOffsetRWall, true, 0);
		RWall = true;
	}
	public void SpawnFarm(){
		if(GameObject.FindGameObjectsWithTag ("Farm").Length < maxFarms){
			SpawnBuilding (FarmPlaceholder, upOffsetFarm, false, 0);
			farm = true;
		}else{
			CloseMenu ();
			Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, you have reached the maximum farm limit in your town.");
		}
	}
	public void SpawnBank(){
		if(GameObject.FindGameObjectsWithTag ("Bank").Length < maxBanks){
			SpawnBuilding (BankPlaceholder, upOffsetBank, false, 0);
			bank = true;
		}else{
			CloseMenu ();
			Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, you have reached the maximum bank limit in your town.");
		}
	}
	public void SpawnMarket(){
		if(GameObject.FindGameObjectsWithTag ("Market").Length == 0){
			SpawnBuilding (MarketPlaceholder, upOffsetMarket, false, 0);
			market = true;
		}else{
			CloseMenu ();
			Debug.Log ("ConstructBuilding: attempted to spawn in a market building but there seems to be one in the map already!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("You can only have one market in your town!");
		}
	}
	public void SpawnTC(){
		if(GameObject.FindGameObjectsWithTag ("Castle").Length > 0){
			if(GameObject.FindGameObjectsWithTag ("TrainingCamp").Length == 0){
				SpawnBuilding (TCPlaceholder, upOffsetTC, true, 0);
				TC = true;
			}else{
				CloseMenu ();
				Debug.Log ("ConstructBuilding: attempted to spawn in a TC building but there seems to be one in the map already!");
				Master.GetComponent <GameMaster>().PassErrorMessage ("You can only have one training camp in your town!");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you must have a castle to build this building.");
		}

	}
	public void SpawnCastle(){
		if(GameObject.FindGameObjectsWithTag ("Castle").Length == 0){
			SpawnBuilding (CastlePlaceholder, upOffsetCastle, false, 0);
			castle = true;
		}else{
			CloseMenu ();
			Debug.Log ("ConstructBuilding: attempted to spawn in a castle building but there seems to be one in the map already!");
			Master.GetComponent <GameMaster>().PassErrorMessage ("You can only have one castle in your town!");
		}
	}
	public void SpawnMineShaft(){
		if (GameObject.FindGameObjectsWithTag ("Castle").Length == 1) {
			if (GameObject.FindGameObjectsWithTag ("MineShaft").Length < maxMS) {
				SpawnBuilding (MSPlaceholder, upOffsetMS, true, 0);
				mineshaft = true;
			} else {
				CloseMenu ();
				Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you have reached the maximum mine shaft limit in your town.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you must have a castle to build this building.");
		}
	}
	public void SpawnBlacksmith(){
		if (GameObject.FindGameObjectsWithTag ("Castle").Length == 1) {
			if (GameObject.FindGameObjectsWithTag ("Blacksmith").Length < maxBS) {
				SpawnBuilding (BlacksmithPlaceholder, upOffsetBlacksmith, true, 0);
				blacksmith = true;
			} else {
				CloseMenu ();
				Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you can only have one blacksmith building in your town.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you must have a castle to build this building.");
		}
	}
	public void SpawnPUB(){
		if (GameObject.FindGameObjectsWithTag ("Castle").Length == 1) {
			if (GameObject.FindGameObjectsWithTag ("PlayerUpgradesBuilding").Length == 0) {
				SpawnBuilding (PUBPlaceholder, upOffsetPUB, true, 0);
				PUB = true;
			} else {
				CloseMenu ();
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can only have one player upgrades building in your town!");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you must have a castle to build this building.");
		}
	}

	void Update(){
		if(castle || outpost || barracks || house || TP || AC || mineshaft || bank || tavern || PUB || storage || market || DT || BT || WWall || RWall || blacksmith || TC || farm || spikes){
			if(currentPlaceholder != null){
				currentPlaceholder.transform.localEulerAngles = new Vector3 (currentPlaceholder.transform.localEulerAngles.x, buildingDegrees*degreesOfRotation,currentPlaceholder.transform.localEulerAngles.z);
			}
		}
		if(castle){//if LOGIC returns true, that means it has to stop the loop by turning off the building bool
			state = Logic (CastlePrefab, upOffsetCastle, false, CWCost, CRCost, CICost, 0, CGCost, 0);
			if(state == LogicExitState.Placed){
				castle = false;
			}else if(state == LogicExitState.Placed){castle = false;}
		}
		else if(outpost){
			state =Logic (OutpostPrefab, upOffsetOutpost, false, outpostWoodCost, outpostRockCost, outpostIronCost, 0, 0, 0);
			if(state == LogicExitState.Placed){
				outpost = false;
				outpostWoodCost += (int)(outpostWoodCost * costMultiplyer);
				outpostRockCost += (int)(outpostRockCost * costMultiplyer);
				outpostIronCost += (int)(outpostIronCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){outpost = false;}
		}
		else if(spikes){
			state =Logic (Spikes, upOffsetSpikes, false, SpikesWoodCost, 0, 0, 0, 0, 0);
			if(state == LogicExitState.Placed){
				spikes = false;
				SpikesWoodCost += (int)(SpikesWoodCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){spikes = false;}
		}
		else if(barracks){
			state =Logic (BarracksPrefab, upOffsetBarracks, true, BarrWCost, BarrRCost, BarrICost, 0, BarrGCost, BarrHCost);
			if(state == LogicExitState.Placed){
				barracks = false;
				BarrWCost += (int)(BarrWCost * costMultiplyer);
				BarrRCost += (int)(BarrRCost * costMultiplyer);
				BarrICost += (int)(BarrICost * costMultiplyer);
				BarrGCost += (int)(BarrGCost * costMultiplyer);
				BarrHCost += (int)(BarrHCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){barracks = false;}
		}
		else if(TC){
			state =Logic (TCPrefab, upOffsetTC, true, TCWCost, TCRCost, TCICost, 0, TCGCost, 0);
			if(state == LogicExitState.Placed){
				TC = false;
				TCWCost += (int)(TCWCost * costMultiplyer);
				TCRCost += (int)(TCRCost * costMultiplyer);
				TCICost += (int)(TCICost * costMultiplyer);
				TCGCost += (int)(TCGCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){TC = false;}
		}
		else if(AC){
			state =Logic (ACPrefab, upOffsetAC, false, ACWCost, ACRCost, ACICost, ACFCost, ACGCost, 0);
			if(state == LogicExitState.Placed){ //idk if AC is solid
				AC = false;
				ACWCost += (int)(ACWCost * costMultiplyer);
				ACRCost += (int)(ACRCost * costMultiplyer);
				ACICost += (int)(ACICost * costMultiplyer);
				ACFCost += (int)(ACFCost * costMultiplyer);
				ACGCost += (int)(ACGCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){AC = false;}
		}
		else if(BT){
			state =Logic (BTPrefab, upOffsetBT, true, BTWCost, 0, BTICost, 0, 0, 0);
			if(state == LogicExitState.Placed){
				BT = false;
				BTWCost += (int)(BTWCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){BT = false;}
		}
		else if(DT){
			state =Logic (DTPrefab, upOffsetDT, true, DTWCost, DTRCost, 0, 0, DTGCost, 0);
			if(state == LogicExitState.Placed){
				DT = false;
				DTWCost += (int)(DTWCost * costMultiplyer);
				DTRCost += (int)(DTRCost * costMultiplyer);
				DTICost += (int)(DTICost * costMultiplyer);
				DTGCost += (int)(DTGCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){DT = false;}
		}
		else if(storage){
			state =Logic (StoragePrefab, upOffsetStorage, true, storageWoodCost, storageRockCost, 0, 0, 0, 0);
			if(state == LogicExitState.Placed){
				storage = false;
				storageWoodCost += (int)(storageWoodCost * costMultiplyer);
				storageRockCost += (int)(storageRockCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){storage = false;}
		}
		else if(tavern){
			state =Logic (TavernPrefab, upOffsetTavern, true, tavernWoodCost, tavernRockCost, tavernIronCost, tavernFoodCost, tavernGoldCost, tavernHideCost);
			if(state == LogicExitState.Placed){
				tavern = false;
				tavernWoodCost += (int)(tavernWoodCost * costMultiplyer);
				tavernRockCost += (int)(tavernRockCost * costMultiplyer);
				tavernIronCost += (int)(tavernIronCost * costMultiplyer);
				tavernFoodCost += (int)(tavernFoodCost * costMultiplyer);
				tavernGoldCost += (int)(tavernGoldCost * costMultiplyer);
				tavernHideCost += (int)(tavernHideCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){tavern = false;}
		}
		else if(TP){
			state =Logic (TPPrefab, upOffsetTP, true, TPWoodCost, TPRockCost, TPIronCost, TPFoodCost, TPGoldCost, TPHideCost);
			if(state == LogicExitState.Placed){
				TP = false;
				TPWoodCost += (int)(TPWoodCost * costMultiplyer);
				TPRockCost += (int)(TPRockCost * costMultiplyer);
				TPIronCost += (int)(TPIronCost * costMultiplyer);
				TPFoodCost += (int)(TPFoodCost * costMultiplyer);
				TPGoldCost += (int)(TPGoldCost * costMultiplyer);
				TPHideCost += (int)(TPHideCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){TP = false;}
		}
		else if(mineshaft){
			state =Logic (MSPrefab, upOffsetMS, true, MSWCost, 0, MSICost, 0, MSGCost, MSHCost);
			if(state == LogicExitState.Placed){
				mineshaft = false;
				MSWCost += (int)(MSWCost * costMultiplyer);
				MSICost += (int)(MSICost * costMultiplyer);
				MSGCost += (int)(MSGCost * costMultiplyer);
				MSHCost += (int)(MSHCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){mineshaft = false;}
		}
		else if(WWall){
			state =Logic (WWallPrefab, upOffsetWWall, true, WWallCost, 0, 0, 0, 0, 0);
			if(state == LogicExitState.Placed){
				WWall = false;
				WWallCost += (int)(WWallCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){WWall = false;}
		}
		else if(RWall){
			state =Logic (RWallPrefab, upOffsetRWall, false, 0, RWallCostRock, RWallCostIron, 0, 0, 0);
			if(state == LogicExitState.Placed){
				RWall = false;
				RWallCostRock += (int)(RWallCostRock * costMultiplyer);
				RWallCostIron += (int)(RWallCostIron * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){RWall = false;}
		}
		else if(house){
			state =Logic (HousePrefab, upOffsetHouse, true, houseWoodCost, houseRockCost, houseIronCost, houseFoodCost, 0, 0);
			if(state == LogicExitState.Placed){
				house = false;
				houseWoodCost += (int)(houseWoodCost * costMultiplyer);
				houseRockCost += (int)(houseRockCost * costMultiplyer);
				houseIronCost += (int)(houseIronCost * costMultiplyer);
				houseFoodCost += (int)(houseFoodCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){house = false;}
		}
		else if(market){
			state =Logic (MarketPrefab, upOffsetMarket, false, marketWoodCost, marketRockCost, marketIronCost, marketFoodCost, 0, 0);
			if(state == LogicExitState.Placed){
				market = false;
			}else if(state == LogicExitState.Placed){market = false;}
		}
		else if(blacksmith){
			state =Logic (BlacksmithPrefab, upOffsetBlacksmith, true, BSWoodCost, BSRockCost, BSIronCost, BSFoodCost, BSGoldCost, BSHideCost);
			if(state == LogicExitState.Placed){
				blacksmith = false;
				BSWoodCost += (int)(BSWoodCost * costMultiplyer);
				BSRockCost += (int)(BSRockCost * costMultiplyer);
				BSIronCost += (int)(BSIronCost * costMultiplyer);
				BSFoodCost += (int)(BSFoodCost * costMultiplyer);
				BSGoldCost += (int)(BSGoldCost * costMultiplyer);
				BSHideCost += (int)(BSHideCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){blacksmith = false;}
		}
		else if(PUB){
			state =Logic (PUBPrefab, upOffsetPUB, true, PUBWoodCost, PUBRockCost, PUBIronCost, 0, PUBGoldCost, 0);
			if(state == LogicExitState.Placed){
				PUB = false;
			}else if(state == LogicExitState.Placed){PUB = false;}
		}
		else if(farm){
			state =Logic (FarmPrefab, upOffsetFarm, false, FWCost, FRCost, FICost, FFCost, 0, FHCost);
			if(state == LogicExitState.Placed){
				farm = false;
				FWCost += (int)(FWCost * costMultiplyer);
				FRCost += (int)(FRCost * costMultiplyer);
				FICost += (int)(FICost * costMultiplyer);
				FFCost += (int)(FFCost * costMultiplyer);
				FHCost += (int)(FHCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){farm = false;}
		}
		else if(bank){
			state = Logic (BankPrefab, upOffsetBank, false, BWCost, BRCost, BICost, BFCost, BGCost, 0);
			if(state == LogicExitState.Placed){
				bank = false;
				BWCost += (int)(BWCost * costMultiplyer);
				BRCost += (int)(BRCost * costMultiplyer);
				BICost += (int)(BICost * costMultiplyer);
				BFCost += (int)(BFCost * costMultiplyer);
				BGCost += (int)(BGCost * costMultiplyer);
				BuildMenu.GetComponent <BuildMenu>().UpdateAllCosts ();
			}else if(state == LogicExitState.Placed){bank = false;}
		}
	}

	public void Back(){
		Menu.SetActive (true);
		TB.SetActive (false);
		DB.SetActive (false);
		MB.SetActive (false);
	}

	public void DefenceBuildings(){
		if (GameObject.FindGameObjectsWithTag ("Castle").Length == 1) {
			DB.SetActive (true);
			Menu.SetActive (false);
		}else{
			Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, you must first build a castle to access defencive buildings.");
		}
	}

	public void TownBuildings(){
		TB.SetActive (true);
		Menu.SetActive (false);
	}

	public void MiscBuildings(){
		MB.SetActive (true);
		Menu.SetActive (false);
	}

	public void RotateBuilding(){
		if(buildingDegrees >= 7){
			buildingDegrees = 0;
		}else{
			buildingDegrees++;
		}
	}

	public bool UpgradesLogic(GameObject building, float offSet, bool isSolid, int WCost, int RCost, int ICost, int FCost, int GCost, int HCost){//RETURNS TRUE IF THIS LOGIC HAS SERVED IT'S PURPOSE, AND SO THE CORRESPONGING BUILDING BOOL SHOULD BE TURNED OFF SO THE LOOP STOPS
		if(Physics.Raycast (mainCamera.transform.position, mainCamera.transform.forward, out hit, distance)){
			place = hit.point;
			place.y += offSet;
			currentPlaceholder.transform.position = place;
			if(isSolid){
				if(Vector3.Distance (place, Base.transform.position) > Base.GetComponent <BaseBuilding>().townBuildDistanceLimit){
					canPlace = false;
					rend.sharedMaterial = mat [1];
				}else{
					canPlace = true;
					rend.sharedMaterial = mat [0];
				}
			}else{
				if(Vector3.Distance (place, Base.transform.position) > Base.GetComponent <BaseBuilding>().townBuildDistanceLimit){
					canPlace = false;
					foreach(Renderer curr in rendList){
						if(curr.gameObject.name != "RadiusRing"){
							curr.sharedMaterial = mat [1];
						}
					}
				}else{
					canPlace = true;
					foreach(Renderer curr in rendList){
						if(curr.gameObject.name != "RadiusRing"){
							curr.sharedMaterial = mat [0];
						}
					}
				}
			}
		}
		if(Input.GetKeyDown (KeyCode.Mouse1)){
			if(canPlace){
				if(Master.GetComponent <GameMaster>().gWood >= WCost && Master.GetComponent <GameMaster>().gRock >= RCost && Master.GetComponent <GameMaster>().gIron >= ICost && Master.GetComponent <GameMaster>().gHide >= HCost && Master.GetComponent <GameMaster>().gGold >= GCost && Master.GetComponent <GameMaster>().gHide >= HCost){//CHANGE
					Destroy (BuildingParent.transform.Find ("Placeholder").gameObject);
					GameObject curr = Instantiate (building, place, Quaternion.Euler (currentPlaceholder.transform.localEulerAngles.x, buildingDegrees*degreesOfRotation,currentPlaceholder.transform.localEulerAngles.z), BuildingParent.transform.transform);
					Master.GetComponent <GameMaster> ().Subtract ("Wood", WCost);
					Master.GetComponent <GameMaster> ().Subtract ("Rock", RCost);
					Master.GetComponent <GameMaster> ().Subtract ("Iron", ICost);
					Master.GetComponent <GameMaster> ().Subtract ("Food", FCost);
					Master.GetComponent <GameMaster> ().Subtract ("Gold", GCost);
					Master.GetComponent <GameMaster> ().Subtract ("Hide", HCost);
					Collider[] hitRes = Physics.OverlapBox (curr.transform.position, curr.GetComponent <BoxCollider> ().bounds.extents, Quaternion.identity, resMask);
					foreach(Collider c in hitRes){
						Destroy (c.gameObject);
					}
					mainCamera.GetComponent <PlayerStats> ().AddXp (Random.Range (minXp, maxXp));
					canPlace = false;
					return true;
				}else{
					Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, you do not have enough resources in your storage to spawn this building!");
					return false;
				}
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you must build closer to your base or upgrade your castle.");
				return false;
			}
		}else if(Input.GetKeyDown (KeyCode.Mouse2)){
			Destroy (BuildingParent.transform.Find ("Placeholder").gameObject);
			return false;
		}
		return false;
	}

}
