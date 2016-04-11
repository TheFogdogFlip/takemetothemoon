using UnityEngine;
using System.Collections;

public class EventObject : MonoBehaviour {
    public Speed speed;
    Vector3 targetPos;
    public float distanceLeft;
	public float objectSpeed = 1f;
	public float damage = 1000f;
	public NotificationTypes notificationType;

    void Update()
    {
		this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, targetPos, objectSpeed * Time.deltaTime);
		distanceLeft = (transform.localPosition - targetPos).magnitude;
    }

    public void Init()
    {
        Vector3 newPos = new Vector3(Random.Range(-0.6f, 0.6f), Random.Range(-11.7f, -10.3f), 600f);
		transform.localPosition = newPos;
		targetPos = newPos;
        targetPos.z = -17f;
    }

	public void SetTargetPos(Vector3 target)
	{
		targetPos = target;
	}
}
