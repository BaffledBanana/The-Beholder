using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blacksmith : MonoBehaviour {

	public int steel, woodPerIron = 3, warmOvenTime, loadingIncrement, loadTime, ironPerSteel = 2, iron, wood, buildingLevel, armorFixCost, armorFixAmount, armorUpgrCost, armorUpgrAmount, steelCapacity, swordUpgrCost;
	public List<GameObject> swords;
	public int upgrWood1, upgrRock1, upgrIron1, upgrGold1, upgrPoints1, upgrWood2, upgrRock2, upgrIron2, upgrGold2, upgrPoints2, upgrWood3, upgrRock3, upgrIron3, upgrGold3, upgrPoints3;

	private Text loadingText, cost, status;
	private bool warmOven, readyToSmelt, recycled;
	private int wo, rts, scaleX, i = 0, x;
	private GameObject Master, cam, buildingInfo, myCanvas, loadingBar, miscBuild;
	private RaycastHit hit;
	private Vector3 place;

	// Use this for initialization
	void Start () {
		Master = GameObject.Find ("GameMaster");
		cam = GameObject.Find ("Main Camera");
		miscBuild = GameObject.Find ("GUIBUG");
		buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
		myCanvas = gameObject.transform.Find ("Canvas").gameObject;
		myCanvas.SetActive (false);
		loadingBar = myCanvas.transform.Find ("LoadingBar").gameObject;
		loadingText = myCanvas.transform.Find ("LoadingText").GetComponent <Text> ();
		loadingBar.transform.localScale = new Vector3 (0, 1, 1);
		status = myCanvas.transform.Find ("Status").GetComponent <Text> ();
		swords = new List<GameObject>(10);
		status.text = "Waiting...";
		cost = myCanvas.transform.Find ("Cost").GetComponent <Text> ();
		cost.text = "Cost: \r\n\t Wood: " + upgrWood2 + "\r\n\t Rock: " + upgrRock2 + "\r\n\t Iron: " + upgrIron2 + "\r\n\t Gold: " + upgrGold2 + "\r\n\t Xp points: " + upgrPoints2 + "\r\n Building level: "+ buildingLevel +"/3";
		buildingInfo.GetComponent <BuildingInformation>().UpdateText (status.text + "\r\n Building level: " + buildingLevel);
	}
	
	// Update is called once per frame
	void Update () {
		if(warmOven){
			if(wo >= warmOvenTime){
				warmOven = false;
				readyToSmelt = true;
				status.text = "Oven is warm... \r\n Wood: " + wood + "\r\n Iron: " + iron + "\r\n Steel: " + steel + "/(max)" + steelCapacity;
				buildingInfo.GetComponent <BuildingInformation>().UpdateText (status.text + "\r\n Building level: " + buildingLevel);
			}else{
				wo++;
				status.text = "Warming oven... \r\n Wood: " + wood + "\r\n Iron: " + iron + "\r\n Steel: " + steel + "/(max)" + steelCapacity;
				buildingInfo.GetComponent <BuildingInformation>().UpdateText (status.text + "\r\n Building level: " + buildingLevel);
			}
		}
		if(readyToSmelt){ 															//if the oven is warm
			if(steel < steelCapacity){												//if there is enough space for more steel
				if(wood - woodPerIron >= 0){										//if enough wood left
					if(iron - ironPerSteel >= 0){									//if enough iron left
						if(scaleX + loadingIncrement < 100){						//if loading bar is not 100%
							if(rts >= loadTime){									//if an increment should be done on the loading bar
								scaleX += loadingIncrement;
								loadingBar.transform.localScale = new Vector3 (scaleX/100f, 1, 1);
								loadingText.text = scaleX + " %";
								status.text = "Smelting iron... \r\n Wood: " + wood + "\r\n Iron: " + iron + "\r\n Steel: " + steel + "/(max)" + steelCapacity;
								buildingInfo.GetComponent <BuildingInformation>().UpdateText (status.text + "\r\n Building level: " + buildingLevel);
								rts = 0;
							}else{
								rts++;
								//status.text = "Smelting iron... \r\n Wood: " + wood + "\r\n Iron: " + iron + "\r\n Steel: " + steel;
								//buildingInfo.GetComponent <BuildingInformation>().UpdateText (status.text + "\r\n Building level: " + buildingLevel);
							}
						}else{
							steel++;
							iron -= ironPerSteel;
							wood -= woodPerIron * ironPerSteel;
							scaleX = 0;
							rts = 0;
							loadingBar.transform.localScale = new Vector3 (scaleX/100f, 1, 1);
							loadingText.text = scaleX + " %";
							status.text = "Smelting iron... \r\n Wood: " + wood + "\r\n Iron: " + iron + "\r\n Steel: " + steel + "/(max)" + steelCapacity;
							buildingInfo.GetComponent <BuildingInformation>().UpdateText (status.text + "\r\n Building level: " + buildingLevel);
						}
					}else{
						x++;
						if(x >= 2000){
							readyToSmelt = false;
						}
						status.text = "Out of iron";
					}
				}else{
					x++;
					if(x >= 2000){
						readyToSmelt = false;
					}
					status.text = "Out of wood";
				}
			}
		}

	}

	public void InputIron(string amount){
		if(cam.GetComponent <PlayerStats>().pIron >= int.Parse (amount) && cam.GetComponent <PlayerStats>().pWood >= int.Parse (amount) * woodPerIron){
			if(readyToSmelt == false){
				warmOven = true;
				wo = 0;
				rts = 0;
				iron += int.Parse (amount);
				wood += iron * woodPerIron;
				cam.GetComponent <PlayerStats> ().pIron -= iron;
				cam.GetComponent <PlayerStats> ().pWood -= wood;
				cam.GetComponent<PLayerUIHandler> ().UpdateText ("Wood", cam.GetComponent <PlayerStats> ().pWood, cam.GetComponent <PlayerStats> ().maxWood);
				cam.GetComponent<PLayerUIHandler> ().UpdateText ("Iron", cam.GetComponent <PlayerStats> ().pIron, cam.GetComponent <PlayerStats> ().maxIron);
				status.text = "Added resources: \r\n Wood: " + wood + "\r\n Iron: " + iron;
				buildingInfo.GetComponent <BuildingInformation>().UpdateText (status.text + "\r\n Building level: " + buildingLevel);
			}else{
				wo = 0;
				rts = 0;
				warmOven = false;
				iron += int.Parse (amount);
				wood += iron * woodPerIron;
				cam.GetComponent <PlayerStats> ().pIron -= iron;
				cam.GetComponent <PlayerStats> ().pWood -= wood;
				cam.GetComponent<PLayerUIHandler> ().UpdateText ("Wood", cam.GetComponent <PlayerStats> ().pWood, cam.GetComponent <PlayerStats> ().maxWood);
				cam.GetComponent<PLayerUIHandler> ().UpdateText ("Iron", cam.GetComponent <PlayerStats> ().pIron, cam.GetComponent <PlayerStats> ().maxIron);
				status.text = "Added resources: \r\n Wood: " + wood + "\r\n Iron: " + iron;
				buildingInfo.GetComponent <BuildingInformation>().UpdateText (status.text + "\r\n Building level: " + buildingLevel);
			}
		}else{
			Debug.Log ("Not enough res");
			Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have enough wood or iron in your inventory.");
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

	public void FixArmor(){
		if(cam.GetComponent <PlayerStats>().armor < cam.GetComponent <PlayerStats>().maxArmor){
			if(steel >= armorFixCost){
				steel -= armorFixCost;
				if(cam.GetComponent <PlayerStats>().armor + armorFixAmount <= cam.GetComponent <PlayerStats>().maxArmor){
					cam.GetComponent <PlayerStats> ().armor += armorFixAmount;
					cam.GetComponent<PLayerUIHandler> ().UpdateText ("Armor", cam.GetComponent <PlayerStats> ().armor, cam.GetComponent <PlayerStats> ().maxArmor);
				}else{
					cam.GetComponent <PlayerStats> ().armor = cam.GetComponent <PlayerStats> ().maxArmor;
					cam.GetComponent<PLayerUIHandler> ().UpdateText ("Armor", cam.GetComponent <PlayerStats> ().armor, cam.GetComponent <PlayerStats> ().maxArmor);
				}
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough steel.");
			}
		}else{
			Debug.Log ("Armor is at max");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Your armour does not need fixing.");
		}
	}

	public void UpgradeArmor(){
		if(steel >= armorUpgrCost){
			steel -= armorUpgrCost;
			cam.GetComponent <PlayerStats> ().maxArmor += armorUpgrAmount;
			cam.GetComponent<PLayerUIHandler> ().UpdateText ("Armor", cam.GetComponent <PlayerStats> ().armor, cam.GetComponent <PlayerStats> ().maxArmor);
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough steel.");
		}
	}

	public void UpgradeSword(){
		if(steel >= swordUpgrCost){
			if(i <= swords.Count){
				cam.GetComponent <Interact>().Sword = swords[i];
				steel -= swordUpgrCost; 
				i++;
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, there are no more sword upgrades available.");
			}
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough steel.");
		}
	}

	public void UpgradeBuilding(){
		if(buildingLevel == 0){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood1 && Master.GetComponent <GameMaster>().gRock >= upgrRock1 && Master.GetComponent <GameMaster>().gIron >= upgrIron1 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints1 && Master.GetComponent <GameMaster>().gGold >= upgrGold1){
				steelCapacity += 50;
				loadTime -= 5;
				warmOvenTime -= 50;
				armorFixCost -= 5;
				armorFixAmount += 5;
				armorUpgrAmount += 10;
				swordUpgrCost -= 5;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().BSQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood1);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold1);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints1;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				status.text = "Waiting... \r\n Wood: " + wood + "\r\n Iron: " + iron + "\r\n Steel: " + steel + "/(max)" + steelCapacity;
				buildingInfo.GetComponent <BuildingInformation>().UpdateText (status.text + "\r\n Building level: " + buildingLevel);				
				cost.text = "Cost: \r\n\t Wood: " + upgrWood2 + "\r\n\t Rock: " + upgrRock2 + "\r\n\t Iron: " + upgrIron2 + "\r\n\t Gold: " + upgrGold2 + "\r\n\t Xp points: " + upgrPoints2 + "\r\n Building level: "+ buildingLevel +"/3";
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}	else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 1){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood2 && Master.GetComponent <GameMaster>().gRock >= upgrRock2 && Master.GetComponent <GameMaster>().gIron >= upgrIron2 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints2 && Master.GetComponent <GameMaster>().gGold >= upgrGold2){
				steelCapacity += 50;
				loadTime -= 5;
				warmOvenTime -= 50;
				armorFixCost -= 5;
				armorUpgrAmount += 10;
				armorFixAmount += 5;
				swordUpgrCost -= 5;
				woodPerIron = 2;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().BSQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood2);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold2);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints2;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "Cost: \r\n\t Wood: " + upgrWood3 + "\r\n\t Rock: " + upgrRock3 + "\r\n\t Iron: " + upgrIron3 + "\r\n\t Gold: " + upgrGold3 + "\r\n\t Xp points: " + upgrPoints3 + "\r\n Building level: "+ buildingLevel +"/3";
				status.text = "Waiting... \r\n Wood: " + wood + "\r\n Iron: " + iron + "\r\n Steel: " + steel + "/(max)" + steelCapacity;
				buildingInfo.GetComponent <BuildingInformation>().UpdateText (status.text + "\r\n Building level: " + buildingLevel);	
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 2){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood3 && Master.GetComponent <GameMaster>().gRock >= upgrRock3 && Master.GetComponent <GameMaster>().gIron >= upgrIron3 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints3 && Master.GetComponent <GameMaster>().gGold >= upgrGold3){
				steelCapacity += 50;
				loadTime -= 5;
				warmOvenTime -= 50;
				armorFixCost -= 5;
				armorUpgrAmount += 10;
				armorFixAmount += 5;
				swordUpgrCost -= 5;
				ironPerSteel = 1;
				woodPerIron = 1;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().BSQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood3);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold3);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints3;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "You have upgraded this building to it's maximum. "+ "\r\n Building level: " + buildingLevel + "/3";
				status.text = "Waiting... \r\n Wood: " + wood + "\r\n Iron: " + iron + "\r\n Steel: " + steel + "/(max)" + steelCapacity;
				buildingInfo.GetComponent <BuildingInformation>().UpdateText (status.text + "\r\n Building level: " + buildingLevel);
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
		if(recycled){
			iron += (steel * ironPerSteel);
			if(Master.GetComponent <GameMaster>().CalculateStorage () + iron <= Master.GetComponent <GameMaster>().storage){
				Master.GetComponent <GameMaster> ().Add ("Iron", iron);
			}
		}
	}

	public void DestroyBuilding(){
		CloseMenu ();
		recycled = true;
		BuildingDiedProtocol ();
		Destroy (gameObject, 0.1f);
	}

}
