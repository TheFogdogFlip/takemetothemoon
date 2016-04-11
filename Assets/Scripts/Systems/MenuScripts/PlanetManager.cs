using UnityEngine;
using System.Collections;

public class PlanetManager
{

    private GameObject parentGameObject;
    private GameObject menuCamera;
    private GameObject currentSolarSystem;

    public GameObject MenuCamera
    {
        get { return menuCamera; }
        set { this.menuCamera = value; }
    }

    public GameObject ParentGameObject
    {
        get { return parentGameObject; }
        set { this.parentGameObject = value; }
    }

    public GameObject CurrentSolarSystem
    {
        get { return this.currentSolarSystem; }
        set { this.currentSolarSystem = value; }
    }

    //Temporary code for loading a solar system
    public void LoadPlanetData(SolarSystem solarSystem)
    {

        if (ApplicationModel.Instance.GameSettings.AccessToken != string.Empty)
        {
            CreateSolarSystemInMenu();
        }
        else
        {
            currentSolarSystem = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Solar System"));
            currentSolarSystem.transform.parent = parentGameObject.transform;
            currentSolarSystem.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public bool ChangePlanetFocus(string planetName)
    {
        foreach (Transform planet in currentSolarSystem.GetComponentsInChildren<Transform>())
        {
            if (planet.name == planetName)
            {
                menuCamera.GetComponent<PlanetFocus>().ChangePlanetFocus(planet.gameObject);
                menuCamera.GetComponent<PlanetFocus>().CalculateDistance();
                menuCamera.GetComponent<PlanetFocus>().StartCameraMotion();
                return true;
            }
        }
        return false;
    }

    // Use this for initialization
    public void SetCameraToPlanet()
    {
        //Change the camera at menu to be focused at the first object of the current solarsystem
        menuCamera.GetComponent<PlanetFocus>().ChangeSolarSystem(currentSolarSystem);

        foreach (Transform planetTransform in currentSolarSystem.GetComponentsInChildren<Transform>())
        {
            if (planetTransform.name == ApplicationModel.Instance.OldCurrentPlanet)
            {
                Vector3 cameraPos = menuCamera.transform.position;
                cameraPos.x = planetTransform.transform.position.x;
                menuCamera.transform.position = new Vector3(cameraPos.x, cameraPos.y, cameraPos.z);
                menuCamera.GetComponent<PlanetFocus>().ChangePlanetFocus(planetTransform.transform.gameObject);
                break;
            }
        }
    }


    private void CreateSolarSystemInMenu()
    {
        GameObject gameobject = new GameObject();
        gameobject.AddComponent<PlanetLayout>();

        gameobject.GetComponent<PlanetLayout>().planetgroup = gameobject;
        gameobject.GetComponent<PlanetLayout>().distance = new Vector3(40, 0, 0);

        gameobject.transform.parent = parentGameObject.transform;
        gameobject.transform.localPosition = new Vector3(0, 0, 0);

        foreach (Planet planet in ApplicationModel.Instance.Player.SolarSystem.GetPlanets())
        {
            GameObject planet_menu = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Planet"));
            planet_menu.name = planet.Name;
            planet_menu.transform.parent = gameobject.transform;
            Material mat;
            //temp code for texture thingy
            switch (planet_menu.name)
            {
                    case "Mercury":
                        mat = Resources.Load("Materials/Planet/Mercury", typeof(Material)) as Material;
                        planet_menu.GetComponent<MeshRenderer>().material = mat;
                        break;
                    case "Venus":
                        mat = Resources.Load("Materials/Planet/Venus", typeof(Material)) as Material;
                        planet_menu.GetComponent<MeshRenderer>().material = mat;
                        break;
                    case "Moon":
                        mat = Resources.Load("Materials/Planet/TheMoon", typeof(Material)) as Material;
                        planet_menu.GetComponent<MeshRenderer>().material = mat;
                        break;
                    case "Mars":
                        mat = Resources.Load("Materials/Planet/Mars", typeof(Material)) as Material;
                        planet_menu.GetComponent<MeshRenderer>().material = mat;
                        break;
                    case "Jupiter":
                        mat = Resources.Load("Materials/Planet/Jupiter", typeof(Material)) as Material;
                        planet_menu.GetComponent<MeshRenderer>().material = mat;
                        break;
                    case "Saturn":
                        mat = Resources.Load("Materials/Planet/Saturn", typeof(Material)) as Material;
                        planet_menu.GetComponent<MeshRenderer>().material = mat;
                        break;
                    case "Uranus":
                        mat = Resources.Load("Materials/Planet/Uranus", typeof(Material)) as Material;
                        planet_menu.GetComponent<MeshRenderer>().material = mat;
                        break;
                    case "Neptune":
                      mat = Resources.Load("Materials/Planet/Neptune", typeof(Material)) as Material;
                        planet_menu.GetComponent<MeshRenderer>().material = mat;
                        break;
                    case "Pluto":
                        mat = Resources.Load("Materials/Planet/Pluto", typeof(Material)) as Material;
                        planet_menu.GetComponent<MeshRenderer>().material = mat;
                        break;
                    default:
                        break;
                }
            }

        gameobject.GetComponent<PlanetLayout>().UpdateLayout();

        currentSolarSystem = gameobject;
    }
}
