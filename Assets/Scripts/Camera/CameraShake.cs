using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;
	
	// How long the object should shake for.
	public double maxShake = 0.05;
	public float minSpeed = 30000.0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	public double shakeAmount = 0.001;
	public Speed speed;
	private Camera gameCam;
	Vector3 originalPos;

	void Start()
	{
		gameCam = GetComponent<Camera> ();
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}
	
	void Update()
	{

		gameCam.fieldOfView = 60.0f-speed.actualMultiplierCounter*5.0f;
		gameCam.fieldOfView = Mathf.Clamp (gameCam.fieldOfView, 50.0f, 60.0f);

		if (speed.actualSpeed > minSpeed)
		{
			double additionalShake = speed.actualMultiplierCounter*shakeAmount;
			if (additionalShake > maxShake) {
				additionalShake = maxShake;
			}
			camTransform.localPosition = originalPos + Random.insideUnitSphere * (float)additionalShake;
		
		}
		else
		{
			camTransform.localPosition = originalPos;
		}
	}
}