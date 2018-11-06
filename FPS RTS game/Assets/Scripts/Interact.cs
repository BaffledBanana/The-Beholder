using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour {

	public float moveSelectorAmount, distance, troopSelectDistance, swingDistance, pushForce;
	public GameObject GlobalValuesUIMenu, BuildMenu, selected, Pick, Sword, Hammer, PlayerUI, Waypoint, MainMenu, PlayerShield;
	public LayerMask myLMask, swordMask, hammerMask, pushObjMask;
	public AudioClip walk, swing, swingHit, hammerHit;
	public AudioSource sound;
	public int swordTimer, hammerTimer;
	public bool showBuild = false, openMainMenu = false;

	private Vector3 PSStart, PSBlock, PSStartRot;
	private GameObject Master, constB, sprintBar;
	private Transform toolbarSelector;
	private RaycastHit hit;
	private List<GameObject> tools, myTroops;
	private GameObject[] tmpTroops;
	private List<Transform> slots;
	private Transform[] slotArray;
	private int up, down, i, x, y;
	private bool showGlobal = true, selecting, addToList, isBlocking, canSwing = true, isWalking, canHammer, isSprinting;		//set to false uppon alpha release
	private float s;

		//IMPORTANT every building type must be named after the same thing the class in named and must contain an OpenMenu() and must be tagged the same name
		//IMPORTANT when adding a new slot on GUI it must be on top of all previous ones but they can change position on screen
	void Awake(){
		constB = GameObject.Find ("ConstructBuilding");
		if(GameObject.Find ("GlobalValuesUIMenu") != null){
			GlobalValuesUIMenu = GameObject.Find ("GlobalValuesUIMenu");
			GlobalValuesUIMenu.SetActive (true);
		}
		BuildMenu = GameObject.FindGameObjectWithTag ("ConstructBuilding");
		tools = new List<GameObject> ();
		slots = new List<Transform> ();
		tmpTroops = new GameObject[500];
		slotArray = new Transform[10];
		PlayerUI = GameObject.Find ("PlayerUI");
		toolbarSelector = PlayerUI.transform.Find ("ToolbarSelector");
		i = 0;
		myLMask = LayerMask.GetMask ("Ground");
	}

	void Start(){
		MainMenu = GameObject.Find ("MainMenu");
		MainMenu.SetActive (false);
		sound = GetComponent <AudioSource> ();
		//sound = gameObject.transform.parent.gameObject.GetComponent <AudioSource> ();
		sound.clip = walk;
		Master = GameObject.Find ("GameMaster");
		Pick = transform.parent.transform.Find ("PickAxe_2").gameObject;
		Sword = transform.parent.transform.Find ("Sword").gameObject;
		Hammer = transform.parent.transform.Find ("Hammer").gameObject;
		PlayerShield = gameObject.transform.parent.Find ("PlayerShield").gameObject;
		PSStart = PlayerShield.transform.localPosition;
		PSStartRot = PlayerShield.transform.localEulerAngles;
		PSBlock = gameObject.transform.parent.Find ("PSBlockPos").localPosition;
		PlayerShield.SetActive (false);
		tools.Add (Pick);
		tools.Add (Sword);
		tools.Add (Hammer);
		selected = tools [0];
		Pick.SetActive (true);
		Hammer.SetActive (false);
		Sword.SetActive (false);
		slotArray = PlayerUI.GetComponentsInChildren <Transform> ();
		foreach (Transform slot in slotArray){
			if(slot.tag == "Slot"){
				slots.Add (slot);
			}
		}
		myTroops = new List<GameObject> ();
		s = GetComponent <PlayerStats> ().sprintMax;
		sprintBar = GameObject.Find ("SprintBar");
		//Debug.Log ("Slots: " + slots.Count);
	}

	void Update () {
		//my cheats :D
		if(Input.GetKey (KeyCode.R) && Input.GetKey (KeyCode.O) && Input.GetKeyDown (KeyCode.L)){
				gameObject.GetComponent <PlayerStats>().Add ("Wood", 100);
				gameObject.GetComponent <PlayerStats>().Add ("Rock", 100);
				gameObject.GetComponent <PlayerStats>().Add ("Iron", 100);
				gameObject.GetComponent <PlayerStats>().Add ("Food", 100);
				gameObject.GetComponent <PlayerStats>().Add ("Gold", 100);
				gameObject.GetComponent <PlayerStats>().Add ("Hide", 100);
				Master.GetComponent <GameMaster>().Add ("Wood", 100);
				Master.GetComponent <GameMaster>().Add ("Rock", 100);
				Master.GetComponent <GameMaster>().Add ("Iron", 100);
				Master.GetComponent <GameMaster>().Add ("Food", 100);
				Master.GetComponent <GameMaster>().Add ("Gold", 100);
				Master.GetComponent <GameMaster>().Add ("Hide", 100);
				Debug.Log ("Sneaky cunt you just cheated");
				Master.GetComponent <GameMaster>().PassErrorMessage ("Please don't cheat ;-;");
		}
		//all the actual code

		if(canSwing == false && isBlocking == false){
			if(x >= swordTimer){
				x = 0;
				canSwing = true;
			}else{
				x++;
			}
		}
		if(canHammer == false){
			if(y >= hammerTimer){
				y = 0;
				canHammer = true;
			}else{
				y++;
			}
		}

		if(Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.A)){
			if(sound.isPlaying == false){
				sound.clip = walk;
				sound.Play ();
				sound.loop = true;
			}
		}
		if(Input.GetKeyUp (KeyCode.W) || Input.GetKeyUp (KeyCode.S) || Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.A)){
			if(Input.GetKey (KeyCode.W) == false && Input.GetKey (KeyCode.S) == false && Input.GetKey (KeyCode.D) == false && Input.GetKey (KeyCode.A) == false){
				if(sound.isPlaying){
					sound.clip = walk;
					sound.Stop ();
					sound.loop = false;
				}
			}
		}

		if(Input.GetKeyDown (KeyCode.LeftShift)){
			gameObject.transform.parent.GetComponent <PlayerControler>().movementSpeed = GetComponent <PlayerStats>().sprintSpeed;
			isSprinting = true;
		}else if(Input.GetKeyUp (KeyCode.LeftShift)){
			gameObject.transform.parent.GetComponent <PlayerControler>().movementSpeed = GetComponent <PlayerStats>().walkSpeed;
			isSprinting = false;
		}

		if(isSprinting){
			if(s > 0){
				sprintBar.GetComponent <Transform>().localScale = new Vector3(1, s/GetComponent <PlayerStats>().sprintMax, 1);
				s--;
			}else{
				gameObject.transform.parent.GetComponent <PlayerControler>().movementSpeed = GetComponent <PlayerStats>().walkSpeed;
				isSprinting = false;
			}
		}else if(s < GetComponent <PlayerStats>().sprintMax){
			s += GetComponent <PlayerStats>().sprintRegen;
			sprintBar.GetComponent <Transform>().localScale = new Vector3(1, s/GetComponent <PlayerStats>().sprintMax, 1);
		}

		if(Input.GetKeyDown (KeyCode.Escape)){
			Esc();
		}
		//if(selected.name == "PickAxe_2"){
			//Debug.Log (selected.name);
			//gameObject.GetComponent <Gather> ().canGather = true;
			if(Input.GetKeyDown (KeyCode.R)){
				constB.GetComponent <ConstructBuilding>().RotateBuilding ();
			}

			if(Input.GetKeyDown (KeyCode.T)){
				distance = gameObject.GetComponent <PlayerStats> ().interactDistance;
				if(Physics.Raycast (gameObject.transform.position, transform.forward, out hit, distance)){
					if(hit.collider.CompareTag ("House")){
						hit.collider.GetComponent<House> ().CollectTax ();
					}
				}
			}
			if(Input.GetKeyDown (KeyCode.E)){
				distance = gameObject.GetComponent <PlayerStats> ().interactDistance;
				if(Physics.Raycast (gameObject.transform.position, transform.forward, out hit, distance)){
					if(hit.collider.CompareTag ("BaseBuilding")){
						hit.collider.GetComponent<BaseBuilding> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("Outpost")){//just put the other buildings name in the string
						hit.collider.GetComponent<Outpost> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("Market")){//just put the other buildings name in the string
						hit.collider.GetComponent<Market> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("House")){//just put the other buildings name in the string
						hit.collider.GetComponent<House> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("PlayerUpgradesBuilding")){//just put the other buildings name in the string
						hit.collider.GetComponent<PlayerUpgradesBuilding> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("Farm")){//just put the other buildings name in the string
						hit.collider.GetComponent<Farm> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("Bank")){//just put the other buildings name in the string
						hit.collider.GetComponent<Bank> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("Storage")){//just put the other buildings name in the string
						hit.collider.GetComponent<StorageBuilding> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("Tavern")){//just put the other buildings name in the string
						hit.collider.GetComponent<Tavern> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("Blacksmith")){//just put the other buildings name in the string
						hit.collider.GetComponent<Blacksmith> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("MineShaft")){//just put the other buildings name in the string
						hit.collider.GetComponent<MineShaft> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("TreePlantation")){//just put the other buildings name in the string
						hit.collider.GetComponent<TreePlantation> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("Wall")){//just put the other buildings name in the string
						hit.collider.GetComponent<Wall> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("Barracks")){//just put the other buildings name in the string
						hit.collider.GetComponent<Barracks> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("AnimalCoop")){//just put the other buildings name in the string
						hit.collider.GetComponent<AnimalCoop> ().OpenMenu ();
					}else if(hit.collider.CompareTag ("Castle")){//just put the other buildings name in the string
						hit.collider.GetComponent<Castle> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("TrainingCamp")){//just put the other buildings name in the string
						hit.collider.GetComponent<TrainingCamp> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("DefenceTower")){//just put the other buildings name in the string
						hit.collider.transform.Find ("Detector").GetComponent<DefenceTower> ().OpenMenu ();
					}
					else if(hit.collider.CompareTag ("BellTower")){//just put the other buildings name in the string
						hit.collider.transform.Find ("Detector").GetComponent<BellTower> ().OpenMenu ();
					}
				}
			}
			else if(Input.GetKeyDown (KeyCode.B)){
				showBuild = !showBuild;
				if(showBuild){
					BuildMenu.GetComponent <ConstructBuilding>().OpenMenu();
				}else{
					BuildMenu.GetComponent <ConstructBuilding>().CloseMenu();
				}
			}
		//}
		if(selected.name == "Sword"){
			//gameObject.GetComponent <Gather> ().canGather = false;
			//Debug.Log (selected.name);
			//what to do when sword is selected...
			if(Input.GetKeyDown (KeyCode.V)){
				foreach(GameObject t in myTroops){
					t.GetComponent <Troop>().Deselect ();
				}
				myTroops.Clear ();
				tmpTroops = GameObject.FindGameObjectsWithTag ("SSTroop");
				foreach(GameObject t in tmpTroops){
					if(t.layer == 8){//friendly troop
						t.GetComponent <Troop>().Select ();
						myTroops.Add (t);
					}
				}
				tmpTroops = GameObject.FindGameObjectsWithTag ("ShieldTroop");
				foreach(GameObject t in tmpTroops){
					if(t.layer == 8){//friendly troop
						t.GetComponent <Troop>().Select ();
						myTroops.Add (t);
					}
				}
			}
			else if(Input.GetKeyDown (KeyCode.Tab)){
				foreach(GameObject t in myTroops){
					t.GetComponent <Troop>().Deselect ();
				}
				myTroops.Clear ();
				tmpTroops = GameObject.FindGameObjectsWithTag ("SSTroop");
				foreach(GameObject t in tmpTroops){
					if(t.layer == 8 && t.GetComponent <Renderer>().isVisible){//if friendly troop is visible by any camera in the scene then select that troop
						t.GetComponent <Troop>().Select ();
						myTroops.Add (t);
					}
				}
				tmpTroops = GameObject.FindGameObjectsWithTag ("ShieldTroop");
				foreach(GameObject t in tmpTroops){
					if(t.layer == 8 && t.GetComponent <Renderer>().isVisible){//if friendly troop is visible by any camera in the scene then select that troop
						t.GetComponent <Troop>().Select ();
						myTroops.Add (t);
					}
				}
			}

			if(Input.GetKeyDown (KeyCode.Mouse0) && Input.GetKey (KeyCode.LeftControl) == false){
				Swing ();
			}
			if(Input.GetKeyDown (KeyCode.Mouse1) && Input.GetKey (KeyCode.LeftControl) == false){
				isBlocking = true;
				Block ();
			}
			if(Input.GetKeyUp (KeyCode.Mouse1) && Input.GetKey (KeyCode.LeftControl) == false){
				isBlocking = false;
				Block ();
			}
			if(Input.GetKey (KeyCode.LeftControl) && Input.GetKey (KeyCode.Mouse0)){//selects troops
				//while buttons are being pressed
				//Debug.Log ("CTRL and mouse0 are being held down");
				if(Physics.Raycast (gameObject.transform.position, transform.forward, out hit, troopSelectDistance)){
					if(hit.collider.gameObject.layer == 8){
						addToList = true;
						foreach(GameObject troop in myTroops){
							if(troop == hit.collider.gameObject){
								addToList = false;
								break;
							}else{
								addToList = true;
							}
						}
						if(addToList){
							myTroops.Add (hit.collider.gameObject);
							hit.collider.gameObject.GetComponent <Troop>().Select ();
							Debug.Log ("Troop selected: " + hit.collider.gameObject.name);
						}
					}
				}
			}
			if(Input.GetKeyDown (KeyCode.Mouse2) && Input.GetKey (KeyCode.LeftControl) == false){ //middle mouse clears the selection
				foreach(GameObject troop in myTroops){
					troop.GetComponent <Troop>().Deselect ();
				}
				myTroops.Clear ();
			}
			if(Input.GetKeyDown (KeyCode.Mouse2) && Input.GetKey (KeyCode.LeftControl)){//middle mouse + ctrl makes troops attack an enemy or charge at a building
				if(Physics.Raycast (gameObject.transform.position, transform.forward, out hit, 1000f)){
					if(hit.collider.gameObject.layer == 9 || hit.collider.gameObject.layer == 10){//9 = enemy layer, 10 = enemy building
						Instantiate (Waypoint, hit.point, Quaternion.identity, GameObject.Find ("GameMaster").transform);

						if(myTroops.Count == 0){
							Master.GetComponent <GameMaster>().PassErrorMessage ("You have not selected troops to command!");
						}else{
							foreach(GameObject troop in myTroops){
								if(troop != null){
									troop.GetComponent <Troop>().Charge (hit.collider.gameObject);
								}
							}
						}
					}
				}
			}
			if(Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.Mouse1)){//right mouse + ctrl makes troops move where you are pointing
				//Debug.Log ("Pressed");
				if(Physics.Raycast (gameObject.transform.position, transform.forward, out hit, 1000f)){
					//Debug.Log (hit.collider.gameObject.name);
					if(hit.collider.gameObject.tag == "Ground"){//if pointing at the ground
						Instantiate (Waypoint, hit.point, Quaternion.identity, GameObject.Find ("GameMaster").transform);
						if(myTroops.Count == 0){
							Master.GetComponent <GameMaster>().PassErrorMessage ("You have not selected troops to command!");
						}else{
							foreach(GameObject troop in myTroops){
								if(troop != null){
									troop.GetComponent <Troop>().Move (hit.point);
								}
							}
						}
					}
				}
			}
		}
		if(selected.name == "Hammer"){
			if(Input.GetKey (KeyCode.Mouse0)){
				if(canHammer){
					Hammer.GetComponent <Animator>().SetTrigger ("SwingHammer");
					if(Physics.Raycast (gameObject.transform.position, transform.forward, out hit, 10f, hammerMask)){
						if(hit.collider.gameObject.GetComponent <BuildHammer>() != null){
							if(hit.collider.gameObject.GetComponent <BuildHammer>().isBuilt == false){
								if(hit.collider.gameObject.GetComponent <BuildHammer> ().Build (gameObject.GetComponent <PlayerStats>().hammerStrenght)){//returns true if the building is built with this hammer hit, false if you should still build it
									sound.clip = hammerHit;
									sound.loop = false;
									sound.PlayDelayed(0f);
									//Debug.Log ("Building built!");
								}
								sound.clip = hammerHit;
								sound.loop = false;
								sound.PlayDelayed(0f);
								//Debug.Log ("Pressed build");
							}else{
								Master.GetComponent <GameMaster>().PassErrorMessage ("This building has been completely built.");
							}
						}
					}
					canHammer = false;
				}
			}else if(Input.GetKeyDown (KeyCode.Mouse1)){
				if(Physics.Raycast (gameObject.transform.position, transform.forward, out hit, 10f, pushObjMask)){
					hit.collider.gameObject.GetComponent <Rigidbody>().AddForce (transform.forward * pushForce);
				}
			}
		}
		if(Input.GetKeyDown (KeyCode.Q)){
			if(showGlobal){
				GlobalValuesUIMenu.SetActive (true);
			} else{GlobalValuesUIMenu.SetActive (false);
			}
			showGlobal = !showGlobal;
		}
		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			var delta = Input.GetAxis ("Mouse ScrollWheel");
			//Debug.Log("SCROLLING! delta: " + delta* Time.deltaTime + " / " + Input.GetAxis("Mouse ScrollWheel"));
			if (delta > Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime && delta >= 0) { //stopping scroll up
				if (i < tools.Count - 1) {
					i++;
					//toolbarSelector.transform.Translate (0, moveSelectorAmount, 0);
					toolbarSelector.transform.position = slots [i].position;
				} else {
					//toolbarSelector.transform.Translate (0, -moveSelectorAmount * i, 0);
					i = 0;
					toolbarSelector.transform.position = slots [i].position;
				}
				selected = tools [i];
				if(selected.name == "Sword"){
					gameObject.GetComponent <Gather> ().canGather = false;
					Pick.SetActive (false);
					Hammer.SetActive (false);
					Sword.SetActive (true);
					PlayerShield.SetActive (true);
				}
				if(selected.name == "PickAxe_2"){
					gameObject.GetComponent <Gather> ().canGather = true;
					Pick.SetActive (true);
					Hammer.SetActive (false);
					Sword.SetActive (false);
					PlayerShield.SetActive (false);
				}
				if(selected.name == "Hammer"){
					gameObject.GetComponent <Gather> ().canGather = false;
					Pick.SetActive (false);
					Hammer.SetActive (true);
					Sword.SetActive (false);
					PlayerShield.SetActive (false);
				}
				//Debug.Log ("Scrolled up! Selected: " + selected.name);
			} else if (delta * Time.deltaTime > Input.GetAxis ("Mouse ScrollWheel") && delta <= 0) {//stopping scroll down
				if (i > 0) {
					i--;
					//toolbarSelector.transform.Translate (0, -moveSelectorAmount, 0);
					toolbarSelector.transform.position = slots [i].position;
					//Debug.Log ("i: " + i);
				} else {
					i = tools.Count - 1;
					//toolbarSelector.transform.Translate (0, moveSelectorAmount * i, 0);
					toolbarSelector.transform.position = slots [i].position;
					//Debug.Log ("i: " + i);
				}
				selected = tools [i];
				if(selected.name == "Sword"){
					gameObject.GetComponent <Gather> ().canGather = false;
					Pick.SetActive (false);
					Hammer.SetActive (false);
					Sword.SetActive (true);
					PlayerShield.SetActive (true);
				}
				if(selected.name == "PickAxe_2"){
					gameObject.GetComponent <Gather> ().canGather = true;
					Pick.SetActive (true);
					Hammer.SetActive (false);
					Sword.SetActive (false);
					PlayerShield.SetActive (false);
				}
				if(selected.name == "Hammer"){
					gameObject.GetComponent <Gather> ().canGather = false;
					Pick.SetActive (false);
					Hammer.SetActive (true);
					Sword.SetActive (false);
					PlayerShield.SetActive (false);
				}
				//Debug.Log ("Scrolled down! Selected: " + selected.name);
			}
		}
  }

	public void Swing(){
		if(canSwing && isBlocking == false){
			//Debug.Log("Swung");
			Sword.GetComponent <Animator>().SetTrigger ("SwingSword");
			if(Physics.Raycast (gameObject.transform.position, transform.forward, out hit, swingDistance, swordMask)){
				if(hit.collider.gameObject.layer == 9){//9 is enemy layer
					hit.collider.gameObject.GetComponent <Troop> ().DisplayHealth ();
					hit.collider.gameObject.GetComponent <Troop> ().TakeDmg (gameObject.GetComponent <PlayerStats> ().strength, gameObject);
					sound.clip = swingHit;
					sound.loop = false;
					sound.PlayDelayed (0f);
					//do the animation here
				}else if(hit.collider.gameObject.tag == "Animal"){
					hit.collider.gameObject.GetComponent <Animal>().TakeDmg (gameObject.GetComponent <PlayerStats> ().strength, gameObject);
					sound.clip = swingHit;
					sound.loop = false;
					sound.PlayDelayed (0f);
					//Debug.Log ("Hit animal: " + hit.collider.gameObject.name);
				}
			}else{
				sound.clip = swing;
				sound.loop = false;
				sound.PlayDelayed (0f);
			}
			canSwing = false;
		}
	}

	public void Block(){
		//isBlocking = !isBlocking;
		if(isBlocking){
			//put on shield
			PlayerShield.transform.localPosition = PSBlock;
			PlayerShield.transform.localEulerAngles = new Vector3 (180, -10, -110);
			canSwing = false;
			gameObject.GetComponent <PlayerStats> ().isBlocking = true;
		}
		else{
			//take shield off
			canSwing = true;
			PlayerShield.transform.localPosition = PSStart;
			PlayerShield.transform.localEulerAngles = PSStartRot;
			gameObject.GetComponent <PlayerStats> ().isBlocking = false;
		}
	}

	void Esc(){
		openMainMenu = !openMainMenu;
		try{
			GameObject.FindGameObjectWithTag("BuildMenu").SetActive(false);
		}catch{}
		GameObject[] menus = new GameObject[3];
		menus = GameObject.FindGameObjectsWithTag("Menu");
		foreach(GameObject m in menus){
			if(m != null){
				if(m.activeSelf){
					m.SetActive(false);
					openMainMenu = false;
					Master.GetComponent <GameMaster>().EnablePlayer ();
				}
			}
		}

		if(openMainMenu){
			MainMenu.SetActive (true);
			Master.GetComponent <GameMaster>().DisablePlayer ();
		}else{
			MainMenu.SetActive (false);
			Master.GetComponent <GameMaster>().EnablePlayer ();
		}
	}

	public void PlayPickSwing(){
		Pick.GetComponent <Animator>().SetTrigger ("SwingPick");
	}
}
