using UnityEngine;
using System.Collections;

public class AutoSaver : MonoBehaviour {

	// Use this for initialization
    private float startTime;
    public float intervallBetweenSavesInMinutes = 1.0f;
    private Speed speed;
	public EventManager eventManager;
	void Start () {
        speed = GameObject.Find("Clickmesh").GetComponent<Speed>();

		GameControl.control.Init ();
		GameControl.control.LoadGame ();
		UpgradesMenuManager.Instance.Initialize ();
		RepairsMenuManager.Instance.Initialize ();
        startTime = Time.time;
		CancelAllNotifications ();
	}
	
	// Update is called once per frame
	void Update () {
        // Check if it has gone a minute before adding new information to the tempfile
        if (Time.time - startTime > 60.0f * intervallBetweenSavesInMinutes)
        {
            startTime = Time.time;
            GameControl.control.Save(Application.persistentDataPath + "/tempInfo.dat");
            ApplicationModel.Instance.SendData(speed);
        }
        // Saves the game to the .dat file before the game is quited
        if (Input.GetKeyDown(KeyCode.Escape))
        {
			GameControl.control.SaveData();
            double timeToNotifyPlanetReached = speed.GetBaseETA();
            if (timeToNotifyPlanetReached < 1213200)
            {
                LocalNotification.SendNotification((int)NotificationTypes.planetReached, (long)timeToNotifyPlanetReached - 10*60, "Take Me To the Moon", "You are closing in to a planet" + "\n" + "Get ready to land in 10 minutes", Color.white);
                if (timeToNotifyPlanetReached > 608400)
                {
                    LocalNotification.SendNotification((int)NotificationTypes.planetReachedInTwoWeeks, 3600, "Take Me To the Moon", "You will arrive in approximately two weeks", Color.white);
                }
                else
                {
                    if (timeToNotifyPlanetReached > 21600)
                    {
                        LocalNotification.SendNotification((int)NotificationTypes.planetReachedInOneWeek, 3600, "Take Me To the Moon", "You will arrive in approximately one week", Color.white);
                    }
                    else if (timeToNotifyPlanetReached > 7200 && timeToNotifyPlanetReached < 10800)
                    {
                        LocalNotification.SendNotification((int)NotificationTypes.planetReachedInOneDay, 3600, "Take Me To the Moon", "You will arrive tomorrow", Color.white);
                    }
                }
                
            }


			SetupEventsNotifications();
            
            Application.Quit();
        }
	}

	void CancelAllNotifications()
	{
		for (int i = 0; i < (int)NotificationTypes.numberOfNotifications; i++) {
			LocalNotification.CancelNotification(i);
		}
	}

	void SetupEventsNotifications()
	{
		for (int i = 0; i < eventManager.eventObjects.Length; i++) {
			if (eventManager.eventObjects[i].gameObject.activeSelf){
				EventObject eventObj = eventManager.eventObjects[i];
				if (eventObj.distanceLeft > 300) {
					LocalNotification.SendNotification((int)eventObj.notificationType, (long)eventObj.distanceLeft-300, "Take Me To the Moon", "WARNING! Incoming danger", Color.white);
				}               
			}
		}
	}
}
