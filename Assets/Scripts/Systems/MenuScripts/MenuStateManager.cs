using System;
using UnityEngine;

public class MenuStateManager : MonoBehaviour {

    private PlanetManager planetManager;
    public Menu planetMenu;
    public Menu registerMenu;
    public Menu loginMenu;
    public Menu startMenu;
    public Menu highScoreMenu;

    public void MenuScreen()
    {
        planetManager.LoadPlanetData(ApplicationModel.Instance.Player.SolarSystem);
        planetManager.SetCameraToPlanet();
        planetManager.ChangePlanetFocus(ApplicationModel.Instance.Player.CurrentPlanet);
        this.GetComponent<MenuManager>().ShowMenu(planetMenu);
    }
    public void RegisterMenu()
    {
        this.GetComponent<MenuManager>().ShowMenu(registerMenu);
    }
    public void LoginMenu()
    {
        if (ApplicationModel.Instance.GameSettings.autologin)
        {
            //direct login to game
            ApplicationModel.Instance.AutoLogin(ApplicationModel.Instance.GameSettings.email, ApplicationModel.Instance.GameSettings.hashkey);
        }
        else
        {
            this.GetComponent<MenuManager>().ShowMenu(loginMenu);
        }
    }
    public void OfflineMenu()
    {
        ApplicationModel.Instance.OfflineMode();
    }
	public void StartScreen()
    {
        this.GetComponent<MenuManager>().ShowMenu(startMenu);
    }
    //log in off line
	public void StartGame()
	{
		ApplicationModel.Instance.ChangeToGameState ();
	}

    public void HighScore()
    {
        if (ApplicationModel.Instance.GameSettings.AccessToken != string.Empty)
        {
            ApplicationModel.Instance.GetHighScore();
            this.GetComponent<MenuManager>().ShowMenu(highScoreMenu);
        }
        else
        {
            this.GetComponent<MenuManager>().ShowMenu(planetMenu);
        }
    }

    public void Awake()
    {
        if (ApplicationModel.Instance.GameSettings.startupLoad)
        {
            if (GameControl.control.LoadSettings(Application.persistentDataPath + "/gameSettings.dat") == 1)
            {
                Debug.Log("Successful load of application settings");
            }
            else
            {
                Debug.Log("Unsuccessful load of application settings");
            }
            ApplicationModel.Instance.GameSettings.startupLoad = false;
        }
    }
    void Start()
    {
        planetManager = new PlanetManager();
        planetManager.MenuCamera = GameObject.Find("Startmenu Camera");
        planetManager.ParentGameObject = GameObject.Find("StartMenu");

		if (ApplicationModel.Instance.IsCurrentlyPlaying ()) {
			this.MenuScreen ();
		}
    }
}
