using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateItemHealthBar : MonoBehaviour {

	// Use this for initialization
	private Upgrade itemUpgrade;
	private Image image;
	void Start () {
		image = GetComponent<Image> ();		
	}
	
	// Update is called once per frame
	void Update () {
		image.fillAmount = (itemUpgrade.health / itemUpgrade.GetCurrentLevelHealth());
	}

	public void SetItemUpgrade(Upgrade upgrade)
	{
		itemUpgrade = upgrade;
	}
}
