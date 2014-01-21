using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveActor : MonoBehaviour {
	public Vector3 endPosition;
	public Vector3 startPosition;

	private float speed = 0.9f;
	private float startTime;

	private bool walking = false;
	private List<GameObject> pushedBy = new List<GameObject>();
	private Queue<Vector3> moveQueue = new Queue<Vector3>();

	public Floor currentFloor;
	public Floor nextFloor;

	// ------------------------------------------------------------------------

	void Start () {
		startPosition = endPosition = transform.position;
		getFloor().assign(gameObject);
	}

	// ------------------------------------------------------------------------

	void Update() {
		Vector3 currentPosition = transform.position;
		float journeyLength = Vector3.Distance(currentPosition, endPosition);

		if(!walking && journeyLength > 0) {
			startTime = 0.0f;
			walking = true;
			startPosition = transform.position;
		} else if(walking && journeyLength > 0) {
			startTime += Time.deltaTime * speed;
			transform.position = Vector3.Lerp(startPosition, endPosition, startTime);
		} else {
			if(currentFloor) {
				currentFloor.release(gameObject);
			}
			currentFloor = null;
			nextFloor = null;
			walking = false;
		}
	}

	// ------------------------------------------------------------------------

	void FixedUpdate() {
		// select the nearest pusher which pushed the object
		// pushing has always priority over manual movements
		Vector3 v = Vector3.zero;

		if(pushedBy.Count > 0) {
			v = transform.position;
			float distance = 9999;
			foreach(GameObject g in pushedBy) {
				float d = Vector3.Distance(transform.position, g.transform.position);
				if(d < distance) {
					distance = d;
					v = g.transform.forward;
				}

				if(g.name == "PSH") {
					v = g.transform.forward;
					break;
				}
			}
			pushedBy.Clear();
			moveQueue.Clear();
		} else if(moveQueue.Count > 0 && !walking) {
			v = moveQueue.Dequeue();
		}

		if(!walking && v != Vector3.zero && assignNextField(v)) {
			endPosition += v;
		}
	}

	// ------------------------------------------------------------------------

	public bool assignNextField(Vector3 direction) {
		RaycastHit hit;

		// check if something is standing on the field
		Debug.DrawRay (endPosition + Vector3.up * 0.25f, direction, Color.blue, 0.5f);
		if(Physics.Raycast(endPosition + Vector3.up * 0.25f, direction, out hit, 1f)) {
			return false;
		}

		// check if floor is available
		Debug.DrawRay(endPosition + direction + Vector3.up * 0.25f, Vector3.down * 0.3f, Color.green, 0.5f);
		if(Physics.Raycast(endPosition + direction + Vector3.up * 0.25f, Vector3.down, out hit, 0.3f)) {
//			Debug.Log(hit.collider.tag + " / " + hit.collider.gameObject.name);
			if(hit.collider.tag != "Floor" && hit.collider.tag != "Lift")
				return false;

			if(!hit.collider.gameObject.GetComponent<Floor>().isFree(gameObject)) {
				return false;
			}

			// Prevent from moving if lift still in animation
			if(hit.collider.tag == "Lift"
				&& hit.collider.gameObject.GetComponent<Lift>().isPlaying()) {
				return false;
			}
			currentFloor = getFloor();
			nextFloor = hit.collider.gameObject.GetComponent<Floor>();
			nextFloor.assign(gameObject);
			return true;
		}
		return false;
	}

	// ------------------------------------------------------------------------

	public void set(Vector3 position) {
		endPosition = position;
		transform.position = position;
	}

	// ------------------------------------------------------------------------

	public void push(GameObject by) {
		// prevent from adding the same pusher multiple times per step
		if(!walking && !pushedBy.Contains(by) && assignNextField(by.transform.forward))
			pushedBy.Add(by);
	}

	// ------------------------------------------------------------------------

	public void move(Vector3 t) {
		if(walking || pushedBy.Count > 0)
			return;
		moveQueue.Enqueue(t);
	}

	// ------------------------------------------------------------------------

	public void returnToOldPosition() {
		Debug.Log("name: " + gameObject.name + " / " + startPosition + " / " + endPosition + " / " + transform.position);
		endPosition = startPosition;
		startPosition = transform.position;
		startTime = 0;
	}

	// ------------------------------------------------------------------------

	public void up() {
		if(walking) {
			return;
		}
		Lift lift = getLift();
		if(lift) {
			lift.up();
		}
	}

	// ------------------------------------------------------------------------

	public void down() {
		if(walking) {
			return;
		}
		Lift lift = getLift();
		if(lift) {
			lift.down();
		}
	}

	// ------------------------------------------------------------------------

	public void turnLeft() {
		gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, -90);
	}

	// ------------------------------------------------------------------------

	public void turnRight() {
		gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, 90);
	}

	// ------------------------------------------------------------------------

	Lift getLift() {
		RaycastHit hit;
		if(Physics.Raycast(transform.position + Vector3.up * 0.25f, Vector3.down, out hit, 0.3f)) {
//			Debug.Log(hit.collider.tag + " / " + hit.collider.gameObject.name);
			if(hit.collider.tag == "Lift") {
				return hit.collider.gameObject.GetComponent<Lift>();
			}
		}
		return null;
	}

	// ------------------------------------------------------------------------

	Floor getFloor() {
		RaycastHit hit;
		if(Physics.Raycast(transform.position + Vector3.up * 0.25f, Vector3.down, out hit, 0.3f)) {
//			Debug.Log(hit.collider.tag + " / " + hit.collider.gameObject.name);
			if(hit.collider.tag == "Floor" || hit.collider.tag == "Lift") {
				return hit.collider.gameObject.GetComponent<Floor>();
			}
		}
		return null;
	}

	// ------------------------------------------------------------------------

	public bool Walking {
		get {
			return walking;
		}
	}
}
