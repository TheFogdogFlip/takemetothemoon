using UnityEngine;
using System.Collections;

public class Upgrade : MonoBehaviour {

	// Use this for initialization

	//public string upgradeName = "Grandma";
	public int level = 1;
	public float baseHealth = 1000f;
	public float health = 1000f;
    public float percentHealthIncPerLevel = 0.15f;
	public float durability = 1f;
	public double baseValue = 10;
	public double currentValue = 10;
    public float percentValueIncrease = 0.15f;
	public double baseCost = 20; 
	public double currentCost = 20;
	public float percentPriceIncrease = 0.15f;
	public double currentRepairCost = 20;
	public UpgradeTypes type = UpgradeTypes.Clicker;
	//public UpgradeSubTypes subType = UpgradeSubTypes.Embeded;

	void Start () {
		UpdateCurrentValue ();
		UpdateCurrentCost ();
		UpdateCurrentRepairCost ();
	}

	// Update is called once per frame
	void Update () {
	}

	public double GetCurrentValue()
	{
		UpdateCurrentValue ();
		return currentValue;
	}

	public double GetCurrentLevelValue()
	{
		//return (baseValue+(baseValue*(percentValueIncrease)*(float)(level-1))); //constant
        return (baseValue * Mathf.Pow((1f + percentValueIncrease), (float)(level - 1))); //exponential
	}

	public double GetNextLevelValue()
	{
		//return (baseValue+(baseValue*(percentValueIncrease)*(float)(level))); //constant
		return (baseValue * Mathf.Pow((1f + percentValueIncrease), (float)(level))); //exponential
	}

	public void UpdateCurrentValue()
	{
		currentValue = GetCurrentLevelValue() * (health / GetCurrentLevelHealth());
	}

	public void Degrade ()
	{
		Damage (10f);
	}

	public void Damage(float damage)
	{
		health = health - damage * (1f-durability);
        health = Mathf.Clamp(health, 0f, GetCurrentLevelHealth());
		UpdateCurrentValue ();
		UpdateCurrentRepairCost ();
	}

	public void SetHealth(float newHealth)
	{
		health = newHealth;
		UpdateCurrentValue ();
		UpdateCurrentRepairCost ();
	}
	
	public double GetCost()
	{
		UpdateCurrentCost ();
		return currentCost;
	}

	void UpdateCurrentCost()
	{
		currentCost = (baseCost * Mathf.Pow ((1f+percentPriceIncrease), (float)level));
	}

	public void LevelUp(int levels)
	{
		level += levels;
		UpdateHealth ();
		UpdateCurrentValue ();
		UpdateCurrentCost ();
		UpdateCurrentRepairCost ();
	}

	public void LevelUp()
	{
		level += 1;
		UpdateHealth ();
		UpdateCurrentValue ();
		UpdateCurrentCost ();
		UpdateCurrentRepairCost ();
	}

	public void LevelUpTen()
	{
		level += 10;
		UpdateHealth ();
		UpdateCurrentValue ();
		UpdateCurrentCost ();
		UpdateCurrentRepairCost ();
	}

	public void UpdateCurrentRepairCost()
	{
		currentRepairCost = GetCurrentLevelCost() * (1f-(health / GetCurrentLevelHealth()));
	}

	public double GetCurrentLevelCost()
	{
		return baseCost * Mathf.Pow ((1f+percentPriceIncrease), (float)(level-1));
	}

	public void UpdateHealth()
	{
		health = baseHealth+baseHealth*percentHealthIncPerLevel*(level-1);
	}

	public float GetNextLevelHealth()
	{
		return baseHealth+baseHealth*percentHealthIncPerLevel*(level);
	}

	public float GetCurrentLevelHealth()
	{
		return baseHealth+baseHealth*percentHealthIncPerLevel*(level-1);
	}

	public double GetRepairCost()
	{
		UpdateCurrentRepairCost ();
		return currentRepairCost;
	}

	public void Repair()
	{
		UpdateHealth ();
		UpdateCurrentValue ();
		UpdateCurrentRepairCost ();
	}

	public bool IsBroken()
	{
		return health < GetCurrentLevelHealth();
	}
}
