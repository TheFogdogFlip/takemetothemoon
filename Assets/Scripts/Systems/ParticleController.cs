using UnityEngine;
using System.Collections;

public class ParticleController : MonoBehaviour {

	public double maxStartSpeed = 1000;
	public double maxEmissionRate = 500;
	public double maxScaleLength = 500;

	public Speed speed;
	private ParticleSystemRenderer pSystemRenderer;
	private ParticleSystem pSystem;
	
	// Use this for initialization
	void Start () {
		pSystem = gameObject.GetComponent<ParticleSystem> ();
		pSystemRenderer = (ParticleSystemRenderer)pSystem.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (speed.currentPlanet.distanceToTravel <= speed.currentPlanet.traveledDistance)
        {
			pSystem.emissionRate = 0;
			return;
		}
		double newStartSpeed = (speed.actualSpeed*(speed.decelerationFactor*0.001));
		if (newStartSpeed > maxStartSpeed) {
			newStartSpeed = maxStartSpeed;
		}
		pSystem.startSpeed = (float)newStartSpeed;
		double newEmissionRate = (speed.actualSpeed*(speed.decelerationFactor*0.001));
		if (newEmissionRate > maxEmissionRate) {
			newEmissionRate = maxEmissionRate;
		}
		pSystem.emissionRate = (float)newEmissionRate;
		double newLengthScale = (speed.actualSpeed*(speed.decelerationFactor*0.001));
		if (newLengthScale > maxScaleLength) {
			newLengthScale = maxScaleLength;
		}
		pSystemRenderer.lengthScale = (float)newLengthScale;
	}
}
