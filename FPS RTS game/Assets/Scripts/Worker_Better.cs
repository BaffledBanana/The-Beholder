using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker_Better : MonoBehaviour {

	public bool wood, rock, iron, food;
	public int checkTimer, moveTimer, minWood, maxWood, minRock, maxRock, minIron, maxIron, minFood, maxFood, compDist, penaltyDist, compAmount, penaltyAmount;
	public float checkRadiusOffset, step;

	private GameObject myBase, myRes, Master;
	private Vector3 myBaseLoc, myResLoc;
	private List<GameObject> resources;
	private string myName;
	private int x, t, min, max, tempMin, tempMax, amount;
	private Collider[] detected;
	private bool toBase, toRes, stop, messure;
	private float checkRadius, dstFromGround;

	// Use this for initialization
	void Start () {
		messure = true;
		toRes = true;
		toBase = false;
		stop = false;
		checkRadius = gameObject.GetComponent <Renderer> ().bounds.extents.magnitude + checkRadiusOffset;
		detected = new Collider[50];
		resources = new List<GameObject>();
		myBase = gameObject.transform.parent.parent.gameObject;
		myBaseLoc = myBase.transform.position;
		Master = GameObject.Find ("GameMaster");
		if(wood){
			myName = "Wood";
			min = minWood;
			max = maxWood;
		}else if(rock){
			myName = "Rock";
			min = minRock;
			max = maxRock;
		}
		else if(iron){
			myName = "Iron";
			min = minIron;
			max = maxIron;
		}
		else if(food){
			myName = "Food";
			min = minFood;
			max = maxFood;
		}
		ReceiveMyRes (myName);
	}
	
	// Update is called once per frame
	void Update () {
		if(x >= checkTimer){
			x = 0;
			Check ();
		}else{
			x++;
		}
		if(t >= moveTimer){
			t = 0;
			if(stop != true){
				if(toBase){
					MoveToBase ();
				}else if(toRes){
					MoveToRes ();
				}
			}
		}else{
			t++;
		}
	}

	void ReceiveMyRes(string workerName){
		resources = myBase.GetComponent <Outpost>().RequestResources (workerName);
		//here i can calculate which of the resources is closest and go for that one
		if(resources.Count > 0){
			myRes = resources [Random.Range (0, resources.Count)];
			myResLoc = myRes.transform.position;
		}else{
			stop = true;
			myRes = null;
		}
	}

	void Check(){
		if(messure){
			dstFromGround = transform.position.y;
			messure = false;
		}
		detected = Physics.OverlapSphere (gameObject.transform.position, checkRadius);
		if(myRes != null){
			foreach(Collider det in detected){
				if(det.gameObject == myRes){//if arrived at res
					tempMin = min;
					tempMax = max;
					if(Vector3.Distance (myBaseLoc, myResLoc) < penaltyDist){
						if(min - penaltyAmount < 1){
							tempMin = 0;
						}else{
							tempMin -= penaltyAmount;
						}
						if(max - penaltyAmount < 2){
							tempMax = 2;
						}else{
							tempMax -= penaltyAmount;
						}
					}else if(Vector3.Distance (myBaseLoc, myResLoc) > compDist){
						tempMin += compAmount;
						tempMax += compAmount;
					}
					amount = myRes.GetComponent <Resource>().Mine (tempMin, tempMax, gameObject);
					toBase = true;
					toRes = false;
				}else if(det.gameObject == myBase){//if arrived at base
					Master.GetComponent <GameMaster> ().Add (myName, amount);
					amount = 0;
					toBase = false;
					toRes = true;
				}
			}
		}else{
			ReceiveMyRes (myName);
			if(myRes == null){
				stop = true;
			}else{
				Check ();
			}
		}
	}

	void MoveToBase(){//simply moves to the baseLoc
		transform.position = Vector3.MoveTowards (transform.position, myBaseLoc, step);
		if(messure == false){
			transform.position = new Vector3 (transform.position.x, dstFromGround, transform.position.z);
		}
	}

	void MoveToRes(){//simply moves to the resLoc
		transform.position = Vector3.MoveTowards (transform.position, myResLoc, step);
		if(messure == false){
			transform.position = new Vector3 (transform.position.x, dstFromGround, transform.position.z);
		}
	}

	public void GetNewRes(){
		ReceiveMyRes (myName);
	}

}
