using UnityEngine;
using System.Collections;

public class HighScoreMenuScript : MonoBehaviour {

    public MenuManager menuManager;
    public Menu planetMenu;

	// Use this for initialization
	void Start () {
	}

    public void BackToPlanetMenu()
    {
        RemoveScoreBoard();
        menuManager.ShowMenu(planetMenu);
    }

    public void RemoveScoreBoard()
    {
        foreach (Transform child in GameObject.Find("Content Panel").transform)
        {
            Destroy(child.gameObject);
        }
    }

}