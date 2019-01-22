using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenu : MonoBehaviour {

	public Text houseCost, storageCost, outpostCost, barracksCost, TPCost, TCCost, bankCost, marketCost, spikesCost, DTCost, tavernCost, BTCost, BSCost, MSCost, ACCost, PUBCost, castleCost, farmCost, WWCost, RWCost;

	public Transform townsBuildings, defenceBuildings, miscBuildings;

	private GameObject constructBuilding;

	void Start () {
		constructBuilding = GameObject.Find ("ConstructBuilding");

		townsBuildings = gameObject.transform.Find ("TownBuildings");
		defenceBuildings = gameObject.transform.Find ("DefenceBuildings");
		miscBuildings = gameObject.transform.Find ("MiscBuildings");
		//towns building costs
		houseCost = townsBuildings.Find ("SpawnHouse").Find ("Cost").GetComponent <Text>();
		storageCost = townsBuildings.Find ("SpawnStorage").Find ("Cost").GetComponent <Text>();
		outpostCost = townsBuildings.Find ("SpawnOutpost").Find ("Cost").GetComponent <Text>();
		barracksCost = townsBuildings.Find ("SpawnBarracks").Find ("Cost").GetComponent <Text>();
		TPCost = townsBuildings.Find ("SpawnTreePlantation").Find ("Cost").GetComponent <Text>();
		TCCost = townsBuildings.Find ("SpawnTrainingCamp").Find ("Cost").GetComponent <Text>();
		bankCost = townsBuildings.Find ("SpawnBank").Find ("Cost").GetComponent <Text>();
		marketCost = townsBuildings.Find ("SpawnMarket").Find ("Cost").GetComponent <Text>();
		farmCost = townsBuildings.Find ("SpawnFarm").Find ("Cost").GetComponent <Text>();
		BSCost = townsBuildings.Find ("SpawnBlacksmith").Find ("Cost").GetComponent <Text>();
		tavernCost = townsBuildings.Find ("SpawnTavern").Find ("Cost").GetComponent <Text>();
		MSCost = townsBuildings.Find ("SpawnMineShaft").Find ("Cost").GetComponent <Text>();
		ACCost = townsBuildings.Find ("SpawnAnimalCoop").Find ("Cost").GetComponent <Text>();
		castleCost = townsBuildings.Find ("SpawnCastle").Find ("Cost").GetComponent <Text>();
		PUBCost = townsBuildings.Find ("SpawnPUB").Find ("Cost").GetComponent <Text>();
		//defence building costs
		DTCost = defenceBuildings.Find ("DefenceTowerCost").GetComponent <Text>();
		BTCost = defenceBuildings.Find ("BellTowerCost").GetComponent <Text>();
		spikesCost = defenceBuildings.Find ("WoodSpikesCost").GetComponent <Text>();
		WWCost = defenceBuildings.Find ("WoodWallCost").GetComponent <Text>();
		RWCost = defenceBuildings.Find ("RockWallCost").GetComponent <Text>();
		
		UpdateAllCosts ();
	}

	public void UpdateAllCosts(){
		//towns building cost updates
		houseCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().houseWoodCost + "\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().houseRockCost + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().houseIronCost + "\n\t Food: " + constructBuilding.GetComponent <ConstructBuilding>().houseFoodCost;
		storageCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().storageWoodCost + "\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().storageRockCost;
		outpostCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().outpostWoodCost + "\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().outpostRockCost + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().outpostIronCost;
		barracksCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().BarrWCost + "\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().BarrRCost + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().BarrICost + "\n\t Gold: " + constructBuilding.GetComponent <ConstructBuilding>().BarrGCost + "\n\t Hide: " + constructBuilding.GetComponent <ConstructBuilding>().BarrHCost;
		TPCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().TPWoodCost + "\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().TPRockCost + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().TPIronCost + "\n\t Food: " + constructBuilding.GetComponent <ConstructBuilding>().TPFoodCost + "\n\t Gold: " + constructBuilding.GetComponent <ConstructBuilding>().TPGoldCost + "\n\t Hide: " + constructBuilding.GetComponent <ConstructBuilding>().TPHideCost;
		TCCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().TCWCost + "\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().TCRCost + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().TCICost + "\n\t Gold: " + constructBuilding.GetComponent <ConstructBuilding>().TCGCost;
		bankCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().BWCost + "\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().BRCost + "\n\t Iron: "+ constructBuilding.GetComponent <ConstructBuilding>().BICost + "\n\t Food: "+ constructBuilding.GetComponent <ConstructBuilding>().BFCost + "\n\t Gold: "+ constructBuilding.GetComponent <ConstructBuilding>().BGCost;
		marketCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().marketWoodCost + "\n\t Rock: "+ constructBuilding.GetComponent <ConstructBuilding>().marketRockCost + "\n\t Iron: "+ constructBuilding.GetComponent <ConstructBuilding>().marketIronCost + "\n\t Food: "+ constructBuilding.GetComponent <ConstructBuilding>().marketFoodCost;
		farmCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().FWCost + "\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().FRCost + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().FICost + "\n\t Food: " + constructBuilding.GetComponent <ConstructBuilding>().FFCost + "\n\t Hide: " + constructBuilding.GetComponent <ConstructBuilding>().FHCost;
		BSCost.text = "Cost: \n\tWood: " + constructBuilding.GetComponent <ConstructBuilding>().BSWoodCost + "\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().BSRockCost + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().BSIronCost + "\n\t Food: " + constructBuilding.GetComponent <ConstructBuilding>().BSFoodCost + "\n\t Gold: " + constructBuilding.GetComponent <ConstructBuilding>().BSGoldCost + "\n\t Hide: " + constructBuilding.GetComponent <ConstructBuilding>().BSHideCost;
		tavernCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().tavernWoodCost + "\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().tavernRockCost + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().tavernIronCost + "\n\t Food: " + constructBuilding.GetComponent <ConstructBuilding>().tavernFoodCost + "\n\t Gold: " + constructBuilding.GetComponent <ConstructBuilding>().tavernGoldCost + "\n\t Hide: " + constructBuilding.GetComponent <ConstructBuilding>().tavernHideCost;
		MSCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().MSWCost + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().MSICost + "\n\t Gold: " + constructBuilding.GetComponent <ConstructBuilding>().MSGCost + "\n\t Hide: " + constructBuilding.GetComponent <ConstructBuilding>().MSHCost;
		ACCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().ACWCost + "\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().ACRCost + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().ACICost + "\n\t Food: " + constructBuilding.GetComponent <ConstructBuilding>().ACFCost + "\n\t Gold: " + constructBuilding.GetComponent <ConstructBuilding>().ACGCost;
		castleCost.text = "Cost:\n\t Wood:" + constructBuilding.GetComponent <ConstructBuilding>().CWCost + "\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().CRCost + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().CICost + "\n\t Gold: " + constructBuilding.GetComponent <ConstructBuilding>().CGCost;
		PUBCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().PUBWoodCost + "\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().PUBRockCost + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().PUBIronCost + "\n\t Gold: " + constructBuilding.GetComponent <ConstructBuilding>().PUBGoldCost;
		//defence building cost updates
		DTCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().DTWCost + "\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().DTRCost + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().DTICost + "\n\t Gold: " + constructBuilding.GetComponent <ConstructBuilding>().DTGCost;
		BTCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().BTWCost + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().BTICost;
		spikesCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().SpikesWoodCost;
		WWCost.text = "Cost:\n\t Wood: " + constructBuilding.GetComponent <ConstructBuilding>().WWallCost;
		RWCost.text = "Cost:\n\t Rock: " + constructBuilding.GetComponent <ConstructBuilding>().RWallCostRock + "\n\t Iron: " + constructBuilding.GetComponent <ConstructBuilding>().RWallCostIron;
	}

}
