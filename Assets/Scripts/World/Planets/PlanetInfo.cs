using UnityEngine;
using System.Collections;

public class PlanetInfo : MonoBehaviour {

    private string planetName;
    private double planetDistance;
    private string planetDesc;

    public string PlanetName
    {
        get { return this.planetName; }
        set { this.planetName = value; }
    }
    public double PlanetDistance
    {
        get { return this.planetDistance; }
        set { this.planetDistance = value; }
    }
    public string PlanetDesc
    {
        get { return this.planetDesc; }
        set { this.planetDesc = value; }
    }
}
