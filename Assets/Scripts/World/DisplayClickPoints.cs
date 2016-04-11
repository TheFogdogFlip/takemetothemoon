using UnityEngine;
using System.Collections;

public class DisplayClickPoints : MonoBehaviour {
	
	// Use this for initialization
	public TextObjectsPool textsPool;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TapPoints(float points)
	{
		GameObject text = textsPool.Enqueue ();
		if (text) {
			text.transform.position = new Vector3 (Random.Range (-3.0f, 1.0f), Random.Range (1.0f, 4.0f), 0);
			text.transform.rotation = new Quaternion(0f,0f,0f,0f);
			TextMesh tMesh = text.GetComponent<TextMesh> ();
			tMesh.color = Color.white;
			double value = (double)points;
			string unit = "";
			UnitsConverter.Convert(ref value, ref unit);
			tMesh.text = "+"+value.ToString("f1") + unit;
			tMesh.characterSize = 0.08f;
			ClickPoints cp = text.GetComponent<ClickPoints> ();
			cp.floatUpSpeed = 1f;
			cp.fadeTime = 0.5f;
			text.GetComponent<ClickPoints> ().Animate ();
		}
	}

}
