using UnityEngine;
using System.Collections;

public class ClickPoints : MonoBehaviour {

	public float fadeTime=0.5f;
	public float floatUpSpeed=1.0f;
	float startTime=0;
	bool animate = false;
	Color textColor;
	// Use this for initialization
	void Start () {
		//textsPool = GameObject.Find ("Game").GetComponent<TextObjectsPool> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (animate) {
			//Make the text move upwards
			transform.Translate(0,Time.deltaTime*floatUpSpeed,0);
			
			//Compute and set the alpha value
			float newAlpha=1.0f-(Time.time-startTime)/fadeTime;
			//newAlpha = Mathf.Clamp (newAlpha, 0, 1);
			Color previousColor = GetComponent<TextMesh> ().color;
			previousColor.a = newAlpha;
			GetComponent<TextMesh>().color=previousColor;
			
			//If alpha has decreased to zero, destroy this game object
			if (newAlpha<=0)
			{
				animate = false;
				transform.parent.GetComponent<TextObjectsPool>().Dequeue(gameObject);
			}
		}
	}

	public void Animate()
	{
		startTime=Time.time;
		animate = true;
	}
}
