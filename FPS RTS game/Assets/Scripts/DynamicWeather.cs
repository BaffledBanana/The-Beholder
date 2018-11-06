using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWeather : MonoBehaviour {

	public int dayCycleTime, nightCycleTime;
	public float timeOfDay, startBlendAngle, sunMaxIntensity = 1.6f, minFog, maxFog;
	[Range(0, 1)] public float blendAmount;

	public Color nightFogColor, dayFogColor, morningFogColor;

	private float sunRotAmountX, sunX;
	private int cycleTime;
	private bool doBlend, doCheck, day;

	private Color lerpedColor;
	private Light mySun;

	void Start(){
		mySun = GameObject.Find ("TheSun").GetComponent <Light>();
		day = true;
		//dayFogColor = RenderSettings.fogColor;
		//from [0, 30] to [0, 1.6f]
	}

	void Update () {

		//CODE TO KEEP PERFECT TIME

		if (timeOfDay < cycleTime) {
			timeOfDay += Time.deltaTime;
		} else {
			day = !day;
			if(day){
				cycleTime = dayCycleTime;
			}else{
				cycleTime = nightCycleTime;
			}
			timeOfDay = 0;
		} 

		//CODE FOR BLENDING THE SKYBOXES

		if(sunX >= 0 && sunX <= startBlendAngle){
			BlendToDay ();
		}else if(sunX >= 180 - startBlendAngle && day && sunX < 180){
			BlendToNight ();
		}

		//CODE FOR ROTATING THE SUN

		if(day){
			sunX = mapRange (timeOfDay, 0, cycleTime, 0, 180);	
		}else {
			sunX = mapRange (timeOfDay, 0, cycleTime, -180, 0);
		}
		if(sunX >= 360){
			sunX = 0;
		}
		if(sunX < 90 && day){
			mySun.intensity = mapRange (sunX, 0, 90, 0, sunMaxIntensity);
		}else if(sunX > 90 && sunX < 180 && day){
			mySun.intensity = mapRange (sunX, 90, 180, sunMaxIntensity, 0);
		}else if(sunX > -180 && day == false){
			mySun.intensity = 0;
		}
		mySun.transform.localEulerAngles = new Vector3(sunX, mySun.transform.localRotation.y, mySun.transform.localRotation.z);
	
		//CODE FOR CONTROLING THE FOG

		if(day == false){
			RenderSettings.fogDensity = maxFog * 2;
			RenderSettings.fogColor = nightFogColor;
		}else{
			if(sunX < 90){
				RenderSettings.fogDensity = mapRange (sunX, 0, 89, maxFog, minFog);
				RenderSettings.fogColor = Color.Lerp (dayFogColor, morningFogColor, blendAmount);
			}else if(sunX > 90 && sunX < 180){
				RenderSettings.fogDensity = mapRange (sunX, 91, 179, minFog, maxFog);
				RenderSettings.fogColor = Color.Lerp (dayFogColor, nightFogColor, blendAmount);
			}
		}
		//RenderSettings.fogColor = Color.Lerp (dayFogColor, nightFogColor, blendAmount);
	}

	void BlendToNight(){
		blendAmount = mapRange (sunX, 180 - startBlendAngle, 180, 0, 1);
		if(blendAmount <= 1){
			RenderSettings.skybox.SetFloat ("_Blend", blendAmount);
		}else {
			blendAmount = 1;
			RenderSettings.skybox.SetFloat ("_Blend", blendAmount);
		} 
	}

	void BlendToDay(){
		blendAmount = mapRange (sunX, 0, startBlendAngle, 1, 0);// 0 is day skybox and 1 is night skybox
		if(blendAmount >= 0){
			RenderSettings.skybox.SetFloat ("_Blend", blendAmount);
		}else {
			blendAmount = 0;
			RenderSettings.skybox.SetFloat ("_Blend", blendAmount);
		} 
	}

	float mapRange(float s, float a1,float a2,float b1,float b2){
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}
}
