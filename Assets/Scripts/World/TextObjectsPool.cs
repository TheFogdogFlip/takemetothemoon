using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextObjectsPool : MonoBehaviour {

	// Use this for initialization
	Queue<GameObject> textsPool = new Queue<GameObject>(); 
	HashSet<GameObject> usedTexts = new HashSet<GameObject>();
	public bool autoGrow = false;

	void Start () {
		for (int i = 0; i < 30; i++) {
			GameObject text = Instantiate(Resources.Load("Prefabs/TextPoints") as GameObject);
			text.transform.SetParent(this.transform,false);
			textsPool.Enqueue(text);
		}
	}

	public GameObject Enqueue()
	{
		GameObject text;
		if (textsPool.Count > 0) {
			text = textsPool.Dequeue ();
			usedTexts.Add (text);
			return text;
		}
		else if (autoGrow) {
			text = Instantiate(Resources.Load("Prefabs/TextPoints") as GameObject);
			text.transform.SetParent(this.transform,false);
			usedTexts.Add (text);
			return text;
		}
		else {
			return null;
		}
	}

	public void Dequeue (GameObject gameObject)
	{
		if (usedTexts.Contains(gameObject)) {
			usedTexts.Remove (gameObject);
			textsPool.Enqueue (gameObject);
		}
	}
}
