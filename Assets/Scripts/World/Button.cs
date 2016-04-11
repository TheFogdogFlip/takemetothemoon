using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	public int CCount;
	public float moveStep;
	void Start(){

	}
	
	void OnTouchDown() {
	}
	void OnTouchUp() {
		//accelerate
		gameObject.GetComponent<Speed>().Accelerate();	

	}
	void OnTouchStay() {
	}
	void OnTouchExit() {
	}
}
