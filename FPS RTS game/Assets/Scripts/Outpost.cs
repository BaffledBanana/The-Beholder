using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Outpost : MonoBehaviour {

	private GameObject Master, Menu, PlayerUI, BuildingManager, buildingInfo, loadingBar, miscBuild;
	private List<Collider> woodRes, rockRes, ironRes, foodRes;
	private Collider[] allRes;
	private Transform[] workers;
	private int recycledWood, recycledRock, recycledIron, wQueue, rQueue, iQueue, fQueue, x, z;
	private bool inQueue;
	private Transform SpawnLoc, orgSpawnLoc;
	private float y, maxPercentage;
	private List<GameObject> resGOs;

	public Text cost, queueInfo;
	public float radius, maxRadius, dstBtwWorkerSpawn;//put this in upgrades building and make it get an array of all workers and make it call the ResLoc function in all of them
	public GameObject WoodWorker, RockWorker, IronWorker, FoodWorker;
	public int wWorkerWCost, wWorkerRCost, wWorkerICost, wWorkerFCost, rWorkerWCost, rWorkerRCost, rWorkerICost, rWorkerFCost, iWorkerWCost, iWorkerRCost, iWorkerICost, iWorkerFCost, fWorkerWCost, fWorkerRCost, fWorkerICost, fWorkerFCost;
	public int upgrGoldCost, upgrWoodCost, upgrRockCost, upgrIronCost, maxWorkers, buildingLevel, SpawnTime, numberInRow;

	//If max radius gets bigger we need to run Test() again
	//IMPORTANT all outposts must have their canvases named "Canvas"

	void Awake () {
		woodRes = new List<Collider>();
		rockRes = new List<Collider>();
		ironRes = new List<Collider>();
		foodRes = new List<Collider>();
		resGOs = new List<GameObject> ();
		allRes = new Collider[15];
		Master = GameObject.FindGameObjectWithTag ("GameMaster");
		miscBuild = GameObject.Find ("GUIHUD");
		Menu = gameObject.transform.Find ("Canvas").gameObject;
		Menu.SetActive (false);
		PlayerUI = GameObject.Find ("PlayerUI");
		BuildingManager = GameObject.Find ("ConstructBuilding");
		buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
		workers = gameObject.transform.Find ("Workers").GetComponentsInChildren<Transform> ();
		queueInfo = Menu.transform.Find ("QueueInformation").GetComponent <Text>();
		loadingBar = Menu.transform.Find ("LoadingBar").gameObject;
		loadingBar.transform.localScale = new Vector3 (0, 1, 1);
		SpawnLoc = gameObject.transform.Find ("SpawnLocation").GetComponent <Transform> ();
		orgSpawnLoc = SpawnLoc;
		cost = Menu.transform.Find ("Cost").GetComponent <Text>();
		cost.text = "Cost: " + "\r\n Wood: " + upgrWoodCost + "\r\n Rock: " + upgrRockCost + "\r\n Iron: " + upgrIronCost + "\r\n Gold: " + upgrGoldCost;
	}
	void Start(){
		Test ();
		Master.GetComponent <GameMaster>().Add ("Happyness", 2);
		Master.GetComponent <GameMaster>().AddXpToPlayer (50);
		buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Workers at this outpost: " + (workers.Length - 1) + "/(max)" + maxWorkers + "\r\n Found wood resources: " + woodRes.Count + "\r\n Found rock resources: " + rockRes.Count + "\r\n Found iron resources: " + ironRes.Count + "\r\n Found food resources: " + foodRes.Count + "\r\n Building level: " + buildingLevel);
		queueInfo.text = "Currently in queue: " + "\r\n wood workers: " + wQueue + "\r\n rock workers: " + rQueue + "\r\n iron workers: " + iQueue + "\r\n food gatherers: " + fQueue;				
		maxPercentage = SpawnTime;
		UpdateWorkerCosts ();
	}

	public void OpenMenu(){
		if(gameObject.GetComponent <BuildHammer>().isBuilt){
			PlayerUI.SetActive (false);
			Master.GetComponent <GameMaster> ().DisablePlayer ();
			Menu.SetActive (true);	
			GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (true);
		}
	}
	public void CloseMenu(){
		PlayerUI.SetActive (true);
		Master.GetComponent <GameMaster> ().EnablePlayer ();
		Menu.SetActive (false);
		GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (false);
	}
	public void SpawnWoodWorker(){
		if(workers.Length < maxWorkers){
			if(Master.GetComponent <GameMaster>().population > 0 && Master.GetComponent <GameMaster>().workers < Master.GetComponent <GameMaster>().population){
				if(Master.GetComponent<GameMaster> ().gWood >= wWorkerWCost && Master.GetComponent<GameMaster> ().gRock >= wWorkerRCost && Master.GetComponent<GameMaster> ().gFood >= wWorkerFCost){
					Master.GetComponent<GameMaster> ().Subtract("Wood", wWorkerWCost);
					Master.GetComponent<GameMaster> ().Subtract ("Rock", wWorkerRCost);
					Master.GetComponent<GameMaster> ().Subtract ("Food", wWorkerFCost);
					Master.GetComponent <GameMaster>().Add ("Workers", 1);
					inQueue = true;
					wQueue++;
					queueInfo.text = "Currently in queue: " + "\r\n wood workers: " + wQueue + "\r\n rock workers: " + rQueue + "\r\n iron workers: " + iQueue + "\r\n food gatherers: " + fQueue;				
				}
				else{ Debug.Log("Outpost: Not enough resources");
					Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough resources in your town.");
				}
			}else{
				Debug.Log ("Outpost: not enough pop");
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough unoccupied citizens in your town.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, this outpost is at maximum worker capacity.");

		}
	}
	public void SpawnRockWorker(){
		if(workers.Length < maxWorkers){
			if (Master.GetComponent <GameMaster> ().population > 0 && Master.GetComponent <GameMaster> ().workers < Master.GetComponent <GameMaster> ().population) {
				if(Master.GetComponent<GameMaster> ().gIron >= rWorkerICost && Master.GetComponent<GameMaster> ().gWood >= rWorkerWCost && Master.GetComponent<GameMaster> ().gRock >= rWorkerRCost && Master.GetComponent<GameMaster> ().gFood >= rWorkerFCost){
					Master.GetComponent<GameMaster> ().Subtract("Wood", rWorkerWCost);
					Master.GetComponent<GameMaster> ().Subtract ("Rock", rWorkerRCost);
					Master.GetComponent<GameMaster> ().Subtract ("Food", rWorkerFCost);
					Master.GetComponent<GameMaster> ().Subtract ("Iron", rWorkerICost);
					Master.GetComponent <GameMaster>().Add ("Workers", 1);
					inQueue = true;
					rQueue++;
					queueInfo.text = "Currently in queue: " + "\r\n wood workers: " + wQueue + "\r\n rock workers: " + rQueue + "\r\n iron workers: " + iQueue + "\r\n food gatherers: " + fQueue;				
				}
				else{ Debug.Log("Outpost: Not enough resources");
					Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough resources in your town.");		
				}
			}else{
				Debug.Log ("Outpost: not enough pop");
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough unoccupied citizens in your town.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, this outpost is at maximum worker capacity.");

		}
	}
	public void SpawnIronWorker(){
		if(workers.Length < maxWorkers){
			if (Master.GetComponent <GameMaster> ().population > 0 && Master.GetComponent <GameMaster> ().workers < Master.GetComponent <GameMaster> ().population) {
				if(Master.GetComponent<GameMaster> ().gIron >= iWorkerICost && Master.GetComponent<GameMaster> ().gWood >= iWorkerWCost && Master.GetComponent<GameMaster> ().gRock >= iWorkerRCost && Master.GetComponent<GameMaster> ().gFood >= iWorkerFCost){
					Master.GetComponent<GameMaster> ().Subtract("Wood", iWorkerWCost);
					Master.GetComponent<GameMaster> ().Subtract ("Rock", iWorkerRCost);
					Master.GetComponent<GameMaster> ().Subtract ("Iron", iWorkerICost);
					Master.GetComponent<GameMaster> ().Subtract ("Food", iWorkerFCost);
					Master.GetComponent <GameMaster>().Add ("Workers", 1);
					inQueue = true;
					iQueue++;
					queueInfo.text = "Currently in queue: " + "\r\n wood workers: " + wQueue + "\r\n rock workers: " + rQueue + "\r\n iron workers: " + iQueue + "\r\n food gatherers: " + fQueue;				
				}
				else{ Debug.Log("Outpost: Not enough res");
					Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough resources in your town.");		
				}
			}else{
				Debug.Log ("Outpost: not enough pop");
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough unoccupied citizens in your town.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, this outpost is at maximum worker capacity.");

		}
	}

	public void SpawnFoodWorker (){
		if(workers.Length < maxWorkers){
			if (Master.GetComponent <GameMaster> ().population > 0 && Master.GetComponent <GameMaster> ().workers < Master.GetComponent <GameMaster> ().population) {
				if (Master.GetComponent<GameMaster> ().gIron >= fWorkerICost && Master.GetComponent<GameMaster> ().gWood >= fWorkerWCost && Master.GetComponent<GameMaster> ().gRock >= fWorkerRCost && Master.GetComponent<GameMaster> ().gFood >= fWorkerFCost) {
					Master.GetComponent<GameMaster> ().Subtract("Wood", fWorkerWCost);
					Master.GetComponent<GameMaster> ().Subtract ("Rock", fWorkerRCost);
					Master.GetComponent<GameMaster> ().Subtract ("Food", fWorkerFCost);
					Master.GetComponent<GameMaster> ().Subtract ("Iron", fWorkerICost);
					Master.GetComponent <GameMaster>().Add ("Workers", 1);
					inQueue = true;
					fQueue++;
					queueInfo.text = "Currently in queue: " + "\r\n wood workers: " + wQueue + "\r\n rock workers: " + rQueue + "\r\n iron workers: " + iQueue + "\r\n food gatherers: " + fQueue;				
				} else {
					Debug.Log ("Outpost: Not enough resources");
					Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough resources in your town.");		
				}
			}else{
				Debug.Log ("Outpost: not enough pop");
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough unoccupied citizens in your town.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, this outpost is at maximum worker capacity.");

		}
	}
	public void ExpandRadius(){
			radius += 5;
			Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGoldCost);
			Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWoodCost);
			Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRockCost);
			Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrIronCost);
			workers = gameObject.transform.Find ("Workers").GetComponentsInChildren<Transform> ();
			if(workers.Length > 1){
				foreach(Transform curr in workers){
					curr.GetComponent <Worker>().ReResLocWorker ();
				}
			}
		cost.text = "Cost: " + "\r\n Wood: " + upgrWoodCost + "\r\n Rock: " + upgrRockCost + "\r\n Iron: " + upgrIronCost + "\r\n Gold: " + upgrGoldCost;
	}

	public List<Collider> ResLoc(string name){
		Test ();
		if(name == "Wood"){
			return woodRes;		
		}
		else if(name == "Rock"){
			return rockRes; 	
		}
		else if(name == "Iron"){
			return ironRes;		
		}
		else if(name == "Food"){
			return foodRes;		
		}
		else {return null;}	
	}
	public void Test(){
		woodRes.Clear ();
		rockRes.Clear ();
		foodRes.Clear ();
		ironRes.Clear (); 
		//allRes.Clear (allRes, 0, allRes.Length);
		allRes = Physics.OverlapSphere (gameObject.transform.position, radius); //gets all the things around the outpost
		/*if(allRes.Length <= 0){
			Debug.Log ("Outpost: didnt find any res");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, no resources were detected in this area, you might want to rebuild the outpost closer to the resources or upgrade the building.");
		} */
		foreach(Collider current in allRes){   	//sorts all the gathered information into individual arrays
			if(current.CompareTag ("Wood")){
				woodRes.Add (current);
				//Debug.Log ("Outpost: this is how much wood has been found - " + woodRes.Count);//this function has problems, the more workers you hire for a certain building the last ones will keep checking for other resources longer in their own private arrays
			}
			else if(current.CompareTag ("Rock")){
				rockRes.Add (current);
			}
			else if(current.CompareTag ("Iron")){
				ironRes.Add (current);
			}
			else if(current.CompareTag ("Food")){
				foodRes.Add (current);
			}
		}
		/*if(woodRes.Count == 0 && rockRes == 0 && ironRes.Count == 0 && foodRes.Count == 0){
			Debug.Log ("Outpost: didnt find any res");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("No resources were detected in this area, you might want to rebuild the outpost closer to the resources.");
		} */
		workers = gameObject.transform.Find ("Workers").GetComponentsInChildren<Transform> ();
		buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Workers at this outpost: " + (workers.Length - 1) + "/(max)" + maxWorkers + "\r\n Found wood resources: " + woodRes.Count + "\r\n Found rock resources: " + rockRes.Count + "\r\n Found iron resources: " + ironRes.Count + "\r\n Found food resources: " + foodRes.Count + "\r\n Building level: " + buildingLevel);
	}
	public void DestroyOutpost(){
		workers = gameObject.transform.Find ("Workers").GetComponentsInChildren<Transform> ();
		recycledWood = (int)Math.Round( (workers.Length -1) * (((wWorkerWCost + rWorkerWCost + iWorkerWCost) / 3) * Master.GetComponent <GameMaster>().recyclePercentage));
		recycledRock = (int)Math.Round((workers.Length -1) * (((wWorkerRCost + rWorkerRCost + iWorkerRCost) / 3) * Master.GetComponent <GameMaster>().recyclePercentage));
		recycledIron = (int)Math.Round((workers.Length -1) * (((wWorkerICost + rWorkerICost + iWorkerICost) / 3) * Master.GetComponent <GameMaster>().recyclePercentage));
		recycledWood += (int)Math.Round (BuildingManager.GetComponent <ConstructBuilding> ().outpostWoodCost * Master.GetComponent <GameMaster>().recyclePercentage, 0);
		recycledRock += (int)Math.Round (BuildingManager.GetComponent <ConstructBuilding> ().outpostRockCost * Master.GetComponent <GameMaster>().recyclePercentage, 0);
		recycledIron += (int)Math.Round (BuildingManager.GetComponent <ConstructBuilding> ().outpostIronCost * Master.GetComponent <GameMaster>().recyclePercentage, 0);
		Master.GetComponent <GameMaster>().Add ("Wood", recycledWood);
		Master.GetComponent <GameMaster>().Add ("Rock", recycledRock);
		Master.GetComponent <GameMaster>().Add ("Iron", recycledIron);
		//Master.GetComponent <GameMaster>().Add ("Population", (workers.Length -1));
		if(Master.GetComponent <GameMaster>().workers > 0 && workers.Length - 1 > 0){
			Master.GetComponent <GameMaster> ().Subtract ("Workers", (workers.Length -1));	
		}
		CloseMenu ();
		Debug.Log ("Outpost has been recycled.");
		Master.GetComponent <GameMaster> ().PassErrorMessage ("Outpost has been recycled for some of it's resources.");
		Destroy (gameObject, 0f);
	}

	public void UpgradeBuilding(){
		if(radius + 5 < maxRadius && Master.GetComponent <GameMaster>().gGold >= upgrGoldCost && Master.GetComponent <GameMaster>().gWood >= upgrWoodCost && Master.GetComponent <GameMaster>().gRock >= upgrRockCost && Master.GetComponent <GameMaster>().gIron >= upgrIronCost){
			ExpandRadius ();
			//other upgrade stuff
			maxWorkers += 10;
			gameObject.GetComponent <BuildingHealth> ().health += 100;
			gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
			miscBuild.GetComponent <MiscBuildMenu> ().MSQueue++;
			miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
			buildingLevel++;
			ResLoc ("Wood");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
			workers = gameObject.transform.Find ("Workers").GetComponentsInChildren<Transform> ();
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Workers at this outpost: " + (workers.Length - 1) + "/(max)" + maxWorkers + "\r\n Found wood resources: " + woodRes.Count + "\r\n Found rock resources: " + rockRes.Count + "\r\n Found iron resources: " + ironRes.Count + "\r\n Found food resources: " + foodRes.Count + "\r\n Building level: " + buildingLevel);
		}else{
			Debug.Log ("Outpost: not enough res to expand radius or it's at max");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you do not have enough resources to expand this outposts radius or it has reached it's maximum radius.");
		}
	}

	void Update(){
		if(inQueue  && gameObject.GetComponent <BuildHammer>().isBuilt){
			x++;
			y = x;
			if(x >= SpawnTime){
				if(wQueue <= 5 && wQueue > 0){
					for(int i = 0; i < wQueue; i++){
						Spawn (WoodWorker);
						//SpawnWorker.GetComponent<SpawnWorker>().Spawn("Wood", gameObject, WoodWorker);

						//Debug.Log ("In for loop " + i + "/" + wQueue);
					}
					wQueue = 0;
					SpawnLoc = orgSpawnLoc;
					queueInfo.text = "Currently in queue: " + "\r\n wood workers: " + wQueue + "\r\n rock workers: " + rQueue + "\r\n iron workers: " + iQueue + "\r\n food gatherers: " + fQueue;
					//update building info
				}else if(wQueue > 5 && wQueue > 0){
					for(int i = 0; i < 5; i++){
						Spawn (WoodWorker);
						//SpawnWorker.GetComponent<SpawnWorker>().Spawn("Wood", gameObject, WoodWorker);

					}
					wQueue -= 5;
					SpawnLoc = orgSpawnLoc;
					queueInfo.text = "Currently in queue: " + "\r\n wood workers: " + wQueue + "\r\n rock workers: " + rQueue + "\r\n iron workers: " + iQueue + "\r\n food gatherers: " + fQueue;					//update building info
				}
				if(rQueue <= 5 && rQueue > 0){
					for(int i = 0; i < rQueue; i++){
						Spawn (RockWorker);
						//SpawnWorker.GetComponent<SpawnWorker>().Spawn("Rock", gameObject, RockWorker);

					}
					rQueue = 0;
					SpawnLoc = orgSpawnLoc;
				queueInfo.text = "Currently in queue: " + "\r\n wood workers: " + wQueue + "\r\n rock workers: " + rQueue + "\r\n iron workers: " + iQueue + "\r\n food gatherers: " + fQueue;			
				}else if(rQueue > 5 && rQueue > 0){
					for(int i = 0; i < 5; i++){
						Spawn (RockWorker);
						//SpawnWorker.GetComponent<SpawnWorker>().Spawn("Rock", gameObject, RockWorker);

					}
					rQueue -= 5;
					SpawnLoc = orgSpawnLoc;
					queueInfo.text = "Currently in queue: " + "\r\n wood workers: " + wQueue + "\r\n rock workers: " + rQueue + "\r\n iron workers: " + iQueue + "\r\n food gatherers: " + fQueue;				
				}
				if(fQueue <= 5 && fQueue > 0){
					for(int i = 0; i < fQueue; i++){
						Spawn (FoodWorker);
						//SpawnWorker.GetComponent<SpawnWorker>().Spawn("Food", gameObject, FoodWorker);

					}
					fQueue = 0;
					SpawnLoc = orgSpawnLoc;
					queueInfo.text = "Currently in queue: " + "\r\n wood workers: " + wQueue + "\r\n rock workers: " + rQueue + "\r\n iron workers: " + iQueue + "\r\n food gatherers: " + fQueue;			
				}else if(fQueue > 5 && fQueue > 0){
					for(int i = 0; i < 5; i++){
						Spawn (FoodWorker);
						//SpawnWorker.GetComponent<SpawnWorker>().Spawn("Food", gameObject, FoodWorker);
					}
					fQueue -= 5;
					SpawnLoc = orgSpawnLoc;
					queueInfo.text = "Currently in queue: " + "\r\n wood workers: " + wQueue + "\r\n rock workers: " + rQueue + "\r\n iron workers: " + iQueue + "\r\n food gatherers: " + fQueue;				
				}
				if(iQueue <= 5 && iQueue > 0){
					for(int i = 0; i < iQueue; i++){
						Spawn (IronWorker);
						//SpawnWorker.GetComponent<SpawnWorker>().Spawn("Iron", gameObject, IronWorker);
					}
					iQueue = 0;
					SpawnLoc = orgSpawnLoc;
					queueInfo.text = "Currently in queue: " + "\r\n wood workers: " + wQueue + "\r\n rock workers: " + rQueue + "\r\n iron workers: " + iQueue + "\r\n food gatherers: " + fQueue;			
				}else if(iQueue > 5 && iQueue > 0){
					for(int i = 0; i < 5; i++){
						Spawn (IronWorker);
						//SpawnWorker.GetComponent<SpawnWorker>().Spawn("Iron", gameObject, IronWorker);

					}
					iQueue -= 5;
					SpawnLoc = orgSpawnLoc;
					queueInfo.text = "Currently in queue: " + "\r\n wood workers: " + wQueue + "\r\n rock workers: " + rQueue + "\r\n iron workers: " + iQueue + "\r\n food gatherers: " + fQueue;				
					//buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Workers at this outpost: " + (workers.Length - 1) + "/(max)" + maxWorkers + "\r\n Found wood resources: " + woodRes.Count + "\r\n Found rock resources: " + rockRes.Count + "\r\n Found iron resources: " + ironRes.Count + "\r\n Found food resources: " + foodRes.Count + "\r\n Building level: " + buildingLevel);
				}
				if(wQueue == 0 && rQueue == 0 && fQueue == 0 && iQueue == 0){
					loadingBar.transform.localScale = new Vector3 (0, 1, 1);
					x = 0;
					y = 0;
					z = 0;
					inQueue = false;
					workers = gameObject.transform.Find ("Workers").GetComponentsInChildren<Transform> ();
					buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Workers at this outpost: " + (workers.Length - 1) + "/(max)" + maxWorkers + "\r\n Found wood resources: " + woodRes.Count + "\r\n Found rock resources: " + rockRes.Count + "\r\n Found iron resources: " + ironRes.Count + "\r\n Found food resources: " + foodRes.Count + "\r\n Building level: " + buildingLevel);
				}
				x = 0;
				y = 0;
				z = 0;
			}
			loadingBar.transform.localScale = new Vector3 ((y / maxPercentage), 1, 1);
		}
	}

	void Spawn(GameObject curr){
		if(Physics.OverlapSphere (SpawnLoc.position, dstBtwWorkerSpawn).Length == 0){
			Instantiate (curr, gameObject.transform.Find ("SpawnLocation").transform.transform.position, Quaternion.identity, gameObject.transform.Find ("Workers").transform);
			workers = gameObject.transform.Find ("Workers").GetComponentsInChildren<Transform> ();
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Workers at this outpost: " + (workers.Length - 1) + "/(max)" + maxWorkers + "\r\n Found wood resources: " + woodRes.Count + "\r\n Found rock resources: " + rockRes.Count + "\r\n Found iron resources: " + ironRes.Count + "\r\n Found food resources: " + foodRes.Count + "\r\n Building level: " + buildingLevel);
		}else{
			SpawnLoc.position = new Vector3 (SpawnLoc.position.x + dstBtwWorkerSpawn, SpawnLoc.position.y, SpawnLoc.position.z);
			Spawn (curr);
			z++;
			if(z >= numberInRow + 2){
				SpawnLoc.position = new Vector3 (SpawnLoc.position.x - dstBtwWorkerSpawn*(numberInRow + 3), SpawnLoc.position.y, SpawnLoc.position.z - dstBtwWorkerSpawn);
				z = 0;
			}
		}
	}

	public List<GameObject> RequestResources(string resName){
		resGOs.Clear ();
		foreach(Collider res in ResLoc (resName)){
			resGOs.Add (res.gameObject);
		}
		return resGOs;
	}

	public void UpdateWorkerCosts(){
		Menu.gameObject.transform.Find ("SpawnWoodWorker").Find ("Cost").GetComponent <Text>().text = "Cost: \n\t Wood: " + wWorkerWCost + "\n\t Rock: " + wWorkerRCost + "\n\t Iron: " + wWorkerICost + "\n\t Food: " + wWorkerFCost;
		Menu.gameObject.transform.Find ("SpawnRockWorker").Find ("Cost").GetComponent <Text>().text = "Cost: \n\t Wood: " + rWorkerWCost + "\n\t Rock: " + rWorkerRCost + "\n\t Iron: " + rWorkerICost + "\n\t Food: " + rWorkerFCost;
		Menu.gameObject.transform.Find ("SpawnIronWorker").Find ("Cost").GetComponent <Text>().text = "Cost: \n\t Wood: " + iWorkerWCost + "\n\t Rock: " + iWorkerRCost + "\n\t Iron: " + iWorkerICost + "\n\t Food: " + iWorkerFCost;
		Menu.gameObject.transform.Find ("SpawnFoodWorker").Find ("Cost").GetComponent <Text>().text = "Cost: \n\t Wood: " + fWorkerWCost + "\n\t Rock: " + fWorkerRCost + "\n\t Iron: " + fWorkerICost + "\n\t Food: " + fWorkerFCost;
	}

}
