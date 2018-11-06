using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Market : MonoBehaviour {

	private int inputAmount, goldAmount, c, w, r, i, f, h, change;
	private GameObject Master, Menu, buildingInfo, cam, miscBuild;
	private RaycastHit hit;
	private Vector3 place;
	private Text cost;

	public float woodToGoldRate = 11, rockToGoldRate = 30, ironToGoldRate = 60, foodToGoldRate = 15, hideToGoldRate, harshRateChangeScaler = 10, convFeeRate = 10, upOffsetUpgr1,upOffsetUpgr2, upOffsetUpgr3;
	public int conversionFee, woodRecovRate, ironRecovRate, rockRecovRate, foodRecovRate, hideRecovRate, minRateChange, maxRateChange, convFeeRecovRate;
	public int buildingLevel, upgrWood1, upgrRock1, upgrIron1, upgrGold1, upgrPoints1, upgrWood2, upgrRock2, upgrIron2, upgrGold2, upgrPoints2, upgrWood3, upgrRock3, upgrIron3, upgrGold3, upgrPoints3, upgrConvFeeRate1, upgrConvFeeRate2, upgrConvFeeRate3, upgrWRecovRate, upgrRRecovRate, upgrIRecovRate, upgrFRecovRate, upgrHRecovRate;

	// Use this for initialization
	void Start () {
		Master = GameObject.Find ("GameMaster");
		Menu = GameObject.Find ("MarketBuildingUI");
		miscBuild = GameObject.Find ("GUIHUD");
		Menu.SetActive (false);
		cam = GameObject.Find ("Main Camera");
		buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
		buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Wood to gold rate: " + woodToGoldRate/100 + "\r\n Rock to gold rate: " + rockToGoldRate/100 + "\r\n Iron to gold rate: " + ironToGoldRate/100 + "\r\n Food to gold rate: " + foodToGoldRate/100 + "\r\n Hide to gold rate: " + hideToGoldRate/100 + "\r\n Building level: " + buildingLevel);
		Master.GetComponent <GameMaster>().Add ("Happyness", 3);
		cost = transform.Find ("Canvas").Find ("MarketBuildingUI").Find ("Cost").GetComponent <Text>();
		cost.text = "Cost: \n\t Wood: " + upgrWood1 + "\n\t Rock: " + upgrRock1 + "\n\t Iron: " + upgrIron1 + "\n\t Gold: " + upgrGold1 + "\n\t Xp points: " + upgrPoints1 + "\n\t Building level: "+ buildingLevel +"/3";
	}

	public void UpgradeBuilding(){
		if(buildingLevel == 0){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood1 && Master.GetComponent <GameMaster>().gRock >= upgrRock1 && Master.GetComponent <GameMaster>().gIron >= upgrIron1 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints1 && Master.GetComponent <GameMaster>().gGold >= upgrGold1){
				convFeeRate -= upgrConvFeeRate1;
				harshRateChangeScaler--;
				woodToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				ironToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				foodRecovRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				rockToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				hideToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				woodRecovRate -= upgrWRecovRate;
				rockRecovRate -= upgrRRecovRate;
				ironRecovRate -= upgrIRecovRate;
				foodRecovRate -= upgrFRecovRate;
				hideRecovRate -= upgrHRecovRate;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().marketQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood1);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold1);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints1;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Wood to gold rate: " + woodToGoldRate/100 + "\r\n Rock to gold rate: " + rockToGoldRate/100 + "\r\n Iron to gold rate: " + ironToGoldRate/100 + "\r\n Food to gold rate: " + foodToGoldRate/100 + "\r\n Hide to gold rate: " + hideToGoldRate/100 + "\r\n Building level: " + buildingLevel);
				cost.text = "Cost: \r\n\t Wood: " + upgrWood2 + "\r\n\t Rock: " + upgrRock2 + "\r\n\t Iron: " + upgrIron2 + "\r\n\t Gold: " + upgrGold2 + "\r\n\t Xp points: " + upgrPoints2 + "\r\n Building level: "+ buildingLevel +"/3";
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}	else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 1){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood2 && Master.GetComponent <GameMaster>().gRock >= upgrRock2 && Master.GetComponent <GameMaster>().gIron >= upgrIron2 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints2 && Master.GetComponent <GameMaster>().gGold >= upgrGold2){
				convFeeRate -= upgrConvFeeRate2;
				harshRateChangeScaler--;
				woodToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				ironToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				foodRecovRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				rockToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				hideToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				woodRecovRate -= upgrWRecovRate;
				rockRecovRate -= upgrRRecovRate;
				ironRecovRate -= upgrIRecovRate;
				foodRecovRate -= upgrFRecovRate;
				hideRecovRate -= upgrHRecovRate;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().marketQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood2);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold2);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints2;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "Cost: \r\n\t Wood: " + upgrWood3 + "\r\n\t Rock: " + upgrRock3 + "\r\n\t Iron: " + upgrIron3 + "\r\n\t Gold: " + upgrGold3 + "\r\n\t Xp points: " + upgrPoints3 + "\r\n Building level: "+ buildingLevel +"/3";
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Wood to gold rate: " + woodToGoldRate/100 + "\r\n Rock to gold rate: " + rockToGoldRate/100 + "\r\n Iron to gold rate: " + ironToGoldRate/100 + "\r\n Food to gold rate: " + foodToGoldRate/100 + "\r\n Hide to gold rate: " + hideToGoldRate/100 + "\r\n Building level: " + buildingLevel);			
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 2){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood3 && Master.GetComponent <GameMaster>().gRock >= upgrRock3 && Master.GetComponent <GameMaster>().gIron >= upgrIron3 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints3 && Master.GetComponent <GameMaster>().gGold >= upgrGold3){
				convFeeRate -= upgrConvFeeRate3;
				harshRateChangeScaler--;
				woodToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				ironToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				foodRecovRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				rockToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				hideToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
				woodRecovRate -= upgrWRecovRate;
				rockRecovRate -= upgrRRecovRate;
				ironRecovRate -= upgrIRecovRate;
				foodRecovRate -= upgrFRecovRate;
				hideRecovRate -= upgrHRecovRate;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().marketQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood3);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold3);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints3;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "You have upgraded this building to it's maximum. "+ "\r\n Building level: " + buildingLevel + "/3";
				buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Wood to gold rate: " + woodToGoldRate/100 + "\r\n Rock to gold rate: " + rockToGoldRate/100 + "\r\n Iron to gold rate: " + ironToGoldRate/100 + "\r\n Food to gold rate: " + foodToGoldRate/100 + "\r\n Hide to gold rate: " + hideToGoldRate/100 + "\r\n Building level: " + buildingLevel);
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

	public void InputWood(string temp){
		int.TryParse (temp, out inputAmount);
		if(Master.GetComponent <GameMaster>().gWood >= inputAmount && woodToGoldRate > 0){//CHANGE
			
			if((int)Math.Round (inputAmount * (woodToGoldRate/100), 0) - conversionFee > 0){//CHANGE
				goldAmount = (int)Math.Round (inputAmount * (woodToGoldRate/100), 0) - conversionFee;
				Debug.Log ("This is how much gold was calculated: " + goldAmount);
				if(Master.GetComponent <GameMaster>().CalculateStorage () + goldAmount - inputAmount <= Master.GetComponent <GameMaster>().storage){
					Master.GetComponent <GameMaster> ().Subtract ("Wood", inputAmount);//CHANGE
					Master.GetComponent <GameMaster> ().Add ("Gold", goldAmount);
					woodToGoldRate -= (int)Math.Round ((Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange)) * inputAmount/harshRateChangeScaler);//CHANGE
					conversionFee += (int)Math.Round (inputAmount/convFeeRate, 0);
					buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Wood to gold rate: " + woodToGoldRate/100 + "\r\n Rock to gold rate: " + rockToGoldRate/100 + "\r\n Iron to gold rate: " + ironToGoldRate/100 + "\r\n Food to gold rate: " + foodToGoldRate/100 + "\r\n Hide to gold rate: " + hideToGoldRate/100 + "\r\n Building level: " + buildingLevel);
					w = 0;//CHANGE
					c = 0;
				}	else{
					Debug.Log ("Your storage is going to fill up too much!");
					Master.GetComponent <GameMaster> ().PassErrorMessage ("Doing this action will result in a storage capacity overflow. Try a lower amount.");
				}
			}else {
				Debug.Log ("Conversion fee is bigger than the amount of gold you would get from this!");
				Master.GetComponent <GameMaster> ().PassErrorMessage ("The markets conversion fee exceeds the amount of gold you would get from this action. try a higher amount or a different resource!");
			}
		}else{Debug.Log ("You dont have enough wood in your storage or the gold rate is < 0");//CHANGE
			Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have that much wood in your storage!");//CHANGE
		}
	}

	public void InputRock(string temp){
		int.TryParse (temp, out inputAmount);
		if(Master.GetComponent <GameMaster>().gRock >= inputAmount && rockToGoldRate > 0){//CHANGE

			if((int)Math.Round (inputAmount * (rockToGoldRate/100), 0) - conversionFee > 0){//CHANGE
				goldAmount = (int)Math.Round (inputAmount * (ironToGoldRate/100), 0) - conversionFee;//CHANGE
				Debug.Log ("This is how much gold was calculated: " + goldAmount);
				if(Master.GetComponent <GameMaster>().CalculateStorage () + goldAmount - inputAmount <= Master.GetComponent <GameMaster>().storage){
					Master.GetComponent <GameMaster> ().Subtract ("Rock", inputAmount);//CHANGE
					Master.GetComponent <GameMaster> ().Add ("Gold", goldAmount);
					rockToGoldRate -= (int)Math.Round ((Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange)) * inputAmount/harshRateChangeScaler);//CHANGE
					conversionFee += (int)Math.Round (inputAmount/convFeeRate, 0);
					buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Wood to gold rate: " + woodToGoldRate/100 + "\r\n Rock to gold rate: " + rockToGoldRate/100 + "\r\n Iron to gold rate: " + ironToGoldRate/100 + "\r\n Food to gold rate: " + foodToGoldRate/100 + "\r\n Hide to gold rate: " + hideToGoldRate/100 + "\r\n Building level: " + buildingLevel);
					r = 0;//CHANGE
					c = 0;
				}	else{
					Debug.Log ("Your storage is going to fill up too much!");
					Master.GetComponent <GameMaster> ().PassErrorMessage ("Doing this action will result in a storage capacity overflow. Try a lower amount.");
				}
			}else {
				Debug.Log ("Conversion fee is bigger than the amount of gold you would get from this!");
				Master.GetComponent <GameMaster> ().PassErrorMessage ("The markets conversion fee exceeds the amount of gold you would get from this action. try a higher amount or a different resource!");
			}
		}else{Debug.Log ("You dont have enough rock in your storage or the gold rate is < 0");//CHANGE
			Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have that much rock in your storage!");//CHANGE
		}
	}

	public void InputIron(string temp){
		int.TryParse (temp, out inputAmount);
		if(Master.GetComponent <GameMaster>().gIron >= inputAmount && ironToGoldRate > 0){//CHANGE

			if((int)Math.Round (inputAmount * (ironToGoldRate/100), 0) - conversionFee > 0){//CHANGE
				goldAmount = (int)Math.Round (inputAmount * (ironToGoldRate/100), 0) - conversionFee;//CHANGE
				Debug.Log ("This is how much gold was calculated: " + goldAmount);
				if(Master.GetComponent <GameMaster>().CalculateStorage () + goldAmount - inputAmount <= Master.GetComponent <GameMaster>().storage){
					Master.GetComponent <GameMaster> ().Subtract ("Iron", inputAmount);//CHANGE
					Master.GetComponent <GameMaster> ().Add ("Gold", goldAmount);
					ironToGoldRate -= (int)Math.Round ((Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange)) * inputAmount/harshRateChangeScaler);//CHANGE
					conversionFee += (int)Math.Round (inputAmount/convFeeRate, 0);
					buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Wood to gold rate: " + woodToGoldRate/100 + "\r\n Rock to gold rate: " + rockToGoldRate/100 + "\r\n Iron to gold rate: " + ironToGoldRate/100 + "\r\n Food to gold rate: " + foodToGoldRate/100 + "\r\n Hide to gold rate: " + hideToGoldRate/100 + "\r\n Building level: " + buildingLevel);
					i = 0;//CHANGE
					c = 0;
				}	else{
					Debug.Log ("Your storage is going to fill up too much!");
					Master.GetComponent <GameMaster> ().PassErrorMessage ("Doing this action will result in a storage capacity overflow. Try a lower amount.");
				}
			}else {
				Debug.Log ("Conversion fee is bigger than the amount of gold you would get from this!");
				Master.GetComponent <GameMaster> ().PassErrorMessage ("The markets conversion fee exceeds the amount of gold you would get from this action. try a higher amount or a different resource!");
			}
		}else{Debug.Log ("You dont have enough iron in your storage or the gold rate is < 0");//CHANGE
			Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have that much iron in your storage!");//CHANGE
		}
	}

	public void InputFood(string temp){
		int.TryParse (temp, out inputAmount);
		if(Master.GetComponent <GameMaster>().gFood >= inputAmount && foodToGoldRate > 0){//CHANGE

			if((int)Math.Round (inputAmount * (foodToGoldRate/100), 0) - conversionFee > 0){//CHANGE
				goldAmount = (int)Math.Round (inputAmount * (foodToGoldRate/100), 0) - conversionFee;//CHANGE
				Debug.Log ("This is how much gold was calculated: " + goldAmount);
				if(Master.GetComponent <GameMaster>().CalculateStorage () + goldAmount - inputAmount <= Master.GetComponent <GameMaster>().storage){
					Master.GetComponent <GameMaster> ().Subtract ("Food", inputAmount);//CHANGE
					Master.GetComponent <GameMaster> ().Add ("Gold", goldAmount);
					ironToGoldRate -= (int)Math.Round ((Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange)) * inputAmount/harshRateChangeScaler);//CHANGE
					conversionFee += (int)Math.Round (inputAmount/convFeeRate, 0);
					buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Wood to gold rate: " + woodToGoldRate/100 + "\r\n Rock to gold rate: " + rockToGoldRate/100 + "\r\n Iron to gold rate: " + ironToGoldRate/100 + "\r\n Food to gold rate: " + foodToGoldRate/100 + "\r\n Hide to gold rate: " + hideToGoldRate/100 + "\r\n Building level: " + buildingLevel);
					f = 0;//CHANGE
					c = 0;
				}	else{
					Debug.Log ("Your storage is going to fill up too much!");
					Master.GetComponent <GameMaster> ().PassErrorMessage ("Doing this action will result in a storage capacity overflow. Try a lower amount.");
				}
			}else {
				Debug.Log ("Conversion fee is bigger than the amount of gold you would get from this!");
				Master.GetComponent <GameMaster> ().PassErrorMessage ("The markets conversion fee exceeds the amount of gold you would get from this action. try a higher amount or a different resource!");
			}
		}else{Debug.Log ("You dont have enough food in your storage or the gold rate is < 0");//CHANGE
			Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have that much food in your storage!");//CHANGE
		}
	}
	public void InputHide(string temp){
		int.TryParse (temp, out inputAmount);
		if(Master.GetComponent <GameMaster>().gHide >= inputAmount && hideToGoldRate > 0){//CHANGE

			if((int)Math.Round (inputAmount * (hideToGoldRate/100), 0) - conversionFee > 0){//CHANGE
				goldAmount = (int)Math.Round (inputAmount * (hideToGoldRate/100), 0) - conversionFee;//CHANGE
				Debug.Log ("This is how much gold was calculated: " + goldAmount);
				if(Master.GetComponent <GameMaster>().CalculateStorage () + goldAmount - inputAmount <= Master.GetComponent <GameMaster>().storage){
					Master.GetComponent <GameMaster> ().Subtract ("Hide", inputAmount);//CHANGE
					Master.GetComponent <GameMaster> ().Add ("Gold", goldAmount);
					hideToGoldRate -= (int)Math.Round ((Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange)) * inputAmount/harshRateChangeScaler);//CHANGE
					conversionFee += (int)Math.Round (inputAmount/convFeeRate, 0);
					buildingInfo.GetComponent <BuildingInformation> ().UpdateText ("Wood to gold rate: " + woodToGoldRate/100 + "\r\n Rock to gold rate: " + rockToGoldRate/100 + "\r\n Iron to gold rate: " + ironToGoldRate/100 + "\r\n Food to gold rate: " + foodToGoldRate/100 + "\r\n Hide to gold rate: " + hideToGoldRate/100 + "\r\n Building level: " + buildingLevel);
					h = 0;//CHANGE
					c = 0;
				}	else{
					Debug.Log ("Your storage is going to fill up too much!");
					Master.GetComponent <GameMaster> ().PassErrorMessage ("Doing this action will result in a storage capacity overflow. Try a lower amount.");
				}
			}else {
				Debug.Log ("Conversion fee is bigger than the amount of gold you would get from this!");
				Master.GetComponent <GameMaster> ().PassErrorMessage ("The markets conversion fee exceeds the amount of gold you would get from this action. try a higher amount or a different resource!");
			}
		}else{Debug.Log ("You dont have enough hide in your storage or the gold rate is < 0");//CHANGE
			Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have that much hide in your storage!");//CHANGE
		}
	}

	void Update(){
		if(w >= woodRecovRate){
			woodToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
			w = 0;
		}
		if(r >= rockRecovRate){
			rockToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
			r = 0;
		}
		if(i >= ironRecovRate){
			ironToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
			i = 0;
		}
		if(f >= foodRecovRate){
			foodToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
			f = 0;
		}
		if(h >= hideRecovRate){
			hideToGoldRate += Master.GetComponent <GameMaster> ().GenerateRandom (minRateChange, maxRateChange);
			h = 0;
		}
		if(c >= convFeeRecovRate && conversionFee - convFeeRate > 0){
			conversionFee -= (int)Math.Round (convFeeRate, 0);
			c = 0;
		}
		w++;
		r++;
		i++;
		f++;
		c++;
		h++;

	}

	public void OpenMenu(){
		if(gameObject.GetComponent <BuildHammer>().isBuilt){
			Master.GetComponent <GameMaster> ().DisablePlayer ();
			Menu.SetActive (true);	
			GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (true);
		}
	}
	public void CloseMenu(){
		Master.GetComponent<GameMaster> ().EnablePlayer ();
		Menu.SetActive (false);
		GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (false);
	}

	public void DestroyBuilding(){
		CloseMenu ();
		//BuildingDiedProtocol ();
		Destroy (gameObject, 0.1f);
	}

}
