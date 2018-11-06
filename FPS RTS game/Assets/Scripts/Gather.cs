using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gather : MonoBehaviour {

		private RaycastHit hit;
		private float distance;
	public AudioClip woodSound, rockSound, ironSound, foodSound;
	public AudioSource sound;
		public bool canMineWood = true, canMineRock = true, canMineIron = true, canMineFood = true, canMineHide = true, canGather = true;//make private because you dont need to see this
		private int amount = 0, current = 0, x = 0, max = 0;
		public int gatherSpeed, minXp, maxXp;
		public int maxWood, minWood, maxRock, minRock, maxIron, minIron, maxFood, minFood, maxHide, minHide;//GATHERING INFO

		
	void Start () {
		sound = gameObject.transform.parent.gameObject.GetComponent <AudioSource> ();
		sound.clip = woodSound;
		gatherSpeed = gameObject.GetComponent <PlayerStats> ().gatherSpeed;
		//WOOD SETUP
				current = gameObject.GetComponent <PlayerStats> ().Add ("Wood", 0);
				max = gameObject.GetComponent <PlayerStats>().maxWood;
				gameObject.GetComponent<PLayerUIHandler> ().UpdateText ("Wood", current, max);
		//ROCK SETUP
				current = gameObject.GetComponent <PlayerStats> ().Add ("Rock", 0);
				max = gameObject.GetComponent <PlayerStats>().maxRock;
				gameObject.GetComponent<PLayerUIHandler> ().UpdateText ("Rock", current, max);
		//IRON SETUP
				current = gameObject.GetComponent <PlayerStats> ().Add ("Iron", 0);
				max = gameObject.GetComponent <PlayerStats>().maxIron;
				gameObject.GetComponent<PLayerUIHandler> ().UpdateText ("Iron", current, max);
		//FOOD SETUP
				current = gameObject.GetComponent <PlayerStats> ().Add ("Food", 0);
				max = gameObject.GetComponent <PlayerStats>().maxFood;
				gameObject.GetComponent<PLayerUIHandler> ().UpdateText ("Food", current, max);
		//HIDE SETUP
				current = gameObject.GetComponent <PlayerStats> ().Add ("Hide", 0);
				max = gameObject.GetComponent <PlayerStats>().maxHide;
				gameObject.GetComponent<PLayerUIHandler> ().UpdateText ("Hide", current, max);
		//GOLD SETUP
				current = gameObject.GetComponent <PlayerStats> ().Add ("Gold", 0);
				max = gameObject.GetComponent <PlayerStats>().maxGold;
				gameObject.GetComponent<PLayerUIHandler> ().UpdateText ("Gold", current, max);
	}

	public void UpdateGatherSpeed(int newSpeed){
		gatherSpeed = newSpeed;
	}

	void Update () {
		x++;
		if(canGather){
			if(Input.GetKey (KeyCode.Mouse0)){
				if(x >= gatherSpeed){
					distance = gameObject.GetComponent <PlayerStats> ().distance;
					if(Physics.Raycast (gameObject.transform.position, transform.forward, out hit, distance)){
						if(hit.collider.gameObject.CompareTag ("Wood")){
							sound.clip = woodSound;
							sound.PlayDelayed (0.25f);
							GetComponent <Interact>().PlayPickSwing ();
							amount = hit.collider.GetComponent<Resource>().Mine (minWood, maxWood, gameObject);
							current = gameObject.GetComponent <PlayerStats> ().Add ("Wood", amount);
							max = gameObject.GetComponent <PlayerStats>().maxWood;
							gameObject.GetComponent<PLayerUIHandler> ().UpdateText ("Wood", current, max);
							gameObject.GetComponent <PlayerStats>().AddXp(Random.Range (minXp, maxXp));
							if(current >= max){
								canMineWood = false;
							}
						}
						else if(hit.collider.gameObject.CompareTag ("Rock")){
							sound.clip = rockSound;
							sound.PlayDelayed (0.25f);
							GetComponent <Interact>().PlayPickSwing ();
							amount = hit.collider.GetComponent<Resource>().Mine (minRock, maxRock, gameObject);
							current = gameObject.GetComponent <PlayerStats> ().Add ("Rock", amount);
							max = gameObject.GetComponent <PlayerStats>().maxRock;
							gameObject.GetComponent<PLayerUIHandler> ().UpdateText ("Rock", current, max);
							gameObject.GetComponent <PlayerStats>().AddXp(Random.Range (minXp, maxXp));
							if(current >= max){
								canMineRock = false;
							}
						}
						else if(hit.collider.gameObject.CompareTag ("Iron")){
							sound.clip = ironSound;
							sound.PlayDelayed (0.25f);
							GetComponent <Interact>().PlayPickSwing ();
							amount = hit.collider.GetComponent<Resource>().Mine (minIron, maxIron, gameObject);
							current = gameObject.GetComponent <PlayerStats> ().Add ("Iron", amount);
							max = gameObject.GetComponent <PlayerStats>().maxIron;
							gameObject.GetComponent<PLayerUIHandler> ().UpdateText ("Iron", current, max);
							gameObject.GetComponent <PlayerStats>().AddXp(Random.Range (minXp, maxXp));
							if(current >= max){
								canMineIron = false;
							}
						}
						else if(hit.collider.gameObject.CompareTag ("Hide")){
							amount = hit.collider.GetComponent<Resource>().Mine (minHide, maxHide, gameObject);
							current = gameObject.GetComponent <PlayerStats> ().Add ("Hide", amount);
							max = gameObject.GetComponent <PlayerStats>().maxHide;
							gameObject.GetComponent<PLayerUIHandler> ().UpdateText ("Hide", current, max);
							gameObject.GetComponent <PlayerStats>().AddXp(Random.Range (minXp, maxXp));
							if(current >= max){
								canMineHide = false;
							}
						}
						else if(hit.collider.gameObject.CompareTag ("Food")){
							sound.clip = foodSound;
							sound.PlayDelayed (0.25f);
							GetComponent <Interact>().PlayPickSwing ();
							amount = hit.collider.GetComponent<Resource>().Mine (minFood, maxFood, gameObject);
							current = gameObject.GetComponent <PlayerStats> ().Add ("Food", amount);
							max = gameObject.GetComponent <PlayerStats>().maxFood;
							gameObject.GetComponent<PLayerUIHandler> ().UpdateText ("Food", current, max);
							gameObject.GetComponent <PlayerStats>().AddXp(Random.Range (minXp, maxXp));
							if(current >= max){
								canMineFood = false;
							}
						}
						x = 0;
					}
				}
			}
		}
	}
}
