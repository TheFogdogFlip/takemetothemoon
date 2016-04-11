using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateAutoPilotTime : MonoBehaviour {

	// Use this for initialization
	public Speed speed;
	Text text;
	void Start () {
	}

	public void UpdateText()
	{
		text = GetComponent<Text> ();
		float seconds = Mathf.Floor ((float)speed.autoPilotTimeLeft % 60.0f);
		float minutes = Mathf.Floor (((float)speed.autoPilotTimeLeft / 60.0f) % 60.0f);
		float hours = Mathf.Floor (((float)speed.autoPilotTimeLeft / 3600.0f) % 24.0f);
		float days = Mathf.Floor ((float)speed.autoPilotTimeLeft/86400);
		text.text = "Auto Pilot\nTime remaining: " + days + "d " + hours + "h " + minutes + "m "+seconds+"s" + "\nLast Bonus Received: " + speed.autoPilotBonusPoints.ToString("f2") + " Points";
	}
}
