using UnityEngine;
using System.Collections;

public class TouchGesture
{
	[System.Serializable]
	public class GestureSettings //Swipe distances
	{
		public float minSwipeDist = 200;
		public float maxSwipeTime = 10;
	}
	private GestureSettings settings;
	private float swipeStartTime;
	private bool couldBeSwipe;
	private Vector2 startPos;
	private int stationaryForFrames;
	private TouchPhase lastPhase;
	private Speed speed;
	public TouchGesture(GestureSettings gestureSettings)
	{
		this.settings = gestureSettings;
		this.speed = GameObject.Find ("Clickmesh").GetComponent<Speed> ();
	}

	public IEnumerator CheckHorizontalSwipes(System.Action onLeftSwipe, System.Action onRightSwipe)
	{
		while (true) 
		{	
			foreach(Touch touch in Input.touches)
			{
				switch(touch.phase)
				{
				case TouchPhase.Began: // when finger first touched the screen
					couldBeSwipe = true;
					startPos = touch.position;
					swipeStartTime = Time.time;
					stationaryForFrames = 0;
					break;
				case TouchPhase.Stationary: 
					if(isContinouslyStationary(frames:4))
						couldBeSwipe = false;
					break;
				case TouchPhase.Ended: // when finger leaves the screen
					if(isASwipe(touch))
					{
						couldBeSwipe = false;

						if(Mathf.Sign (touch.position.x - startPos.x) == 1f)
						{
							onRightSwipe();
						}
						else
						{
							onLeftSwipe();
						}
					}
					break;
				}
				lastPhase = touch.phase;
			}
			yield return null;
		}
	}

	private bool isContinouslyStationary(int frames)
	{
		if (lastPhase == TouchPhase.Stationary) {
			stationaryForFrames++;
		} else
			stationaryForFrames = 1;

		return stationaryForFrames > frames;
	}

	private bool isASwipe(Touch touch)
	{
		float swipeTime = Time.time - swipeStartTime;
		float swipteDist = Mathf.Abs (touch.position.x - startPos.x);
		return couldBeSwipe && swipeTime < settings.maxSwipeTime && swipteDist > settings.minSwipeDist && speed.clicksPerSecond < 3.0f;
	}
}
