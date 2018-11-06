using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

	public int checkTime, moveCycleTime, stopTime, health, maxHealth, minHide, maxHide, minFood, maxFood, birthTimer, minAgeing, maxAgeign, ageingTimer;
	public float step, reasonableDst2Player, runStep;
	public bool inCoop;
	//public AudioClip runningSound, sheep;
	//public AudioSource sound;
	public GameObject coopAnimalPrefab, wildAnimalPrefab;

	private Vector3 dir;
	private bool doMove, doStop, doRun;
	private int x, y, a, z, b, c;
	private GameObject cam;

	// Use this for initialization
	void Start () {
		/*sound = GetComponent <AudioSource> ();
		sound.clip = runningSound;
		sound.loop = true;
		sound.Play (); */
		cam = GameObject.Find ("Main Camera");
		gameObject.transform.Rotate (0, Random.Range (0, 360), 0, Space.Self);
		x = moveCycleTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(x >= moveCycleTime){
			//choose random action to do
			a = Random.Range (1, 3);
			if(a == 1){
				//rotate to random dir
				gameObject.transform.Rotate (0, Random.Range (0, 360), 0, Space.Self);
				//tell it to move and not stop
				if(doRun == false){
					doMove = true;
					doStop = false;
				}
			}else{
				if(doRun == false){
					doStop = true;
					doMove = false;
				}
			}
			x = 0;
		}else{
			x++;
		}
		if(y >= checkTime){
			CheckForPlayer (cam);
			y = 0;
		}else{
			y++;
		}
		if(doMove){
			Move ();
		}
		if(doStop){
			Stop ();
		}
		if(doRun){
			Run ();
		}
		if(b >= ageingTimer){
			TakeDmg (Random.Range (minAgeing, maxAgeign), null);
			b = 0;
		}else{
			b++;
		}
		if(c >= birthTimer){
			if(inCoop){
				GameObject curr = Instantiate (coopAnimalPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform.parent);
				curr.transform.parent.parent.GetComponent <AnimalCoop>().AddToList(curr);
			}else{
				Instantiate (wildAnimalPrefab, gameObject.transform.position, Quaternion.identity, GameObject.Find ("WildAnimals").transform);
			}
			c = 0;
		}else{
			c++;
		}
	}

	void Move(){
		gameObject.transform.position += transform.forward * step;
		//sound.Stop ();
		/*if(sound.isPlaying == false || sound.clip == sheep){
			sound.clip = runningSound;
			sound.Play ();	
		} */
	}

	void Run(){
		transform.Translate (0, 0, 1 * step * 2.718f * runStep);
		//sound.Stop ();
		/*if(sound.isPlaying == false || sound.clip == sheep){
			sound.clip = runningSound;
			sound.Play ();	
		} */
	}

	void Stop(){
		//sound.Stop ();
		/*if(sound.isPlaying == true && sound.clip == runningSound){
			sound.Stop ();
			sound.clip = sheep;
			sound.Play ();
		} */
		doMove = false;
		doRun = false;
		z++;
		if(z % 100 == 0){
			gameObject.transform.Rotate (0, Random.Range (0, 15), 0, Space.Self);
		}
		if(z >= stopTime){
			doStop = false;
			doMove = true;
		}
	}

	void CheckForPlayer(GameObject attacker){
			if(Vector3.Distance (gameObject.transform.position, attacker.transform.position) <= reasonableDst2Player){
				doRun = true;
				doStop = false;
				doMove = false;
			}else{
				doRun = false;
				doStop = false;
				doMove = true;
			}
	}

	public bool TakeDmg(int amount, GameObject attacker){
		if(health - amount > 0){
			health -= amount;
			if(attacker != null){
				CheckForPlayer (attacker);
			}
			return false;
		}else{
			health = 0;
			if(inCoop){
				gameObject.transform.parent.parent.GetComponent <AnimalCoop> ().Died (gameObject, Random.Range (minHide, maxHide));
				cam.GetComponent <PlayerStats>().AddXp (10);
				GameObject.Find ("GameMaster").GetComponent <GameMaster>().Add ("Food", Random.Range (minFood, maxFood));
			}else{
				if(attacker == GameObject.Find ("Main Camera")){
					cam.GetComponent <PlayerStats>().AddXp (10);
					cam.GetComponent <PlayerStats> ().Add ("Hide", Random.Range (minHide, maxHide));
					cam.GetComponent <PlayerStats> ().Add ("Food", Random.Range (minFood, maxFood));
				}
			}
			Destroy (gameObject, 0.1f);
			return true;
		}
	}

}
