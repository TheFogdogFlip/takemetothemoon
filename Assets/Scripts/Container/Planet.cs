using UnityEngine;
using System.Collections;

public class Planet {
    private string name;
    private double distance;
    private long index;
    public Planet(string name,double distance,long index)
    {
        this.name = name;
        this.distance = distance;
        this.index = index;
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public double Distance
    {
        get { return distance; }
        set { distance = value; }
    }
    public long Index
    {
        get { return index; }
        set { index = value; }
    }

}
