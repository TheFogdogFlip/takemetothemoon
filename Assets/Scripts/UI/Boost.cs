using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Boost : MonoBehaviour {

	public double boostAmount;
	public double maxBoost = 3600;
	public double boostDecelerationFactor = 0.99999;
	private bool boostEnabled;
	public float smoothFactor = 0.9f;
	//public double actualBoost = 0.0f;
	public double hoursToFullRegen = 100;
	Speed speed;
	public Image boostImage;
	float startTime;
	public float degradationTime = 1f;
	// Use this for initialization
	void Start () {
		boostEnabled = false;
		speed = GetComponent<Speed> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (boostEnabled) {
			if (Time.time > startTime + degradationTime) {
				boostAmount = boostAmount--;
				startTime = Time.time;
			}
			//boostAmount*=boostDecelerationFactor; 
			//actualBoost = actualBoost * smoothFactor + boostAmount * (1.0f - smoothFactor);
			if (boostAmount < 0.001) {
				ToggleBoost();
			}

		}
	}

	public void ToggleBoost()
	{
		if (boostEnabled) {
			boostEnabled = false;
			boostImage.color = new Color(1f,0.39f,0.0f,1.0f);
		}
		else {
			boostEnabled = true;
			startTime = Time.time;
			boostImage.color = new Color(1f,0.72f,0.0f,1.0f);
		}
	}

	public void SetBoostAmount(double boost, double timeDiff)
	{
		boostAmount = boost+(timeDiff/hoursToFullRegen);
		//actualBoost = boostAmount;
		if (boostAmount > maxBoost) {
			boostAmount = maxBoost;
		}
	}

	public bool GetBoostStatus()
	{
		return boostEnabled;
	}
}
