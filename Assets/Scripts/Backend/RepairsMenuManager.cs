using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RepairsMenuManager : MonoBehaviour {

	// Use this for initialization
	private static RepairsMenuManager instance;

	public static RepairsMenuManager Instance
	{
		get
		{
			//If _instance hasn't been set yet, we grab it from the scene!
			//This will only happen the first time this reference is used.
			if(instance == null)
				instance = GameObject.FindObjectOfType<RepairsMenuManager>();
			return instance;
		}
	}

	public Upgrades upgrades;
	public Speed speed;
	public UpgradesController upgradesController;

	public GameObject[] menuItems;
	Dictionary<UpgradeTypes,Upgrade> ownedUpgrades;


	public float priceDecimals = 2;
	public float healthDecimals = 2;
	public GameObject repairAllItem;
	public float totalHealth;
	public float totalMaxHealth;
	double totalRepairPrice;

	public void Initialize()
	{
		GetObjects ();
		BuildMenu ();
		UpdateMenu ();
	}

	void GetObjects()
	{
		menuItems = new GameObject[upgrades.GetNumberOfUpgrades()];
		ownedUpgrades = upgrades.ownedUpgrades;
	}

	void BuildMenu()
	{
		totalRepairPrice = 0;
		totalHealth = 0;
		totalMaxHealth = 0;
		int j = 0;
		foreach (KeyValuePair<UpgradeTypes,Upgrade> upgradeItem in ownedUpgrades) {
			if (upgradeItem.Key != UpgradeTypes.Clicker && upgradeItem.Key != UpgradeTypes.AutoPilot1h && upgradeItem.Key != UpgradeTypes.AutoPilot6h && upgradeItem.Key != UpgradeTypes.AutoPilot12h && upgradeItem.Key != UpgradeTypes.AutoPilot3d && upgradeItem.Key != UpgradeTypes.AutoPilot1w) {
				Upgrade upgrade = upgradeItem.Value;

				string name = upgrade.type.ToString();
				float health = upgrade.health;
				double repaircost = upgrade.GetRepairCost();
				float maxhealth = upgrade.GetCurrentLevelHealth();

				totalRepairPrice+=repaircost;
				totalHealth+=health;
				totalMaxHealth+=maxhealth;

				GameObject menuItem = GameObject.Instantiate(Resources.Load ("Prefabs/RepairMenuItem")) as GameObject;
				menuItem.GetComponentInChildren <UpdateItemHealthBar> ().SetItemUpgrade (upgrade);
				menuItem.GetComponent<GUIMenuItem>().type = upgrade.type;

				Text[] comps = menuItem.GetComponentsInChildren<Text>();
				Text txt = comps[0];
				txt.text = name.ToUpper();
				txt = comps[1];
				double hvalue = (double)health;
				double mhvalue = (double)maxhealth;
				string hunit = "";
				string mhunit = "";
				UnitsConverter.Convert(ref hvalue, ref hunit);
				UnitsConverter.Convert(ref mhvalue, ref mhunit);
				txt.text = "Health: " + hvalue.ToString("f"+healthDecimals) + hunit + "/" +mhvalue.ToString("f"+healthDecimals) + mhunit;
				txt = comps[2];
				string runit = "";
				UnitsConverter.Convert(ref repaircost, ref runit);
				txt.text = "Repair: $" + repaircost.ToString("f"+priceDecimals) + runit;

				menuItem.transform.SetParent(transform,false);

				menuItems[j] = menuItem;
				//parent = menuItems[i].transform;
				j++;
			}
		}
		//menuItems [3].transform.SetSiblingIndex (0);
	}

	// Update is called once per frame
	void Update () {
		CheckIfCanRepair ();
	}


	void CheckIfCanRepair()
	{
		totalRepairPrice = 0;
		totalHealth = 0;
		totalMaxHealth = 0;
		bool isAnyItemBroken = false;
		foreach (GameObject menuItem in menuItems) {
			if (menuItem == null) {
				break;
			}
			UpgradeTypes itemType = menuItem.GetComponent<GUIMenuItem>().type;
			Upgrade ownedUpgrade = upgrades.GetOwnedUpgrade (itemType);

			double repairCost = ownedUpgrade.GetRepairCost();
			totalRepairPrice+=repairCost;
			totalHealth+=ownedUpgrade.health;
			totalMaxHealth+=ownedUpgrade.GetCurrentLevelHealth();
			//check first if item is broken
			//check if i can repair specific item
			bool isItemBroken = ownedUpgrade.IsBroken();
			//Debug.Log (isItemBroken);
			if (isItemBroken && repairCost <= speed.points) {
				//enable in list
				isAnyItemBroken = true;
				if (!menuItem.GetComponent<Selectable>().IsInteractable()) {
					menuItem.GetComponent<Selectable>().interactable = true;
				}
			}
			else {
				//disable in list
				if (menuItem.GetComponent<Selectable>().IsInteractable()) {
					menuItem.GetComponent<Selectable>().interactable = false;
				}
			}
		}
		if (isAnyItemBroken && totalRepairPrice <= speed.points) {
			//enable repair all 
			if (!repairAllItem.GetComponent<Selectable>().IsInteractable()) {
				repairAllItem.GetComponent<Selectable>().interactable = true;
			}
		}
		else {
			//disable repair all
			if (repairAllItem.GetComponent<Selectable>().IsInteractable()) {
				repairAllItem.GetComponent<Selectable>().interactable = false;
			}
		}
		UpdateRepairAllButton ();
	}

	public void RepairAll()
	{
        AudioManager.Instance.AudioSources["Tap"].Play();
		instance.totalRepairPrice = 0;
		instance.totalHealth = 0;
		instance.totalMaxHealth = 0;
		foreach (GameObject menuItem in instance.menuItems) {
			if (menuItem == null) {
				break;
			}

			UpgradeTypes itemType = menuItem.GetComponent<GUIMenuItem>().type;
			Upgrade ownedUpgrade = instance.upgrades.GetOwnedUpgrade (itemType);

			instance.speed.points-=ownedUpgrade.GetRepairCost();
			instance.upgradesController.RepairUpgrade(itemType);

			double repairCost = ownedUpgrade.GetRepairCost();
			float health = ownedUpgrade.health;
			float maxHealth = ownedUpgrade.GetCurrentLevelHealth();

			instance.totalRepairPrice+=repairCost;
			instance.totalHealth+=health;
			instance.totalMaxHealth+=ownedUpgrade.GetCurrentLevelHealth();

			Text[] comps = menuItem.GetComponentsInChildren<Text>();
			Text txt = comps[1];
			double hvalue = (double)health;
			double mhvalue = (double)maxHealth;
			string hunit = "";
			string mhunit = "";
			UnitsConverter.Convert(ref hvalue, ref hunit);
			UnitsConverter.Convert(ref mhvalue, ref mhunit);
			txt.text = "Health: " + hvalue.ToString("f"+instance.healthDecimals) + hunit + "/" +mhvalue.ToString("f"+instance.healthDecimals) + mhunit;
			txt = comps[2];
			string runit = "";
			UnitsConverter.Convert(ref repairCost, ref runit);
			txt.text = "Repair: $" + repairCost.ToString("f"+instance.priceDecimals) + runit;
		}
	}

	public void Repair(GameObject menuItem)
	{
        AudioManager.Instance.AudioSources["Tap"].Play();
		UpgradeTypes itemType = menuItem.GetComponent<GUIMenuItem>().type;
		Upgrade ownedUpgrade = instance.upgrades.GetOwnedUpgrade (itemType);

		float healthBefore = ownedUpgrade.health;
		double repairCost = ownedUpgrade.GetRepairCost ();
		instance.speed.points-=repairCost;
		instance.upgradesController.RepairUpgrade(itemType);

		Text[] comps = menuItem.GetComponentsInChildren<Text>();
		Text txt = comps[1];
		double hvalue = ownedUpgrade.health;
		string hunit = "";
		UnitsConverter.Convert(ref hvalue, ref hunit);
		txt.text = "Health: " + hvalue.ToString("f"+instance.healthDecimals) + hunit;
		txt = comps[2];
		double rvalue = ownedUpgrade.GetRepairCost ();
		string runit = "";
		UnitsConverter.Convert(ref rvalue, ref runit );
		txt.text = "Repair: $" + rvalue.ToString("f"+instance.priceDecimals) + runit;

		//rather than updating values of all items we just fix the value for the repaired item and update total with difference
		float healthAfter = ownedUpgrade.health;
		instance.totalHealth += (healthAfter - healthBefore);
		instance.totalRepairPrice -= repairCost;
	}

	public void UpdateRepairAllButton()
	{
		Text[] texts = instance.repairAllItem.GetComponentsInChildren<Text>();
		Text rtxt = texts[1];
		double hvalue = instance.totalHealth;
		double mhvalue = instance.totalMaxHealth;
		string hunit = "";
		string mhunit = "";
		UnitsConverter.Convert(ref hvalue, ref hunit);
		UnitsConverter.Convert(ref mhvalue, ref mhunit);
		rtxt.text = "Health: " + hvalue.ToString("f"+instance.healthDecimals) + hunit + "/" +mhvalue.ToString("f"+instance.healthDecimals) + mhunit;
		rtxt = texts[2];
		double totalRepairValue = instance.totalRepairPrice;
		string runit = "";
		UnitsConverter.Convert(ref totalRepairValue, ref runit);
		rtxt.text = "Repair: $" + totalRepairValue.ToString("f"+instance.priceDecimals) + runit;	
	}
	
	public void UpdateMenu()
	{
		foreach (GameObject menuItem in menuItems) {
			if (menuItem == null) {
				break;
			}
			UpgradeTypes itemType = menuItem.GetComponent<GUIMenuItem>().type;
			Upgrade ownedUpgrade = upgrades.GetOwnedUpgrade(itemType);

			Text[] comps = menuItem.GetComponentsInChildren<Text>();
			Text txt = comps[1];
			double hvalue = ownedUpgrade.health;
			double mhvalue = ownedUpgrade.GetCurrentLevelHealth();
			string hunit = "";
			string mhunit = "";
			UnitsConverter.Convert(ref hvalue, ref hunit);
			UnitsConverter.Convert(ref mhvalue, ref mhunit);
			txt.text = "Health: " + hvalue.ToString("f"+healthDecimals) + hunit + "/" +mhvalue.ToString("f"+healthDecimals) + mhunit;
			txt = comps[2];
			double repairCost = ownedUpgrade.GetRepairCost();
			string runit = "";
			UnitsConverter.Convert(ref repairCost, ref runit);
			txt.text = "Repair: $" + repairCost.ToString("f"+priceDecimals) + runit;
		}
	}

	public void AddItem(UpgradeTypes itemType)
	{
		GameObject menuItem = GameObject.Instantiate(Resources.Load ("Prefabs/RepairMenuItem")) as GameObject;
		Upgrade ownedUpgrade = upgrades.GetOwnedUpgrade(itemType);
		menuItem.GetComponentInChildren <UpdateItemHealthBar> ().SetItemUpgrade (ownedUpgrade);

		double health = (double)ownedUpgrade.health;
		string name = ownedUpgrade.type.ToString();
		double repaircost = ownedUpgrade.GetRepairCost();

		Text[] comps = menuItem.GetComponentsInChildren<Text>();
		Text txt = comps[0];
		txt.text = name.ToUpper();
		txt = comps[1];
		string hunit = "";
		UnitsConverter.Convert(ref health, ref hunit);
		txt.text = "Health: " + health.ToString("f"+healthDecimals) + hunit;
		txt = comps[2];
		string runit = "";
		UnitsConverter.Convert(ref repaircost, ref runit);
		txt.text = "Repair: $" + repaircost.ToString("f"+priceDecimals) + runit;	
		menuItem.GetComponent<GUIMenuItem> ().type = itemType;

		menuItem.transform.SetParent(transform,false);

		//as soon i make it inot the dictionary we can use add
		for (int i = 0; i < menuItems.Length; i++) {
			if (menuItems[i] == null) {
				menuItems[i] = menuItem;
				break;
			}
		}	
	}
}
