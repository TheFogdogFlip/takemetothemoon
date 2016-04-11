using UnityEngine;
using System.Collections;

public class DangerDetector : MonoBehaviour {

	// Use this for initialization

	public EventManager em;

    void OnTriggerEnter(Collider other)
    {
		em.WarnAboutDanger (other.gameObject);
    }
}
