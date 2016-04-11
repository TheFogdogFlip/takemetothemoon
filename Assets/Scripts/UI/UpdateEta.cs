using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateEta : MonoBehaviour {
	private Text displayText;
	public Speed speed;
	public float updateInterval = 1.0f;
	private float lastTime = 0.0f;
	// Use this for initialization
	void Start () {
		displayText = this.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float currentTime = Time.time;
		if (currentTime > lastTime + updateInterval) 
		{
			float seconds = Mathf.Floor ((float)speed.ETA % 60.0f);
			float minutes = Mathf.Floor (((float)speed.ETA / 60.0f) % 60.0f);
			float hours = Mathf.Floor (((float)speed.ETA / 3600.0f) % 24.0f);
			float days = Mathf.Floor ((float)speed.ETA/86400);

            double val = (double)days;
            string vunit = "";
            UnitsConverter.Convert(ref val, ref vunit);
            if (vunit == " Infinity")
            {
                displayText.text = "ETA: N/A";
            }
            else
            {
                displayText.text = "ETA: " + val.ToString("f0") + vunit + " d " + hours + "h " + minutes + "m " + seconds + "s";
            }
			lastTime = currentTime;
		}
	}
}
