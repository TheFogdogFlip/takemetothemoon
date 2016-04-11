using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInput : MonoBehaviour {

	public LayerMask touchInputMask;

	private List<GameObject> touchList = new List<GameObject>();
	private GameObject[] touchesOld;
	private RaycastHit hit;
	private float distance = 1500.0f;
	private Speed speed;

	// Update is called once per frame
	void Start()
	{
		Input.multiTouchEnabled = false;
		speed = GetComponent<Speed> ();
	}

	void Update () {
		// Check if running in unity
#if UNITY_EDITOR
		if(Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) {
			touchesOld = new GameObject[touchList.Count];
			touchList.CopyTo (touchesOld);
			touchList.Clear();
			Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);

            ////Checks if any UI or other elements are in the way, if true then cancel the raycast method
            //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() ) {
            //    return;
            //}


			if(Physics.Raycast (ray, out hit,distance,touchInputMask)){
				GameObject recipient = hit.transform.gameObject;
				touchList.Add (recipient);
				
				if(Input.GetMouseButtonDown(0)) {
					recipient.SendMessage("OnTouchDown", SendMessageOptions.DontRequireReceiver);
				}
				if(Input.GetMouseButtonUp(0)) {
					recipient.SendMessage("OnTouchUp", SendMessageOptions.DontRequireReceiver);
				}
				if(Input.GetMouseButton(0)) {
					recipient.SendMessage("OnTouchStay", SendMessageOptions.DontRequireReceiver);
				}
			}
			foreach(GameObject g in touchesOld) {
				if(!touchList.Contains (g)) {
					g.SendMessage("OnTouchExit", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
#endif
		// If running in Android SDK
		if(Input.touchCount > 0) {
			touchesOld = new GameObject[touchList.Count];
			touchList.CopyTo (touchesOld);
			touchList.Clear();

			foreach(Touch touch in Input.touches) {
				Ray ray = GetComponent<Camera>().ScreenPointToRay(touch.position);
				if(Physics.Raycast (ray, out hit, distance,touchInputMask)){
					GameObject recipient = hit.transform.gameObject;
					touchList.Add (recipient);

                    ////Checks if any UI or other elements are in the way, if true then cancel the raycast method
                    //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
                    //    return;
                    //}


					if(touch.phase == TouchPhase.Began) {
						recipient.SendMessage("OnTouchDown", SendMessageOptions.DontRequireReceiver);
					}
					if(touch.phase == TouchPhase.Ended) {
						recipient.SendMessage("OnTouchUp", SendMessageOptions.DontRequireReceiver);
					}
					if(touch.phase == TouchPhase.Stationary) {
						recipient.SendMessage("OnTouchStay", SendMessageOptions.DontRequireReceiver);
					}
					if(touch.phase == TouchPhase.Stationary) {
						recipient.SendMessage("OnTouchExit", SendMessageOptions.DontRequireReceiver);
					}
				}
			}
			foreach(GameObject g in touchesOld) {
				if(!touchList.Contains (g)) {
					g.SendMessage("OnTouchExit", SendMessageOptions.DontRequireReceiver);
				}
			}
		}

	}
}
