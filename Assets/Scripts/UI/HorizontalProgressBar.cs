using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HorizontalProgressBar : MonoBehaviour {
	public double barDisplay; //current progress
	private Vector3 targetPos;
	private Image wImg;
	private Vector3 startPos;
	private double previousDistance = 0;
	public Speed speed;
	float worldDistance;
	void Start()
	{
		targetPos = new Vector3 (360, 0, 0);
		worldDistance = (targetPos - this.transform.localPosition).magnitude;
	}

	void Update () 
	{
        barDisplay = speed.currentPlanet.traveledDistance / speed.currentPlanet.distanceToTravel;
		double barDisplayStep = barDisplay-previousDistance;
		float moveStep = worldDistance * (float)barDisplayStep;
		this.transform.localPosition = Vector3.MoveTowards (this.transform.localPosition, targetPos, moveStep);
		previousDistance = barDisplay;
	}
}
