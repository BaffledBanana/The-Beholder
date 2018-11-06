using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingCamp : MonoBehaviour {

	public float addMovementSpeed;
	public int addHealth, addStrength, addSwingSpeed, HIncrICost, HIncrHCost, HIncrGCost, SIncrICost, SIncrFCost, SIncrGCost, MSIncrHCost, MSIncrGCost, SSIncrFCost, SSIncrGCost, RIncrWCost, RIncrHCost, RIncrGCost, trainTime;

	private Text HCost, MSCost, SCost, SSCost; //TODO make these ^ costs go up and display the new ones with these texts
	private List<GameObject> troops, archers;
	private GameObject[] troopArray, archerArray;
	private GameObject Master, myCanvas, loadingBar, buildingInfo;
	private int x;
	private float y, maxTimer;
	private bool upgrHealth, upgrStrength, upgrMS, upgrSS, upgrRange, load;

	void Start(){
		troops = new List<GameObject> ();
		archers = new List<GameObject> ();
		Master = GameObject.Find ("GameMaster");
		myCanvas = gameObject.transform.Find ("Canvas").gameObject;
		HCost = myCanvas.transform.Find ("HCost").GetComponent <Text> ();
		MSCost = myCanvas.transform.Find ("MSCost").GetComponent <Text> ();
		SCost = myCanvas.transform.Find ("SCost").GetComponent <Text> ();
		SSCost = myCanvas.transform.Find ("SSCost").GetComponent <Text> ();
		loadingBar = myCanvas.transform.Find ("LoadingBar").gameObject;
		buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
		buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Waiting...");
		maxTimer = trainTime;
		loadingBar.transform.localScale = new Vector3 (0, 1, 1);
		myCanvas.SetActive (false);
		Calculate ();
	}

	void Update(){
		if(load){
			if(x >= trainTime){
				if(upgrHealth){
					foreach(GameObject troop in troops){
						troop.SetActive (true);
						troop.GetComponent <Troop>().maxHealth += addHealth;
						troop.GetComponent <Troop> ().health += addHealth;
					}
					upgrHealth = false;
					load = false;
					loadingBar.transform.localScale = new Vector3 (0, 1, 1);
					buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops and archers being trained: " + (troops.Count + archers.Count) + "\r\n Progress: " + (y/maxTimer) + "%");

				}else if(upgrStrength){
					foreach(GameObject troop in troops){
						troop.SetActive (true);
						troop.GetComponent <Troop>().minStrength += addStrength;
						troop.GetComponent <Troop> ().maxStrength += addStrength;
					}
					upgrStrength = false;
					load = false;
					loadingBar.transform.localScale = new Vector3 (0, 1, 1);
					buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops and archers being trained: " + (troops.Count + archers.Count) + "\r\n Progress: " + (y/maxTimer) + "%");

				}else if(upgrMS){
					foreach(GameObject troop in troops){
						troop.SetActive (true);
						troop.GetComponent <Troop>().step += addMovementSpeed;
					}
					upgrMS = false;
					load = false;
					loadingBar.transform.localScale = new Vector3 (0, 1, 1);
					buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops and archers being trained: " + (troops.Count + archers.Count) + "\r\n Progress: " + (y/maxTimer) + "%");


				}else if(upgrSS){
					foreach(GameObject troop in troops){
						troop.SetActive (true);
						if(troop.GetComponent <Troop>().attackTimer - addSwingSpeed > 0){
							troop.GetComponent <Troop>().attackTimer -= addSwingSpeed;
						}else{
							troop.GetComponent <Troop>().attackTimer = 0;
							Debug.Log ("Max swing speed reached");
							Master.GetComponent <GameMaster>().PassErrorMessage ("You have reached the maximum attack speed of your military.");
						}
					}
					upgrSS = false;
					load = false;
					loadingBar.transform.localScale = new Vector3 (0, 1, 1);
					buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops and archers being trained: " + (troops.Count + archers.Count) + "\r\n Progress: " + (y/maxTimer) + "%");

				}else if(upgrRange){

					load = false;
					upgrRange = false;
					loadingBar.transform.localScale = new Vector3 (0, 1, 1);
					buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops and archers being trained: " + (troops.Count + archers.Count) + "\r\n Progress: " + (y/maxTimer) + "%");

				}
				x = 0;
			}else{
				x++;
				y = x;
				loadingBar.transform.localScale = new Vector3 (y/maxTimer, 1, 1);
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops and archers being trained: " + (troops.Count + archers.Count) + "\r\n Progress: " + (int)((y / maxTimer) * 100) + "%");
			} 
		}
	}

	public void UpgradeHealth(){
		FindTroops ();
		if(troops.Count > 0){
			if(Master.GetComponent <GameMaster>().gIron >= HIncrICost * troops.Count && Master.GetComponent <GameMaster>().gHide >= HIncrHCost * troops.Count && Master.GetComponent <GameMaster>().gGold >= HIncrGCost * troops.Count){
				Master.GetComponent <GameMaster> ().Subtract ("Iron", HIncrICost * troops.Count);
				Master.GetComponent <GameMaster> ().Subtract ("Hide", HIncrHCost * troops.Count);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", HIncrGCost * troops.Count);
				upgrHealth = true;
				load = true;
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops and archers being trained: " + (troops.Count + archers.Count) + "\r\n Progress: " + (y/maxTimer) + "%");
				foreach(GameObject troop in troops){
					troop.SetActive (false);
				}
			}else{
				Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have enough resources to upgrade your units.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You dont have any troops.");
		}
	}

	public void UpgradeStrength(){
		FindTroops ();
		if(troops.Count > 0){
			if(Master.GetComponent <GameMaster>().gIron >= SIncrICost * troops.Count && Master.GetComponent <GameMaster>().gFood >= SIncrFCost * troops.Count && Master.GetComponent <GameMaster>().gGold >= SIncrGCost * troops.Count){
				Master.GetComponent <GameMaster> ().Subtract ("Iron", SIncrICost * troops.Count);
				Master.GetComponent <GameMaster> ().Subtract ("Food", SIncrFCost * troops.Count);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", SIncrGCost * troops.Count);
				upgrStrength = true;
				load = true;
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops and archers being trained: " + (troops.Count + archers.Count) + "\r\n Progress: " + (y/maxTimer) + "%");

				foreach(GameObject troop in troops){
					troop.SetActive (false);
				}
			}else{
				Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have enough resources to upgrade your units.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You dont have any troops.");

		}
	}

	public void UpgradeMovementSpeed(){
		FindTroops ();
		if(troops.Count > 0){
			if(Master.GetComponent <GameMaster>().gHide >= MSIncrHCost * troops.Count && Master.GetComponent <GameMaster>().gGold >= MSIncrGCost * troops.Count){
				Master.GetComponent <GameMaster> ().Subtract ("Hide", MSIncrHCost * troops.Count);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", MSIncrGCost * troops.Count);
				upgrMS = true;
				load = true;
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops and archers being trained: " + (troops.Count + archers.Count) + "\r\n Progress: " + (y/maxTimer) + "%");

				foreach(GameObject troop in troops){
					troop.SetActive (false);
				}
			}else{
				Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have enough resources to upgrade your units.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You dont have any troops.");

		}
	}

	public void UpgradeSwingSpeed(){
		FindTroops ();
		if(troops.Count > 0){
			if(Master.GetComponent <GameMaster>().gFood >= SSIncrFCost * troops.Count && Master.GetComponent <GameMaster>().gGold >= SSIncrGCost * troops.Count){
				Master.GetComponent <GameMaster> ().Subtract ("Food", SSIncrFCost * troops.Count);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", SSIncrGCost * troops.Count);
				upgrSS = true;
				load = true;
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops and archers being trained: " + (troops.Count + archers.Count) + "\r\n Progress: " + (y/maxTimer) + "%");

				foreach(GameObject troop in troops){
					troop.SetActive (false);
				}
			}else{
				Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have enough resources to upgrade your units.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You dont have any troops.");

		}
	}

	public void UpgradeRange(){
		FindArchers ();
		//for archers
		if(archers.Count > 0){
			if(Master.GetComponent <GameMaster>().gWood >= RIncrWCost * archers.Count && Master.GetComponent <GameMaster>().gGold >= RIncrGCost * archers.Count && Master.GetComponent <GameMaster>().gHide >= RIncrHCost * archers.Count){
				Master.GetComponent <GameMaster> ().Subtract ("Wood", RIncrWCost * archers.Count);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", RIncrGCost * archers.Count);
				Master.GetComponent <GameMaster> ().Subtract ("Hide", RIncrHCost * archers.Count);
				upgrRange = true;
				load = true;
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops and archers being trained: " + (troops.Count + archers.Count) + "\r\n Progress: " + (y/maxTimer) + "%");

				foreach(GameObject archer in archers){
					archer.SetActive (false);
				}
			}else{
				Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have enough resources to upgrade your units.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You dont have any archers.");

		}
	}

	void FindTroops(){
		troops.Clear ();
		troopArray = GameObject.FindGameObjectsWithTag ("SSTroop");
		foreach(GameObject troop in troopArray){
			if(troop.layer == 8){//friendly
				troops.Add (troop);
			}
		}
		troopArray = GameObject.FindGameObjectsWithTag ("ShieldTroop");
		foreach(GameObject troop in troopArray){
			if(troop.layer == 8){//friendly
				troops.Add (troop);
			}
		}
	}

	void FindArchers(){
		archers.Clear ();
		archerArray = GameObject.FindGameObjectsWithTag ("Archer");
		foreach(GameObject archer in archerArray){
			if(archer.layer == 8){//friendly
				archers.Add (archer);
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
		GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (true);
	}

	public void Calculate(){
		FindTroops ();
		//FindArchers ();
		HCost.text = "Cost: \r\n Iron: " + HIncrICost * troops.Count + "\r\n Hide: " + HIncrHCost * troops.Count + "\r\n Gold: " + HIncrGCost * troops.Count;
		SCost.text = "Cost: \r\n Iron: " + SIncrICost * troops.Count + "\r\n Food: " + SIncrFCost * troops.Count + "\r\n Gold: " + SIncrGCost * troops.Count;
		MSCost.text = "Cost: \r\n Hide: " + MSIncrHCost * troops.Count + "\r\n Gold: " + MSIncrGCost * troops.Count;
		SSCost.text = "Cost: \r\n Food: " + SSIncrFCost * troops.Count + "\r\n Gold: " + SSIncrGCost * troops.Count;

	}

	public void BuildingDiedProtocol(){
		foreach(GameObject troop in troops){
			troop.SetActive (true);
		}
		load = false;
		upgrRange = false;
		upgrMS = false;
		upgrHealth = false;
		upgrSS = false;
		upgrStrength = false;
		loadingBar.transform.localScale = new Vector3 (0, 1, 1);
		buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Troops and archers being trained: " + (troops.Count + archers.Count) + "\r\n Progress: " + (y/maxTimer) + "%");
	}

	public void DestroyBuilding(){
		CloseMenu ();
		BuildingDiedProtocol ();
		Destroy (gameObject, 0.1f);
	}

}
