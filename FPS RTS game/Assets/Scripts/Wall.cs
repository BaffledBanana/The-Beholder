using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wall : MonoBehaviour {

	public bool wood, rock, isWall, isPlaceholder, isDebugged;
	public int woodWallDecayTime, rockWallDecayTime, WWFixCost, RWFixCost, IWFixCost, WWHideCost, WWWoodCost, RWRockCost, RWIronCost;

	private GameObject cam, myCanvas, Master;
	private int x;
	private Text status;

	// Use this for initialization
	void Start () {
		cam = GameObject.Find ("Main Camera");
		if(isDebugged == false){
			gameObject.transform.LookAt (cam.transform);
			gameObject.transform.Rotate (0, 90, 0);
			//gameObject.transform.localEulerAngles = new Vector3 (0, transform.localEulerAngles.y, transform.localEulerAngles.z);
			gameObject.transform.localEulerAngles = new Vector3 (0, transform.localEulerAngles.y, 0);
		}
		if(isWall){
			myCanvas = gameObject.transform.Find ("Canvas").gameObject;
			myCanvas.SetActive (false);
			Master = GameObject.Find ("GameMaster");
			status = myCanvas.transform.Find ("Status").GetComponent <Text> ();
			if(wood){
				gameObject.GetComponent <BuildingHealth>().health = gameObject.GetComponent <BuildingHealth>().maxHealth;
				status.text = "Wall strength/max strength \r\n" + gameObject.GetComponent <BuildingHealth>().health + "/" + gameObject.GetComponent <BuildingHealth>().maxHealth + "\r\n To fix wall: " + WWFixCost + " wood" + "\r\n to upgrade wall: " + WWWoodCost + " wood and " + WWHideCost + " hide";
				myCanvas.name = "WallCanvas";
				myCanvas.transform.parent = GameObject.Find ("WallMenus").transform;
			}else if(rock){
				gameObject.GetComponent <BuildingHealth>().health = gameObject.GetComponent <BuildingHealth>().maxHealth;
				status.text = "Wall strength/max strength \r\n" + gameObject.GetComponent <BuildingHealth>().health + "/" + gameObject.GetComponent <BuildingHealth>().maxHealth + "\r\n To fix wall: " + RWFixCost + " rock and " + IWFixCost + " iron" + "\r\n to upgrade wall: " + RWRockCost + " rock and " + RWIronCost + " iron";
				myCanvas.name = "WallCanvas";
				myCanvas.transform.parent = GameObject.Find ("WallMenus").transform;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(isPlaceholder){
			gameObject.transform.LookAt (cam.transform);
			gameObject.transform.Rotate (0, 90, 0);
			gameObject.transform.localEulerAngles = new Vector3 (0, transform.localEulerAngles.y, transform.localEulerAngles.z);
		}
		if(wood){
			if(x >= woodWallDecayTime){
				if(gameObject.GetComponent <BuildingHealth>().gameObject.GetComponent <BuildingHealth>().health > 0){
					gameObject.GetComponent <BuildingHealth>().health--;
					status.text = "Wall strength/max strength \r\n" + gameObject.GetComponent <BuildingHealth>().health + "/" + gameObject.GetComponent <BuildingHealth>().maxHealth + "\r\n To fix wall: " + WWFixCost + " wood" + "\r\n to upgrade wall: " + WWWoodCost + " wood and " + WWHideCost + " hide";
				}else{
					Master.GetComponent <GameMaster>().PassErrorMessage ("Attention! One of your walls has decayed due to ageing!");
					Destroy (gameObject);
				}
				x = 0;
			}else{
				x++;
			}
		}else if(rock){
			if(x >= rockWallDecayTime){
				if(gameObject.GetComponent <BuildingHealth>().health > 0){
					gameObject.GetComponent <BuildingHealth>().health--;
					status.text = "Wall strength/max strength \r\n" + gameObject.GetComponent <BuildingHealth>().health + "/" + gameObject.GetComponent <BuildingHealth>().maxHealth + "\r\n To fix wall: " + RWFixCost + " rock and " + IWFixCost + " iron" + "\r\n to upgrade wall: " + RWRockCost + " rock and " + RWIronCost + " iron";
				}else{
					Master.GetComponent <GameMaster>().PassErrorMessage ("Attention! One of your walls has decayed due to ageing!");
					Destroy (gameObject);
				}
				x = 0;
			}else{
				x++;
			}
		}
	}

	public void UpgradeBuilding(){
		if(wood){
			if(Master.GetComponent <GameMaster>().gWood >= WWWoodCost && Master.GetComponent <GameMaster>().gHide >= WWHideCost){
				Master.GetComponent <GameMaster> ().Subtract ("Wood", WWWoodCost);
				Master.GetComponent <GameMaster> ().Subtract ("Hide", WWHideCost);
				gameObject.GetComponent <BuildingHealth>().maxHealth += 30;
				gameObject.GetComponent <BuildingHealth>().health = gameObject.GetComponent <BuildingHealth>().maxHealth;
				status.text = "Wall strength/max strength \r\n" + gameObject.GetComponent <BuildingHealth>().health + "/" + gameObject.GetComponent <BuildingHealth>().maxHealth + "\r\n To fix wall: " + WWFixCost + " wood" + "\r\n to upgrade wall: " + WWWoodCost + " wood and " + WWHideCost + " hide";
			}else{
				Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have enough wood or hide to upgrade this wall.");
			}
		}else{
			if(Master.GetComponent <GameMaster>().gRock >= RWRockCost && Master.GetComponent <GameMaster>().gIron >= RWIronCost){
				Master.GetComponent <GameMaster> ().Subtract ("Rock", RWRockCost);
				Master.GetComponent <GameMaster> ().Subtract ("Iron", RWIronCost);
				gameObject.GetComponent <BuildingHealth>().maxHealth += 50;
				gameObject.GetComponent <BuildingHealth>().health = gameObject.GetComponent <BuildingHealth>().maxHealth;
				status.text = "Wall strength/max strength \r\n" + gameObject.GetComponent <BuildingHealth>().health + "/" + gameObject.GetComponent <BuildingHealth>().maxHealth + "\r\n To fix wall: " + RWFixCost + " rock and " + IWFixCost + " iron" + "\r\n to upgrade wall: " + RWRockCost + " rock and " + RWIronCost + " iron";

			}else{
				Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have enough rock or iron to upgrade this wall.");
			}
		}
	}

	public void Fix(){
		if(wood){
			if(gameObject.GetComponent <BuildingHealth>().health < gameObject.GetComponent <BuildingHealth>().maxHealth){
				if(Master.GetComponent <GameMaster>().gWood >= WWFixCost){
					Master.GetComponent <GameMaster> ().Subtract ("Wood", WWFixCost);
					gameObject.GetComponent <BuildingHealth>().health = gameObject.GetComponent <BuildingHealth>().maxHealth;
					status.text = "Wall strength/max strength \r\n" + gameObject.GetComponent <BuildingHealth>().health + "/" + gameObject.GetComponent <BuildingHealth>().maxHealth + "\r\n To fix wall: " + WWFixCost + " wood" + "\r\n to upgrade wall: " + WWWoodCost + " wood and " + WWHideCost + " hide";
				}else{
					Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have enough wood to fix this wall.");
				}
			}else{
				Master.GetComponent <GameMaster>().PassErrorMessage ("This wall does not need fixing.");
			}
		}else if(rock){
			if(gameObject.GetComponent <BuildingHealth>().health < gameObject.GetComponent <BuildingHealth>().maxHealth){
				if(Master.GetComponent <GameMaster>().gRock >= RWFixCost && Master.GetComponent <GameMaster>().gIron >= IWFixCost){
					Master.GetComponent <GameMaster> ().Subtract ("Rock", RWFixCost);
					Master.GetComponent <GameMaster> ().Subtract ("Iron", IWFixCost);
					gameObject.GetComponent <BuildingHealth>().health = gameObject.GetComponent <BuildingHealth>().maxHealth;
					status.text = "Wall strength/max strength \r\n" + gameObject.GetComponent <BuildingHealth>().health + "/" + gameObject.GetComponent <BuildingHealth>().maxHealth + "\r\n To fix wall: " + RWFixCost + " rock and " + IWFixCost + " iron" + "\r\n to upgrade wall: " + RWRockCost + " rock and " + RWIronCost + " iron";

				}else{
					Master.GetComponent <GameMaster>().PassErrorMessage ("You do not have enough rock or iron to fix this wall.");
				}
			}else{
				Master.GetComponent <GameMaster>().PassErrorMessage ("This wall does not need fixing.");
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
		GameObject.Find ("GUIHUD").transform.Find ("BackDrop").gameObject.SetActive (false);
	}

	public void DestroyBuilding(){
		CloseMenu ();
		Destroy (myCanvas);
		Destroy (gameObject);
	}

}
