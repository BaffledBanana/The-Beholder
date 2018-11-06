using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalValuesUI : MonoBehaviour {

		private Text woodText, rockText, foodText, ironText, populationText, goldText, workersText, hideText, happynessText, storageText;
		private GameObject Master, happynessBar;

		void Awake(){
		Master = GameObject.Find ("GameMaster");
		woodText = gameObject.transform.Find ("gWoodText").GetComponent <Text> (); //IMPORTANT this is what the children of GlobalValuesUI should be called
		foodText = gameObject.transform.Find ("gFoodText").GetComponent <Text> ();
		rockText = gameObject.transform.Find ("gRockText").GetComponent <Text> ();
		ironText = gameObject.transform.Find ("gIronText").GetComponent <Text> ();
		populationText = gameObject.transform.Find ("gPopulationText").GetComponent <Text> ();
		goldText = gameObject.transform.Find ("gGoldText").GetComponent <Text> ();
		workersText = gameObject.transform.Find ("gWorkersText").GetComponent <Text> ();
		hideText = gameObject.transform.Find ("gHideText").GetComponent <Text> ();
		happynessBar = gameObject.transform.Find ("HappynessBar").gameObject;
		happynessText = gameObject.transform.Find ("HappynessText").GetComponent <Text> ();
		storageText = gameObject.transform.Find ("StorageText").GetComponent <Text> ();
		}

		void Start(){
			GetValues ();
		}

		public void UpdateText(string name, int amount, int max){
			if(name == "Wood"){
			woodText.text = amount.ToString ();
			}
			else if(name == "Rock"){
			rockText.text = amount.ToString ();
			}
		else if(name == "Food"){
			foodText.text = amount.ToString ();
			}
		else if(name == "Iron"){
			ironText.text = amount.ToString ();
			}
		else if(name == "Gold"){
			goldText.text = amount.ToString ();
			}
		else if(name == "Population"){
			populationText.text = "Population: " + amount + "/" + max;
			}
		else if(name == "Workers"){
			workersText.text = "Workers: " + amount + "/" + max;
			}
		else if(name == "Hide"){
			hideText.text = amount.ToString ();
		}
		else if(name == "Happyness"){
			happynessText.text = "Happyness: " + amount;
			happynessBar.transform.localScale = new Vector3((float)(amount/ 100f) , 0.5624871f, 1);
		}
		else if(name == "Storage"){
			storageText.text = "Storage: " + amount + "/" + max;
		}
		}

		public void GetValues(){
		Master.GetComponent <GameMaster>().UpdateValues ();
		}
}
