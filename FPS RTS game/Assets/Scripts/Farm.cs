using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Farm : MonoBehaviour {


	private GameObject myCanvas, Master, buildingInfo, current;
	private RaycastHit hit;
	private Vector3 place, spotForResLogger;
	//private GameObject[] findPlots;
	private List<GameObject> plots;
	private bool canPlacePlot;
	private int i;

	public Text plotCost, upgrCost;
	public int hired, maxHire, min, max, HWCost, HRCost, HICost, HFCost, HHCost, maxPlots, buildingLevel, plotWCost, plotRCost,plotICost, plotFCost, plotHCost, plotGCost, upgrWCost, upgrRCost, upgrICost, upgrFCost, upgrGCost, upgrPCost;
	public float fertility, upOffsetPlot;
	public GameObject FarmWorker, spawnLocation, Plot, cam, Placeholder, ResMineLogPrefab;
	//IMPORTANT farm must contain a child called "Workers"
	//IMPORTANT all farms need a tag "Farm"

	void Start(){
		spotForResLogger = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + gameObject.transform.localScale.y, gameObject.transform.position.z);
		plots = new List<GameObject>(50);
		//findPlots = new GameObject[50];
		Master = GameObject.Find ("GameMaster");
		cam = GameObject.Find ("Main Camera");
		myCanvas = gameObject.transform.Find ("Canvas").gameObject;
		plotCost = myCanvas.transform.Find ("PlotCost").GetComponent <Text>();
		upgrCost = myCanvas.transform.Find ("UpgrCost").GetComponent <Text>();
		buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
		buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Workers in this farm: " + hired + "/" + maxHire + "\r\n Plots on this farm: " + plots.Count + "/" + maxPlots + "\r\n Building level: " + buildingLevel);
		Master.GetComponent <GameMaster>().Add ("Happyness", 1);
		//Master.GetComponent <GameMaster>().AddXpToPlayer (50);
		plotCost.text = "Cost: " + "\r\n Wood: " + plotWCost + "\r\n Rock: " + plotRCost + "\r\n Iron: " + plotICost + "\r\n Food: " + plotFCost + "\r\n Hide: " + plotHCost + "\r\n Gold: " + plotGCost;
		upgrCost.text = "Cost: " + "\r\n Wood: " + upgrWCost + "\r\n Rock: " + upgrRCost + "\r\n Iron: " + upgrICost + "\r\n Food: " + upgrFCost + "\r\n Gold: " + upgrGCost + "\r\n Xp points: " + upgrPCost;
		CloseMenu ();
	}

	public int AddFood(){
		if (gameObject.GetComponent <BuildHammer> ().isBuilt) {
			int amount = (int)Math.Round ((Master.GetComponent <GameMaster> ().GenerateRandom (min, max) + (hired * 2.71828f) * plots.Count * fertility), 0);
			GameObject curr = Instantiate (ResMineLogPrefab, spotForResLogger, Quaternion.identity, gameObject.transform);
			curr.GetComponent <ResMineLog> ().Display ("Food", amount, 0, 400);
			return amount;
		}else {
			return 0;
		}
	}
	public void Hire(){
		if(hired < maxHire && Master.GetComponent <GameMaster>().population > 0 && Master.GetComponent <GameMaster>().workers < Master.GetComponent <GameMaster>().population){
			if(Master.GetComponent <GameMaster>().gWood >= HWCost && Master.GetComponent <GameMaster>().gRock >= HRCost && Master.GetComponent <GameMaster>().gIron >= HICost && Master.GetComponent <GameMaster>().gFood >= HFCost && Master.GetComponent <GameMaster>().gHide >= HHCost){
				hired++;
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Workers in this farm: " + hired + "/" + maxHire + "\r\n Plots on this farm: " + plots.Count + "/" + maxPlots + "\r\n Building level: " + buildingLevel);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", HRCost);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", HICost);
				Master.GetComponent <GameMaster> ().Subtract ("Food", HFCost);
				Master.GetComponent <GameMaster> ().Subtract ("Hide", HHCost);
				//Master.GetComponent <GameMaster> ().Subtract ("Population", 1);
				Master.GetComponent <GameMaster> ().Add ("Workers", 1);
				Instantiate (FarmWorker, spawnLocation.transform.position, Quaternion.identity, gameObject.transform.Find ("Workers"));
			}else{
				Debug.Log ("Farm: Not enough res");
				Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, you do not have enough resources to hire a farmer.");
			}
		}else{
			Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, you do not have enough unoccupied citizens to hire a farmer. Or you have reached the maximum amount of farmers to hire on this farm.");
		}
	}

	public void Fire(){
		if(gameObject.transform.Find ("FarmWorker") != null){
			Destroy (gameObject.transform.Find ("FarmWorker"));
			hired--;
		buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Workers in this farm: " + hired + "/" + maxHire + "\r\n Plots on this farm: " + plots.Count + "/" + maxPlots + "\r\n Building level: " + buildingLevel);
		}else{
			Debug.Log ("Farm: No farm workers to fire");
		}
	}

	public void PurchasePlot(){
		if(plots.Count < maxPlots && Master.GetComponent <GameMaster>().gWood >= plotWCost && Master.GetComponent <GameMaster>().gRock >= plotRCost && Master.GetComponent <GameMaster>().gIron >= plotICost && Master.GetComponent <GameMaster>().gFood >= plotFCost && Master.GetComponent <GameMaster>().gHide >= plotHCost && Master.GetComponent <GameMaster>().gGold >= plotGCost){
			canPlacePlot = true;
			fertility -= 0.1f;
			CloseMenu ();
			Master.GetComponent <GameMaster> ().Subtract ("Wood", plotWCost);
			Master.GetComponent <GameMaster> ().Subtract ("Rock", plotRCost);
			Master.GetComponent <GameMaster> ().Subtract ("Iron", plotICost);
			Master.GetComponent <GameMaster> ().Subtract ("Food", plotFCost);
			Master.GetComponent <GameMaster> ().Subtract ("Hide", plotHCost);
			Master.GetComponent <GameMaster> ().Subtract ("Gold", plotGCost);
		buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Workers in this farm: " + hired + "/" + maxHire + "\r\n Plots on this farm: " + plots.Count + "/" + maxPlots + "\r\n Building level: " + buildingLevel);
		}else{
			Debug.Log ("Not enough res or max plots");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you do not have enough resources for this or you have reached maximum plot amount. Try upgrading the farm.");
		}
	}

	public void UpgradeBuilding(){
		if(cam.GetComponent <PlayerStats>().xpPoints >= upgrPCost && Master.GetComponent <GameMaster>().gWood >= upgrWCost && Master.GetComponent <GameMaster>().gRock >= upgrRCost && Master.GetComponent <GameMaster>().gIron >= upgrICost && Master.GetComponent <GameMaster>().gFood >= upgrFCost && Master.GetComponent <GameMaster>().gGold >= upgrGCost){
			Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWCost);
			Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRCost);
			Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrICost);
			Master.GetComponent <GameMaster> ().Subtract ("Food", upgrFCost);
			Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGCost);
			//cam.GetComponent <PlayerStats> ().xpPoints -= upgrPCost;
			cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats> ().xpPoints, 0);
			gameObject.GetComponent <BuildingHealth> ().health += 100;
			gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
			buildingLevel++;
			fertility += 0.1f;
			upgrWCost = (int)(upgrWCost*1.61803f); // (1.30357) λ Conway's constant
			upgrRCost = (int)(upgrRCost * 1.41421f); // (1.41421) square root of 2
			upgrICost = (int)(upgrICost*1.61803f); // (1.61803) golden ratio
			upgrFCost = (int)(upgrFCost*1.30357f); //(1.20205) Apéry's constant
			upgrGCost = (int)(upgrGCost*1.20205f); //(1.09868) Lengyel's constant
			upgrPCost = (int)(upgrPCost*1.30357f); //(2.71828) eulers number
			maxPlots += 2;
			maxHire += 2;
			buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Workers in this farm: " + hired + "/" + maxHire + "\r\n Plots on this farm: " + plots.Count + "/" + maxPlots + "\r\n Building level: " + buildingLevel);
			upgrCost.text = "Cost: " + "\r\n Wood: " + upgrWCost + "\r\n Rock: " + upgrRCost + "\r\n Iron: " + upgrICost + "\r\n Food: " + upgrFCost + "\r\n Gold: " + upgrGCost + "\r\n Xp points: " + upgrPCost;
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Please place your building upgrade!");
			CloseMenu ();
		}else{
			Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, you do not have enough reources for this action.");
		}
	}

	public void OpenMenu(){
		if(canPlacePlot == false && gameObject.GetComponent <BuildHammer>().isBuilt){
			if(gameObject.GetComponent <BuildHammer>().isBuilt){
				Master.GetComponent <GameMaster> ().DisablePlayer ();
				myCanvas.SetActive (true);
				GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (true);
			}
		}
	}
	public void CloseMenu(){
		myCanvas.SetActive (false);
		Master.GetComponent <GameMaster>().EnablePlayer ();
		GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (false);
	}

	void Update(){
		if(canPlacePlot && gameObject.GetComponent <BuildHammer>().isBuilt){
			Debug.Log ("Place your plot");
			if(GameObject.Find ("Placeholder") != null){//CHANGE
				Destroy (GameObject.Find ("Placeholder"));//CHANGE
			}
			if(Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, 15)){
				place = hit.point;
				place.y += upOffsetPlot;//CHANGE
			}
			if(Input.GetKeyDown (KeyCode.Mouse1) && hit.collider.CompareTag ("Ground")){
				current = Instantiate (Plot, place, Quaternion.identity, gameObject.transform);	//CHANGE
				canPlacePlot = false;
				plots.Add (current);
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Workers in this farm: " + hired + "/" + maxHire + "\r\n Plots on this farm: " + plots.Count + "/" + maxPlots + "\r\n Building level: " + buildingLevel);
			}
			else if(hit.collider != null && hit.collider.CompareTag ("Ground")){
				GameObject current = Instantiate (Placeholder, place, Quaternion.identity, gameObject.transform);//CHANGE
				current.name = "Placeholder";//CHANGE
			}
		}
	}

	public void BuildingDiedProtocol(){
		if(hired > 0){
			Master.GetComponent <GameMaster> ().Subtract ("Workers", hired);
			Master.GetComponent <GameMaster> ().Add ("Population", hired);
		}
		hired = 0;
	}

	public void DestroyBuilding(){
		CloseMenu ();
		BuildingDiedProtocol ();
		Destroy (gameObject, 0.1f);
	}

}
