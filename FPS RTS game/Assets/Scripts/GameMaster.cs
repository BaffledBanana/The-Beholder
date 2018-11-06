using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class GameMaster : MonoBehaviour {

	public Text gameTimerText;
	public int standardCycleLength = 1000, happyness = 100, quitGameTimer = 500, hpynssTimer;
	public int population = 0, ssTroops = 0, workers = 0, storage = 500, maxPopulation = 5, pFoodConsuption = 3, wFoodConsumption = 1, ssTFoodConsuption = 3, sTFoodConsuption = 4, sTroops = 0, ssTroopGCost, sTroopGCost;
	public int gWood, gRock, gIron, gFood, gGold, gHide;
	public float recyclePercentage = 0.6f;
	public bool debugMode, constructStructures;

	private int x = 0, maxHappyness = 150, minHappyness = 50, happynessCounter, y, z, h, seconds, minutes, hours;
	private GameObject GlobalStats, ErrorMessageLog, currentWorker, player, cam;
	private GameObject[] allWorkers, allSSTroops, someHouses, farms, banks, mineShafts, allShieldTroops;
	private List<GameObject> filteredSST, filteredST;
	private bool quitGame = false, foodWasAt0 = false;
	private float gameTimer, jmpSpeed, mvmSpeed, msSpeed;
	private string timerString;

	void Awake () {
		//Application.targetFrameRate = 100;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	GlobalStats = GameObject.Find ("GlobalValuesUIMenu");
		ErrorMessageLog = GameObject.Find ("ErrorMessageHandler");
		player = GameObject.Find ("Player");
		cam = GameObject.Find ("Main Camera");
		GameObject.Find("BackDrop").SetActive(false);
	}

	void Start(){
		jmpSpeed = GameObject.Find ("Player").GetComponent <PlayerControler> ().jumpSpeed;
		mvmSpeed = GameObject.Find ("Player").GetComponent <PlayerControler> ().movementSpeed;
		msSpeed = GameObject.Find ("Player").GetComponent <PlayerControler> ().mouseSpeed;
		filteredSST = new List<GameObject> ();
		filteredST = new List<GameObject> ();
		if(debugMode){
			storage = 10000;
			gWood = 1000;
			gRock = 1000;
			gIron = 1000;
			gFood = 1000;
			gGold = 1000;
			gHide = 1000;
			cam.GetComponent <PlayerStats>().maxWood = 1000;
			cam.GetComponent <PlayerStats>().maxRock = 1000;
			cam.GetComponent <PlayerStats>().maxIron = 1000;
			cam.GetComponent <PlayerStats>().maxFood = 1000;
			cam.GetComponent <PlayerStats>().maxGold = 1000;
			cam.GetComponent <PlayerStats>().maxHide = 1000;
			cam.GetComponent <PlayerStats>().pWood = 1000;
			cam.GetComponent <PlayerStats>().pRock = 1000;
			cam.GetComponent <PlayerStats>().pIron = 1000;
			cam.GetComponent <PlayerStats>().pFood = 1000;
			cam.GetComponent <PlayerStats>().pGold = 1000;
			cam.GetComponent <PlayerStats>().pHide = 1000;
			cam.GetComponent <PlayerStats> ().xpPoints = 100;
		}
		UpdateValues ();
	}

	void Update () {
		//THE MAIN GAME UI TIMER
		gameTimer += Time.deltaTime;
		seconds = (int)(gameTimer % 60);
		minutes = (int)(gameTimer / 60) % 60;
		hours = (int)(gameTimer / 3600) % 24;
		timerString = string.Format ("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
		gameTimerText.text = timerString;
		//GAME OVER PROTOCOL
		if(quitGame){
			if(z >= quitGameTimer){
				DisablePlayer ();
				SceneManager.LoadScene ("Normal");
			}else{
				z++;
			}
		}
		//MAIN GAME LOOP AKA"TICKS" CHECKS
		x++;
		if (x == standardCycleLength) {
		//stuff happens here every "tick"
			//Debug.Log (Time.time);
			//ALL BUILDING CHECKS HERE(stuff that needs to be done to all instances of the building in the map)
			FarmCheck ();
			MineCheck ();
			BankCheck ();
			TroopCheck ();
			CitizenCheck ();
			//HappynessCheck ();
			/*if(h >= hpynssTimer){
				Subtract ("Happyness", 1);
				h = 0;
			}else{
				h++;
			} */
			Debug.Log ("One tick happened!");
			x = 0;
		}
	}

	public void Add(string name, int amount){
		if(name == "Wood" || name == "Rock" || name == "Iron" || name == "Food" || name == "Hide" || name == "Gold"){
			if (CalculateStorage () + amount <= storage) {
				if(name == "Wood"){
					gWood += amount;
				}
				else if(name == "Rock"){
					gRock += amount;
				}
				else if(name == "Iron"){
					gIron += amount;
				}
				else if(name == "Food"){
					gFood += amount;
				}
				else if(name == "Hide"){
					gHide += amount;
				}
				else if(name == "Gold"){
					gGold += amount;
				}
			}else{
				PassErrorMessage ("Your storage is full.");
			}
		}
		else if(name == "Happyness"){
			if(happyness + amount <= maxHappyness){
				happyness += amount;
				GlobalStats.GetComponent <GlobalValuesUI>().UpdateText ("Happyness", happyness - 100, 0);
			}else if(happyness + amount > maxHappyness){
				happyness = maxHappyness;
				GlobalStats.GetComponent <GlobalValuesUI>().UpdateText ("Happyness", happyness - 100, 0);
			}
		}
		else if(name == "Population" && population + amount <= maxPopulation){ //&& workers !=  0
			population += amount;
		}else if(name == "MaxPopulation"){ //&& workers !=  0
			maxPopulation += amount;
		}
		else if(name == "SSTroops"){
			ssTroops++;
		}else if(name == "ShieldTroops"){
			sTroops++;
		}
		else if(name == "Workers"){
		//Debug.Log ("GameMaster: worker added");
			workers += amount;
		}
		UpdateValues ();
	}

	public void Subtract(string name, int amount){
		if(name == "Wood" || name == "Rock" || name == "Iron" || name == "Food" || name == "Hide" || name == "Gold"){
			if(name == "Wood" && gWood >= amount){
				gWood -= amount;
			}
			else if(name == "Rock" && gRock >= amount){
				gRock -= amount;
			}
			else if(name == "Iron" && gIron >= amount){
				gIron -= amount;
			}
			else if(name == "Food"){
				gFood -= amount;
			}
			else if(name == "Gold" && gGold >= amount){
				gGold -= amount;
			}
			else if(name == "Hide" && gHide >= amount){
				gHide -= amount;
			}else{
				PassErrorMessage ("You do not have enough resources.");
			}
		}
		else if(name == "Population" && population > 0){
			population -= amount;
		}
		else if(name == "MaxPopulation" && maxPopulation > 0){
			maxPopulation -= amount;
		}
		else if(name == "SSTroops"){
			ssTroops--;
		}else if(name == "ShieldTroops"){
			sTroops--;
		}
		else if(name == "Happyness"){
			if(happyness - amount >= minHappyness){
				happyness -= amount;
				GlobalStats.GetComponent <GlobalValuesUI>().UpdateText ("Happyness", happyness - 100, 0);
			}else if(happyness - amount < minHappyness){
				happyness = minHappyness;
				GlobalStats.GetComponent <GlobalValuesUI>().UpdateText ("Happyness", happyness - 100, 0);
			}
		}
		else if(name == "Workers" && workers > 0){
			workers -= amount;
		}
		UpdateValues ();
	}

	public void UpdateValues(){			//HERE UPDATE STATS ON SCREEN
		GlobalStats.GetComponent <GlobalValuesUI>().UpdateText ("Wood", gWood, storage);
		GlobalStats.GetComponent <GlobalValuesUI>().UpdateText ("Rock", gRock, storage);
		GlobalStats.GetComponent <GlobalValuesUI>().UpdateText ("Food", gFood, storage);
		GlobalStats.GetComponent <GlobalValuesUI>().UpdateText ("Iron", gIron, storage);
		GlobalStats.GetComponent <GlobalValuesUI>().UpdateText ("Gold", gGold, storage);
		GlobalStats.GetComponent <GlobalValuesUI>().UpdateText ("Hide", gHide, storage);
		GlobalStats.GetComponent <GlobalValuesUI>().UpdateText ("Population", population, maxPopulation);
		GlobalStats.GetComponent <GlobalValuesUI>().UpdateText ("Workers", workers, population);
		GlobalStats.GetComponent <GlobalValuesUI>().UpdateText ("Storage", CalculateStorage (), storage);
		GlobalStats.GetComponent <GlobalValuesUI>().UpdateText ("Happyness", happyness - 100, 0);
	}

	public int CalculateStorage(){
		return gWood + gRock + gIron + gFood + gGold + gHide;
	}
	public void PassErrorMessage(string msg){
		ErrorMessageLog.GetComponent<ErrorMessageHandler> ().PostError (msg);
	}
	public int GenerateRandom(int min, int max){
		return Random.Range (min, max);
	}
	public void DisablePlayer(){
		player.GetComponent <PlayerControler> ().mouseSpeed = 0;
		player.GetComponent <PlayerControler> ().movementSpeed = 0;
		player.GetComponent <PlayerControler> ().jumpSpeed = 0;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
	public void EnablePlayer(){
		player.GetComponent <PlayerControler> ().movementSpeed = mvmSpeed;
		player.GetComponent <PlayerControler> ().mouseSpeed = msSpeed;
		player.GetComponent <PlayerControler> ().jumpSpeed = jmpSpeed;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void FarmCheck(){
		farms = GameObject.FindGameObjectsWithTag ("Farm");
		foreach(GameObject farm in farms){
			Add ("Food", farm.GetComponent <Farm>().AddFood ());
		}
	}

	public void AddXpToPlayer(int amount){
		cam.GetComponent <PlayerStats>().AddXp (amount);
	}

	void MineCheck(){
		mineShafts = GameObject.FindGameObjectsWithTag ("MineShaft");
		foreach(GameObject curr in mineShafts){
			Add ("Rock", curr.GetComponent <MineShaft>().MineRock ());
			Add ("Iron", curr.GetComponent <MineShaft>().MineIron ());
		}
	}
	void BankCheck(){
		banks = GameObject.FindGameObjectsWithTag ("Bank");
		foreach(GameObject bank in banks){
			bank.GetComponent <Bank>().AddGold ();
			bank.GetComponent <Bank> ().ShouldPayLoan ();
		}
	}
	void TroopCheck(){
		allSSTroops = GameObject.FindGameObjectsWithTag ("SSTroop");
		allShieldTroops = GameObject.FindGameObjectsWithTag ("ShieldTroop");
		filteredST.Clear ();
		filteredSST.Clear ();
		foreach(GameObject troop in allSSTroops){
			if(troop.layer == 8){//friendly troop
				filteredSST.Add (troop);
			}
		}
		foreach(GameObject troop in allShieldTroops){
			if(troop.layer == 8){//friendly troop
				filteredST.Add (troop);
			}
		}

		if(filteredSST.Count > 0 || filteredST.Count > 0){
			if (gGold >= (filteredSST.Count * ssTroopGCost) + (filteredST.Count * sTroopGCost)) {
				Subtract ("Gold", (filteredSST.Count * ssTroopGCost) + (filteredST.Count * sTroopGCost));
			} else {
				Debug.Log ("Ran out of money to pay troops!");
				PassErrorMessage ("You have ran out of gold to pay your troops, they are quitting!");
				if (filteredST.Count >= 5) {
					y = 0;
					foreach (GameObject currTroop in filteredST) {
						if (y < 5) {
							Destroy (currTroop);
						}
						y++;
					}
				} else if (filteredST.Count > 0) {
					foreach (GameObject currTroop in filteredST) {
						Destroy (currTroop);
					}
				} else if (filteredSST.Count >= 5) {
					y = 0;
					foreach (GameObject currTroop in filteredSST) {
						if (y < 5) {
							Destroy (currTroop);
						}
						y++;
					}
				} else if (filteredSST.Count > 0) {
					foreach (GameObject currTroop in filteredSST) {
						Destroy (currTroop);
					}
				}
			}
		}

	}

	public void CitizenCheck(){
		if(gFood >= (population * pFoodConsuption) + (workers * wFoodConsumption) + (sTroops * sTFoodConsuption) + (ssTroops * ssTFoodConsuption)){
			foodWasAt0 = false;
		}
		Subtract("Food", (population * pFoodConsuption) + (workers * wFoodConsumption) + (sTroops * sTFoodConsuption) + (ssTroops * ssTFoodConsuption));
		if(gFood <= 0){
			if(foodWasAt0){
				GameOver ("You have run out of food! Game over!");
			}else{
				PassErrorMessage ("If your food is still below 0 at the next game tick, the game will end!");
				foodWasAt0 = true;
			}
		}

		/*else {
			Subtract ("Food", gFood);
			//GameOver ("You have run out of food and one of your citizens has died! Game over!");
			allWorkers = GameObject.FindGameObjectsWithTag ("Worker");
			allSSTroops = GameObject.FindGameObjectsWithTag ("SSTroop");
			allShieldTroops = GameObject.FindGameObjectsWithTag ("ShieldTroop");
			if(allShieldTroops.Length > 0){
				currentWorker = GameObject.FindGameObjectWithTag ("ShieldTroop");
				Destroy (currentWorker);
				Subtract ("ShieldTroops", 1);
				Subtract ("Population", 1);
				someHouses = GameObject.FindGameObjectsWithTag ("House");
				foreach(GameObject currHouse in someHouses){
					if(currHouse.GetComponent <House>().population > 0){
						currHouse.GetComponent <House> ().population -= 1;
						break;
					}
				}
			}
			else if(allSSTroops.Length > 0){
				currentWorker = GameObject.FindGameObjectWithTag ("SSTroop");
				Destroy (currentWorker);
				Subtract ("SSTroops", 1);
				Subtract ("Population", 1);
				someHouses = GameObject.FindGameObjectsWithTag ("House");
				foreach(GameObject currHouse in someHouses){
					if(currHouse.GetComponent <House>().population > 0){
						currHouse.GetComponent <House> ().population -= 1;
						break;
					}
				}
			}
			else if(allWorkers.Length > 0){
				currentWorker = GameObject.FindGameObjectWithTag ("Worker");
				Destroy (currentWorker);
				Subtract ("Workers", 1);
				Subtract ("Population", 1);
				someHouses = GameObject.FindGameObjectsWithTag ("House");
				foreach(GameObject currHouse in someHouses){
					if(currHouse.GetComponent <House>().population > 0){
						currHouse.GetComponent <House> ().population -= 1;
						break;
					}
				}
			}else {
				Subtract ("Population", 1);
				someHouses = GameObject.FindGameObjectsWithTag ("House");
				foreach(GameObject currHouse in someHouses){
					if(currHouse.GetComponent <House>().population > 0){
						currHouse.GetComponent <House> ().population -= 1;
						break;
					}
				}
				if(population == 0){
					GameOver ("All of your citizens have died. Game over!");
				}
			}
			Subtract ("Happyness", 15);
		} */
	}

	/*public void HappynessCheck(){
		if(happyness == minHappyness){
			if(happynessCounter >= 3){
				GameOver ("Your towns happyness has been at it's lowest for far too long. The town has decided to revolt! Game over!");
			}else if(happynessCounter == 2){
				PassErrorMessage ("Your towns happyness has been at it's lowest for too long, if you do not imporve it, the town will revolt.");
			}
			happynessCounter++;
		}else{
			happynessCounter = 0;
		}
	} */

	public void GameOver(string msg){
		PassErrorMessage (msg);
		quitGame = true;
	}

}
