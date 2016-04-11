using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System;


public class LoginSystem {

    public GameObject db;

    public LoginSystem()
    {

    }

    public bool OfflineLogin()
    {
        //First check if any saved data is in store in phone, if not then create new data.
        if (GameControl.control.LoadGameData())
        {
            Userdata player = new Userdata();
            player.CurrentDistanceTraveled = GameControl.control.gameData.planedCurrentDistance;
			player.CurrentPlanet = GameControl.control.gameData.currentPlanet;

            SolarSystem system = new SolarSystem();
            system.SystemName = "Solar System";
			system.AddPlanet("Mercury", 91652559700,1);
			system.AddPlanet("Venus", 50290000000,2);
			system.AddPlanet("Moon", 384403000,0);
			system.AddPlanet("Mars", 119740000000,3);
			system.AddPlanet("Jupiter", 720420000000,4);
			system.AddPlanet("Saturn", 646270000000,5);
			system.AddPlanet("Uranus", 1448950000000,6);
			system.AddPlanet("Neptune", 1627450000000,7);
			system.AddPlanet("Pluto", 4265932000000,8);

            player.SolarSystem = system;

            ApplicationModel.Instance.Player = player;
            return true;
        }
        //Create new data
        else
        {
            Userdata player = new Userdata();
            player.CurrentDistanceTraveled = 0;
            player.CurrentPlanet = "Moon";

			SolarSystem system = new SolarSystem();
			system.SystemName = "Solar System";
            system.AddPlanet("Mercury", 91652559700, 1);
            system.AddPlanet("Venus", 50290000000, 2);
            system.AddPlanet("Moon", 384403000, 0);
            system.AddPlanet("Mars", 119740000000, 3);
            system.AddPlanet("Jupiter", 720420000000, 4);
            system.AddPlanet("Saturn", 646270000000, 5);
            system.AddPlanet("Uranus", 1448950000000, 6);
            system.AddPlanet("Neptune", 1627450000000, 7);
            system.AddPlanet("Pluto", 4265932000000, 8);
			
			player.SolarSystem = system;

            ApplicationModel.Instance.Player = player;
            return true;
        }
    }

    public void FacebookLogin()
    {
        GameObject.Find("DB").GetComponent<FacebookManager>().FBlogin();
    }

    public void LoginUser(string email,string password,bool auto_login)
    {
        DB loginparser = db.GetComponent<DB>();
        Dictionary<string, string> playerndbprofile = new Dictionary<string, string>();
        playerndbprofile.Add("UserKey", password);
        playerndbprofile.Add("Email", email);

        loginparser.POST(NetworkEnums.LOGIN.ToString(), playerndbprofile, (www) =>{
            if (www.error != null)
            {
                //do nothing
                Debug.Log(www.error);
                GameObject.Find("Login_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                GameObject.Find("BackLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                return;
            }
            //check recieved data
            if (www.text == "Credentials false , Login failed")
            {
                //do nothing
                Debug.Log(www.text);
                GameObject.Find("Login_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                GameObject.Find("BackLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                return;
            }
            if (www.text == "User Already Logged In")
            {
                //do nothing
                Debug.Log(www.text);
                GameObject.Find("Login_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                GameObject.Find("BackLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                return;
            }

            GameObject.Find("Login_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
            GameObject.Find("BackLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;

            ApplicationModel.Instance.GameSettings.autologin = auto_login;
            ApplicationModel.Instance.GameSettings.email = email;
            ApplicationModel.Instance.GameSettings.hashkey = password;
            GameControl.control.SaveSettings(Application.persistentDataPath + "/gameSettings.dat");
            ApplicationModel.Instance.GameSettings.AccessToken = www.text;
            ApplicationModel.Instance.Player = new Userdata();
            GetSolarSystem();
        });
    }

    public void AutoLogin(string email,string password)
    {
        DB loginparser = db.GetComponent<DB>();
        Dictionary<string, string> playerndbprofile = new Dictionary<string, string>();
        playerndbprofile.Add("UserKey", password);
        playerndbprofile.Add("Email", email);

        loginparser.POST(NetworkEnums.LOGIN.ToString(), playerndbprofile, (www) =>
        {
            if (www.error != null)
            {
                //do nothing
                Debug.Log(www.error);
                GameObject.Find("ToLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                GameObject.Find("Offline_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                GameObject.Find("Register_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                return;
            }
            //check recieved data
            if (www.text == "Credentials false , Login failed")
            {
                //do nothing
                Debug.Log(www.text);
                GameObject.Find("ToLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                GameObject.Find("Offline_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                GameObject.Find("Register_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                return;
            }
            if (www.text == "User Already Logged In")
            {
                //do nothing
                Debug.Log(www.text);
                GameObject.Find("ToLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                GameObject.Find("Offline_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                GameObject.Find("Register_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                return;
            }
            GameObject.Find("ToLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
            GameObject.Find("Offline_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
            GameObject.Find("Register_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;

            GameControl.control.SaveSettings(Application.persistentDataPath + "/gameSettings.dat");
            ApplicationModel.Instance.GameSettings.AccessToken = www.text;
            ApplicationModel.Instance.Player = new Userdata();
            GetSolarSystem();
        });
    }
    public void GetSolarSystem()
    {
        DB loginparser = db.GetComponent<DB>();

        System.Threading.Thread.Sleep(500);

        Dictionary<string, string> accessToken = new Dictionary<string, string>();
        accessToken.Add("AccessToken", ApplicationModel.Instance.GameSettings.AccessToken);

        loginparser.POST(NetworkEnums.GETSOLAR.ToString(), accessToken, (www) =>
        {
            if (www.error != null)
            {
                //handle error
                Debug.Log(www.error);
                return;
            }
            //check text
            if (www.text == "Wrong Login Sequence")
            {
                //back to screen
                Debug.Log(www.text);
                return;
            }
            Debug.Log(www.text);
            Debug.Log("Solarsystem data recieved");
            var jsonobject = Facebook.MiniJSON.Json.Deserialize(www.text) as Dictionary<string, object>;
            var solarsystemdata = ((Dictionary<string, object>)jsonobject["solarsystem"]);
            var currentplanetdata = (string) jsonobject["currentplanet"];
            var planets = ((List<object>)solarsystemdata["planets"]);
            SolarSystem system = new SolarSystem();
            system.SystemName = (string)solarsystemdata["name"];
            foreach (object item in (List<object>)solarsystemdata["planets"])
            {
                var tempplanet = (Dictionary<string, object>)item;
                system.AddPlanet((string)tempplanet["name"],(double) tempplanet["distance"],(long)tempplanet["index"]);
            }
            ApplicationModel.Instance.Player.SolarSystem = system;
            ApplicationModel.Instance.Player.CurrentPlanet = currentplanetdata;
            ApplicationModel.Instance.SolarSystemSetup();
        });
    }
    public void RegisterUser(string Email,string Username,string Password)
    {
        DB loginparser = GameObject.Find("DB").GetComponent<DB>();
        Dictionary<string,string> registeruserinfo = new Dictionary<string,string>();
        registeruserinfo.Add("Email",Email);
        registeruserinfo.Add("Username",Username);
        registeruserinfo.Add("UserKey",MD5Hash.MD5ComputeHash(Password));
        loginparser.POST(NetworkEnums.REGISTER.ToString(), registeruserinfo, (www) =>
        {
            if (www.error != null)
            {
                Debug.Log(www.error);
                GameObject.Find("Login_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                GameObject.Find("BackLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                return;
            }

            if (www.text == "Email already exists")
            {
                Debug.Log(www.text);
                GameObject.Find("Login_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                GameObject.Find("BackLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                // 
                return;
            }

            if (www.text == "Username already exists")
            {
                Debug.Log(www.text);
                GameObject.Find("Login_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                GameObject.Find("BackLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                return;
            }
            Debug.Log(www.text);
            GameObject.Find("Login_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
            GameObject.Find("BackLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
        });
    }

    public void GetHighScore()
    {
        DB loginparser = db.GetComponent<DB>();

        Dictionary<string, string> accessToken = new Dictionary<string, string>();
        accessToken.Add("AccessToken", ApplicationModel.Instance.GameSettings.AccessToken);

        loginparser.POST(NetworkEnums.HIGHSCORE.ToString(), accessToken, (www) =>
        {
            if (www.error != null)
            {
                Debug.Log(www.error);
                return;
            }

            if (www.text == "Error retrieving high score")
            {
                Debug.Log(www.text);
                return;
            }

            if (www.text == "Nothing to give")
            {
                Debug.Log(www.text);
                return;
            }

            Debug.Log(www.text);

            var items = Facebook.MiniJSON.Json.Deserialize(www.text) as List<object>;

            GameObject HighScore = GameObject.Find("Content Panel");

            foreach (object item in items)
            {
                var data = item as Dictionary<string, object>;
                string gametime = (string)data["time"];
                string name = (string)data["username"];
                GameObject scoreobject = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/ScorePanel"));
                foreach (Transform child in scoreobject.transform)
                {
                    if (child.name == "Name")
                    {
                        child.GetComponent<UnityEngine.UI.Text>().text = name;
                    }
                    if (child.name == "Time")
                    {
                        child.GetComponent<UnityEngine.UI.Text>().text = gametime;
                    }
                }

                scoreobject.transform.SetParent(HighScore.transform,false);
            }
        });
    }


    public void SendStartData()
    {
        DB loginparser = db.GetComponent<DB>();

        Dictionary<string, string> accessToken = new Dictionary<string, string>();
        accessToken.Add("AccessToken", ApplicationModel.Instance.GameSettings.AccessToken);
        accessToken.Add("StartingCurrentPlanet", "start game");

        loginparser.POST(NetworkEnums.SENDSTARTDATA.ToString(), accessToken, (www) =>
        {
            if (www.error != null)
            {
                Debug.Log(www.error);
                GameObject.Find("Begin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                GameObject.Find("HighScore").GetComponent<UnityEngine.UI.Button>().interactable = true;
                return;
            }

            if (www.text == "AccessToken Wrong")
            {
                Debug.Log(www.text);
                GameObject.Find("Begin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
                GameObject.Find("HighScore").GetComponent<UnityEngine.UI.Button>().interactable = true;
                return;
            }
                
            Debug.Log(www.text);
            GameObject.Find("Begin_Button").GetComponent<UnityEngine.UI.Button>().interactable = true;
            GameObject.Find("HighScore").GetComponent<UnityEngine.UI.Button>().interactable = true;
            Application.LoadLevel("GameScene");
        });
    }


    public void SendData(Speed speed)
    {
        DB loginparser = db.GetComponent<DB>();

        var senddata = new Dictionary<string, string>();
        senddata["traveled_distance"] = speed.currentPlanet.traveledDistance.ToString();
        senddata["completed"] = "false";

        Dictionary<string, string> accessToken = new Dictionary<string, string>();
        accessToken.Add("AccessToken", ApplicationModel.Instance.GameSettings.AccessToken);
        accessToken.Add("UpdatePlanetInfo",Facebook.MiniJSON.Json.Serialize(senddata));

        loginparser.POST(NetworkEnums.SENDDATA.ToString(), accessToken, (www) =>
        {
            if (www.error != null)
            {
                Debug.Log(www.error);
                return;
            }

            if (www.text == "AccessToken Wrong")
            {
                Debug.Log(www.text);
                return;
            }

            Debug.Log(www.text);
        });
    }

    public void SendCompletedData()
    {
        DB loginparser = db.GetComponent<DB>();

        var senddata = new Dictionary<string, string>();
        senddata["traveled_distance"] = GameObject.Find("Clickmesh").GetComponent<Speed>().currentPlanet.traveledDistance.ToString();
        senddata["completed"] = "true";

        Dictionary<string, string> accessToken = new Dictionary<string, string>();
        accessToken.Add("AccessToken", ApplicationModel.Instance.GameSettings.AccessToken);
        accessToken.Add("CompletedCurrentPlanet", Facebook.MiniJSON.Json.Serialize(senddata));

        loginparser.POST(NetworkEnums.SENDDATA.ToString(), accessToken, (www) =>
        {
            if (www.error != null)
            {
                Debug.Log(www.error);
                return;
            }

            if (www.text == "AccessToken Wrong")
            {
                Debug.Log(www.text);
                return;
            }

            Debug.Log(www.text);
        });
    }
}
