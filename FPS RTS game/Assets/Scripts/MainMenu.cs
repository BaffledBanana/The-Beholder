using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public GameObject Main, PlayMenu, OptionsMenu;

	private bool inverted;
	private List<GameObject> menus;
	private GameObject[] walls;
	// Use this for initialization
	void Start () {
		walls = GameObject.FindGameObjectsWithTag("Wall");
		foreach(GameObject w in walls){
			try{
				w.transform.Find("Canvas").gameObject.SetActive(false);
			}catch{}
		}
		menus = new List<GameObject>();
		menus.Add(PlayMenu);
		menus.Add(OptionsMenu);
		menus.Add(Main);
	}

	public void Play(){
		foreach(GameObject menu in menus){
			if(menu == PlayMenu){
				menu.SetActive(true);
			}else{
				menu.SetActive(false);
			}
		}
	}

	public void Options(){
		foreach(GameObject menu in menus){
			if(menu == OptionsMenu){
				menu.SetActive(true);
			}else{
				menu.SetActive(false);
			}
		}
	}

	public void Back(){
		foreach(GameObject menu in menus){
			if(menu == Main){
				menu.SetActive(true);
			}else{
				menu.SetActive(false);
			}
		}
	}

	public void InvertY(){
		inverted = !inverted;
		if(inverted){
			GameObject.Find("PlayerControler").GetComponent<PlayerControler>().InvertY();
			GameObject.Find("InvertText").GetComponent<Text>().text = "ON";
		}else{
			GameObject.Find("PlayerControler").GetComponent<PlayerControler>().InvertY();
			GameObject.Find("InvertText").GetComponent<Text>().text = "OFF";
		}
	}

	public void Quit(){
		Application.Quit();
	}

}
