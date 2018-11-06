using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {
		
		public int richness, minRichness, maxRichness;
		private int amount;
	private Transform[] workers;
	private GameObject cam;

		public string resName;
		public bool on = true, wood = false, rock = false, iron = false, food = false, hide = false;
		public GameObject ResMineLogPrefab, ResMessages;
	public float MsgDstToWood, MsgDstForFood;

		void Start(){
		richness = Random.Range (minRichness, maxRichness);
		workers = new Transform[30];
		cam = GameObject.Find ("Main Camera");
		if(wood){
			resName = "Wood";
		}
		else if(rock){
			resName = "Rock";
		}
		else if(iron){
			resName = "Iron";
		}
		else if(food){
			resName = "Food";
		}
		else if(hide){
			resName = "Hide";
		}
		ResMessages = GameObject.Find ("ResMessages");
		}

	public int Mine(int min, int max, GameObject worker){
			if(on){
			if(richness <= 0){
				on = false;
				gameObject.GetComponent<MeshRenderer> ().enabled = false;
				if(worker != cam){
					workers = worker.transform.parent.GetComponentsInChildren <Transform>();
					foreach(Transform curr in workers){
						if(curr.CompareTag ("Workers") == false){
							curr.gameObject.GetComponent <Worker_Better> ().GetNewRes ();
						}
					}
				} 

				Destroy (gameObject);
				return 0;
			}
			else if(richness >= max){
				amount = Random.Range (min, max);
				GameObject curr = Instantiate (ResMineLogPrefab, gameObject.transform.position + Vector3.up, Quaternion.identity, ResMessages.transform);
				if(wood){
					//Debug.Log ("Move the fucking thing");
					curr.transform.position = new Vector3 (curr.transform.position.x, curr.transform.position.y + 1.5f, curr.transform.position.z);

					curr.transform.position = Vector3.MoveTowards (curr.transform.localPosition, GameObject.Find ("Player").transform.position, MsgDstToWood);
				}else if(food){
					curr.transform.Translate (0, MsgDstForFood, 0);
				}
				curr.GetComponent <ResMineLog>().Display (resName, amount, 0, 100); 
				richness -= amount;
				return amount;
			}
			else if (richness < max){ 
				amount = Random.Range (min, richness);
				GameObject curr = Instantiate (ResMineLogPrefab, gameObject.transform.position + Vector3.up, Quaternion.identity, ResMessages.transform);
				if(wood){
					Debug.Log ("Move the fucking thing");
					curr.transform.position = new Vector3 (curr.transform.position.x, curr.transform.position.y + 1.5f, curr.transform.position.z);
					curr.transform.position = Vector3.MoveTowards (curr.transform.localPosition, GameObject.Find ("Player").transform.position, MsgDstToWood);
				}
				curr.GetComponent <ResMineLog>().Display (resName, amount, 0, 100);
				richness = 0; 
				return amount;
			}
			else{
				return 0;
				}
			}
			else {
				gameObject.GetComponent<MeshRenderer> ().enabled = false;
			if(worker != cam){
				workers = worker.transform.parent.GetComponentsInChildren <Transform>();
				foreach(Transform curr in workers){
					if(curr.CompareTag ("Workers") == false){
						curr.gameObject.GetComponent <Worker_Better> ().GetNewRes ();
					}
				}
			}
			Destroy (gameObject);
				return 0;
			}
		}
}
