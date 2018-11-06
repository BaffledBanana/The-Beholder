using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealSpikes : MonoBehaviour {

	public int DmgToEnemy, attTimer;

	private int x = 0;
	private List<Collider> enemies;
	private Collider enemyToAttack;

	void Start(){
		enemies = new List<Collider> ();
	}

	public void OnTriggerEnter(Collider other){
		if(other.gameObject.layer == 9 && gameObject.layer == 11){//detects enemy when building is friendly
			enemies.Add (other);
			Debug.Log ("Enemy entered");
		}else if(other.gameObject.layer == 8 && gameObject.layer == 10){//detects friendly when building is enemy
			enemies.Add (other);
			Debug.Log ("Enemy entered");
		}
	}

	public void OnTriggerExit(Collider leaver){
		if(leaver.gameObject.layer == 9 && gameObject.layer == 11){//removes from list enemy when building is friendly
			enemies.Remove (leaver);
			Debug.Log ("Enemy left");
		}else if(leaver.gameObject.layer == 8 && gameObject.layer == 10){//removes from list friendly when building is enemy
			enemies.Remove (leaver);
			Debug.Log ("Enemy left");
		}
	}

	void Update(){
		if(x >= attTimer){
			if(enemies.Count > 0){
				enemyToAttack = enemies [Random.Range (0, enemies.Count)];
				if(enemyToAttack != null){
					enemyToAttack.gameObject.GetComponent <Troop>().TakeDmg (DmgToEnemy, gameObject);
					Debug.Log ("Spikes: Attacked enemy");
				}else{
					enemies.Remove (enemyToAttack);
				}
				x = 0;
			}
		}else{
			x++;
		}
	}

}
