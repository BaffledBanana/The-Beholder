using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bank : MonoBehaviour {

	private GameObject Master, cam, myCanvas, buildingInfo, miscBuild;
	private GameObject[] allHouses;
	private int number, x, i, y, z;
	private RaycastHit hit;
	private Vector3 place;

	public GameObject bankExclemationPoint;
	public bool hired = false, loanTaken = false;
	public Text goldAmount, dontHireText, loanText, cost;
	public int capacity, goldDeposited, bankTimer = 3000, hireWoodCost, hireRockCost, hireFoodCost, hireIronCost, hireGoldCost, hireHideCost, loan, nextPayment;
	public float depositRate = 0.03f, loanRate = 1.4f;
	public int buildingLevel, lvl1Capacity, lvl2Capacity, lvl3Capacity, upgrWood1, upgrRock1, upgrIron1, upgrGold1, upgrPoints1, upgrWood2, upgrRock2, upgrIron2, upgrGold2, upgrPoints2, upgrWood3, upgrRock3, upgrIron3, upgrGold3, upgrPoints3;

	void Start(){
		Master = GameObject.Find ("GameMaster");
		cam = GameObject.Find ("Main Camera");
		myCanvas = gameObject.transform.Find ("Canvas").gameObject;
		miscBuild = GameObject.Find ("GUIHUD");
		myCanvas.SetActive (false);
		dontHireText.enabled = false;
		bankExclemationPoint.SetActive (false);
		loan = 0;
		nextPayment = 0;
		loanText.text = "You do not have a loan at this bank.";
		buildingInfo = gameObject.transform.Find ("BuildingInformation").gameObject;
		buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Deposited: " + goldDeposited + "\r\n Current loan left: " + loan + "\r\n Deposit rate: " + depositRate + "\r\n Loan rate: " + loanRate + "\r\n Capacity: " + capacity + "\r\n Building level: " + buildingLevel);
		cost.text = "Cost: \r\n\t Wood: " + upgrWood1 + "\r\n\t Rock: " + upgrRock1 + "\r\n\t Iron: " + upgrIron1 + "\r\n\t Gold: " + upgrGold1 + "\r\n\t Xp points: " + upgrPoints1 + "\r\n Building level: "+ buildingLevel +"/3";
	}

	public void Deposit(string temp){
		if(gameObject.GetComponent <BuildHammer>().isBuilt){
		int.TryParse (temp, out number);
		if(cam.GetComponent <PlayerStats>().pGold >= number){
			if(number + goldDeposited <= capacity){
				goldDeposited += number;
				depositRate += 0.03f;
				loanRate -= 0.01f;
				goldAmount.text = "Gold currently deposited in this bank: " + goldDeposited + "/" + capacity;
				cam.GetComponent <PlayerStats> ().pGold -= number;
				cam.GetComponent <PLayerUIHandler>().UpdateText ("Gold", cam.GetComponent <PlayerStats> ().pGold, cam.GetComponent <PlayerStats> ().maxGold);
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Deposited: " + goldDeposited + "\r\n Current loan left: " + loan + "\r\n Deposit rate: " + depositRate + "\r\n Loan rate: " + loanRate + "\r\n Capacity: " + capacity + "\r\n Building level: " + buildingLevel);			}else{
				Debug.Log ("The bank is full");
				Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, this particular bank cannot store any more gold. Try another one.");
			}
		}else{
			Debug.Log ("You dont have that much gold in inventory");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have that much gold in your inventory.");
		}
		}
	}

	public void TakeOutDeposit(string temp){
		if(gameObject.GetComponent <BuildHammer>().isBuilt){
		int.TryParse (temp, out number);
		if(goldDeposited - number >= 0){
			if(cam.GetComponent <PlayerStats> ().pGold + number <= cam.GetComponent <PlayerStats> ().maxGold){
				cam.GetComponent <PlayerStats> ().pGold += number;
				goldDeposited -= number;
				depositRate -= 0.01f;
				loanRate += 0.07f;
				goldAmount.text = "Gold currently deposited in this bank: " + goldDeposited + "/" + capacity;
				cam.GetComponent <PLayerUIHandler>().UpdateText ("Gold", cam.GetComponent <PlayerStats> ().pGold, cam.GetComponent <PlayerStats> ().maxGold);
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Deposited: " + goldDeposited + "\r\n Current loan left: " + loan + "\r\n Deposit rate: " + depositRate + "\r\n Loan rate: " + loanRate + "\r\n Capacity: " + capacity + "\r\n Building level: " + buildingLevel);			}else{
				Debug.Log ("You dont have that much space in your inventory");
				Master.GetComponent <GameMaster> ().PassErrorMessage ("Your inventory gold capacity is too small.");
			}
		}else{
			Debug.Log ("There is not that much gold in the bank");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Your bank account does not have that much gold in it.");
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

	public void AddGold(){
		if(gameObject.GetComponent <BuildHammer>().isBuilt){
		goldDeposited += (int)(goldDeposited * depositRate);
		goldAmount.text = "Gold currently deposited in this bank: " + goldDeposited + "/" + capacity;
		buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Deposited: " + goldDeposited + "\r\n Current loan left: " + loan + "\r\n Deposit rate: " + depositRate + "\r\n Loan rate: " + loanRate + "\r\n Capacity: " + capacity + "\r\n Building level: " + buildingLevel);	
		}
		}

	void Update(){
		//FIRST THE BANK DOES EVERYTHING IT NEEDS TO DO ON ITS OWN
		if(x >= bankTimer  && gameObject.GetComponent <BuildHammer>().isBuilt){
			if(hired){
				i = 0;
				allHouses = GameObject.FindGameObjectsWithTag ("House");
				if(allHouses.Length > 0){
					foreach(GameObject house in allHouses){
						if(i > 5){
							break;
						}else{
							if(house.GetComponent <House>().collectTaxes){
								goldDeposited += house.GetComponent <House>().BankCollect ();
								i++;
							}
						}
					}
					Debug.Log ("Bank collected tax from this many houses: " + i);
					buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Deposited: " + goldDeposited + "\r\n Current loan left: " + loan + "\r\n Deposit rate: " + depositRate + "\r\n Loan rate: " + loanRate + "\r\n Capacity: " + capacity + "\r\n Building level: " + buildingLevel);					goldAmount.text = "Gold currently deposited in this bank: " + goldDeposited + "/" + capacity;
				}
			}
			x = 0;
		}
		x++;
	}

	public void Hire(){
		if(Master.GetComponent <GameMaster>().population > 0 && Master.GetComponent <GameMaster>().workers < Master.GetComponent <GameMaster>().population  && gameObject.GetComponent <BuildHammer>().isBuilt){
			if(cam.GetComponent <PlayerStats>().pWood >= hireWoodCost && cam.GetComponent <PlayerStats>().pRock >= hireRockCost && cam.GetComponent <PlayerStats>().pIron >= hireIronCost && cam.GetComponent <PlayerStats>().pFood >= hireFoodCost && cam.GetComponent <PlayerStats>().pHide >= hireHideCost && cam.GetComponent <PlayerStats>().pGold >= hireGoldCost){
				if(hired == false){
					hired = true;
					dontHireText.enabled = true;
					//Master.GetComponent <GameMaster> ().Subtract ("Population", 1);
					Master.GetComponent <GameMaster> ().Add ("Workers", 1);
					cam.GetComponent <PlayerStats>().pWood -= hireWoodCost;
					cam.GetComponent <PlayerStats>().pRock -= hireRockCost;
					cam.GetComponent <PlayerStats>().pIron -= hireIronCost;
					cam.GetComponent <PlayerStats>().pFood -= hireFoodCost;
					cam.GetComponent <PlayerStats>().pHide -= hireHideCost;
					cam.GetComponent <PlayerStats>().pGold -= hireGoldCost;
					cam.GetComponent <PLayerUIHandler>().UpdateText ("Wood", cam.GetComponent <PlayerStats> ().pWood, cam.GetComponent <PlayerStats> ().maxWood);
					cam.GetComponent <PLayerUIHandler>().UpdateText ("Rock", cam.GetComponent <PlayerStats> ().pRock, cam.GetComponent <PlayerStats> ().maxRock);
					cam.GetComponent <PLayerUIHandler>().UpdateText ("Iron", cam.GetComponent <PlayerStats> ().pIron, cam.GetComponent <PlayerStats> ().maxIron);
					cam.GetComponent <PLayerUIHandler>().UpdateText ("Food", cam.GetComponent <PlayerStats> ().pFood, cam.GetComponent <PlayerStats> ().maxFood);
					cam.GetComponent <PLayerUIHandler>().UpdateText ("Hide", cam.GetComponent <PlayerStats> ().pHide, cam.GetComponent <PlayerStats> ().maxHide);
					cam.GetComponent <PLayerUIHandler>().UpdateText ("Gold", cam.GetComponent <PlayerStats> ().pGold, cam.GetComponent <PlayerStats> ().maxGold);
				}else{
					Debug.Log (gameObject.name + " This bank already has a worker.");
					Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, this bank already has a tax collector, you can only have one per bank.");
				}
			}else{
				Debug.Log ("You dont have enough res");
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough resources to hire a tax collector.");
			}	
		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough free citizens to hire a tax collector.");

		}
	}

	public void TakeOutALoan(string temp){
		if(loanTaken && gameObject.GetComponent <BuildHammer>().isBuilt){
			Debug.Log ("You already have a loan");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Sorry, you already have a loan at this bank. Try another one.");
		}
		else if(Master.GetComponent <GameMaster>().CalculateStorage () + loan <= Master.GetComponent <GameMaster>().storage && gameObject.GetComponent <BuildHammer>().isBuilt){
			int.TryParse (temp, out loan);
			Master.GetComponent <GameMaster> ().Add ("Gold", loan);
			loan = (int)(loan * loanRate);
			nextPayment = (int)((loan * loanRate) / 30);
			y = 0;
			z = 0;
			loanRate += 0.1f;
			loanTaken = true;
			loanText.text = "Total loan to pay: " + loan + "\r\n Next payment: " + nextPayment;
		buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Deposited: " + goldDeposited + "\r\n Current loan left: " + loan + "\r\n Deposit rate: " + depositRate + "\r\n Loan rate: " + loanRate + "\r\n Capacity: " + capacity + "\r\n Building level: " + buildingLevel);		}else{
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Your storage capatity is too small for that amount of gold.");
		}
	}

	public void PayLoan(){
		if(cam.GetComponent <PlayerStats>().pGold >= nextPayment && loan > nextPayment && gameObject.GetComponent <BuildHammer>().isBuilt){
			loan -= nextPayment;
			cam.GetComponent <PlayerStats> ().pGold -= nextPayment;
			cam.GetComponent <PLayerUIHandler> ().UpdateText ("Gold", cam.GetComponent <PlayerStats> ().pGold, cam.GetComponent <PlayerStats> ().maxGold);
			loanText.text = "Total loan to pay: " + loan + "\r\n Next payment: " + nextPayment;
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Deposited: " + goldDeposited + "\r\n Current loan left: " + loan + "\r\n Deposit rate: " + depositRate + "\r\n Loan rate: " + loanRate + "\r\n Capacity: " + capacity + "\r\n Building level: " + buildingLevel);			y = 0;
			z = 0;
			bankExclemationPoint.SetActive (false);
		}else if(cam.GetComponent <PlayerStats>().pGold >= nextPayment && loan <= nextPayment && gameObject.GetComponent <BuildHammer>().isBuilt){
			cam.GetComponent <PlayerStats> ().pGold -= loan;
			loan = 0;
			loanTaken = false;
			cam.GetComponent <PLayerUIHandler> ().UpdateText ("Gold", cam.GetComponent <PlayerStats> ().pGold, cam.GetComponent <PlayerStats> ().maxGold);
			loanText.text = "You do not have a loan at this bank.";
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Congratulations, you have payed off your loan at this bank!");
			buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Deposited: " + goldDeposited + "\r\n Current loan left: " + loan + "\r\n Deposit rate: " + depositRate + "\r\n Loan rate: " + loanRate + "\r\n Capacity: " + capacity + "\r\n Building level: " + buildingLevel);			y = 0;
			z = 0;
			bankExclemationPoint.SetActive (false);
		} else{
			Debug.Log ("Not enough gold");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough gold in your inventory.");
		}
	}

	public void PayEntireLoan(){
		if(cam.GetComponent <PlayerStats>().pGold >= loan && gameObject.GetComponent <BuildHammer>().isBuilt){
			cam.GetComponent <PlayerStats> ().pGold -= loan;
			cam.GetComponent <PLayerUIHandler> ().UpdateText ("Gold", cam.GetComponent <PlayerStats> ().pGold, cam.GetComponent <PlayerStats> ().maxGold);
			loan = 0;
			nextPayment = 0;
			loanTaken = false;
			loanText.text = "You do not have a loan at this bank.";
			Master.GetComponent <GameMaster> ().PassErrorMessage ("Congratulations, you have payed off your loan at this bank!");
		buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Deposited: " + goldDeposited + "\r\n Current loan left: " + loan + "\r\n Deposit rate: " + depositRate + "\r\n Loan rate: " + loanRate + "\r\n Capacity: " + capacity + "\r\n Building level: " + buildingLevel);		}else{
			Debug.Log ("Not enough gold");
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough gold in your inventory.");
		}
	}

	public void ShouldPayLoan(){
		if(gameObject.GetComponent <BuildHammer>().isBuilt){
		y++;
		if(y >= 4 && loanTaken){
			bankExclemationPoint.SetActive (true);
			Debug.Log ("Should pay loan now");
			z++;
			if(z >= 2){
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You have not payed your loan in time!");
				if(Master.GetComponent <GameMaster> ().gGold >= nextPayment * 2){
					Master.GetComponent <GameMaster> ().Subtract ("Gold", nextPayment * 2);
					Master.GetComponent <GameMaster> ().Subtract ("Happyness", 10);
					loan -= (int)(nextPayment / 2);
					loanText.text = "Total loan to pay: " + loan + "\r\n Next payment: " + nextPayment;
					buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Deposited: " + goldDeposited + "\r\n Current loan left: " + loan + "\r\n Deposit rate: " + depositRate + "\r\n Loan rate: " + loanRate + "\r\n Capacity: " + capacity + "\r\n Building level: " + buildingLevel);					bankExclemationPoint.SetActive (false);
					z = 0;
					y = 0;	
					Debug.Log ("Force payed loan");
				}
				else{
					loanTaken = false; //this is for testing, remove this line of code
					//something bad happens here, like maybe you start loosing houses or workers. For now its just a game over.
					Debug.Log ("Missed loan payments so much that you ran out of gold in your town and so its game over");
					Master.GetComponent <GameMaster> ().PassErrorMessage ("Your town has drowned in debt that you could not pay off in time. Game over!");
				}
			}
		}
		}
	}

	public void UpgradeBuilding(){
		if(buildingLevel == 0 && gameObject.GetComponent <BuildHammer>().isBuilt){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood1 && Master.GetComponent <GameMaster>().gRock >= upgrRock1 && Master.GetComponent <GameMaster>().gIron >= upgrIron1 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints1 && Master.GetComponent <GameMaster>().gGold >= upgrGold1){
				capacity += lvl1Capacity;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().bankQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood1);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock1);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold1);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints1;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Deposited: " + goldDeposited + "\r\n Current loan left: " + loan + "\r\n Deposit rate: " + depositRate + "\r\n Loan rate: " + loanRate + "\r\n Capacity: " + capacity + "\r\n Building level: " + buildingLevel);				cost.text = "Cost: \r\n\t Wood: " + upgrWood2 + "\r\n\t Rock: " + upgrRock2 + "\r\n\t Iron: " + upgrIron2 + "\r\n\t Gold: " + upgrGold2 + "\r\n\t Xp points: " + upgrPoints2 + "\r\n Building level: "+ buildingLevel +"/3";
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				CloseMenu ();
			}	else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 1){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood2 && Master.GetComponent <GameMaster>().gRock >= upgrRock2 && Master.GetComponent <GameMaster>().gIron >= upgrIron2 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints2 && Master.GetComponent <GameMaster>().gGold >= upgrGold2){
				capacity += lvl2Capacity;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().bankQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood2);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock2);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold2);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints2;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "Cost: \r\n\t Wood: " + upgrWood3 + "\r\n\t Rock: " + upgrRock3 + "\r\n\t Iron: " + upgrIron3 + "\r\n\t Gold: " + upgrGold3 + "\r\n\t Xp points: " + upgrPoints3 + "\r\n Building level: "+ buildingLevel +"/3";
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You can now place your building upgrade! (from the misc buildings build menu)");
				buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Deposited: " + goldDeposited + "\r\n Current loan left: " + loan + "\r\n Deposit rate: " + depositRate + "\r\n Loan rate: " + loanRate + "\r\n Capacity: " + capacity + "\r\n Building level: " + buildingLevel);				CloseMenu ();
				CloseMenu ();
			}else{
				Master.GetComponent <GameMaster> ().PassErrorMessage ("You do not have enough of something.");
			}
		}else if(buildingLevel == 2){
			if(Master.GetComponent <GameMaster>().gWood >= upgrWood3 && Master.GetComponent <GameMaster>().gRock >= upgrRock3 && Master.GetComponent <GameMaster>().gIron >= upgrIron3 && cam.GetComponent<PlayerStats>().xpPoints >= upgrPoints3 && Master.GetComponent <GameMaster>().gGold >= upgrGold3){
				capacity += lvl3Capacity;
				gameObject.GetComponent <BuildingHealth> ().health += 100;
				gameObject.GetComponent <BuildingHealth> ().maxHealth += 100;
				buildingLevel++;
				miscBuild.GetComponent <MiscBuildMenu> ().bankQueue++;
				miscBuild.GetComponent <MiscBuildMenu> ().UpdateQueueTexts ();
				Master.GetComponent <GameMaster> ().Subtract ("Wood", upgrWood3);
				Master.GetComponent <GameMaster> ().Subtract ("Rock", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", upgrRock3);
				Master.GetComponent <GameMaster> ().Subtract ("Gold", upgrGold3);
				cam.GetComponent <PlayerStats>().xpPoints -= upgrPoints3;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("XpPoints", cam.GetComponent <PlayerStats>().xpPoints, 0);
				cost.text = "You have upgraded this building to it's maximum. "+ "\r\n Building level: " + buildingLevel + "/3";
				buildingInfo.GetComponent <BuildingInformation>().UpdateText ("Deposited: " + goldDeposited + "\r\n Current loan left: " + loan + "\r\n Deposit rate: " + depositRate + "\r\n Loan rate: " + loanRate + "\r\n Capacity: " + capacity + "\r\n Building level: " + buildingLevel);				CloseMenu ();
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

	public bool BuildingDiedProtocol(){
		if(goldDeposited == 0 && loan == 0){
			return true;
		}else{
			return false;
		}
	}

	public void DestroyBuilding(){
		if(BuildingDiedProtocol ()){
			CloseMenu ();
			Destroy (gameObject, 0.1f);	
		}else{
			Master.GetComponent <GameMaster>().PassErrorMessage ("Sorry, you must take out your deposited gold or pay the entire loan to destroy this bank!");
		}
	}

}
