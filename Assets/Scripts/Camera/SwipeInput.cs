using UnityEngine;
using System.Collections;

public class SwipeInput : MonoBehaviour {

	public TouchGesture.GestureSettings GestureSetting;
	public float rotationSpeed = 100.0f;
	private TouchGesture touch;
	private Vector3 rightDirection;
	private Vector3 leftDirection;
	private Vector3 forwardDirection;
	private Vector3 targetDir;
	void Start()
	{
		forwardDirection = transform.forward;
		touch = new TouchGesture (this.GestureSetting);
		StartCoroutine(touch.CheckHorizontalSwipes(
			onLeftSwipe: () => 
			{ 
				Vector3 tempDir = Quaternion.Euler(0, 90, 0)*forwardDirection;
				float angle = Vector3.Dot(tempDir, new Vector3(0.0f,0.0f,1.0f));
				if (angle > 0.0f) {
					targetDir = Quaternion.Euler(0, 90, 0)*forwardDirection;
					forwardDirection = tempDir;
				}
			},
			onRightSwipe: () => 
			{ 
				Vector3 tempDir = Quaternion.Euler(0, -90, 0)*forwardDirection;
				float angle = Vector3.Dot(tempDir, new Vector3(0.0f,0.0f,1.0f));
				if (angle > 0.0f) {
					targetDir = Quaternion.Euler(0, -90, 0)*forwardDirection;
					forwardDirection = tempDir;
				}
			}
		));
	}
	void Update()
	{
		float step = 3 * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0f);
		transform.localRotation = Quaternion.LookRotation (newDir);
	}

}