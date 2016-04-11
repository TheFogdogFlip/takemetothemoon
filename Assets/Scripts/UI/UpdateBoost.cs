using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateBoost : MonoBehaviour {

	// Use this for initialization
	private Image image;
	public Boost boost;
	void Start () {
		image = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		image.fillAmount = (float)(boost.boostAmount / boost.maxBoost);
	}
}
