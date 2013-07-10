using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
	public float speed = 0.15f;

	private bool enable;
	private bool walking = false;
	private Vector3 endPosition;
	private float startTime;

	// -----------------------------------------------------------------------------------------------------------------

	void Start () {
		enable = false;
		endPosition = transform.position;
	}

	// -----------------------------------------------------------------------------------------------------------------

	void Update() {
		Vector3 startPosition = transform.position;
		float journeyLength = Vector3.Distance(startPosition, endPosition);

		if(!walking && journeyLength > 0) {
			startTime = Time.time;
			walking = true;
		} else if(walking && journeyLength > 0) {
			float distCovered = (Time.time - startTime) * speed;
        	float fracJourney = distCovered / journeyLength;
        	transform.position = Vector3.Lerp(startPosition, endPosition, fracJourney);
		} else {
			walking = false;
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	public bool Enable {
		get {
			return this.enable;
		}
		set {
			this.enable = value;
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void moveForward() {
		endPosition += transform.forward;
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void moveBack() {
		endPosition -= transform.forward;
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void turnLeft() {
		gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, -90);
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void turnRight() {
		gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, 90);
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void fire() {

	}

	// -----------------------------------------------------------------------------------------------------------------

	public void activateLiftUp() {
		endPosition += transform.up;
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void activateLiftDown() {
		endPosition -= transform.up;
	}
}
