﻿using UnityEngine;
using System.Collections;
public class ProgressBar : MonoBehaviour {
	public float barDisplay; //current progress
	public Vector2 pos = new Vector2(130,510);
	public Vector2 size = new Vector2(40,150);
	public Texture2D emptyTex;
	public Texture2D fullTex;
	void OnGUI() {
		//draw the background:
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
		//draw the filled-in part:
		GUI.BeginGroup(new Rect(0,size.y-barDisplay*size.y, size.x, size.y* barDisplay));
		GUI.Box(new Rect(0,0, size.x, size.y), fullTex);
		GUI.EndGroup();
		GUI.EndGroup();
	}
	void Update() {
		//for this example, the bar display is linked to the current time,
		//however you would set this value based on your desired display
		//eg, the loading progress, the player's health, or whatever.
		//barDisplay = 1.0f-(float)(GetComponent<Speed>().baseSpeed/GetComponent<Speed>().currentSpeed);

		barDisplay = ((float)(GetComponent<Speed>().actualMultiplierCounter)/(float)(GetComponent<Speed>().multiplierCountLimit));
		// barDisplay = MyControlScript.staticHealth;
	}
}
