using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventMiniGame : MonoBehaviour {
	
	public Scrollbar scrollbar;
	public float moveSpeed = 1f;
	public Image winField;
	public Image sliderField;
	public GameObject successObject;
	public GameObject failObject;
	public Text dangerText;
	EventManager eventManager;

	GameObject eventObject;

	float direction = -1f;

	public void Init (GameObject gameObject, EventManager eventMan)
	{
		eventObject = gameObject;
		dangerText.text = "Danger!" + eventObject.GetComponent<EventObject> ().notificationType.ToString ();
		eventManager = eventMan;
	}
	
	// Update is called once per frame
	void Update () {
		if (scrollbar.value <= 0f) {
			direction = 1f;
		}
		if (scrollbar.value >= 1f) {
			direction = -1f;
		}
		UpdateValue ();
	}

	void UpdateValue ()
	{
		scrollbar.value += moveSpeed * Time.deltaTime * direction;
	}

	void OnFail()
	{
		UpgradesController.Instance.DamageAllUpgrades((float)GameControl.control.speed.baseSpeed*eventObject.GetComponent<EventObject>().damage);
		failObject.SetActive (true);
	}

	public void EventOnButtonPressed()
	{
		EventMiniGame minigame = GetComponent<EventMiniGame> ();
		minigame.CheckIfWon ();
		minigame.eventObject.SetActive (false);
	}

	void OnSuccess ()
	{
		successObject.SetActive (true);
	}

	void CheckIfWon ()
	{
		float winPercent = winField.rectTransform.localScale.x;
		if (scrollbar.value >= (0.5f-winPercent/2f) && scrollbar.value <= (0.5f+winPercent/2f)) {
			OnSuccess();
		}
		else {
			OnFail();
		}
	}

	public void SuccessAccepted()
	{
		EventMiniGame minigame = GetComponent<EventMiniGame> ();
        //minigame.successObject.SetActive(false);
		//minigame.gameObject.SetActive (false);
		eventManager.EndEvent (minigame);
	}

	public void FailAccepted()
	{
		EventMiniGame minigame = GetComponent<EventMiniGame> ();
        //minigame.failObject.SetActive(false);
		//minigame.gameObject.SetActive (false);
		eventManager.EndEvent (minigame);
	}
}
