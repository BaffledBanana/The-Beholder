using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineShaft : MonoBehaviour {

	public int minRock, maxRock, minIron, maxIron, hired, maxHire, HWCost, HRCost, HICost, HFCost, HHCost, HGCost, FGCost, spdIncrCycle;
	public float speed;
	public int upgrWood1, upgrRock1, upgrIron1, upgrGold1, upgrPoints1, upgrWood2, upgrRock2, upgrIron2, upgrGold2, upgrPoints2, upgrWood3, upgrRock3, upgrIron3, upgrGold3, upgrPoints3;
	public GameObject ResMineLogPrefab;

	private Text cost;
	private GameObject buildingInfo, Master, myCanvas, cam, miscBuild;
	private int x, buildingLevel, amount;
	private Vector3 place;
	private RaycastHit hit;

	void Start(){
		Master = GameObject.Find ("GameMaster");
		buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
		miscBuild = GameObject.Find ("GUIHUD");
		myCanvas = gameObject.transform.Find ("Canvas").gameObject;
		myCanvas.SetActive (false);
		cam = GameObject.Find ("Main Camera");
		cost = myCanvas.transform.Find ("Cost").GetComponent <Text> ();
		cost.text = "Cost: \r\n\t Wood: " + upgrWood1 + "\r\n\t Rock: " + upgrRock1 + "\r\n\t Iron: " + upgrIron1 + "\r\n\t Gold: " + upgrGold1 + "\r\n\t Xp points: " + upgrPoints1 + "\r\n Building level: "+ buildingLevel +"/3";
		buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Hired workers: " + hired + "\r\n Building level: " + buildingLevel + "\r\n Mining speed: " + speed + "\r\n Average rock amount: " + ((int)((minRock + maxRock) / 2)*hired*speed) + "\r\n Average iron amount: " + ((int)((minIron + maxIron) / 2)*hired*speed));
	}

	public int MineRock(){
		if(x >= spdIncrCycle){
			speed += 0.1f;
			x = 0;
		}else{
			x++;	
		}
		buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Hired workers: " + hired + "\r\n Building level: " + buildingLevel + "\r\n Mining speed: " + speed + "\r\n Average rock amount: " + ((int)((minRock + maxRock) / 2)*hired*speed) + "\r\n Average iron amount: " + ((int)((minIron + maxIron) / 2)*hired*speed));
		amount = (int)(Random.Range (minRock, maxRock) * hired * speed);
		GameObject curr = Instantiate (ResMineLogPrefab, gameObject.transform.position + Vector3.up * 2, Quaternion.identity, gameObject.transform);
		curr.GetComponent <ResMineLog> ().Display ("Rock", amount, 0, 400); 
		return amount;
	}

	public int MineIron(){
		amount = (int)(Random.Range (minIron, maxIron) * hired * speed);
		GameObject curr = Instantiate (ResMineLogPrefab, gameObject.transform.position + Vector3.up * 2, Quaternion.identity, gameObject.transform);
		curr.GetComponent <ResMineLog> ().Display ("Iron", amount, 120, 400); 
		return amount;
	}

	public void Hire(){
		if(Master.GetComponent <GameMaster>().gWood >= HWCost && Master.GetComponent <GameMaster>().gRock >= HRCost && Master.GetComponent <GameMaster>().gIron >= HICost && Master.GetComponent <GameMaster>().gFood >= HFCost && Master.GetComponent <GameMaster>().gHide >= HHCost && Master.GetComponent <GameMaster>().gGold >= HGCost){
			if(Master.GetComponent <GameMaster>().population > 0 && Master.GetComponent <GameMaster>().workers < Master.GetComponent <GameMaster>().population && Master.GetComponent <GameMaster>().workers < Master.GetComponent <GameMaster>().maxPopulation){
				if(hired < maxHire){
					hired++;
					Master.GetComponent <GameMaster> ().Add ("Workers", 1);
					Master.GetComponent <GameMaster> ().Subtract ("Wood", HWCost);
					Master.GetComponent <GameMaster> ().Subtract ("Rock", HRCost);
					Master.GetComponent <GameMaster> ().Subtract ("Iron", HICost);
					Master.GetComponent <GameMaster> ().Subtract ("Food", HFCost);
					Master.GetComponent <GameMaster> ().Subtract ("Gold", HGCost);
					Master.GetComponent <GameMaster> ().Subtract ("Hide", HHCost);
					buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Hired workers: " + hired + "\r\n Building level: " + buildingLevel + "\r\n Mining speed: " + speed + "\r\n Average rock amount: " + ((int)((minRock + maxRock) / 2)*hired*speed) + "\r\n Average iron amount: " + ((int)((minIron + maxIron) / 2)*hired*speed));
				}else{
					Master.GetComponent <GameMaster>().PassErrorMessage ("This mine shaft is at maximum worker capacity.");
				}	
			}else{
				Master.GetComponent <GameMaster>().PassErrorMessage("You do not have enough unoccupied people in your town.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you do not have enough resources for this action.");
		}
	}

	public void Fire(){
		if(Master.GetComponent <GameMaster>().gGold >= FGCost){
			if(hired > 0){
				hired--;
				Master.GetComponent <GameMaster> ().Subtract ("Workers", 1);
				Master.GetComponent <GameMaster> ().Add ("Population", 1);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", FGCost);
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Hired workers: " + hired + "\r\n Building level: " + buildingLevel + "\r\n Mining speed: " + speed + "\r\n Average rock amount: " + ((int)((minRock + maxRock) / 2)*hired*speed) + "\r\n Average iron amount: " + ((int)((minIron + maxIron) / 2)*hired*speed));
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("There are no workers in this mine shaft.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you do not have enough resources for this action.");
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
		if(buildingLevel == 0){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood1 && Master.GetComponent <GameMaster>().gRock >= upgrRock1 && Master.GetComponent <GameMaster>().gIron >= upgrIron1 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints1 && Master.GetComponent <GameMaster>().gGold >= upgrGold1){
				maxHire += 5;
				maxIron += 5;
				maxRock += 5;
				minIron += 5;
				minRock += 5;
				speed += 0.2f;
				FGCost -= 3;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().MSQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood1);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold1);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints1;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "Cost: \r\n\t Wood: " + upgrWood2 + "\r\n\t Rock: " + upgrRock2 + "\r\n\t Iron: " + upgrIron2 + "\r\n\t Gold: " + upgrGold2 + "\r\n\t Xp points: " + upgrPoints2 + "\r\n Building level: "+ buildingLevel +"/3";
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Hired workers: " + hired + "\r\n Building level: " + buildingLevel + "\r\n Mining speed: " + speed + "\r\n Average rock amount: " + ((int)((minRock + maxRock) / 2)*hired*speed) + "\r\n Average iron amount: " + ((int)((minIron + maxIron) / 2)*hired*speed));
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}	else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 1){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood2 && Master.GetComponent <GameMaster>().gRock >= upgrRock2 && Master.GetComponent <GameMaster>().gIron >= upgrIron2 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints2 && Master.GetComponent <GameMaster>().gGold >= upgrGold2){
				maxHire += 5;
				maxIron += 5;
				maxRock += 5;
				minIron += 5;
				minRock += 5;
				speed += 0.2f;
				FGCost -= 3;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().MSQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood2);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold2);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints2;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "Cost: \r\n\t Wood: " + upgrWood3 + "\r\n\t Rock: " + upgrRock3 + "\r\n\t Iron: " + upgrIron3 + "\r\n\t Gold: " + upgrGold3 + "\r\n\t Xp points: " + upgrPoints3 + "\r\n Building level: "+ buildingLevel +"/3";
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Hired workers: " + hired + "\r\n Building level: " + buildingLevel + "\r\n Mining speed: " + speed + "\r\n Average rock amount: " + ((int)((minRock + maxRock) / 2)*hired*speed) + "\r\n Average iron amount: " + ((int)((minIron + maxIron) / 2)*hired*speed));			
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 2){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood3 && Master.GetComponent <GameMaster>().gRock >= upgrRock3 && Master.GetComponent <GameMaster>().gIron >= upgrIron3 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints3 && Master.GetComponent <GameMaster>().gGold >= upgrGold3){
				maxHire += 5;
				maxIron += 5;
				maxRock += 5;
				minIron += 5;
				minRock += 5;
				speed += 0.2f;
				FGCost -= 3;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().MSQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood3);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold3);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints3;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "You have upgraded this building to it's maximum. "+ "\r\n Building level: " + buildingLevel + "/3";
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Hired workers: " + hired + "\r\n Building level: " + buildingLevel + "\r\n Mining speed: " + speed + "\r\n Average rock amount: " + ((int)((minRock + maxRock) / 2)*hired*speed) + "\r\n Average iron amount: " + ((int)((minIron + maxIron) / 2)*hired*speed));
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
