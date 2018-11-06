using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildAnimalManager : MonoBehaviour {

	public int checkTimer, maxAnimals, minAnimals;
	public float upOffset;
	public List<GameObject> WildAnimal;

	private GameObject[] animals;
	private List<GameObject> animList;
	private int x, y, z;

	void Start(){
		animals = new GameObject[maxAnimals + 20];
		animList = new List<GameObject> ();
	}

	// Update is called once per frame
	void Update () {
		if(x >= checkTimer){
			animals = GameObject.FindGameObjectsWithTag ("Animal");
			animList.Clear ();
			foreach(GameObject curr in animals){
				animList.Add (curr);
			}
			y = animList.Count;
			if(y >= maxAnimals){
				z = y - maxAnimals;
				for(int i = 0; i < z; i++){
					Destroy (animList[i], 0.1f);
					animList.RemoveAt (i);
				}
			}else if(y <= minAnimals){
				for (int i  = 0; i <= minAnimals; i++){
					Instantiate (WildAnimal[Random.Range (0, WildAnimal.Count)], new Vector3(Random.Range (-20, 20), upOffset, Random.Range (-20, 20)), Quaternion.identity, gameObject.transform);
				}
			}
			x = 0;
		}else{
			x++;
		}
	}
}
