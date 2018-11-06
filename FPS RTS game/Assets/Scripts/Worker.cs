using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Worker : MonoBehaviour {

		public bool wood, rock, iron, food, gather = false, stopping;
	public float radius = 1f, cycleCount = 500, stepFloat = 1, scaler, distOffset, dstBtw4Penalty = 5, dstBtw4Conp = 15;
		public int minWood, maxWood, minRock, maxRock, minIron, maxIron, minFood, maxFood, minePenalty, mineConp;

		private int x, min, max, i, y, amount, tempMin, tempMax;
		private Vector3 myBaseLoc, myResLoc;
		private List<Collider> myRes;
		private Collider[] hitColliders;
		private GameObject Master, myBase;
		private string workerName;
		private float distToRes, buildingThickness, distFromWorker;

		//IMPORTANT each outpost must have a funcion ResLoc() that returns a Vector3 where the resource is, it also has a paramater for which resource type to look for

	void Start () {
		gather = false;
		stopping = true;
		x = 0;
		i = 0;
		distFromWorker = gameObject.GetComponent <Renderer>().bounds.extents.magnitude;
		myBase = gameObject.transform.parent.parent.transform.gameObject;
		gather = false;
		myBaseLoc = gameObject.transform.parent.parent.transform.position;
		Master = GameObject.Find ("GameMaster");
		myRes = new List<Collider> ();
		Master.GetComponent <GameMaster>().AddXpToPlayer (30);
		if(wood){
			workerName = "Wood";
			min = minWood;
			max = maxWood;
		} else if(rock){
		workerName = "Rock";
			min = minRock;
			max = maxRock;
		} else if(iron){
			workerName = "Iron";
			min = minIron;
			max = maxIron;
		} else if(food){
		workerName = "Food";
			min = minFood;
			max = maxFood;
		}
		if(gameObject.transform.parent.parent.transform.gameObject.GetComponent<Outpost> () != null){
			myRes = gameObject.transform.parent.parent.transform.gameObject.GetComponent<Outpost> ().ResLoc (workerName); 
		}
			
		if(myRes != null && myRes.Count > 0 && myRes[i] != null){
			Debug.Log ("Got my resource");
			gather = true;
			stopping = false;
			myResLoc = myRes [i].transform.position;
		}else{
			gather = false;
			Debug.Log ("Worker: stopping");
			gather = false;
			stopping = true;
			Master.GetComponent <GameMaster>().Subtract ("Workers", 1);
			Master.GetComponent <GameMaster> ().PassErrorMessage ("You ordered a worker at one of your outposts, but this outpost did not have any resource within it's range to give the worker something to gather. This worker has been fired.");
			Destroy (gameObject);
		}
	}

	bool Choose(){
		i++;
		if(myRes.Count > 0 && i < myRes.Count && myRes[i] != null){ //should be this instead if you get an index out of bounds exeption : i < myRes.Count
			myResLoc = myRes [i].transform.position;
			return true;
		}
		else{ 
			Debug.Log ("No resources found/returned empty collider array to a worker!"); 
			return false;
		}
	}

	//						amount = myRes [i - 1].gameObject.GetComponent<Resource> ().Mine (min, max, gameObject); 


	void Update (){
		x++;
		if (x >= cycleCount) {
			if(stopping == false){
				if(gather){//goes to the res
					if(myRes[i] != null){
						distToRes = Vector3.Distance (myResLoc, gameObject.transform.position);
						buildingThickness = myRes [i].GetComponent <Renderer>().bounds.extents.magnitude;
						if(myRes[i].tag == "Wood"){
							buildingThickness += 2f;
						}
						if(distToRes <= distFromWorker + buildingThickness + distOffset){
							tempMin = min;
							tempMax = max;
							if(Vector3.Distance (myResLoc, myBaseLoc) <= dstBtw4Penalty){//if the distance between res and base is smaller than this then assign mine penalty
								Debug.Log ("Giving penalty");
								if(max - minePenalty > 1){
									tempMin = 0;
									tempMax -= minePenalty;
								}
								else{
									tempMin = 0;
									tempMax = 2;
								}
							}
							else if(Vector3.Distance (myResLoc, myBaseLoc) > dstBtw4Conp){
								Debug.Log ("Conpensating");
								tempMin += mineConp;
								tempMax += mineConp;
							}
							Debug.Log ("TempMin: " + tempMin + "/" + min + " TempMax: " + tempMax + "/" + max);
							amount = myRes [i].gameObject.GetComponent<Resource> ().Mine (tempMin, tempMax, gameObject);
							gather = false;
						}
					}else{
						ReResLocWorker ();
						i = 0;
					}
				}else{
					distToRes = Vector3.Distance (myBaseLoc, gameObject.transform.position);
					buildingThickness = myBase.GetComponent <Renderer>().bounds.extents.magnitude + 5f;
					//buildingThickness = myBase.GetComponent <Collider> ().bounds.extents;
					//Debug.Log ("Outpost Renderer bounds magnitude: " + buildingThickness);
					if(distToRes <= distFromWorker + buildingThickness + distOffset){
						Master.GetComponent <GameMaster>().Add (workerName, amount);
						gather = true;
					}
				}
				x = 0;
			}
		}
		if(stopping == false){
			Work();	
		}

	}

	void Work(){
		if(stopping == false){
			if(gather){
				gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, myResLoc, stepFloat);
				gameObject.transform.LookAt (myResLoc);
				gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.localScale.y, gameObject.transform.position.z);
			} else{
				gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, myBaseLoc, stepFloat);
				gameObject.transform.LookAt (myBaseLoc);
				gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.localScale.y, gameObject.transform.position.z);

			}
		}
	}
	void Stop(){
	if(Choose ()){
			Debug.Log ("Worker: chose another res and gonna start doing work again");
			Work ();
			gather = true;
			stopping = false;
	}
		else{
		//gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, myBaseLoc, stepFloat);
			//gameObject.transform.LookAt (myBaseLoc);
			Debug.Log ("Worker: stopping");
			gather = false;
			stopping = true;
			//myResLoc = null;
 }
	}

	public void ReResLocWorker(){
		if(gameObject.transform.parent.parent.transform.gameObject.GetComponent<Outpost> ().ResLoc (workerName) != null){
			if(gameObject.transform.parent.parent.transform.gameObject.GetComponent<Outpost> () != null){
				myRes = gameObject.transform.parent.parent.transform.gameObject.GetComponent<Outpost> ().ResLoc (workerName); 
				if(myRes.Count == 0 || myRes == null){
					Stop ();
					gather = false;
					stopping = true;
				}
			}
			if(Choose ()){
				gather = true;
				Work ();
			} else{
				gather = false;
				Stop ();
				stopping = true;
			}
		}else{
			Stop ();
			gather = false;
			stopping = true;
		}
	}
}