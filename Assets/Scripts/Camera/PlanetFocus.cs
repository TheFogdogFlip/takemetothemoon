using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlanetFocus : MonoBehaviour {

    private GameObject solarSystem;
    private GameObject oldFocus;
    private GameObject newFocus;

    private bool changed;

    private Vector3 position;
    private Vector3 distanceNext;
    private Vector3 direction;

    public float speed;

    public void ChangeSolarSystem(GameObject solarSystem)
    {
        this.solarSystem = solarSystem;
    }

    public void ChangePlanetFocus(GameObject Planet)
    {
        oldFocus = newFocus;
        newFocus = Planet;
    }

    public void CalculateDistance()
    {
        distanceNext = newFocus.transform.position - oldFocus.transform.position;
        direction = distanceNext.normalized;
    }

    // will have to do until further notice
    float CalculateRemaining()
    {
        Vector3 currentDistance = newFocus.transform.position - this.transform.position;

        if (currentDistance.x == 0 || distanceNext.x == 0)
        {
            return 0;
        }
        float percent = currentDistance.x / distanceNext.x;
		if (float.IsNaN(percent)) {
			percent = 0.0f;
		}
        if (percent < 0.01f)
        {
            StopCameraMotion();
            return percent;
        }
        else if (percent < 0.6f)
        {
            return 0.6f;
        }
        else{
            return percent;
        }
    }

    public void StartCameraMotion()
    {
        position = this.transform.position;
        changed = true;
    }

    public void StopCameraMotion()
    {
        changed = false;
    }

    void Start()
    {
        position = new Vector3();
        distanceNext = new Vector3();
        changed = false;
    }

    void Update()
    {
        if (changed)
        {
            position.x += (speed * Time.deltaTime * direction.x) * CalculateRemaining();
            this.transform.position = position;
        }
    }

}
