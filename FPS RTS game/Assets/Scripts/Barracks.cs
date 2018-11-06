using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barracks : MonoBehaviour {

	//SS = short sword; ST = shield troop

	public int SSFoodCost, SSIronCost, SSHideCost, SSGoldCost, STWoodCost, STIronCost, STFoodCost, STGoldCost, STHideCost, maxSSTroops, maxShieldTroops;
	public int SSQueue, STQueue, SpawnTime, numberInRow;
	public Transform SpawnLoc;
	public GameObject ShortSwordTroop, ShieldTroop, flagPL, flag;
	public float radius, dstBtwTroopSpawn, flagUpOffset;
	public int upgrWood1, upgrRock1, upgrIron1, upgrGold1, upgrPoints1, upgrWood2, upgrRock2, upgrIron2, upgrGold2, upgrPoints2, upgrWood3, upgrRock3, upgrIron3, upgrGold3, upgrPoints3;

	private Text queueInfo, cost, SSCost, STCost;
	private Transform orgSpawnLoc;
	private int x, z, buildingLevel;
	private bool inQueue, hasSpawn, placeFlag;
	private GameObject Master, loadingBar, myCanvas, buildingInfo, cam, miscBuild, flagPlace, Player;
	private float y, maxPercent;
	private RaycastHit hit;
	private Vector3 place;

	// Use this for initialization
	void Start () {
		Master = GameObject.Find ("GameMaster");
		miscBuild = GameObject.Find ("GUIHUD");
		myCanvas = gameObject.transform.Find ("Canvas").gameObject;
		loadingBar = myCanvas.transform.Find ("LoadingBar").gameObject;
		SpawnLoc = gameObject.transform.Find ("SpawnLoc").GetComponent<Transform> ();
		queueInfo = myCanvas.transform.Find ("QueueInfo").GetComponent <Text> ();
		buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
		cost = myCanvas.transform.Find ("Cost").GetComponent <Text> ();
		cam = GameObject.Find ("Main Camera");
		Player = cam.transform.parent.gameObject;
		buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops from this barracks: " + (gameObject.transform.Find ("Troops").GetComponentsInChildren <Transform> ().Length - 1) + "\r\n Building level: " + buildingLevel);
		queueInfo.text = "Currently in queue: " + "\r\n short sword troops: " + SSQueue + "\r\n shield troops: " + STQueue;
		orgSpawnLoc = SpawnLoc;
		loadingBar.transform.localScale = new Vector3 (0, 1, 1);
		myCanvas.SetActive (false);
		maxPercent = SpawnTime;
		cost.text = "Cost: \r\n\t Wood: " + upgrWood1 + "\r\n\t Rock: " + upgrRock1 + "\r\n\t Iron: " + upgrIron1 + "\r\n\t Gold: " + upgrGold1 + "\r\n\t Xp points: " + upgrPoints1 + "\r\n Building level: "+ buildingLevel +"/3";
		SSCost = myCanvas.transform.Find ("SSCost").GetComponent <Text> ();
		STCost = myCanvas.transform.Find ("STCost").GetComponent <Text> ();
		SSCost.text = "Cost: \n\t Food: " + SSFoodCost + " \n\t Iron: " + SSIronCost + "\n\t Hide: " + SSHideCost + "\n\t Gold: " + SSGoldCost;
		STCost.text = "Cost: \n\t Wood: " + STWoodCost + " \n\t Iron: " + STIronCost + "\n\t Food: " + STFoodCost + " \n\t Hide: " + SSHideCost + "\n\t Gold: " + STGoldCost;
	}

	// Update is called once per frame
	void Update () {
		if(placeFlag){
			if(Input.GetKeyDown (KeyCode.Mouse1)){
				if(gameObject.transform.Find ("Rally point") != null){
					Destroy (gameObject.transform.Find ("Rally point").gameObject);
				}
				if(Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, 10f)){
					GameObject curr = Instantiate (flag, hit.point, Quaternion.identity, gameObject.transform);
					curr.name = "Rally point";
					curr.transform.Rotate (0, 0, -180);
					curr.transform.Translate (0, flagUpOffset, 0);
				}else{
					GameObject curr = Instantiate (flag, Player.transform.position, Quaternion.identity, gameObject.transform);
					curr.name = "Rally point";
					curr.transform.Rotate (0, 0, -180);
					curr.transform.Translate (0, flagUpOffset, 0);
				}
				placeFlag = false;
				if(gameObject.transform.Find ("Rally point placeholder") != null){
					Destroy (gameObject.transform.Find ("Rally point placeholder").gameObject);
				}
				if(hasSpawn == false){
					hasSpawn = true;
				}
			}else if(Input.GetKeyDown (KeyCode.Mouse2)){
				if(gameObject.transform.Find ("Rally point placeholder") != null){
					Destroy (gameObject.transform.Find ("Rally point placeholder").gameObject);
				}
				placeFlag = false;
			}else{
				if(flagPlace != null){
					if(Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, 10f)){
						flagPlace.transform.position = hit.point;
						flagPlace.transform.Translate (0, flagUpOffset, 0);
					}else{
						flagPlace.transform.position = Player.transform.position;
						flagPlace.transform.Translate (0, flagUpOffset, 0);
					}
				}else{
					placeFlag = false;
				}
			}
		}
		if(inQueue){
			x++;
			y = x;
			if(x >= SpawnTime){
				if(SSQueue <= 5 && SSQueue > 0){
					for(int i = 0; i < SSQueue; i++){
						Spawn (ShortSwordTroop);
						//Debug.Log ("In for loop " + i + "/" + SSQueue);
					}
					SSQueue = 0;
					SpawnLoc = orgSpawnLoc;
					queueInfo.text = "Currently in queue: " + "\r\n short sword troops: " + SSQueue + "\r\n shield troops: " + STQueue;
					//update building info
				}else if(SSQueue > 5 && SSQueue > 0){
					for(int i = 0; i < 5; i++){
						Spawn (ShortSwordTroop);
					}
					SSQueue -= 5;
					SpawnLoc = orgSpawnLoc;
					queueInfo.text = "Currently in queue: " + "\r\n short sword troops: " + SSQueue + "\r\n shield troops: " + STQueue;
					//update building info
				}
				if(STQueue <= 5 && STQueue > 0){
					for(int i = 0; i < STQueue; i++){
						Spawn (ShieldTroop);
					}
					STQueue = 0;
					SpawnLoc = orgSpawnLoc;
					queueInfo.text = "Currently in queue: " + "\r\n short sword troops: " + SSQueue + "\r\n shield troops: " + STQueue;					//update building info
				}else if(STQueue > 5 && STQueue > 0){
					for(int i = 0; i < 5; i++){
						Spawn (ShieldTroop);
					}
					STQueue -= 5;
					SpawnLoc = orgSpawnLoc;
					queueInfo.text = "Currently in queue: " + "\r\n short sword troops: " + SSQueue + "\r\n shield troops: " + STQueue;					//update building info
				}
				if(SSQueue == 0 && STQueue == 0){
					loadingBar.transform.localScale = new Vector3 (0, 1, 1);
					x = 0;
					y = 0;
					z = 0;
					inQueue = false;
				}
				x = 0;
				y = 0;
				z = 0;
			}
			loadingBar.transform.localScale = new Vector3 ((y / maxPercent), 1, 1);
		}
	}

	public void SpawnShortSword(){
		if(Master.GetComponent <GameMaster> ().ssTroops < maxSSTroops){
			if(Master.GetComponent <GameMaster>().population > 0 && Master.GetComponent <GameMaster>().gFood >= SSFoodCost && Master.GetComponent <GameMaster>().gIron >= SSIronCost && Master.GetComponent <GameMaster>().gHide >= SSHideCost && Master.GetComponent <GameMaster>().gGold >= SSGoldCost){
				Master.GetComponent <GameMaster>().Subtract ("Food", SSFoodCost);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", SSIronCost);
				Master.GetComponent <GameMaster> ().Subtract ("Hide", SSHideCost);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", SSGoldCost);
				//Master.GetComponent <GameMaster> ().Subtract ("Population", 1);
				Master.GetComponent <GameMaster> ().Add ("SSTroops", 1);
				SSQueue++;
				inQueue = true;
				queueInfo.text = "Currently in queue: " + "\r\n short sword troops: " + SSQueue + "\r\n shield troops: " + STQueue;			//update building info
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough resources for this.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You have reached the maximum capacity of troops in this barracks. Try upgrading it or building another one.");
		}
	}

	public void SpawnShieldTroop(){
		if(Master.GetComponent <GameMaster> ().sTroops < maxShieldTroops){
			if(Master.GetComponent <GameMaster>().population > 0 && Master.GetComponent <GameMaster>().workers < Master.GetComponent <GameMaster>().population && Master.GetComponent <GameMaster>().gFood >= STFoodCost && Master.GetComponent <GameMaster>().gIron >= STIronCost && Master.GetComponent <GameMaster>().gHide >= STHideCost && Master.GetComponent <GameMaster>().gGold >= STGoldCost && Master.GetComponent <GameMaster>().gWood >= STWoodCost){
				Master.GetComponent <GameMaster>().Subtract ("Food", STFoodCost);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", STIronCost);
				Master.GetComponent <GameMaster> ().Subtract ("Hide", STHideCost);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", STGoldCost);
				Master.GetComponent <GameMaster> ().Subtract ("Wood", STGoldCost);
				//Master.GetComponent <GameMaster> ().Subtract ("Population", 1);
				Master.GetComponent <GameMaster> ().Add ("ShieldTroops", 1);
				STQueue++;
				inQueue = true;
				queueInfo.text = "Currently in queue: " + "\r\n short sword troops: " + SSQueue + "\r\n shield troops: " + STQueue;			//update building info
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough resources for this.");
			}
		}
	}

	void Spawn(GameObject curr){
		if(Physics.OverlapSphere (SpawnLoc.position, radius).Length == 0){
			GameObject currTroop = Instantiate (curr, SpawnLoc.position, Quaternion.identity, gameObject.transform.Find ("Troops"));
			buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops from this barracks: " + (gameObject.transform.Find ("Troops").GetComponentsInChildren <Transform> ().Length - 1) + "\r\n Building level: " + buildingLevel);
			if(hasSpawn){
				currTroop.GetComponent <Troop>().Move (gameObject.transform.Find ("Rally point").position);
				Debug.Log ("telling troop to go to flag");
			}
		}else{
			SpawnLoc.position = new Vector3 (SpawnLoc.position.x + dstBtwTroopSpawn, SpawnLoc.position.y, SpawnLoc.position.z);
			Spawn (curr);
			z++;
			if(z >= numberInRow + 2){
				SpawnLoc.position = new Vector3 (SpawnLoc.position.x - dstBtwTroopSpawn * (numberInRow + 3), SpawnLoc.position.y, SpawnLoc.position.z + dstBtwTroopSpawn * 2);
				z = 0;
			}
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

	public void UpgradeBuilding(){
		if(buildingLevel == 0){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood1 && Master.GetComponent <GameMaster>().gRock >= upgrRock1 && Master.GetComponent <GameMaster>().gIron >= upgrIron1 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints1 && Master.GetComponent <GameMaster>().gGold >= upgrGold1){
				SpawnTime -= 100;
				maxShieldTroops += 30;
				maxSSTroops += 40;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().barrQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood1);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold1);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints1;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops from this barracks: " + (gameObject.transform.Find ("Troops").GetComponentsInChildren <Transform> ().Length - 1) + "\r\n Building level: " + buildingLevel);
				cost.text = "Cost: \r\n\t Wood: " + upgrWood2 + "\r\n\t Rock: " + upgrRock2 + "\r\n\t Iron: " + upgrIron2 + "\r\n\t Gold: " + upgrGold2 + "\r\n\t Xp points: " + upgrPoints2 + "\r\n Building level: "+ buildingLevel +"/3";
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}	else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 1){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood2 && Master.GetComponent <GameMaster>().gRock >= upgrRock2 && Master.GetComponent <GameMaster>().gIron >= upgrIron2 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints2 && Master.GetComponent <GameMaster>().gGold >= upgrGold2){
				SpawnTime -= 100;
				maxShieldTroops += 30;
				maxSSTroops += 40;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().barrQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood2);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold2);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints2;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "Cost: \r\n\t Wood: " + upgrWood3 + "\r\n\t Rock: " + upgrRock3 + "\r\n\t Iron: " + upgrIron3 + "\r\n\t Gold: " + upgrGold3 + "\r\n\t Xp points: " + upgrPoints3 + "\r\n Building level: "+ buildingLevel +"/3";
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops from this barracks: " + (gameObject.transform.Find ("Troops").GetComponentsInChildren <Transform> ().Length - 1) + "\r\n Building level: " + buildingLevel);
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 2){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood3 && Master.GetComponent <GameMaster>().gRock >= upgrRock3 && Master.GetComponent <GameMaster>().gIron >= upgrIron3 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints3 && Master.GetComponent <GameMaster>().gGold >= upgrGold3){
				SpawnTime -= 100;
				maxShieldTroops += 30;
				maxSSTroops += 40;
				Master.GetComponent <GameMaster> ().sTFoodConsuption = 3;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().barrQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood3);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold3);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints3;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "You have upgraded this building to it's maximum. "+ "\r\n Building level: " + buildingLevel + "/3";
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops from this barracks: " + (gameObject.transform.Find ("Troops").GetComponentsInChildren <Transform> ().Length - 1) + "\r\n Building level: " + buildingLevel);
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
		inQueue = false;
	}

	public void DestroyBuilding(){
		CloseMenu ();
		BuildingDiedProtocol ();
		Destroy (gameObject, 0.1f);
	}

	public void PlaceSpawn(){
		CloseMenu ();
		placeFlag = true;
		Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, 10f);
		if(Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, 10f)){
			flagPlace = Instantiate (flagPL, hit.point, Quaternion.identity, gameObject.transform);
			flagPlace.name = "Rally point placeholder";
			flagPlace.transform.Rotate (0, 0, -180);
			flagPlace.transform.Translate (0, flagUpOffset, 0);
		}else{
			flagPlace = Instantiate (flagPL, Player.transform.position, Quaternion.identity, gameObject.transform);
			flagPlace.name = "Rally point placeholder";
			flagPlace.transform.Rotate (0, 0, -180);
			flagPlace.transform.Translate (0, flagUpOffset, 0);
		}
	}

}
