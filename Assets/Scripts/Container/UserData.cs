using UnityEngine;
using System.Collections;

public class Userdata {
    private SolarSystem solarSystem;
    private double currentDistanceTraveled;
    private string currentPlanet;
    public string CurrentPlanet
    {
        get { return currentPlanet; }
        set { currentPlanet = value; }
    }

    public double CurrentDistanceTraveled
    {
        get { return currentDistanceTraveled; }
        set { currentDistanceTraveled = value; }
    }

    public SolarSystem SolarSystem
    {
        get { return solarSystem; }
        set { solarSystem = value; }
    }

    public void nextPlanet()
    {
        foreach (Planet planet in solarSystem.GetPlanets())
        {
            if (planet.Index == solarSystem.GetPlanet(currentPlanet).Index+1)
            {
                currentPlanet = planet.Name;
                break;
            }
        }
    }

}