using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour{

    public GameObject parent;
    public GameObject clickMesh;

    public GameObject ClickMesh
    {
        get { return clickMesh; }
        set { clickMesh = value; }
    }

    public GameObject Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    private void SetupGame(Userdata player)
    {
        GameObject planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        planet.name = player.CurrentPlanet;
        planet.AddComponent<MeshCollider>();
        planet.AddComponent<SphereCollider>();
        planet.AddComponent<CurrentPlanet>();
        planet.GetComponent<CurrentPlanet>().distanceToTravel = player.SolarSystem.GetPlanet(player.CurrentPlanet).Distance;
        planet.GetComponent<CurrentPlanet>().traveledDistance = player.CurrentDistanceTraveled;
        planet.tag = "Planet";
        // need a random material thingy for other systems
        if (player.SolarSystem.SystemName == "SolarSystem")
        {
            Material mat;
            //temp code for texture thingy
            switch (player.CurrentPlanet)
            {
                case "Mercury":
                    mat = Resources.Load("Materials/Planet/Mercury", typeof(Material)) as Material;
                    planet.GetComponent<MeshRenderer>().material = mat;
                    break;
                case "Venus":
                    mat = Resources.Load("Materials/Planet/Venus", typeof(Material)) as Material;
                    planet.GetComponent<MeshRenderer>().material = mat;
                    break;
                case "Moon":
                    mat = Resources.Load("Materials/Planet/TheMoon", typeof(Material)) as Material;
                    planet.GetComponent<MeshRenderer>().material = mat;
                    break;
                case "Mars":
                    mat = Resources.Load("Materials/Planet/Mars", typeof(Material)) as Material;
                    planet.GetComponent<MeshRenderer>().material = mat;
                    break;
                case "Jupiter":
                    mat = Resources.Load("Materials/Planet/Jupiter", typeof(Material)) as Material;
                    planet.GetComponent<MeshRenderer>().material = mat;
                    break;
                case "Saturn":
                    mat = Resources.Load("Materials/Planet/Saturn", typeof(Material)) as Material;
                    planet.GetComponent<MeshRenderer>().material = mat;
                    break;
                case "Uranus":
                    mat = Resources.Load("Materials/Planet/Uranus", typeof(Material)) as Material;
                    planet.GetComponent<MeshRenderer>().material = mat;
                    break;
//				case "Neptune":
//					mat = Resources.Load("Materials/Planet/Uranus", typeof(Material)) as Material;
//					planet.GetComponent<MeshRenderer>().material = mat;
//					break;
				case "Pluto":
					mat = Resources.Load("Materials/Planet/Pluto", typeof(Material)) as Material;
	                    planet.GetComponent<MeshRenderer>().material = mat;
	                    break;
	                default:
	                    break;
            }
        }

        planet.transform.parent = parent.transform;
        planet.transform.localPosition = new Vector3(0, -10, 374);
        planet.layer = 2;

        clickMesh.GetComponent<Speed>().Init(planet);
    }

    public void CleanGame(Userdata player)
    {
        GameObject.Destroy(GameObject.Find(player.CurrentPlanet));
		player.nextPlanet ();
		player.CurrentDistanceTraveled = 0;
		ApplicationModel.Instance.Player = player;
        ApplicationModel.Instance.SendCompletedData();
		ApplicationModel.Instance.ChangeToMenuState();
    }

	void Start()
	{
		this.SetupGame (ApplicationModel.Instance.Player);
	}

}
