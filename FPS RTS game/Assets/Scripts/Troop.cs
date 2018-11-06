using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop : MonoBehaviour {

	public float moveUpOffset, reasonableDistanceToSpot, reasonableDistanceToBuilding, reasonableDistanceToEnemy, RDTPlayer, normalStep, PAStep, moveSpeed, step, checkRange;
	public int health, maxHealth, minStrength, maxStrength, attackTimer, PATimer, surrCheckTimer;
	public LayerMask mask, ground;
	public bool wasAttackingBase;

	private RaycastHit hit;
	private GameObject enemy, squad, cam, Master, troopHealthLog;
	private Vector3 spot;
	private bool doMove, doCharge, doAttackEnemy, doAttackPlayer;
	private int x, y, z;
	private Collider[] objectsInRange;

	// Use this for initialization
	void Awake () {
		troopHealthLog = gameObject.transform.Find ("TroopHealthLog").gameObject;
		cam = GameObject.Find ("Main Camera");
		Master = GameObject.Find ("GameMaster");
		squad = null;
		//reasonableDistanceToEnemy = gameObject.transform.localScale.x + 0.1f;
		moveUpOffset = gameObject.transform.localScale.y;//if you're having problems with the troops going into the ground just delete this and set it manually
	}
	void Start(){
		troopHealthLog.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if(z >= surrCheckTimer){
			z = 0;
			CheckSurroundings ();
			//Debug.Log ("Cycled!");
		}else{
			z++;
		}
		if(doMove){
			if(x >= moveSpeed){
				x = 0;
				transform.position = Vector3.MoveTowards (gameObject.transform.position, spot, step);
				gameObject.transform.LookAt (spot);
				gameObject.transform.localEulerAngles = new Vector3 (0, transform.localEulerAngles.y, transform.localEulerAngles.z);
				if(gameObject.transform.position == spot || Vector3.Distance (gameObject.transform.position, spot) <= reasonableDistanceToSpot){
					doMove = false;
				}
			}else{
				x++;
			}
		}
		if(doCharge){
			if(enemy != null){
				if(Vector3.Distance (gameObject.transform.position, spot) <= reasonableDistanceToBuilding){
					if(x >= attackTimer){
						if(Attack ("Building", enemy)){
							x = 0;
						}else{
							doCharge = false;
							x = 0;
						}
					}else{
						x++;
					}
				}else{
					transform.position = Vector3.MoveTowards (gameObject.transform.position, spot, step);
					gameObject.transform.LookAt (enemy.transform);
					gameObject.transform.localEulerAngles = new Vector3 (0, transform.localEulerAngles.y, transform.localEulerAngles.z);
				}
			}
		}
		if(doAttackEnemy){
			//Debug.Log (doAttackEnemy);
			if(enemy != null){
				//Debug.Log (doAttackEnemy);
				if(Vector3.Distance (gameObject.transform.position, enemy.transform.position) <= reasonableDistanceToEnemy){
					if(x >= attackTimer){
						if(Attack ("Enemy", enemy)){
							x = 0;
						}else{
							doAttackEnemy = false;
							x = 0;
						}
					}else{
						x++;
					}
				}else{
					transform.position = Vector3.MoveTowards (gameObject.transform.position, enemy.transform.position, step);
					gameObject.transform.LookAt (enemy.transform);
					gameObject.transform.localEulerAngles = new Vector3 (0, transform.localEulerAngles.y, transform.localEulerAngles.z);
				}
			}
		}
		if(doAttackPlayer){
			if(enemy != null){
				if(Vector3.Distance (gameObject.transform.position, enemy.transform.position) <= RDTPlayer){
					if(x >= PATimer){
						if(AttackPlayer()){
							x = 0;
						}else{
							doAttackPlayer = false;
							x = 0;
						}
					}else{
						x++;
					}
				}else{
					transform.position = Vector3.MoveTowards (gameObject.transform.position, enemy.transform.position, step);
					gameObject.transform.LookAt (enemy.transform);
					gameObject.transform.localEulerAngles = new Vector3 (0, transform.localEulerAngles.y, transform.localEulerAngles.z);
					//Debug.Log (doAttackEnemy);
				}
			}
		}
		gameObject.transform.position = new Vector3(gameObject.transform.position.x ,gameObject.transform.localScale.y, gameObject.transform.position.z);
	}

	public void Move(Vector3 place){
		enemy = null;
		squad = null;
		doCharge = false;
		doAttackEnemy = false;
		doAttackPlayer = false;
		place.y += moveUpOffset;
		spot = place;
		doMove = true;
		Debug.Log ("Was told to move");
	}

	public void Stop(){
		enemy = null;
		squad = null;
		doMove = false;
		doCharge = false;
		doAttackEnemy = false;
		doAttackPlayer = false;
	}

	public void Charge(GameObject curr){
		enemy = null;
		squad = null;
		doMove = false;
		doCharge = false;
		doAttackEnemy = false;
		if(curr.layer == 10 || curr.layer == 11){//enemy building layer or the friendly building layer
			enemy = curr;
			reasonableDistanceToBuilding = enemy.GetComponent <Renderer>().bounds.extents.magnitude;
			doCharge = true;
			spot = new Vector3(curr.transform.position.x, gameObject.transform.position.y, curr.transform.position.z);
		}else if(curr.layer == 9){//enemy layer
			if(curr.transform.parent != null){
				squad = curr.transform.parent.gameObject;
				enemy = squad.transform.GetChild (Random.Range (0, squad.transform.childCount - 1)).gameObject;
				//reasonableDistanceToEnemy = enemy.GetComponent <Renderer> ().bounds.center.magnitude;
				reasonableDistanceToEnemy = 0.7f;
				doAttackEnemy = true;
			}else{
				enemy = curr;
				reasonableDistanceToEnemy = 0.7f;
				doAttackEnemy = true;
			}
		}
		else if(curr.layer == 12){//player layer
			enemy = curr;

			doAttackPlayer = true;
		}
	}

	bool Attack(string name, GameObject target){
		if(name == "Building"){
			if(target.GetComponent <BuildingHealth>() != null){
				if(target.GetComponent <BuildingHealth>().TakeDmg (Random.Range (minStrength, maxStrength))){//if returns true, it's been destroyed
					return false;
				}else{
					return true;
				}
			}else{
				return false;
			}
		}else if(name == "Enemy"){
			if(target.GetComponent <Troop>().TakeDmg (Random.Range (minStrength, maxStrength), gameObject)){//returns true means it died
				if(squad != null){
					if(squad.transform.childCount > 0){
						//Debug.Log (squad.transform.childCount);
						enemy = squad.transform.GetChild (Random.Range (0, squad.transform.childCount)).gameObject;
						//Debug.Log ("Found another enemy: " + enemy.name);
						return true;//means keep attacking
					}else{
						//Destroy (squad);
						//Find a way to destroy the squad empty gameobject after it has no enemies in it
						return false;//means stop attacking
					}
				}else{
					return false;//means stop attacking
				}
			}else{
				target.GetComponent <Troop>().DisplayHealth ();
				return true;//means keep attacking
			}
		}else{
			Debug.Log (gameObject.name + "Misspelled something");
			return false;//this is just if i misspell something, it doesnt contribute anything, it is only here to please the C# gods and maybe they will have mercy on my soul with their whip of errorings
		}
	}

	public bool TakeDmg(int amount, GameObject attacker){
		if(health - amount > 0){
			health -= amount;
			//should begin attacking back at the attacker
				if(gameObject.layer == 8){//this is a friendly troop
					if(attacker.layer == 9){//if the attacker is an enemy troop
						if(enemy == null){
							enemy = attacker;
							squad = enemy.transform.parent.gameObject;
							doAttackEnemy = true;
						step = normalStep;
						attacker.GetComponent <Troop>().DisplayHealth ();
						}
					}
				}else if(gameObject.layer == 9){//this is an enemy troop
					if(attacker.layer == 8){//if the attacker is a friendly troop
						if(enemy == null){
							enemy = attacker;//there might be an error that means a squad wasnt found when searching for squad because friendlies dont have squads
						squad = null;
						doAttackEnemy = true;
						step = normalStep;
						attacker.GetComponent <Troop>().DisplayHealth ();
						}
					}
				}
				if(gameObject.layer == 9){//this is an enemy troop
					if(attacker.layer == 12){//if the attacker is me
						doAttackEnemy = false;
						doCharge = false;
						doMove = false;
					step = PAStep;
						enemy = attacker;//there might be an error that means a squad wasnt found when searching for squad because friendlies dont have squads
						doAttackPlayer = true;
					}
				}
			if(gameObject.layer == 9){//this is an enemy troop
				if(attacker.layer == 11){//if the attacker is a friendly building
					spot = new Vector3(enemy.transform.position.x, gameObject.transform.position.y, enemy.transform.position.z);
					enemy = attacker;//there might be an error that means a squad wasnt found when searching for squad because friendlies dont have squads
					doCharge = true;
					doMove = false;
					doAttackEnemy = false;
					doAttackPlayer = false;
					step = normalStep;
				}
			} if(gameObject.layer == 8){//this is a friendly troop
				if(attacker.layer == 10){//if the attacker is an enemy building
					enemy = attacker;//there might be an error that means a squad wasnt found when searching for squad because friendlies dont have squads
					spot = new Vector3(enemy.transform.position.x, gameObject.transform.position.y, enemy.transform.position.z);
					doCharge = true;
					doMove = false;
					doAttackEnemy = false;
					doAttackPlayer = false;
					step = normalStep;
				}
			}
			DisplayHealth ();
			troopHealthLog.GetComponent <TroopHealthLog>().UpdateHealth (health, maxHealth);
			return false;//means it lives
		}else{
			//Debug.Log ("Unit has died! : " + gameObject.name + " / " + gameObject.tag + "s" + " -- also added 25 xp");
			cam.GetComponent <PlayerStats>().AddXp (25);
			Master.GetComponent <GameMaster>().Subtract (gameObject.tag + "s", 1);
			if(attacker.GetComponent <Troop>() != null){
				attacker.GetComponent <Troop>().DisplayHealth ();
			}
			DisplayHealth ();
			troopHealthLog.GetComponent <TroopHealthLog>().UpdateHealth (0, maxHealth);
			if(GameObject.FindGameObjectsWithTag ("SSTroop").Length == 0 && GameObject.FindGameObjectsWithTag ("ShieldTroop").Length == 0){
				Master.GetComponent <GameMaster>().GameOver ("Congratulations! You have defeated the enemy! The game will now restart!");
			}
			Destroy (gameObject);
			return true;//means it died
		}
	}
	public bool AttackPlayer(){
		if(enemy.GetComponent <PlayerStats>().TakeDamage (Random.Range (minStrength, maxStrength))){
			return false;
		}else{
			return true;
		}
	}

	public bool Shot(int dmg, GameObject tower){
		DisplayHealth ();
		if(health - dmg > 0){
			health -= dmg;
			troopHealthLog.GetComponent <TroopHealthLog>().UpdateHealth (health, maxHealth);
			return false;
		}else{
			tower.GetComponent <DefenceTower>().TroopDied (gameObject);
			troopHealthLog.GetComponent <TroopHealthLog>().UpdateHealth (0, maxHealth);
			Destroy (gameObject);
			return true;
		}
	}

	public void Select(){
		troopHealthLog.SetActive (true);
	}

	public void Deselect(){
		if(gameObject.layer == 8){//friendly
			troopHealthLog.SetActive (false);
		}
	}

	public void DisplayHealth(){
		troopHealthLog.SetActive (true);
	}

	public void CheckSurroundings(){
		objectsInRange = Physics.OverlapSphere (transform.position, checkRange, mask, QueryTriggerInteraction.UseGlobal);
		//Debug.Log ("Troop: Checked surroundings!");
		if(objectsInRange.Length > 0){
			//Debug.Log ("Troop: Detected something of interest: " + objectsInRange.Length);
			foreach(Collider detected in objectsInRange){
				//Debug.Log (detected.gameObject.name);
				if(detected.gameObject.layer == 9 && gameObject.layer == 8){//spotted enemy and this is friendly
					//Debug.Log ("Troop: i am friendly and spotted enemy troop");
					if(wasAttackingBase){
						enemy = null;
						squad = null;
						doMove = false;
						doCharge = false;
						doAttackEnemy = false;
						doAttackPlayer = false;
						enemy = detected.gameObject;
						squad = null;
						reasonableDistanceToEnemy = 0.7f;
						Attack ("Enemy", enemy);
						break;
					}else{
						Charge (detected.gameObject);
						break;
					}
				}
				else if(detected.gameObject.layer == 8 && gameObject.layer == 9){//spotted friendly and this is enemy
					//Debug.Log ("Troop: i am enemy and spotted friendly");
					if(wasAttackingBase){
						enemy = null;
						squad = null;
						doMove = false;
						doCharge = false;
						doAttackEnemy = false;
						doAttackPlayer = false;
						enemy = detected.gameObject;
						squad = null;
						reasonableDistanceToEnemy = 0.7f;
						Attack ("Enemy", enemy);
						break;
					}else{
						Charge (detected.gameObject);
						break;
					}
				} else if (detected.gameObject.layer == 12 && gameObject.layer == 9) { //spotted me and this is enemy
					//Debug.Log ("Troop: i am enemy and spotted player");
					if(wasAttackingBase){
						enemy = null;
						squad = null;
						doMove = false;
						doCharge = false;
						doAttackEnemy = false;
						doAttackPlayer = false;
						enemy = detected.gameObject.transform.Find ("Main Camera").gameObject;
						squad = null;
						AttackPlayer ();
						//Debug.Log ("Troop: i was attacking base, but now will attack player");
						break;
					}else{
						//Charge (detected.gameObject);
						enemy = detected.gameObject.transform.Find ("Main Camera").gameObject;
						doAttackPlayer = true;
						AttackPlayer ();
						//Debug.Log ("Troop: i wasnt attacking base and will now attack player");
						break;
					}
				}
				else if(detected.gameObject.layer == 11 && gameObject.layer == 9){//spotted friendly building and this is enemy
					if(wasAttackingBase){
						enemy = null;
						squad = null;
						doMove = false;
						doCharge = false;
						doAttackEnemy = false;
						doAttackPlayer = false;
						enemy = detected.gameObject;
						squad = null;
						Attack ("Building", enemy);
						break;
					}else{
						Charge (detected.gameObject);
						break;
					}
				}
				else if(detected.gameObject.layer == 10 && gameObject.layer == 8){//spotted enemy building and this is friendly
					if(wasAttackingBase){
						enemy = null;
						squad = null;
						doMove = false;
						doCharge = false;
						doAttackEnemy = false;
						doAttackPlayer = false;
						enemy = detected.gameObject;
						squad = null;
						Attack ("Building", enemy);
						break;
					}else{
						Charge (detected.gameObject);
						break;
					}
				}else if(wasAttackingBase && enemy == null){
					enemy = GameObject.Find ("BaseBuilding");
					Charge (enemy);
				}
			}
		}
	}

}
