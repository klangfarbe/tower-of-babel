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

	// -----------------------------------------------------------------------------------------------------------------

	void Start () {
		enable = false;
		endPosition = transform.position;
		direction = Direction.NONE;
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
		endPosition += transform.forward;
		direction = Direction.FORWARD;
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void moveBack() {
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
		endPosition += transform.up;
		direction = Direction.UP;
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void activateLiftDown() {
		endPosition -= transform.up;
		direction = Direction.DOWN;
	}

	// -----------------------------------------------------------------------------------------------------------------

	void OnTriggerEnter(Collider other) {
		Debug.Log(direction);
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
					moveBack();
				} else if(direction == Direction.BACK) {
					moveForward();
				} else if(direction == Direction.UP) {
					activateLiftDown();
				} else if(direction == Direction.DOWN) {
					Debug.Log("Moving up again");
					Debug.Log(endPosition);
					activateLiftUp();
					Debug.Log(endPosition);
				}
				break;
		}
	}
}
