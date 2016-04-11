using UnityEngine;
using System.Collections;

public class MoveTexture : MonoBehaviour {
	private MeshRenderer mRenderer;
	public double fps = 0.0001f;
	public double maxSpeed = 80000;
	
	public Speed speed;
	private double lastRecordedDistance = 0.0;
	private double currspeed;
	void Start()
	{
		mRenderer = GetComponent<MeshRenderer> ();
	}

	void Update()
	{
        if (speed.currentPlanet.distanceToTravel <= speed.currentPlanet.traveledDistance)
        {
			return;
		}
		double calc;
		if (speed.actualSpeed > maxSpeed) {
			currspeed = maxSpeed;
			lastRecordedDistance+=maxSpeed*Time.deltaTime;
			calc = lastRecordedDistance*fps;
		}
		else {
			currspeed = currspeed*speed.decelerationFactor;
			double additionalSpeed = currspeed*Time.deltaTime;
			if (additionalSpeed < speed.actualSpeed*Time.deltaTime) {
				additionalSpeed = speed.actualSpeed*Time.deltaTime;
			}
			lastRecordedDistance+=additionalSpeed;
			calc = lastRecordedDistance*fps;
		}
		mRenderer.material.SetTextureOffset ("_MainTex", new Vector2(0, (float)calc));
	}

}




