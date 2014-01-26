using UnityEngine;
using System.Collections;

public class SpiderCamera : MonoBehaviour {
	public GameObject target;
	public float distance = 1;
	public float rotationSpeed = 0.3f;

	private float startTime;
	public Vector3 endPosition;

	// ------------------------------------------------------------------------

	void Update() {
		if(!target) {
			return;
		}

		Actor actor = target.GetComponent<Actor>();

		if(Input.GetKeyDown(KeyCode.UpArrow)) {
			actor.move(target.transform.forward);
		} else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			actor.turnLeft();
			startTime = Time.time;
		} else if(Input.GetKeyDown(KeyCode.DownArrow)) {
			actor.move(-target.transform.forward);
		} else if(Input.GetKeyDown(KeyCode.RightArrow)) {
			actor.turnRight();
			startTime = Time.time;
		} else if(Input.GetKeyDown(KeyCode.Space)) {
			actor.fire();
		} else if(Input.GetKeyDown(KeyCode.Return)) {
			actor.lift();
		}
	}

	// ------------------------------------------------------------------------

	void LateUpdate () {
		if(!target) {
			return;
		}

		Transform t = target.transform.Find("LookAt");
		var currentRotation = Quaternion.Euler (0, t.eulerAngles.y, 0);

		Vector3 startPosition = transform.position;
		endPosition = t.position - currentRotation * Vector3.forward * distance;

		float journeyLength = Vector3.Distance(startPosition, endPosition);

		if(journeyLength > 0) {
			float distCovered = (Time.time - startTime) * rotationSpeed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Slerp(startPosition - t.position, endPosition - t.position, fracJourney);
			transform.position += t.position;
		}
		transform.LookAt(t);
	}
}
