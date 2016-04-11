using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
public class GameControl : MonoBehaviour
{
    // Singleton
    public static GameControl control;
    public Speed speed;
    Upgrades upgrades;
    UpgradesController upgradesController;
	Boost boost;
	EventObject[] eventObjects;
    public GameData gameData;
    void LoadUpgrades()
    {
        if (gameData != null)
        {
            for (int i = 0; i < gameData.upgrades.Length; i++)
            {
                if (gameData.upgrades[i] != null)
                {
                    UpgradeTypes utype = gameData.upgrades[i].type;
                    upgradesController.AddUpgrade(utype);
					if (utype == UpgradeTypes.Clicker) {
						upgradesController.LevelUpUpgrade(utype, gameData.upgrades[i].level-1);
					}
					else {
						upgradesController.LevelUpUpgrade(utype, gameData.upgrades[i].level);
					}
                    upgradesController.SetUpgradeHealth(utype, gameData.upgrades[i].health);
                }
                else
                {
                    return;
                }
            }
        }
    }

    void LoadSpeed()
    {
        if (gameData != null)
        {
			double timeDiff = DateTime.Now.Subtract(gameData.applicationExitTime).TotalSeconds;
            if (gameData.applicationExitTime == DateTime.MinValue)
            {
                timeDiff = 0;
            }
			double offlineDistance = gameData.baseSpeed * timeDiff;
			double offlinePoints = offlineDistance * speed.pointsDistanceMultiplier;
            speed.distance = gameData.distance + offlineDistance;
			speed.points = gameData.points + offlinePoints;
            //speed.baseSpeed = gameData.baseSpeed;
            //speed.distance = gameData.distance;
            //speed.points = gameData.points;
			double offlineAutoPilotDistance = 0;
			double offlineAutoPilotPoints = 0;
			if (timeDiff >= gameData.autoPilotTimeLeft) {
				offlineAutoPilotDistance = gameData.autoPilotTimeLeft * speed.baseSpeed*0.1; //distance
				offlineAutoPilotPoints = offlineAutoPilotDistance*speed.pointsDistanceMultiplier;
				gameData.autoPilotTimeLeft = 0;
			}
			else {
				double offlineBoostTimeLeft = gameData.autoPilotTimeLeft - timeDiff;
				offlineAutoPilotDistance = timeDiff * speed.baseSpeed*0.1;
				offlineAutoPilotPoints = offlineAutoPilotDistance*speed.pointsDistanceMultiplier;
				gameData.autoPilotTimeLeft = offlineBoostTimeLeft;
			}
			speed.distance += offlineAutoPilotDistance;
			speed.points += offlineAutoPilotPoints;
			speed.autoPilotBonusPoints = offlineAutoPilotPoints;
			speed.autoPilotTimeLeft = gameData.autoPilotTimeLeft;
			boost.SetBoostAmount(gameData.boost,timeDiff);

			LoadEventObjectData(timeDiff);
        }
    }

	void LoadEventObjectData(double timeDiff)
	{
		for (int i = 0; i < gameData.events.Length; i++) {
			if (gameData.events[i].active) {
				EventData eventD = gameData.events[i];

				float newEventZPosition = eventD.posz;
				float newEventYPosition = eventD.posy;
				float newEventXPosition = eventD.posx;
				newEventZPosition -= eventObjects[i].objectSpeed*(float)timeDiff;
				if (newEventZPosition < -17f) {
					newEventZPosition = -17f;
				}
				
				Vector3 newPos = new Vector3(newEventXPosition, newEventYPosition, newEventZPosition);
				eventObjects[i].transform.localPosition = newPos;
				eventObjects[i].gameObject.SetActive(true);
				Vector3 target = newPos;
				target.z = -17f;
				eventObjects[i].SetTargetPos(target);
			}
		}
	}
	
	// Awake() is always called before Start(). This is good so that we can setup the singleton before we start creating the tempfile and save data into it
    void Awake()
    {
        // this will check if the singleton already exists, if not then set it so that it wont get destroyed when you change scenes if it existed earlier then destroy it
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
//        GameObject clickmesh = GameObject.Find("Clickmesh");
//        speed = clickmesh.GetComponent<Speed>();
//        upgrades = clickmesh.GetComponent<Upgrades>();
//        upgradesController = clickmesh.GetComponent<UpgradesController>();
    }
	public void Init()
	{
		GameObject clickmesh = GameObject.Find("Clickmesh");
		speed = clickmesh.GetComponent<Speed>();
		upgrades = clickmesh.GetComponent<Upgrades>();
		upgrades.Init ();
		upgradesController = clickmesh.GetComponent<UpgradesController>();
		upgradesController.Init ();
		boost = clickmesh.GetComponent<Boost>();
		eventObjects = GameObject.Find ("/Game").GetComponent<AutoSaver> ().eventManager.eventObjects;
	}
    public bool LoadGameData()
    {
        // First of, check if the tempFile or the gameInfo file was the latest updated, this is crucial if the game was not closed properly
        // If gameInfo was latest updated
        if (File.Exists(Application.persistentDataPath + "/gameInfo.dat") && File.Exists(Application.persistentDataPath + "/tempInfo.dat"))
        {
            if (File.GetLastWriteTime(Application.persistentDataPath + "/tempInfo.dat") > File.GetLastWriteTime(Application.persistentDataPath + "/gameInfo.dat"))
            {
                if (Load(Application.persistentDataPath + "/tempInfo.dat") == -1)
                { //if temp file failed to read
                    if (Load(Application.persistentDataPath + "/gameInfo.dat") == -1)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            // If gameInfo was latest updated
            else
            {
                if (Load(Application.persistentDataPath + "/gameInfo.dat") == -1)
                { //if game file failed to read
                    if (Load(Application.persistentDataPath + "/tempInfo.dat") == -1)
                    {
                        return false;   
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
                
            }
        }
        else
        {
            int result = Load(Application.persistentDataPath + "/gameInfo.dat");
            if (result == 0 || result == -1)
            {
                result = Load(Application.persistentDataPath + "/tempInfo.dat");
                if (result == 0 || result == -1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }
    void Update()
    {
        
    }
    // This function should be called before the game is closed
    public void Save(System.String filePath)
    {
		Debug.Log ("Saving game data to "+ filePath);
        // This will let the file be converted into binary code
        BinaryFormatter bf = new BinaryFormatter();
        // This will create a file inside the persistenDataPath folder, which is located in the %appdata% folder on windows, this is a multi-platform feature
        // Instead of always creating a new file each time the game starts, check if the file exists and if it does then return and use that file instead
        FileStream file;
        if (File.Exists(filePath))
        {
            file = File.Open(filePath, FileMode.Open);
        }
        else
        {
            file = File.Create(filePath);
        }
        GameData data = new GameData();
       
        // Set the variables that has been changed when the game was played and save it to the file
        data.baseSpeed = speed.baseSpeed;
        data.distance = speed.distance;
        data.points = speed.points;
		data.boost = boost.boostAmount;
		data.currentPlanet = speed.currentPlanet.gameObject.name;
		data.planedCurrentDistance = speed.currentPlanet.traveledDistance;
		data.autoPilotTimeLeft = speed.autoPilotTimeLeft;
		data.applicationExitTime = DateTime.Now;
		SaveUpgrades (ref data);
		SaveEventData (ref data);

        bf.Serialize(file, data);
        file.Close();
    }

	void SaveUpgrades(ref GameData data)
	{
		data.upgrades = new UpgradesData[upgrades.GetNumberOfUpgrades()];
		int i = 0;
		foreach (KeyValuePair<UpgradeTypes,Upgrade> upgradeItem in upgrades.ownedUpgrades) {
			data.upgrades[i] = new UpgradesData();
			Upgrade upgrade = upgradeItem.Value;
			data.upgrades[i].level = upgrade.level;
			data.upgrades[i].health = upgrade.health;
			data.upgrades[i].type = upgrade.type;
			i++;
		}
	}

	void SaveEventData(ref GameData data)
	{
		data.events = new EventData[eventObjects.Length];
		for (int i = 0; i < eventObjects.Length; i++) {
			data.events[i] = new EventData();
			data.events[i].active = eventObjects[i].gameObject.activeSelf;
			data.events[i].type = (int)eventObjects[i].notificationType;
			Vector3 eventObjectPos = eventObjects[i].transform.localPosition;
			data.events[i].posx = eventObjectPos.x;
			data.events[i].posy = eventObjectPos.y;
			data.events[i].posz = eventObjectPos.z;
		}
	}

    public void SaveSettings(System.String filePath)
    {
        Debug.Log("Saving game settings to " + filePath);
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file;
        if (File.Exists(filePath))
        {
            file = File.Open(filePath, FileMode.Open);
        }
        else
        {
            file = File.Create(filePath);
        }
        GameSettingsData gameSettings = new GameSettingsData();

        gameSettings.autologin = ApplicationModel.Instance.GameSettings.autologin;
        gameSettings.hashkey = ApplicationModel.Instance.GameSettings.hashkey;
        gameSettings.email = ApplicationModel.Instance.GameSettings.email;

        bf.Serialize(file, gameSettings);
        file.Close();
    }
    public void SaveData()
    {
		Debug.Log ("Saving game data to gameInfo");
        Save(Application.persistentDataPath + "/gameInfo.dat");
    }
    // This function should be called when the game is started
    public int Load(System.String filePath)
    {
		Debug.Log ("Loading game data to GameControl");
        // Firstly check if the file exist
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            //long size = file.Length;

            try
            {
                gameData = bf.Deserialize(file) as GameControl.GameData;
				Debug.Log ("Loaded game data to controller");
				Debug.Log("distance: "+gameData.distance);
				for (int i = 0; i < gameData.upgrades.Length; i++) {
					if (gameData.upgrades[i] != null) {
						Debug.Log("Upgrade: "+gameData.upgrades[i].type.ToString());
					}
				}
				file.Close();
				return 1;
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex);
                Debug.Log("Unable to read file, corrupted?");
				return -1;
            }
        }
		else {
			return 0;
		}

    }
    public int LoadSettings(System.String filePath)
    {
        Debug.Log("Loading game data to GameControl");
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            //long size = file.Length;

            try
            {
                GameSettingsData gamesettings = bf.Deserialize(file) as GameControl.GameSettingsData;
                Debug.Log("Loaded gamesettings data to controller");

                ApplicationModel.Instance.GameSettings.autologin = gamesettings.autologin;
                ApplicationModel.Instance.GameSettings.hashkey = gamesettings.hashkey;
                ApplicationModel.Instance.GameSettings.email = gamesettings.email;

                file.Close();
                return 1;
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex);
                Debug.Log("Unable to read file, corrupted?");
                return -1;
            }
        }
        else
        {
            return 0;
        }
    }
    // When the player has reached a new destination, change the current destination
    public void CheckPointReached()
    {
        Save(Application.persistentDataPath + "/tempInfo.dat");
    }

    public void LoadGame()
    {
		Debug.Log ("Loading game data from GameControl to Game");
        LoadUpgrades();
        LoadSpeed();
    }

    // [Serializable] lets the container be inserted into the file, in this case GameData will be that container
    [Serializable]
    public class GameData
    {
        //saved into the file
        public double baseSpeed;
        public double distance;
        public double points;
		public double boost;
		public string currentPlanet;
		public double planedCurrentDistance;
        public DateTime applicationExitTime;
		public double autoPilotTimeLeft;
        public UpgradesData[] upgrades;
		public EventData[] events;
    }
    [Serializable]
	public class UpgradesData
    {
        public int level;
        public float health;
        public UpgradeTypes type;
    }
    [Serializable]
    public class GameSettingsData
    {
        public bool autologin;
        public string hashkey;
        public string email;
    }
	[Serializable]
	public class EventData
	{
		public float posx;
		public float posy;
		public float posz;
		public bool active;
		public int type;
	}
}
