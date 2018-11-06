using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenceTower : MonoBehaviour {

	public GameObject CannonBall;
	public int fireRate, damage, balls, maxBalls, ironPerBall;

	private bool recycled;
	private Text CBInfo;
	private List<GameObject> enemies;
	private GameObject CannonBallSpawnLoc, Master, cam, exclamationPoint, myCanvas, troopToShoot;
	private int x, temp, iron;

	void Start(){
		Master = GameObject.Find ("GameMaster");
		cam = GameObject.Find ("Main Camera");
		enemies = new List<GameObject> ();
		CannonBallSpawnLoc = gameObject.transform.parent.Find ("CBSpawnLocation").gameObject;
		exclamationPoint = gameObject.transform.parent.Find ("Exclamation point").gameObject;
		exclamationPoint.SetActive (false);
		myCanvas = gameObject.transform.parent.Find ("Canvas").gameObject;
		CBInfo = myCanvas.transform.Find ("CBInfo").GetComponent <Text> ();
		CBInfo.text = "Balls: " + balls + "/" + maxBalls + "\r\n Iron per ball: " + ironPerBall;
		myCanvas.SetActive (false);
	}

	void Update(){
		if(enemies.Count > 0 && gameObject.transform.parent.gameObject.GetComponent <BuildHammer>().isBuilt){
			if(x >= fireRate){
				if(balls > 0){
					GameObject curr = Instantiate (CannonBall, CannonBallSpawnLoc.transform.position, Quaternion.identity);
					curr.GetComponent <CannonBall>().Shoot (enemies[Random.Range (0, enemies.Count)], damage, gameObject);
					curr.gameObject.transform.localScale = new Vector3 (curr.gameObject.transform.localScale.x, curr.gameObject.transform.localScale.y/gameObject.transform.parent.localScale.y, curr.gameObject.transform.localScale.z);
					x = 0;
					balls--;
					CBInfo.text = "Balls: " + balls + "/" + maxBalls + "\r\n Iron per ball: " + ironPerBall;

				}else{
					exclamationPoint.SetActive (true);
				}
			}else{
				x++;
			}
		}
	}

	public void OnTriggerEnter(Collider entered){
		if(gameObject.transform.parent.gameObject.GetComponent <BuildHammer>().isBuilt){
			if(gameObject.layer == 11){//friendly building
				if(entered.gameObject.layer == 9){ //enemy layer
					enemies.Add (entered.gameObject);
					Debug.Log ("Enemy entered");
				}
			}else if(gameObject.layer == 10){//enemy building
				if(entered.gameObject.layer == 8){ //friendly layer
					enemies.Add (entered.gameObject);
				}
			}	
		}
	}

	public void OnTriggerExit(Collider exited){
		if(gameObject.transform.parent.gameObject.GetComponent <BuildHammer>().isBuilt){
			if(gameObject.layer == 11){//friendly building
				if(exited.gameObject.layer == 9){ //enemy layer
					enemies.Remove (exited.gameObject);
					Debug.Log ("Enemy exited");
				}
			}else if(gameObject.layer == 10){//enemy building
				if(exited.gameObject.layer == 8){ //friendly layer
					enemies.Remove (exited.gameObject);
				}
			}	
		}
	}

	public void TroopDied(GameObject troop){
		enemies.Remove (troop);
		Debug.Log ("Removed troop from list");
	}

	public void AddBalls(string amount){
		int.TryParse (amount, out iron);
		temp = (int)(iron / ironPerBall);
		if(cam.GetComponent <PlayerStats>().pIron >= iron){
			if(balls + temp <= maxBalls){
				balls += temp;
				cam.GetComponent <PlayerStats> ().pIron -= iron;
				cam.GetComponent <PLayerUIHandler> ().UpdateText ("Iron", cam.GetComponent <PlayerStats> ().pIron, cam.GetComponent <PlayerStats> ().maxIron);
				CBInfo.text = "Balls: " + balls + "/" + maxBalls + "\r\n Iron per ball: " + ironPerBall;

			}else{
				CBInfo.text = "Balls: " + balls + "/" + maxBalls + "\r\n Iron per ball: " + ironPerBall;
				exclamationPoint.SetActive (false);
				Master.GetComponent<GameMaster>().PassErrorMessage ("This defence tower is at maximum cannon ball capacity.");
			}
		}else{
			Master.GetComponent<GameMaster> ().PassErrorMessage ("You do not have enough iron in your inventory.");
		}
	}

	public void OpenMenu(){
		if(gameObject.transform.parent.gameObject.GetComponent <BuildHammer>().isBuilt){
			Master.GetComponent <GameMaster> ().DisablePlayer ();
			myCanvas.SetActive (true);	
			GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (true);
		}
	}

	public void CloseMenu(){
		myCanvas.SetActive (false);
		Master.GetComponent <GameMaster>().EnablePlayer ();
		GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (false);
	}

	public void BuildingDiedProtocol(){
		if(recycled){
			iron += (balls * ironPerBall);
			if(Master.GetComponent <GameMaster>().CalculateStorage () + iron <= Master.GetComponent <GameMaster>().storage){
				Master.GetComponent <GameMaster> ().Add ("Iron", iron);
			}
		}
	}

	public void DestroyBuilding(){
		CloseMenu ();
		recycled = true;
		BuildingDiedProtocol ();
		Destroy (gameObject.transform.parent.gameObject, 0.1f);
	}

}
