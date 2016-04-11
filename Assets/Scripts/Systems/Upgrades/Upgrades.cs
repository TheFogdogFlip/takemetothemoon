using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Upgrades : MonoBehaviour {


	// Use this for initialization
	public Dictionary<UpgradeTypes,Upgrade> allUpgrades = new Dictionary<UpgradeTypes,Upgrade>();
	public Dictionary<UpgradeTypes,Upgrade> ownedUpgrades = new Dictionary<UpgradeTypes,Upgrade>();
	public Upgrade[] availableUpgrades; 

	//int howBig = myArray.Length;               // get the length of the array
	//myArray[i] = newValue;                     // set a value at position i
	//TheType thisValue = myArray[i];            // get a value from position i
	//System.Array.Resize(ref myArray, size);    //resize array
	
	//string[] weapons = new string[] { "Sword", "Knife", "Gun" };

	public void Init () {
		for (int i = 0; i < availableUpgrades.Length; i++) {
			if (availableUpgrades[i] != null) {
				Upgrade upgrade = availableUpgrades[i];
				allUpgrades.Add(upgrade.type, upgrade);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void AddUpgrade(UpgradeTypes upgrade)
	{
		if (!ownedUpgrades.ContainsKey(upgrade)) {
			ownedUpgrades.Add (upgrade, Instantiate (allUpgrades [upgrade]) as Upgrade);
		}
	}

	public double GetUpgradeValue(UpgradeTypes upgrade)
	{
		return ownedUpgrades[upgrade].GetCurrentValue();
	}

	public Upgrade GetOwnedUpgrade(UpgradeTypes upgrade)
	{
		return ownedUpgrades[upgrade];
	}

	public Upgrade GetAvaiableUpgrade(UpgradeTypes upgrade)
	{
		return allUpgrades[upgrade];
	}

	public void DegradeUpgrades ()
	{
		foreach (KeyValuePair<UpgradeTypes,Upgrade> upgrade in ownedUpgrades) {
			if (upgrade.Key != UpgradeTypes.Clicker) {
				upgrade.Value.Degrade();
			}
		}
	}

	public void UpdateBaseSpeed()
	{
		double newBaseSpeed = 0;
		foreach (KeyValuePair<UpgradeTypes,Upgrade> upgrade in ownedUpgrades) {
			if (upgrade.Key != UpgradeTypes.Clicker) {
				newBaseSpeed+=upgrade.Value.GetCurrentValue();
			}
		}
		GetComponent<Speed> ().UpdateBaseSpeed (newBaseSpeed);
	}

    public int GetNumberOfUpgrades()
    {
        return allUpgrades.Count;
    }

    public void DamageAllUpgrades(float damage)
    {
        foreach (KeyValuePair<UpgradeTypes, Upgrade> upgrade in ownedUpgrades)
        {
            if (upgrade.Key != UpgradeTypes.Clicker)
            {
                upgrade.Value.Damage(damage);
            }
        }
    }

}
