using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

	public EventObject[] eventObjects;
	public EventMiniGame miniGamePrefab;
	List<EventMiniGame> minigames = new List<EventMiniGame>();
    public int minTimeSpawn = 120;
    public int maxTimeSpawn = 180;
	public Transform minigamesParentCanvas;
	public Collider clickmeshCollider;
    float lastSpawnTime = 0;
	EventObject lastEventObject;

	public void SpawnEventObject(EventObject eventObject)
    {
		eventObject.gameObject.SetActive(true);
		eventObject.Init();
    }

    void Start()
    {
        AudioManager.Instance.AudioSources["Ambient1"].Play();
    }

    void Update()
    {
        float timeSpan = Random.Range(minTimeSpawn, maxTimeSpawn);
        if (Time.time > lastSpawnTime + timeSpan)
        {
			//int randomEventIndex = Random(0,eventObjects.Length);
			//if (!eventObjects[randomEventIndex].gameObject.activeSelf)
            //{
			EventObject eventObj = GetAvaiableEvent();
			if (eventObj) {
				SpawnEventObject(eventObj);
			}
			//}
            lastSpawnTime = Time.time;
        }
    }

	EventObject GetAvaiableEvent()
	{
		for (int i = 0; i < eventObjects.Length; i++) {
			int randomIndex = Random.Range(0,eventObjects.Length);
			if (!eventObjects[randomIndex].gameObject.activeSelf)
			{
				return eventObjects[randomIndex];
			}
		}
		return null;
	}

	public void WarnAboutDanger (GameObject gameObject)
	{
		if (gameObject.GetComponent<EventObject>()) {
			EventMiniGame minigame = Instantiate (miniGamePrefab) as EventMiniGame;
			minigames.Add(minigame);
			minigame.Init(gameObject, this);
			minigame.transform.SetParent(minigamesParentCanvas,false);
			//minigame.gameObject.SetActive(true);
			if (clickmeshCollider.enabled) {
				clickmeshCollider.enabled = false;
			}
		}
	}

	public void EndEvent (EventMiniGame minigame)
	{
		if (minigames.Contains (minigame)) {
			minigames.Remove (minigame);
			Destroy(minigame.gameObject);
		}
		if (minigames.Count == 0) {
			clickmeshCollider.enabled = true;
		}
	}
}
