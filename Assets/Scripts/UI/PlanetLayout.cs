using UnityEngine;
using System.Collections;

public class PlanetLayout : MonoBehaviour {

    public GameObject planetgroup;
    public Vector3 distance;

    private ArrayList planets = new ArrayList();

    public void UpdateLayout()
    {
        foreach (Transform child in planetgroup.transform)
        {
            planets.Add(child.gameObject);
        }

        Vector3 baseposition = new Vector3();
        baseposition = planetgroup.transform.position;

        for (int i = 0; i < planets.Count; i++)
        {
            ((GameObject)planets[i]).transform.position = baseposition;
            baseposition += distance;
        }
    }

    public void Clear()
    {
        planets.Clear();
    }
}
