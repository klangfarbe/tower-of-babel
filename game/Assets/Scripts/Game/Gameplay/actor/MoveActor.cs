using UnityEngine;
using System.Collections;

public class MoveActor : MonoBehaviour {
	public float speed = 0.05f;
	
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

	public bool targetFieldIsFree() {
		RaycastHit hit;

		// check if something is standing on the field
		Debug.DrawRay (transform.position + Vector3.up * 0.25f, transform.forward, Color.blue, 1f);
		if(Physics.Raycast(transform.position + Vector3.up * 0.25f, transform.forward, out hit, 1f)) {
			Debug.Log("Hit " + hit.collider.gameObject.name);
			return false;
		}

		// check if floor is available
		Debug.DrawRay(transform.position + transform.forward + Vector3.up * 0.25f, Vector3.down * 0.3f, Color.green, 1f);
		if(Physics.Raycast(transform.position + transform.forward + Vector3.up * 0.25f, Vector3.down, out hit, 0.3f)) {
			return true;
		}
		return false;
	}

	// -----------------------------------------------------------------------------------------------------------------
	
	public void moveForward() {
		if(walking || !targetFieldIsFree ())
			return;
		endPosition += transform.forward;
		direction = Direction.FORWARD;
	}
	
	// -----------------------------------------------------------------------------------------------------------------
	
	public void moveBack() {
		if(walking || !targetFieldIsFree())
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
	
	public void activateLiftUp() {
		if(walking) {
			return;
		}
		RaycastHit hit;
		Debug.DrawRay(transform.position + Vector3.up * 0.25f, Vector3.down * 0.3f, Color.green, 1f);
		if(Physics.Raycast(transform.position + Vector3.up * 0.25f, Vector3.down, out hit, 0.3f)) {
			Debug.Log("Hit " + hit.collider.gameObject.name);
			if(hit.collider.tag == "lift") {
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
		RaycastHit hit;
		Debug.DrawRay(transform.position + Vector3.up * 0.25f, Vector3.down * 0.3f, Color.green, 1f);
		if(Physics.Raycast(transform.position + Vector3.up * 0.25f, Vector3.down, out hit, 0.3f)) {
			Debug.Log("Hit down: " + hit.collider.gameObject.name + " " + hit.collider.tag);
			if(hit.collider.tag == "lift") {
				direction = Direction.DOWN;
				hit.collider.GetComponentInChildren<Animation>().Play("down");
				endPosition += -transform.up;
			}
		}
	}
	
	// -----------------------------------------------------------------------------------------------------------------
	
	void OnTriggerEnter(Collider other) {
		Debug.Log("OnTriggerEnter: " + direction + ", " + other.gameObject.name);
		return;
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
