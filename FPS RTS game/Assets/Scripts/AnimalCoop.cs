using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalCoop : MonoBehaviour {

	public List<GameObject> anim;
	public int aGCost, aICost, aFCost, hired, hireGCost, hireWCost, hireRCost, hireICost, maxHire, maxAnimals;
	public GameObject coopAnimal, ResMineLogPrefab;
	public float efficiency;
	public int upgrWood1, upgrRock1, upgrIron1, upgrGold1, upgrPoints1, upgrWood2, upgrRock2, upgrIron2, upgrGold2, upgrPoints2, upgrWood3, upgrRock3, upgrIron3, upgrGold3, upgrPoints3;

	private Vector3 place, spotForResLogger;
	private RaycastHit hit;
	private Text cost;
	private GameObject Master, cam, buildingInfo, myCanvas, miscBuild;
	private Transform SpawnLoc;
	private int buildingLevel;

	void Start(){
		spotForResLogger = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + gameObject.transform.localScale.y, gameObject.transform.position.z);
		anim = new List<GameObject> ();
		Master = GameObject.Find ("GameMaster");
		cam = GameObject.Find ("Main Camera");
		myCanvas = gameObject.transform.Find ("Canvas").gameObject;
		miscBuild = GameObject.Find ("GUIHUD");
		cost = myCanvas.transform.Find ("Cost").GetComponent <Text>();
		myCanvas.SetActive (false);
		SpawnLoc = gameObject.transform.Find ("SpawnLocation");
		buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
		buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Animals: " + anim.Count + "/(max)" + maxAnimals + "\r\n Hired: " + hired + "/(max)" + maxHire + "\r\n Efficiency: " + efficiency + "\r\n Building level: " + buildingLevel);
		cost.text = "Cost: \r\n\t Wood: " + upgrWood1 + "\r\n\t Rock: " + upgrRock1 + "\r\n\t Iron: " + upgrIron1 + "\r\n\t Gold: " + upgrGold1 + "\r\n\t Xp points: " + upgrPoints1 + "\r\n Building level: "+ buildingLevel +"/3";
	}

	public void AddToList(GameObject toAdd){
		if(anim.Count < maxAnimals  && gameObject.GetComponent <BuildHammer>().isBuilt){
			anim.Add (toAdd);
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Animals: " + anim.Count + "/(max)" + maxAnimals + "\r\n Hired: " + hired + "/(max)" + maxHire + "\r\n Efficiency: " + efficiency + "\r\n Building level: " + buildingLevel);

		}else{
			Destroy (toAdd);
		}
	}

	public void Died(GameObject toDelete, int amount){
		AddHide (amount);
		Debug.Log ("Added hide and got removed from list");
		anim.Remove (toDelete);
		buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Animals: " + anim.Count + "/(max)" + maxAnimals + "\r\n Hired: " + hired + "/(max)" + maxHire + "\r\n Efficiency: " + efficiency + "\r\n Building level: " + buildingLevel);

	}

	public void AddHide(int amount){
		if(Master.GetComponent <GameMaster> ().CalculateStorage () + (int)(amount * hired * efficiency) <= Master.GetComponent <GameMaster> ().storage  && gameObject.GetComponent <BuildHammer>().isBuilt){
			Master.GetComponent <GameMaster> ().Add ("Hide", (int)(amount * hired * efficiency));
			GameObject curr = Instantiate (ResMineLogPrefab, spotForResLogger, Quaternion.identity, gameObject.transform);
			curr.GetComponent <ResMineLog>().Display ("Hide", (int)(amount * hired * efficiency), 0, 400);
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Your storage buildings are full!");
		}
	}

	public void BuyAnimal(){
		if (cam.GetComponent <PlayerStats> ().pGold >= aGCost && cam.GetComponent <PlayerStats> ().pFood >= aFCost  && gameObject.GetComponent <BuildHammer>().isBuilt) {
			GameObject curr = Instantiate (coopAnimal, SpawnLoc.position, Quaternion.identity, gameObject.transform.Find ("Animals"));
			anim.Add (curr);
			cam.GetComponent <PlayerStats> ().pGold -= aGCost;
			cam.GetComponent <PlayerStats> ().pFood -= aFCost;
			cam.GetComponent <PLayerUIHandler>().UpdateText ("Food", cam.GetComponent <PlayerStats> ().pFood, cam.GetComponent <PlayerStats> ().maxFood);
			cam.GetComponent <PLayerUIHandler>().UpdateText ("Gold", cam.GetComponent <PlayerStats> ().pGold, cam.GetComponent <PlayerStats> ().maxGold);
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Animals: " + anim.Count + "/(max)" + maxAnimals + "\r\n Hired: " + hired + "/(max)" + maxHire + "\r\n Efficiency: " + efficiency + "\r\n Building level: " + buildingLevel);
		}else{
			Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have enough of some resource.");
		}
	}

	public void Hire(){
		if(cam.GetComponent <PlayerStats> ().pWood >= hireWCost && cam.GetComponent <PlayerStats> ().pGold >= hireGCost && cam.GetComponent <PlayerStats> ().pRock >= hireRCost && cam.GetComponent <PlayerStats> ().pIron >= hireICost && Master.GetComponent <GameMaster>().workers < Master.GetComponent <GameMaster>().population && hired < maxHire  && gameObject.GetComponent <BuildHammer>().isBuilt){
			cam.GetComponent <PlayerStats> ().pGold -= hireGCost;
			cam.GetComponent <PlayerStats> ().pIron -= hireICost;
			cam.GetComponent <PlayerStats> ().pRock -= hireRCost;
			cam.GetComponent <PlayerStats> ().pWood -= hireWCost;
			cam.GetComponent <PLayerUIHandler>().UpdateText ("Rock", cam.GetComponent <PlayerStats> ().pRock, cam.GetComponent <PlayerStats> ().maxRock);
			cam.GetComponent <PLayerUIHandler>().UpdateText ("Gold", cam.GetComponent <PlayerStats> ().pGold, cam.GetComponent <PlayerStats> ().maxGold);
			cam.GetComponent <PLayerUIHandler>().UpdateText ("Iron", cam.GetComponent <PlayerStats> ().pIron, cam.GetComponent <PlayerStats> ().maxIron);
			cam.GetComponent <PLayerUIHandler>().UpdateText ("Wood", cam.GetComponent <PlayerStats> ().pWood, cam.GetComponent <PlayerStats> ().maxWood);
			Master.GetComponent <GameMaster> ().Add ("Workers", 1);
			hired++;
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Animals: " + anim.Count + "/(max)" + maxAnimals + "\r\n Hired: " + hired + "/(max)" + maxHire + "\r\n Efficiency: " + efficiency + "\r\n Building level: " + buildingLevel);

		}else{
			Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have enough of some resource.");
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
		Master.GetComponent <GameMaster> ().EnablePlayer ();
		GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (false);
	}

	public void UpgradeBuilding(){
		if(buildingLevel == 0  && gameObject.GetComponent <BuildHammer>().isBuilt){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood1 && Master.GetComponent <GameMaster>().gRock >= upgrRock1 && Master.GetComponent <GameMaster>().gIron >= upgrIron1 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints1 && Master.GetComponent <GameMaster>().gGold >= upgrGold1){
				maxHire += 3;
				maxAnimals += 3;
				efficiency = 0.8f;
				//foreach of the list do a function that increases min and max hide on each of the animals + other upgrades
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood1);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold1);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints1;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Animals: " + anim.Count + "/(max)" + maxAnimals + "\r\n Hired: " + hired + "/(max)" + maxHire + "\r\n Efficiency: " + efficiency + "\r\n Building level: " + buildingLevel);
				cost.text = "Cost: \r\n\t Wood: " + upgrWood2 + "\r\n\t Rock: " + upgrRock2 + "\r\n\t Iron: " + upgrIron2 + "\r\n\t Gold: " + upgrGold2 + "\r\n\t Xp points: " + upgrPoints2 + "\r\n Building level: "+ buildingLevel +"/3";
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				miscBuild.GetComponent <MiscBuildMenu> ().ACQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				CloseMenu ();
			}	else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 1){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood2 && Master.GetComponent <GameMaster>().gRock >= upgrRock2 && Master.GetComponent <GameMaster>().gIron >= upgrIron2 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints2 && Master.GetComponent <GameMaster>().gGold >= upgrGold2){
				maxHire += 3;
				maxAnimals += 3;
				efficiency = 1f;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood2);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold2);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints2;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "Cost: \r\n\t Wood: " + upgrWood3 + "\r\n\t Rock: " + upgrRock3 + "\r\n\t Iron: " + upgrIron3 + "\r\n\t Gold: " + upgrGold3 + "\r\n\t Xp points: " + upgrPoints3 + "\r\n Building level: "+ buildingLevel +"/3";
				buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Animals: " + anim.Count + "/(max)" + maxAnimals + "\r\n Hired: " + hired + "/(max)" + maxHire + "\r\n Efficiency: " + efficiency + "\r\n Building level: " + buildingLevel);
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				miscBuild.GetComponent <MiscBuildMenu> ().ACQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				CloseMenu ();
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 2){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood3 && Master.GetComponent <GameMaster>().gRock >= upgrRock3 && Master.GetComponent <GameMaster>().gIron >= upgrIron3 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints3 && Master.GetComponent <GameMaster>().gGold >= upgrGold3){
				maxHire += 3;
				maxAnimals += 3;
				efficiency = 1.2f;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood3);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold3);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints3;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "You have upgraded this building to it's maximum. "+ "\r\n Building level: " + buildingLevel + "/3";
				buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Animals: " + anim.Count + "/(max)" + maxAnimals + "\r\n Hired: " + hired + "/(max)" + maxHire + "\r\n Efficiency: " + efficiency + "\r\n Building level: " + buildingLevel);
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				miscBuild.GetComponent <MiscBuildMenu> ().ACQueue++;
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

	public void BuildingDiedProtocol(){
		if(hired > 0){
			Master.GetComponent <GameMaster> ().Subtract ("Workers", hired);
			Master.GetComponent <GameMaster> ().Add ("Population", hired);
		}
		foreach(GameObject animal in anim){
			animal.GetComponent <Animal>().TakeDmg (1000, null);
		}
		hired = 0;
	}

	public void DestroyBuilding(){
		CloseMenu ();
		BuildingDiedProtocol ();
		Destroy (gameObject, 0.1f);
	}

}
