using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public static class UnitsConverter
{
    public const double Thousand = 1E+3;
	public const double Million = 1E+6;
	public const double Billion = 1E+9;
	public const double Trillion = 1E+12;
	public const double Quadrillion = 1E+15;
	public const double Quintillion = 1E+18;
	public const double Sextillion = 1E+21;
	public const double Septillion = 1E+24;
	public const double Octillion = 1E+27;
	public const double Nonillion = 1E+30;
	public const double Decillion = 1E+33;
	public const double Undecillion = 1E+36;
	public const double Duodecillion = 1E+39;
	public const double Tredecillion = 1E+42;
	public const double Quattuordecillion = 1E+45;
	public const double Quindecillion = 1E+48;
	public const double Sexdecillion = 1E+51;
	public const double Septendecillion = 1E+54;
	public const double Octodecillion = 1E+57;
	public const double Novemdecillion = 1E+60;
	public const double Vigintillion = 1E+63;
	public const double Centillion = 1E+303;
	public static void Convert(ref double number, ref string name)
	{
        if (number < Thousand) {
            //return number
        }
		else if (number < Million) {
			//return number;
            number = number/Thousand;
            name = " k";
		}
		else if (number < Billion) {
			//return Million
			number = number/Million;
			name = " Million";
		}
		else if (number < Trillion) {
			//return Billion
			number = number/Billion;
			name = " Billion";
		}
		else if (number < Quadrillion) {
			//return Trillion
			number = number/Trillion;
			name = " Trillion";
		}
		else if (number < Quintillion) {
			//return Quadrillion
			number = number/Quadrillion;
			name = " Quadrillion";
		}
		else if (number < Sextillion) {
			//return Quintillion
			number = number/Quintillion;
			name = " Quintillion";
		}
		else if (number < Septillion) {
			//return Sextillion
			number = number/Sextillion;
			name = " Sextillion";
		}
		else if (number < Octillion) {
			//return Septillion
			number = number/Septillion;
			name = " Septillion";
		}
		else if (number < Nonillion) {
			//return Octillion
			number = number/Octillion;
			name = " Octillion";
		}
		else if (number < Decillion) {
			//return Nonillion
			number = number/Nonillion;
			name = " Nonillion";
		}
		else if (number < Undecillion) {
			//return Decillion
			number = number/Decillion;
			name = " Decillion";
		}
		else if (number < Duodecillion) {
			//return Undecillion
			number = number/Undecillion;
			name = " Undecillion";
		}
		else if (number < Tredecillion) {
			//return Duodecillion
			number = number/Duodecillion;
			name = " Duodecillion";
		}
		else if (number < Quattuordecillion) {
			//return Tredecillion
			number = number/Tredecillion;
			name = " Tredecillion";
		}
		else if (number < Quindecillion) {
			//return Quattuordecillion
			number = number/Quattuordecillion;
			name = " Quattuordecillion";
		}
		else if (number < Sexdecillion) {
			//return Quindecillion
			number = number/Quindecillion;
			name = " Quindecillion";
		}
		else if (number < Octodecillion) {
			//return Sexdecillion
			number = number/Sexdecillion;
			name = " Sexdecillion";
		}
		else if (number < Novemdecillion) {
			//return Octodecillion
			number = number/Octodecillion;
			name = " Octodecillion";
		}
		else if (number < Vigintillion) {
			//return Novemdecillion
			number = number/Novemdecillion;
			name = " Novemdecillion";
		}
		else if (number < Centillion) {
			//return Vigintillion
			number = number/Vigintillion;
			name = " Vigintillion";
		}
		else if (number < Mathf.Infinity) {
			//return Centillion
			number = number/Centillion;
			name = " Centillion";
		}
		else {
			number = Mathf.Infinity;
            name = " Infinity";
		}
	}
}

public class UpgradesMenuManager : MonoBehaviour {



	// Use this for initialization
	private static UpgradesMenuManager instance;

	public static UpgradesMenuManager Instance
	{
		get
		{
			//If _instance hasn't been set yet, we grab it from the scene!
			//This will only happen the first time this reference is used.
			if(instance == null)
				instance = GameObject.FindObjectOfType<UpgradesMenuManager>();
			return instance;
		}
	}

	public Upgrades upgrades;
	List<GameObject> menuItems = new List<GameObject>(); 
	Dictionary<UpgradeTypes,Upgrade> allUpgrades;
	Dictionary<UpgradeTypes,Upgrade> ownedUpgrades;
	public Speed speed;
	public UpgradesController upgradesController;
	public float priceDecimals = 0;
	public float speedDecimals = 2;
	public UpdateAutoPilotTime AutoPilotText; 
	public TextObjectsPool textsPool;

	public void Initialize()
	{
		GetObjects ();
		BuildMenu ();
		UpdateMenu ();
		AutoPilotText.UpdateText();

	}

	void GetObjects()
	{
		allUpgrades = upgrades.allUpgrades;
		ownedUpgrades = upgrades.ownedUpgrades;
	}

	void BuildMenu()
	{
		GameObject autopilotitem = GameObject.Instantiate(Resources.Load ("Prefabs/AutoPilot1h")) as GameObject;
		autopilotitem.transform.SetParent(transform,false);
		autopilotitem.GetComponent<GUIMenuItem> ().type = UpgradeTypes.AutoPilot1h;
		menuItems.Add(autopilotitem);

		autopilotitem = GameObject.Instantiate(Resources.Load ("Prefabs/AutoPilot6h")) as GameObject;
		autopilotitem.transform.SetParent(transform,false);
		autopilotitem.GetComponent<GUIMenuItem> ().type = UpgradeTypes.AutoPilot6h;
		menuItems.Add(autopilotitem);

		autopilotitem = GameObject.Instantiate(Resources.Load ("Prefabs/AutoPilot12h")) as GameObject;
		autopilotitem.transform.SetParent(transform,false);
		autopilotitem.GetComponent<GUIMenuItem> ().type = UpgradeTypes.AutoPilot12h;
		menuItems.Add(autopilotitem);

		autopilotitem = GameObject.Instantiate(Resources.Load ("Prefabs/AutoPilot3d")) as GameObject;
		autopilotitem.transform.SetParent(transform,false);
		autopilotitem.GetComponent<GUIMenuItem> ().type = UpgradeTypes.AutoPilot3d;
		menuItems.Add(autopilotitem);

		autopilotitem = GameObject.Instantiate(Resources.Load ("Prefabs/AutoPilot1w")) as GameObject;
		autopilotitem.transform.SetParent(transform,false);
		autopilotitem.GetComponent<GUIMenuItem> ().type = UpgradeTypes.AutoPilot1w;
		menuItems.Add(autopilotitem);

		UpdateAutoPilotItems ();

		foreach (KeyValuePair<UpgradeTypes,Upgrade> upgradeItem in allUpgrades) {

			Upgrade upgrade = upgradeItem.Value;
			if (upgrade.type == UpgradeTypes.AutoPilot1h || upgrade.type == UpgradeTypes.AutoPilot6h || upgrade.type == UpgradeTypes.AutoPilot12h || upgradeItem.Key == UpgradeTypes.AutoPilot3d || upgradeItem.Key == UpgradeTypes.AutoPilot1w) {
				continue;	
			}
			else {
				double value = upgrade.GetCurrentLevelValue();
				string vunit = "";
				UnitsConverter.Convert(ref value, ref vunit);

				string name = upgrade.type.ToString();
				double cost = upgrade.GetCost();
				string cunit = "";
				UnitsConverter.Convert(ref cost, ref cunit);
				int level = upgrade.level;
				float bonus = upgrade.percentValueIncrease*100.0f;
				
				GameObject item = GameObject.Instantiate(Resources.Load ("Prefabs/UpgradeMenuItem")) as GameObject;
				item.GetComponent<GUIMenuItem> ().type = upgrade.type;
				Text[] comps = item.GetComponentsInChildren<Text>();
				Text txt = comps[0];
				txt.text = name.ToUpper();
				
				if (ownedUpgrades.ContainsKey(upgrade.type)) {
					txt = comps[1];

					txt.text = value.ToString("f"+speedDecimals) + vunit +" m/s";
					txt = comps[2];

					txt.text = "$ "+cost.ToString("f"+priceDecimals) + cunit;
					txt = comps[3];
                    double val = upgrade.GetNextLevelValue()-value;
                    vunit = "";
                    UnitsConverter.Convert(ref val, ref vunit);
                    txt.text = "+ " + bonus + "%" + " " + val.ToString("f" + speedDecimals) + vunit;
					txt = comps[4];
					txt.text = "Level\n"+level;
				}
				else {
					Selectable selectableButton = item.GetComponent<Selectable>();
					ColorBlock colors = item.GetComponent<Selectable>().colors;
					colors.normalColor = new Color(0.91f,0.55f,0.0f,1.0f);
					colors.disabledColor = new Color(0.68f,0.48f,0.16f,1.0f);
					colors.highlightedColor = new Color(1.0f,0.68f,0.0f,1.0f);
					selectableButton.colors = colors;
					txt = comps[1];
					double bvalue = upgrade.baseValue;
					vunit = "";
					UnitsConverter.Convert(ref bvalue, ref vunit);
					txt.text = bvalue.ToString("f"+speedDecimals) + vunit + " m/s";
					txt = comps[2];
					txt.text = "$ "+cost.ToString("f"+priceDecimals) + cunit;
					txt.color = Color.black;
					txt = comps[3];
                    double val = upgrade.GetNextLevelValue() - bvalue;
                    vunit = "";
                    UnitsConverter.Convert(ref val, ref vunit);
                    txt.text = "+ " + bonus + "%" + " " + val.ToString("f" + speedDecimals) + vunit;
					txt.color = Color.black;
					txt = comps[4];
					txt.text = "NEW";
				}
				item.transform.SetParent(transform,false);
				menuItems.Add(item);
			}
			//parent = menuItems[i].transform;
		}
		//menuItems [3].transform.SetSiblingIndex (0);

	}

	// Update is called once per frame
	void Update () {
		CheckIfCanBuy ();
	}


	void CheckIfCanBuy()
	{
		foreach (GameObject menuItem in menuItems) {
			UpgradeTypes itemType = menuItem.GetComponent<GUIMenuItem> ().type;
			if (ownedUpgrades.ContainsKey (itemType)) {
				if (ownedUpgrades [itemType].GetCost () <= speed.points) {
					//enable in list
					if (!menuItem.GetComponent<Selectable> ().IsInteractable ()) {
						menuItem.GetComponent<Selectable> ().interactable = true;
					}
				} else {
					//disable in list
					if (menuItem.GetComponent<Selectable> ().IsInteractable ()) {
						menuItem.GetComponent<Selectable> ().interactable = false;
					}
				}
			} 
			else {
				if (allUpgrades [itemType].GetCost () <= speed.points) {
					//enable in list
					if (!menuItem.GetComponent<Selectable> ().IsInteractable ()) {
						menuItem.GetComponent<Selectable> ().interactable = true;
					}
				} else {
					//disable in list
					if (menuItem.GetComponent<Selectable> ().IsInteractable ()) {
						menuItem.GetComponent<Selectable> ().interactable = false;
					}
				}
			}
		}
	}

	public void BuyUpgrade(GameObject menuItem)
	{
        AudioManager.Instance.AudioSources["Tap"].Play();
		UpgradeTypes itemType = menuItem.GetComponent<GUIMenuItem> ().type;
		if (instance.ownedUpgrades.ContainsKey(itemType)) {
			Upgrade upgrade = instance.ownedUpgrades[itemType];
			SpawnText(menuItem, upgrade);

			instance.speed.points-=upgrade.GetCost();
			instance.upgradesController.LevelUpUpgrade(itemType);

		}
		else {
			Upgrade upgrade = instance.allUpgrades[itemType];
			SpawnText(menuItem, upgrade);
			instance.speed.points-=upgrade.GetCost();
			instance.upgradesController.AddUpgrade (itemType);
			instance.upgradesController.LevelUpUpgrade(itemType);

			Text[] comps = menuItem.GetComponentsInChildren<Text>();
			Text txt = comps[2];
			txt.color = Color.magenta;
			txt = comps[3];
			txt.color = Color.cyan;

			Selectable selectableButton = menuItem.GetComponent<Selectable>();
			ColorBlock colors = menuItem.GetComponent<Selectable>().colors;
			colors.normalColor = new Color(0.06f,0.08f,0.16f,1.0f);
			colors.disabledColor = new Color(0.2f,0.2f,0.2f,1.0f);
			colors.highlightedColor = new Color(0.09f,0.12f,0.31f,1.0f);
			selectableButton.colors = colors;

			RepairsMenuManager.Instance.AddItem(itemType);
		}

		Upgrade ownedUpgrade = instance.ownedUpgrades [itemType];
		Text[] texts = menuItem.GetComponentsInChildren<Text>();
		Text txtx = texts[1];
		double value = ownedUpgrade.GetCurrentLevelValue ();
		string unit = "";
		UnitsConverter.Convert(ref value, ref unit);
		txtx.text = value.ToString("f"+instance.speedDecimals) + unit +" m/s";
		txtx = texts[2];
		double cost = ownedUpgrade.GetCost();
		unit = "";
		UnitsConverter.Convert(ref cost, ref unit);
		txtx.text = "$ "+cost.ToString("f"+instance.priceDecimals) + unit;

        txtx = texts[3];
        double val = ownedUpgrade.GetNextLevelValue() - value;
        unit = "";
        UnitsConverter.Convert(ref val, ref unit);
        float bonus = ownedUpgrade.percentValueIncrease * 100.0f;
        txtx.text = "+ " + bonus + "%" + " " + val.ToString("f" + speedDecimals) + unit;

		txtx = texts[4];
		txtx.text = "Level\n"+ownedUpgrade.level;
	}


	public void BuyAndLevelUpAllUpgrades()
	{
		foreach (GameObject menuItem in menuItems) {
			UpgradeTypes itemType = menuItem.GetComponent<GUIMenuItem> ().type;
			if (itemType == UpgradeTypes.AutoPilot1h || itemType == UpgradeTypes.AutoPilot6h || itemType == UpgradeTypes.AutoPilot12h || itemType == UpgradeTypes.AutoPilot3d || itemType == UpgradeTypes.AutoPilot1w) {
				continue;	
			}
			else {
				for (int i = 0; i < 5; i++) {
					BuyUpgrade(menuItem);
				}
			}
		}
	}


	public void BuyAutoPilot(GameObject menuItem)
	{
		UpgradeTypes itemType = menuItem.GetComponent<GUIMenuItem> ().type;
		if (instance.allUpgrades.ContainsKey(itemType)) {
			Upgrade upgrade = instance.allUpgrades[itemType];
			instance.speed.autoPilotTimeLeft+=upgrade.baseValue;
			instance.speed.points-=upgrade.GetCost();
			instance.AutoPilotText.UpdateText();
		}
	}

	void SpawnText(GameObject menuItem, Upgrade upgrade)
	{
		GameObject text = instance.textsPool.Enqueue ();
		if (text) {
			text.transform.position = menuItem.transform.position;
			text.transform.rotation = new Quaternion(0f,0f,0f,0f);
			text.transform.Rotate(new Vector3(0.0f,-90.0f,0.0f));
			text.transform.Translate(new Vector3(0.0f,0.0f,-0.1f));
			TextMesh tMesh = text.GetComponent<TextMesh> ();
			double value = upgrade.GetCost();
			string unit = "";
			UnitsConverter.Convert(ref value, ref unit);
			tMesh.text = "-"+value.ToString("f"+priceDecimals) + unit;
			tMesh.color = Color.magenta;
			tMesh.characterSize = 0.003f;
			ClickPoints cp = text.GetComponent<ClickPoints>();
			cp.floatUpSpeed = -0.06f;
			cp.fadeTime = 0.8f;
			cp.Animate();
		}

		text = instance.textsPool.Enqueue ();
		if (text) {
			text.transform.position = menuItem.transform.position;
			text.transform.rotation = new Quaternion (0f, 0f, 0f, 0f);
			text.transform.Rotate (new Vector3 (0.0f, -90.0f, 0.0f));
			text.transform.Translate (new Vector3 (-0.05f, -0.02f, -0.1f));
			TextMesh tMesh = text.GetComponent<TextMesh> ();
			double valueDifference = upgrade.GetNextLevelValue () - upgrade.GetCurrentLevelValue ();
			string unit = "";
			UnitsConverter.Convert (ref valueDifference, ref unit);
			tMesh.text = "+" + valueDifference.ToString ("f" + speedDecimals) + unit;
			tMesh.characterSize = 0.003f;
			tMesh.color = Color.cyan;
			ClickPoints cp = text.GetComponent<ClickPoints> ();
			cp.floatUpSpeed = 0.06f;
			cp.fadeTime = 0.8f;
			cp.Animate ();
		}

		text = instance.textsPool.Enqueue ();
		if (text) {
			text.transform.position = menuItem.transform.position;
			text.transform.rotation = new Quaternion (0f, 0f, 0f, 0f);		
			text.transform.Rotate (new Vector3 (0.0f, -90.0f, 0.0f));
			text.transform.Translate (new Vector3 (-0.05f, 0.0f, -0.1f));
			TextMesh tMesh = text.GetComponent<TextMesh> ();
			double healthDifference = (double)upgrade.GetNextLevelHealth () - upgrade.GetCurrentLevelHealth ();
			string unit = "";
			UnitsConverter.Convert (ref healthDifference, ref unit);
			tMesh.text = "+" + healthDifference.ToString ("f" + RepairsMenuManager.Instance.healthDecimals) + unit;
			tMesh.characterSize = 0.003f;
			tMesh.color = Color.green;
			ClickPoints cp = text.GetComponent<ClickPoints> ();
			cp.floatUpSpeed = 0.06f;
			cp.fadeTime = 0.8f;
			cp.Animate ();
		}
	}

	void UpdateAutoPilotItems()
	{
		foreach (GameObject menuItem in menuItems) {
			UpgradeTypes itemType = menuItem.GetComponent<GUIMenuItem> ().type;;
			Upgrade upgrade = allUpgrades[itemType];
			Text[] comps = menuItem.GetComponentsInChildren<Text>();
			Text txt = comps[2];
			txt.color = Color.magenta;
			double cost = upgrade.GetCost();
			string unit = "";
			UnitsConverter.Convert(ref cost, ref unit);
			txt.text = "$ "+cost.ToString("f"+priceDecimals) + unit;
			txt = comps[3];
		}
	}

	public void UpdateMenu()
	{
		foreach (GameObject menuItem in menuItems) {
			UpgradeTypes itemType = menuItem.GetComponent<GUIMenuItem> ().type;;
			if (ownedUpgrades.ContainsKey(itemType)) {
				Upgrade upgrade = ownedUpgrades[itemType];
				Text[] comps = menuItem.GetComponentsInChildren<Text>();
				Text txt = comps[1];
				double value = upgrade.GetCurrentLevelValue();
				string unit = "";
				UnitsConverter.Convert(ref value, ref unit);
				txt.text = value.ToString("f"+speedDecimals) + unit + " m/s";
				txt = comps[2];
				txt.color = Color.magenta;
				double cost = upgrade.GetCost();
				unit = "";
				UnitsConverter.Convert(ref cost, ref unit);
				txt.text = "$ "+cost.ToString("f"+priceDecimals) + unit;
				txt = comps[3];

                double val = upgrade.GetNextLevelValue() - value;
                unit = "";
                UnitsConverter.Convert(ref val, ref unit);
                float bonus = upgrade.percentValueIncrease * 100.0f;
                txt.text = "+ " + bonus + "%" + " " + val.ToString("f" + speedDecimals) + unit;

				txt.color = Color.cyan;
				txt = comps[4];
				txt.text = "Level\n"+upgrade.level;
				Selectable selectableButton = menuItem.GetComponent<Selectable>();
				ColorBlock colors = menuItem.GetComponent<Selectable>().colors;
				colors.normalColor = new Color(0.06f,0.08f,0.16f,1.0f);
				colors.disabledColor = new Color(0.2f,0.2f,0.2f,1.0f);
				colors.highlightedColor = new Color(0.09f,0.12f,0.31f,1.0f);
				selectableButton.colors = colors;
			}
		}
	}
}
