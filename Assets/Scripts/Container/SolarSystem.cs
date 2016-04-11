using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarSystem {
    private string systemName;
    private List<Planet> planets;
    public string SystemName
    {
        get { return systemName; }
        set { systemName = value; }
    }

    public SolarSystem()
    {
        this.planets = new List<Planet>();
    }

    public List<Planet> GetPlanets()
    {
        return this.planets;
    }

    public void AddPlanet(string name,double distance,long index)
    {
        planets.Add(new Planet(name, distance, index));
    }

    public Planet GetPlanet(string name)
    {
        foreach (Planet planet in planets)
        {
            if (planet.Name == name)
            {
                return planet;
            }
        }
        return new Planet("ERROR", 1,1);
    }

    public void ClearPlanets()
    {
        planets.Clear();
    }
}
