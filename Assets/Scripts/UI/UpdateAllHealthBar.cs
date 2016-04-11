using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateAllHealthBar : MonoBehaviour {

	// Use this for initialization
	private Image image;
	private RepairsMenuManager repairManager;
	Speed speed;
	void Start () {
		image = GetComponent<Image> ();		
		repairManager = RepairsMenuManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		image.fillAmount = (repairManager.totalHealth / repairManager.totalMaxHealth);
	}
}
