using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UpdateDisplayMultiplierCounter : MonoBehaviour {

	// Use this for initialization
	private Image image;
	public Speed speed;
	void Start () {
		image = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		image.fillAmount = (speed.actualMultiplierCounter / speed.multiplierCountLimit);
	}
}
