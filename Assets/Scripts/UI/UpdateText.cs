using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//NEEDS FIXING!
public class UpdateText : MonoBehaviour{


	public GameObject entity;

	public string var;
	public float numberOfDecimals = 0;
	public string postfix = "";
    Text text;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

		double value = (double)(entity.GetComponent<Speed>().GetType ().GetField (var).GetValue(entity.GetComponent<Speed>()));

		string unit = "";
		UnitsConverter.Convert(ref value, ref unit);
		text.text = ""+value.ToString("f"+numberOfDecimals) + unit + " " + postfix;
	}
}