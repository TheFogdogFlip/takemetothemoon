using UnityEngine;
using System.Collections;
using System;

public class Speed : MonoBehaviour {
	
	public double baseSpeed = 10;
	private double targetSpeed;
	public int multiplier = 1;
	public int maxMultiplier = 5;
	public float pointsPerClick = 0.0f;
	public float extraPointsPerSecond = 0.0f;
	public float decelerationFactor = 0.9f;
	public float multiplierDecelerationFactor = 0.98f;
	public double distance;
    public double points;
	private float lastAccelerationTime;
	public float clicksPerSecond;
	private double lastTimeClosed;
	private float newAdditionalSpeed = 0;
	private float targetMultiplierCounter = 0.0f;
	public float actualMultiplierCounter = 0.0f;
	public float multiplierCountLimit = 150.0f;
	public float smoothFactor = 0.9f;
	public double actualSpeed;
	public double ETA;
	public double autoPilotTimeLeft = 0;
	public double autoPilotBonusPoints = 0;
	public double pointsDistanceMultiplier = 100;
	Upgrade clicker;
	double boost;
	Boost boostComp;
	public CurrentPlanet currentPlanet;
	DisplayClickPoints displayClickPoints; 

	public void Init (GameObject planet)
    {
		currentPlanet = planet.GetComponent<CurrentPlanet> ();
		currentPlanet.UpdatePlanetPosition();
    }

	// Use this for initialization
	void Start () {
		boostComp = GetComponent<Boost> ();
		clicker = GetComponent<Upgrades> ().GetOwnedUpgrade (UpgradeTypes.Clicker);
		displayClickPoints = GetComponent<DisplayClickPoints> ();
		//editing prefab in realtime and saving locally 
		//GameObject newPrefab = Resources.Load ("TextPoints") as GameObject;
		//newPrefab.GetComponent<TextMesh> ().text ="mama";
		//end of editing prefab

		//reward for offline 
		//!must get time from server
		//DateTime currentDateTime = DateTime.Now;
		//TimeSpan ts = currentDateTime - Convert.ToDateTime (lastTimeClosed);
		//double extraPoints = ts.TotalSeconds * targetSpeed;
		//distance += Mathf.Round ((float)extraPoints);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
	    {
			lastTimeClosed = DateTime.Now.Subtract(DateTime.MinValue).TotalSeconds;
			Application.Quit(); 
		}
		targetMultiplierCounter*=multiplierDecelerationFactor; 
		actualMultiplierCounter = actualMultiplierCounter * smoothFactor + targetMultiplierCounter * (1.0f - smoothFactor);
		multiplier = (int)(actualMultiplierCounter/30.0f)+1;
		if (actualMultiplierCounter > multiplierCountLimit) {
			actualMultiplierCounter = multiplierCountLimit;
		}
		if (multiplier > maxMultiplier) {
			multiplier = maxMultiplier;
		}
		if (currentPlanet.traveledDistance >= currentPlanet.distanceToTravel) {
			return;
		}

		newAdditionalSpeed *= decelerationFactor;

		if (boostComp.GetBoostStatus()) {
			//boost = clicker.GetCurrentValue () * (double)maxMultiplier;
			boost = baseSpeed * 0.1f;
		}
		else {
			boost = 0;
		}

		targetSpeed = baseSpeed + newAdditionalSpeed + boost;

		actualSpeed = actualSpeed * smoothFactor + targetSpeed * (1 - smoothFactor);

		distance += (actualSpeed*Time.deltaTime);
		points += (actualSpeed * Time.deltaTime)*pointsDistanceMultiplier;
		currentPlanet.MovePlanet(targetSpeed*Time.deltaTime);
		//pretty stuff should be handled by gui instead
		//currentSpeed = Mathf.Round ((float)currentSpeed);
		//distance = Mathf.Round ((float)distance);

		if (Time.time - lastAccelerationTime > 1.0f) {
			clicksPerSecond = 1.0f;
		}
		CalculateETA ();
	}
	
	public void Accelerate()
	{
		AudioManager.Instance.AudioSources ["Tap"].Play ();
		//account for upgrades
		float currentTime = Time.time;
		float timeDiff = currentTime - lastAccelerationTime;
		//if break between clicks was long we make sure he gets at least the amount of 1 click per second
		timeDiff = Mathf.Clamp (timeDiff, 0.01f, 1.0f);

		//calculate points per click with multiplier and clicker upgrade
		pointsPerClick = CalculatePointsPerClick();
		//and we display how many points we add per click
		displayClickPoints.TapPoints (pointsPerClick);

		clicksPerSecond = (1.0f / timeDiff);
		targetMultiplierCounter+=clicksPerSecond; 
		//this translates into the additional speed per second 
		float additionalSpeed = (float)(clicksPerSecond*clicker.GetCurrentValue());
		//we update current speed with baseSpeed and the additional one from fast clicking per second 
		//currentSpeed = baseSpeed+additionalSpeed;

		extraPointsPerSecond = additionalSpeed * multiplier;
		lastAccelerationTime = currentTime;
		newAdditionalSpeed += extraPointsPerSecond;
		//GetComponent<UpgradesController>().LevelUpTenUpgrade (UpgradeTypes.Clicker);

	}

	public float CalculatePointsPerClick()
	{
		//use current multiplier on clicker upgrade and calculate points per click
			return (float)(clicker.GetCurrentValue () * multiplier);
	}

	public void UpdateBaseSpeed(double newBaseSpeed)
	{
		baseSpeed = newBaseSpeed;
	}

	public void CalculateETA()
	{
		if (actualSpeed > 0) 
		{
			double totalTime = currentPlanet.distanceToTravel / actualSpeed;
			double timeTaveled = currentPlanet.traveledDistance / actualSpeed;
			double timeLeft = totalTime - timeTaveled; 
			ETA = timeLeft;
		}
	}

	public double GetBaseETA()
    {
		double totalTime = currentPlanet.distanceToTravel / baseSpeed;
		double timeTaveled = currentPlanet.traveledDistance / baseSpeed;
        double timeLeft = totalTime - timeTaveled;
        ETA = timeLeft;
        return ETA;
	}

	public void GiveMeMoney()
	{
		points += UnitsConverter.Trillion;
	}
}
