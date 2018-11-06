using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour {

	public int maxPopulation = 10, minTax, maxTax, population, forceTaxFee, forceTaxCycle = 20000, forceTaxFeeAmount = 10, buildingLevel, lvl1Pop, lvl2Pop, lvl3Pop;
	public int minCycleLength = 3000, maxCycleLength = 7000, cycleLength, taxCycle = 100, minTaxCycle = 2600, maxTaxCycle = 5000;//randomize the taxCycle
	public int upgrWood1, upgrRock1, upgrIron1, upgrGold1, upgrPoints1, upgrWood2, upgrRock2, upgrIron2, upgrGold2, upgrPoints2, upgrWood3, upgrRock3, upgrIron3, upgrGold3, upgrPoints3;
	public GameObject Coin, myCanvas, Upgr1, Upgr2, Upgr3, Placeholder;
	public bool collectTaxes = false;
	public Text cost;
	public AudioSource source;
	public AudioClip coinSound;

	private RaycastHit hit;
	private Vector3 place;
	private int x, t, f;
	private GameObject Master, cam, buildingInfo, miscBuild;
	private bool canForceTax;

	//IMPORTANT when any upgrade that involves getting more taxes is applied, then forceTaxFeeAmount has to increase as well

	void Awake(){
		myCanvas = gameObject.transform.Find ("Canvas").gameObject;
	}

	void Start () {
		if(gameObject.layer == 11){
			source.clip = coinSound;
			Master = GameObject.Find ("GameMaster");
			miscBuild = GameObject.Find ("GUIHUD");
			cam = GameObject.Find ("Main Camera");
			cycleLength = Random.Range (minCycleLength, maxCycleLength);
			Coin.SetActive (false);
			taxCycle = Random.Range (minTaxCycle, maxTaxCycle);
			canForceTax = false;
			buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
			buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Residents in this house: " + population + "/" + maxPopulation + "\r\n Building level: " + buildingLevel);
			myCanvas.SetActive (false);
		}
	}

	void Update () {
		if(gameObject.layer == 11 && gameObject.GetComponent <BuildHammer>().isBuilt){
			if(x >= cycleLength && population < maxPopulation){
				population += 1;
				//Master.GetComponent <GameMaster>().Add ("MaxPopulation", 1);
				Master.GetComponent <GameMaster>().Add ("Population", 1);
				cycleLength = Random.Range (minCycleLength, maxCycleLength);
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Residents in this house: " + population + "/" + maxPopulation + "\r\n Building level: " + buildingLevel);
				x = 0;
			}
			if(t >= taxCycle){
				TaxIt ();
				taxCycle = Random.Range (minTaxCycle, maxTaxCycle);
				t = 0;
			}
			if(f >= forceTaxCycle){
				canForceTax = true;
			}else{f++;}
			x++;
			t++;

		}
	}
	public void TaxIt(){
		if(gameObject.layer == 11 && gameObject.GetComponent <BuildHammer>().isBuilt){
			collectTaxes = true;
			Coin.SetActive (true);
			forceTaxFee = 0;
		}
	}
	public void CollectTax(){
		if(gameObject.layer == 11 && gameObject.GetComponent <BuildHammer>().isBuilt){
			if(collectTaxes){
				collectTaxes = false;
				Coin.SetActive (false);
				cam.GetComponent <PlayerStats>().Add ("Gold", (Random.Range (minTax, maxTax)) + population - forceTaxFee);
				source.Play ();
				forceTaxFee = 0;
				Master.GetComponent <GameMaster>().Subtract ("Happyness", 1);
			}else{
				Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, these people are not ready to pay their taxes yet.");
			}
		}
	}

	public void ForceTaxes(){
		if(gameObject.layer == 11 && gameObject.GetComponent <BuildHammer>().isBuilt){
			if(population > 0 && canForceTax){
				collectTaxes = true;
				Coin.SetActive (true);
				forceTaxFee += forceTaxFeeAmount;
				canForceTax = false;
				f = 0;
			}else{
				Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, it is too soon to do a forced taxation on the people in some of the houses.");
			}
		}
	}

	public int BankCollect(){
		if(gameObject.layer == 11 && gameObject.GetComponent <BuildHammer>().isBuilt){
			collectTaxes = false;
			Coin.SetActive (false);
			forceTaxFee = 0;
			Master.GetComponent <GameMaster>().Subtract ("Happyness", 1);
			return (Random.Range (minTax, maxTax)) + population - forceTaxFee;
		}else{
			return 0;
		}
	}

	public void UpgradeBuilding(){
		if (gameObject.GetComponent <BuildHammer> ().isBuilt) {
			if (buildingLevel == 0) {
				if (Master.GetComponent <GameMaster> ().gWood >= upgrWood1 && Master.GetComponent <GameMaster> ().gRock >= upgrRock1 && Master.GetComponent <GameMaster> ().gIron >= upgrIron1 && cam.GetComponent<PlayerStats> ().xpPoints >= upgrPoints1 && Master.GetComponent <GameMaster> ().gGold >= upgrGold1) {
					maxPopulation += lvl1Pop;
					Master.GetComponent <GameMaster> ().Add ("MaxPopulation", lvl1Pop);
					gameObject.GetComponent <BuildingHealth> ().health += 100;
					gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
					buildingLevel++;
					miscBuild.GetComponent <MiscBuildMenu> ().houseQueue++;
					miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
					Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood1);
					Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock1);
					Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock1);
					Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold1);
					cam.GetComponent <PlayerStats> ().xpPoints -= upgrPoints1;
					cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats> ().xpPoints, 0);
					buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Residents in this house: " + population + "/" + maxPopulation + "\r\n Building level: " + buildingLevel);
					cost.text = "Cost: \r\n\t Wood: " + upgrWood2 + "\r\n\t Rock: " + upgrRock2 + "\r\n\t Iron: " + upgrIron2 + "\r\n\t Gold: " + upgrGold2 + "\r\n\t Xp points: " + upgrPoints2 + "\r\n Building level: " + buildingLevel + "/3";
					Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
					CloseMenu ();
				} else {
					Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
				}
			} else if (buildingLevel == 1) {
				if (Master.GetComponent <GameMaster> ().gWood >= upgrWood2 && Master.GetComponent <GameMaster> ().gRock >= upgrRock2 && Master.GetComponent <GameMaster> ().gIron >= upgrIron2 && cam.GetComponent<PlayerStats> ().xpPoints >= upgrPoints2 && Master.GetComponent <GameMaster> ().gGold >= upgrGold2) {
					maxPopulation += lvl2Pop;
					Master.GetComponent <GameMaster> ().Add ("MaxPopulation", lvl2Pop);
					gameObject.GetComponent <BuildingHealth> ().health += 100;
					gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
					buildingLevel++;
					miscBuild.GetComponent <MiscBuildMenu> ().houseQueue++;
					miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
					Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood2);
					Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock2);
					Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock2);
					Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold2);
					cam.GetComponent <PlayerStats> ().xpPoints -= upgrPoints2;
					cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats> ().xpPoints, 0);
					cost.text = "Cost: \r\n\t Wood: " + upgrWood3 + "\r\n\t Rock: " + upgrRock3 + "\r\n\t Iron: " + upgrIron3 + "\r\n\t Gold: " + upgrGold3 + "\r\n\t Xp points: " + upgrPoints3 + "\r\n Building level: " + buildingLevel + "/3";
					buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Residents in this house: " + population + "/" + maxPopulation + "\r\n Building level: " + buildingLevel);
					Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
					CloseMenu ();
				} else {
					Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
				}
			} else if (buildingLevel == 2) {
				if (Master.GetComponent <GameMaster> ().gWood >= upgrWood3 && Master.GetComponent <GameMaster> ().gRock >= upgrRock3 && Master.GetComponent <GameMaster> ().gIron >= upgrIron3 && cam.GetComponent<PlayerStats> ().xpPoints >= upgrPoints3 && Master.GetComponent <GameMaster> ().gGold >= upgrGold3) {
					maxPopulation += lvl3Pop;
					Master.GetComponent <GameMaster> ().Add ("MaxPopulation", lvl3Pop);
					gameObject.GetComponent <BuildingHealth> ().health += 100;
					gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
					buildingLevel++;
					miscBuild.GetComponent <MiscBuildMenu> ().houseQueue++;
					miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
					Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood3);
					Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock3);
					Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock3);
					Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold3);
					cam.GetComponent <PlayerStats> ().xpPoints -= upgrPoints3;
					cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats> ().xpPoints, 0);
					cost.text = "You have upgraded this building to it's maximum. " + "\r\n Building level: " + buildingLevel + "/3";
					buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Residents in this house: " + population + "/" + maxPopulation + "\r\n Building level: " + buildingLevel);
					Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
					CloseMenu ();
				} else {
					Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
				}
			} else {
				Debug.Log ("Upgraded to the max!");
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You have upgraded this building to it's maximum.");
			}
		}
	}

	public void OpenMenu(){
		if(gameObject.layer == 11){
			if(gameObject.GetComponent <BuildHammer>().isBuilt){
				Master.GetComponent <GameMaster> ().DisablePlayer ();
				myCanvas.SetActive (true);
				GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (true);
			}
		}
	}
	public void CloseMenu(){
		if(gameObject.layer == 11){
			myCanvas.SetActive (false);
			Master.GetComponent <GameMaster>().EnablePlayer ();
			GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (false);
		}
	}

	public void BuildingDiedProtocol(){
		if(gameObject.layer == 11){//friendly building
			Master.GetComponent <GameMaster>().Subtract ("MaxPopulation", maxPopulation);
			Master.GetComponent <GameMaster>().Subtract ("Population", population);
			population = 0;
			maxPopulation = 0;
			CloseMenu ();
		}
	}

	public void DestroyBuilding(){
		CloseMenu ();
		BuildingDiedProtocol ();
		Destroy (gameObject, 0.1f);
	}

	public void Built(){
		population = 1;
		Master.GetComponent <GameMaster>().Add ("MaxPopulation", 10);
		Master.GetComponent <GameMaster>().Add ("Population", 1);
		buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Residents in this house: " + population + "/" + maxPopulation + "\r\n Building level: " + buildingLevel);
		Master.GetComponent <GameMaster>().Add ("Happyness", 2);
		//Master.GetComponent <GameMaster>().AddXpToPlayer (50);
		cost.text = "Cost: \r\n\t Wood: " + upgrWood1 + "\r\n\t Rock: " + upgrRock1 + "\r\n\t Iron: " + upgrIron1 + "\r\n\t Gold: " + upgrGold1 + "\r\n\t Xp points: " + upgrPoints1 + "\r\n Building level: "+ buildingLevel +"/3";
	}

}
