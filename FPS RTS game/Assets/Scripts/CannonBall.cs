using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour {

	public float step, scaleY;

	private GameObject target, thisTower;
	private bool travel;
	private int damage;

	void Start(){
		transform.localScale = new Vector3 (transform.localScale.x, scaleY, transform.localScale.z);
	}

	// Update is called once per frame
	void Update () {
		if(travel && target != null){
			gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, target.transform.position, step);
			if(gameObject.transform.position == target.transform.position){
				if(target.gameObject.tag == "SSTroop" || target.gameObject.tag == "ShieldTroop"){
					if(target.GetComponent <Troop>().Shot (damage, thisTower)){
						//gameObject.transform.parent.Find ("Detector").GetComponent <DefenceTower>().TroopDied (target);
					}
				}else if(target.gameObject.tag == "Archer"){
					//other.GetComponent <Archer>().TakeDmg (damage);
				}
				Destroy (gameObject);
			}
		}else{
			Debug.Log ("Deleted cannonball");
			Destroy (gameObject);
		}
	}

	public void Shoot(GameObject place, int dmg, GameObject myTower){
		target = place;
		travel = true;
		damage = dmg;
		thisTower = myTower;//this is the detector's gm
	}

	/*public void OnTriggerEnter(Collider other){
		if(other == target){
			if(other.gameObject.tag == "SSTroop" || other.gameObject.tag == "ShieldTroop"){
				other.GetComponent <Troop>().TakeDmg (damage, null);
			}else if(other.gameObject.tag == "Archer"){
				//other.GetComponent <Archer>().TakeDmg (damage);
			}
			Destroy (gameObject);
		}
	} */

}
