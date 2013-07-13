using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
	public float speed = 0.15f;

	private bool enable;
	private bool walking = false;
	private Vector3 endPosition;
	private float startTime;

	// save the current movement direction
	private enum Direction {
		FORWARD, BACK, UP, DOWN, NONE
	}
	private Direction direction;
	private Quaternion cardinalDirection;

	// -----------------------------------------------------------------------------------------------------------------

	void Start () {
		enable = false;
		endPosition = transform.position;
		direction = Direction.NONE;
		cardinalDirection = CardinalDirection.North;
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
			direction = Direction.NONE;
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
		if(walking)
			return;
		endPosition += transform.forward;
		direction = Direction.FORWARD;
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void moveBack() {
		if(walking)
			return;
		endPosition -= transform.forward;
		direction = Direction.BACK;
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
		if(walking) {
			return;
		}

		Debug.DrawRay(transform.position + Vector3.up * 0.5f, Vector3.down);

		RaycastHit hit;
		if(Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out hit, 1)) {
			if(hit.collider.tag == "lift") {
				Debug.Log("Hit lift");
				direction = Direction.UP;
				hit.collider.GetComponentInChildren<Animation>().Play("up");
				endPosition += transform.up;
			}
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void activateLiftDown() {
		if(walking) {
			return;
		}
				Debug.DrawRay(transform.position, Vector3.down * 0.5f);

		RaycastHit hit;
		if(Physics.Raycast(transform.position, Vector3.down, out hit, 0.25f)) {
			if(hit.collider.tag == "lift") {
				Debug.Log("Hit lift");
				direction = Direction.UP;
				hit.collider.GetComponentInChildren<Animation>().Play("down");
				Debug.Log(endPosition);
				endPosition += -transform.up;
				Debug.Log(endPosition);
			}
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	void OnTriggerEnter(Collider other) {
		//Debug.Log("OnTriggerEnter: " + direction + ", " + other.gameObject.name);
		var type = other.gameObject.name;
		switch(type) {
			case "---":
			case "BOX1":
			case "BOX2":
			case "LFD":
			case "LFU":
			case "FLR1":
			case "FLR2":
				if(direction == Direction.FORWARD) {
					endPosition -= transform.forward;
				} else if(direction == Direction.BACK) {
					endPosition += transform.forward;
				}
				break;
			case "lift":
				//Debug.Log("lift");//transform. endPosition other.gameObject.transform
				break;
		}
	}
}
