using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateDistanceToTravel : MonoBehaviour {

    public Speed speed;
	// Use this for initialization
    
    public float numberOfDecimals = 0;
    public string postfix = "";
    Text text;
    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

        double value = speed.currentPlanet.distanceToTravel;

        string unit = "";
        UnitsConverter.Convert(ref value, ref unit);
        text.text = "" + value.ToString("f" + numberOfDecimals) + unit + " " + postfix;
    }
}
