using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 

public sealed class ApplicationModel{

	private static readonly ApplicationModel instance = new ApplicationModel();
	private Userdata player;
	private LoginSystem loginmanager;
    private string oldCurrentPlanet;
    private GameSettings gameSettings;
    public GameSettings GameSettings
    {
        get { return gameSettings; }
        set { gameSettings = value; }
    }
    public string OldCurrentPlanet
    {
        get { return oldCurrentPlanet; }
        set { oldCurrentPlanet = value; }
    }
	private ApplicationModel(){
		loginmanager = new LoginSystem ();
        gameSettings = new GameSettings();
        gameSettings.startupLoad = true;
	}
	public static ApplicationModel Instance
	{
		get{
			return instance;
		}
	}
	public Userdata Player
	{
		get{
			return this.player;
		}
		set{
			this.player = value;
		}
	}

    public LoginSystem Loginmanager
    {
        get { return loginmanager; }
        set { loginmanager = value; }
    }

	public void ChangeToGameState()
	{
        OldCurrentPlanet = Player.CurrentPlanet;
        GameObject.Find("Begin_Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
        GameObject.Find("HighScore").GetComponent<UnityEngine.UI.Button>().interactable = false;
        if (gameSettings.AccessToken !=  null)
        {
            loginmanager.SendStartData();
        }
        else
        {
            Application.LoadLevel("GameScene");
        }
	}

	public void ChangeToMenuState()
	{
		Application.LoadLevel("MenuScene");
	}

    public void OfflineMode()
    {
        loginmanager.OfflineLogin();
        //decide if to switch to game directly or to the menu
        GameObject unity = UnityEngine.GameObject.Find("MenuSystem");
        OldCurrentPlanet = Player.CurrentPlanet;
        unity.GetComponent<MenuStateManager>().MenuScreen();
    }

	public void LoginUser(string email,string password,bool auto_login)
	{
        GameObject.Find("Login_Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
        GameObject.Find("BackLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
        loginmanager.LoginUser(email, password, auto_login);
	}

    public void AutoLogin(string email,string password)
    {
        GameObject.Find("ToLogin_Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
        GameObject.Find("Offline_Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
        GameObject.Find("Register_Button").GetComponent<UnityEngine.UI.Button>().interactable = false;
        loginmanager.AutoLogin(email, password);
    }

    public void RegisterUser(string email,string username,string password)
    {
        loginmanager.RegisterUser(email, username, password);
    }

    public void GetHighScore()
    {
        loginmanager.GetHighScore();
    }

    public void LoggedInSetup()
    {
       loginmanager.GetSolarSystem();
    }

    public void SolarSystemSetup()
    {
		//GameObject DB = GameObject.Find ("/DB");
        //DB.GetComponent<FacebookManager>().UIFBAvatar = GameObject.Find("FB Avatar");
        //DB.GetComponent<FacebookManager>().UIFBUserName = GameObject.Find("FB User Name");
        //DB.GetComponent<FacebookManager>().SetProfilePicture();

        GameObject unity = UnityEngine.GameObject.Find("MenuSystem");
        OldCurrentPlanet = Player.CurrentPlanet;
        unity.GetComponent<MenuStateManager>().MenuScreen();
    }

	public bool IsCurrentlyPlaying()
	{
		return Player != null;
	}

    public void SendData(Speed speed)
    {
        loginmanager.SendData(speed);
    }

    public void SendCompletedData()
    {
        loginmanager.SendCompletedData();
    }

}

public class GameSettings
{
    public bool autologin;
    public string hashkey;
    public string email;
    public bool startupLoad;
    public string AccessToken;
}