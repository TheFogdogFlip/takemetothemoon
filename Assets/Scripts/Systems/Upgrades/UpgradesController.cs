using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpgradesController : MonoBehaviour {


    // Use this for initialization
    private static UpgradesController instance;

    public static UpgradesController Instance
    {
        get
        {
            //If _instance hasn't been set yet, we grab it from the scene!
            //This will only happen the first time this reference is used.
            if (instance == null)
                instance = GameObject.FindObjectOfType<UpgradesController>();
            return instance;
        }
    }


	//private Speed speed;
	public Upgrades upgrades;
	private float startTime;
	public float degradationTime = 10.0f;
	// Use this for initialization

	public void Init()
	{
		AddUpgrade (UpgradeTypes.Clicker);
	}

	void Start () {
		startTime = Time.time;
	}

	void Update()
	{
		//every upgrade degrades over time 
		if (Time.time > startTime + degradationTime) {
			DegradeUpgrades();
			RepairsMenuManager.Instance.UpdateMenu();
			startTime = Time.time;
		}
	}

	void DegradeUpgrades ()
	{
		upgrades.DegradeUpgrades ();
		upgrades.UpdateBaseSpeed ();
	}

	public void LevelUpUpgrade(UpgradeTypes upgrade)
	{
		upgrades.GetOwnedUpgrade (upgrade).LevelUp ();
		upgrades.UpdateBaseSpeed ();
	}

	public void LevelUpTenUpgrade(UpgradeTypes upgrade)
	{
		upgrades.GetOwnedUpgrade (upgrade).LevelUpTen ();
		upgrades.UpdateBaseSpeed ();
	}

    public void LevelUpUpgrade(UpgradeTypes upgrade, int level)
    {
        upgrades.GetOwnedUpgrade(upgrade).LevelUp(level);
        upgrades.UpdateBaseSpeed();
    }

	public void DamageUpgrade(UpgradeTypes upgrade, float damage)
	{
		upgrades.GetOwnedUpgrade (upgrade).Damage (damage);
		upgrades.UpdateBaseSpeed ();
	}

    public void DamageAllUpgrades(float damage)
    {
        upgrades.DamageAllUpgrades(damage);
        upgrades.UpdateBaseSpeed();
    }

	public void RepairUpgrade(UpgradeTypes upgrade)
	{
		upgrades.GetOwnedUpgrade (upgrade).Repair ();
		upgrades.UpdateBaseSpeed ();
	}

	public void AddUpgrade(UpgradeTypes upgrade)
	{
		upgrades.AddUpgrade (upgrade);
		upgrades.UpdateBaseSpeed ();
	}

	public void SetUpgradeHealth(UpgradeTypes upgrade, float health)
	{
		upgrades.GetOwnedUpgrade (upgrade).SetHealth (health);
		upgrades.UpdateBaseSpeed ();
	}
}
