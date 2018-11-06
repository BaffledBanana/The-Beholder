using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab : MonoBehaviour {

	public enum Research {None, Tavern};
	public Research researching;

	public void ResearchTavern(){
		if(researching == Research.None){
			researching = Research.Tavern;
		}
	}
}
