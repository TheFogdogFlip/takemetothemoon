using UnityEngine;
using System.Collections;

public class CurrentPlanet : MonoBehaviour {
	public double distanceToTravel =  384403000; //meters
	public double traveledDistance;
	public float moveStep;
	Vector3 targetPos;
	Vector3 startPos;
	float worldDistance;
	// Use this for initialization
	void Start () {
		targetPos = GameObject.Find ("Game Camera").transform.position;
		targetPos.z = targetPos.z+5;
		targetPos.y = targetPos.y - 0.5f;
		startPos = this.transform.position;

		worldDistance = (targetPos - startPos).magnitude;
		UpdatePlanetPosition ();
	}
	
	// Update is called once per frame
	void Update () {
	}
	void OnGUI()
	{
		if (traveledDistance >= distanceToTravel) {
			if (GUI.Button(new Rect(Screen.width/2 - 100, Screen.height/2 - 50, 200, 100), "Press to land")){
				GameControl.control.Save(Application.persistentDataPath + "/tempInfo.dat");
				GameControl.control.LoadGameData();
				GameObject.Find ("GameSystem").GetComponent<GameStateManager> ().CleanGame(ApplicationModel.Instance.Player);
			}
		}
	}
	public void MovePlanet(double moveDist){
		if (traveledDistance >= distanceToTravel) {
			return;
		}
		traveledDistance += moveDist;
		//float procTotalStep = (float)(traveledDistance / distanceToTravel);
		float procStep = (float)(moveDist / distanceToTravel);

		moveStep = worldDistance * procStep;

		this.transform.position = Vector3.MoveTowards (this.transform.position, targetPos, moveStep);
	}

	public void UpdatePlanetPosition()
	{
		float procTotalStep = (float)(traveledDistance / distanceToTravel);

		moveStep = worldDistance * procTotalStep;
		
		this.transform.position = Vector3.MoveTowards (this.transform.position, targetPos, moveStep);
	}

}
