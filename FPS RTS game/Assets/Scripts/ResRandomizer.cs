using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResRandomizer : MonoBehaviour {

	public List<GameObject> wood, rock, iron, food, wildAnimal;
	public int woodAmount, rockAmount, ironAmount, foodAmount, animalAmount, minX, maxX, minZ, maxZ, minXA, maxXA, minZA, maxZA;
	public float upOffsetWood, upOffsetRock, upOffsetIron, upOffsetFood, upOffsetAnimal, offsetMax;
	public LayerMask groundMask;

	private RaycastHit hit;
	private Transform Resources, wildAnimals;

	// Use this for initialization
	void Start () {
		Resources = GameObject.Find ("Resources").transform;
		wildAnimals = GameObject.Find ("WildAnimals").transform;

		for(int i = 0; i < woodAmount; i++){
			GameObject curr = Instantiate (wood[Random.Range (0, wood.Count)], new Vector3 (Random.Range (minX, maxX), 10, Random.Range (minZ, maxZ)), Quaternion.Euler (0, Random.Range (0, 360), 0), Resources);
			curr.transform.position = new Vector3 (curr.transform.position.x + Random.Range (0, offsetMax), curr.transform.position.y, curr.transform.position.z + Random.Range (0, offsetMax));
			if(Physics.Raycast(curr.transform.position, Vector3.down, out hit, groundMask)){
				curr.transform.position = new Vector3(curr.transform.position.x, hit.point.y + upOffsetWood, curr.transform.position.z);
				//curr.transform.rotation = Quaternion.LookRotation(hit.normal);
			}
			Collider[] hitStuff = Physics.OverlapSphere (curr.transform.position, 0.5f);
			foreach(Collider c in hitStuff){
				if(c.tag != curr.tag && c.tag != "Ground" && c.tag != "Rock" && c.tag != "Iron" && c.tag != "Food"  && c.tag != "Animal"){
					Destroy (curr);
					//Debug.Log ("Destroyed a res");
					i--;
					break;
				}
			}
		}
		for(int i = 0; i < rockAmount; i++){
			GameObject curr = Instantiate (rock[Random.Range (0, rock.Count)], new Vector3 (Random.Range (minX, maxX), 10, Random.Range (minZ, maxZ)), Quaternion.Euler (0, Random.Range (0, 360), 0), Resources);
			curr.transform.position = new Vector3 (curr.transform.position.x + Random.Range (0, offsetMax), curr.transform.position.y, curr.transform.position.z + Random.Range (0, offsetMax));
			if(Physics.Raycast(curr.transform.position, Vector3.down, out hit, groundMask)){
				curr.transform.position = new Vector3(curr.transform.position.x, hit.point.y + upOffsetRock, curr.transform.position.z);
				//curr.transform.rotation = Quaternion.LookRotation(hit.normal);
			}
			Collider[] hitStuff = Physics.OverlapSphere (curr.transform.position, 0.5f);
			foreach(Collider c in hitStuff){
				if(c.tag != curr.tag && c.tag != "Ground"  && c.tag != "Wood" && c.tag != "Iron" && c.tag != "Food"  && c.tag != "Animal"){
					Destroy (curr);
					//Debug.Log ("Destroyed a res");
					i--;
					break;
				}
			}
		}
		for(int i = 0; i < ironAmount; i++){
			GameObject curr = Instantiate (iron[Random.Range (0, iron.Count)], new Vector3 (Random.Range (minX, maxX), 10, Random.Range (minZ, maxZ)), Quaternion.Euler (0, Random.Range (0, 360), 0), Resources);
			curr.transform.position = new Vector3 (curr.transform.position.x + Random.Range (0, offsetMax), curr.transform.position.y, curr.transform.position.z + Random.Range (0, offsetMax));
			if(Physics.Raycast(curr.transform.position, Vector3.down, out hit, groundMask)){
				curr.transform.position = new Vector3(curr.transform.position.x, hit.point.y + upOffsetIron, curr.transform.position.z);
				//curr.transform.rotation = Quaternion.LookRotation(hit.normal);
			}
			Collider[] hitStuff = Physics.OverlapSphere (curr.transform.position, 0.5f);
			foreach(Collider c in hitStuff){
				if(c.tag != curr.tag && c.tag != "Ground"  && c.tag != "Rock" && c.tag != "Wood" && c.tag != "Food"  && c.tag != "Animal"){
					Destroy (curr);
					//Debug.Log ("Destroyed a res");
					i--;
					break;
				}
			}
		}
		for(int i = 0; i < foodAmount; i++){
			GameObject curr = Instantiate (food[Random.Range (0, food.Count)], new Vector3 (Random.Range (minX, maxX), 10, Random.Range (minZ, maxZ)), Quaternion.Euler (0, Random.Range (0, 360), 0), Resources);
			curr.transform.position = new Vector3 (curr.transform.position.x + Random.Range (0, offsetMax), curr.transform.position.y, curr.transform.position.z + Random.Range (0, offsetMax));
			if(Physics.Raycast(curr.transform.position, Vector3.down, out hit, groundMask)){
				curr.transform.position = new Vector3(curr.transform.position.x, hit.point.y + upOffsetFood, curr.transform.position.z);
				//curr.transform.rotation = Quaternion.LookRotation(hit.normal);
			}
			Collider[] hitStuff = Physics.OverlapSphere (curr.transform.position, 0.5f);
			foreach(Collider c in hitStuff){
				if(c.tag != curr.tag && c.tag != "Ground"  && c.tag != "Rock" && c.tag != "Iron" && c.tag != "Wood"  && c.tag != "Animal"){
					Destroy (curr);
					i--;
					break;
				}
			}
		}
		for(int i = 0; i < animalAmount; i++){
			GameObject curr = Instantiate (wildAnimal[Random.Range (0, wildAnimal.Count)], new Vector3 (Random.Range (minXA, maxXA), 10, Random.Range (minZA, maxZA)), Quaternion.Euler (0, Random.Range (0, 360), 0), wildAnimals);
			if(Physics.Raycast(curr.transform.position, Vector3.down, out hit, groundMask)){
				curr.transform.position = new Vector3(curr.transform.position.x, hit.point.y + upOffsetAnimal, curr.transform.position.z);
				//curr.transform.rotation = Quaternion.LookRotation(hit.normal);
			}
		}
		Destroy (gameObject);
	}

}
