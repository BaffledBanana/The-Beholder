using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PLayerUIHandler : MonoBehaviour {
		
		public Text woodText, rockText, ironText, foodText, goldText, hideText, armorText, xpText, xpPointsText;
		public GameObject armorBar, xpBar;
		private float amountF, maxF;

	void Awake () {
		if(GameObject.Find ("GUIHUD") != null){
			woodText = GameObject.Find ("WoodText").GetComponent<Text> ();
			rockText = GameObject.Find ("RockText").GetComponent<Text> ();
			ironText = GameObject.Find ("IronText").GetComponent<Text> ();
			foodText = GameObject.Find ("FoodText").GetComponent<Text> ();
			goldText = GameObject.Find ("GoldText").GetComponent<Text> (); 
			hideText = GameObject.Find ("HideText").GetComponent<Text> (); 
			armorBar = GameObject.Find ("ArmorBar");
			xpBar = GameObject.Find ("XpBar");
		}
	}

	public void UpdateText(string name, int amount, int max){
		if(name == "Wood"){
			woodText.text = amount + "/" + max;
		}
		else if (name == "Rock"){
			rockText.text = amount + "/" + max;
		}
		else if (name == "Iron"){
			ironText.text = amount + "/" + max;
		}
		else if (name == "Food"){
			foodText.text = amount + "/" + max;
		}
		else if (name == "Gold"){
			goldText.text = amount + "/" + max;
		}
		else if (name == "Hide"){
			hideText.text = amount + "/" + max;
		}
		else if (name == "XpPoints"){
			xpPointsText.text = "Points: " + amount;
		}
		else if (name == "Armor"){
			armorText.text = "Armor: " + amount + "/" + max;
			amountF = amount;
			maxF = max;
			armorBar.transform.localScale = new Vector3 (amountF/maxF, 0.5750015f, 1);
		}
	}

	public void UpdateXp(int amount, int nextLevel, int level){
		amountF = amount;
		maxF = nextLevel;
		xpBar.transform.localScale = new Vector3 (amountF / maxF, 0.5750015f, 1);
		xpText.text = "Experience: " + amount + "/" + nextLevel + " Level: " + level;
	}

}
