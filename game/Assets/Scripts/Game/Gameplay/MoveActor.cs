using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveActor : MonoBehaviour {
	public Vector3 endPosition;
	public Vector3 startPosition;

	private float deadZone = 0.005f;

	private float speed = 0.7f;
	private float startTime;
	private bool smooth = false;


	private List<GameObject> pushedBy = new List<GameObject>();
	private Queue<Vector3> moveQueue = new Queue<Vector3>();

	public Floor nextFloor;

	// ------------------------------------------------------------------------

	void Start () {
		startPosition = endPosition = transform.position;
	}

	// ------------------------------------------------------------------------

	void Update() {
		if(Walking) {
			startTime += Time.deltaTime * speed;
			transform.position = Vector3.Lerp(startPosition, endPosition, startTime);
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
		} else if(moveQueue.Count > 0 && !Walking) {
			v = moveQueue.Dequeue();
		}

		if(!Walking) {
			if(nextFloor) {
				nextFloor.release(gameObject);
			}
			nextFloor = null;
			if(v != Vector3.zero && assignNextField(v)) {
				endPosition += v;
				smooth = true;
			}
		}
	}

	// ------------------------------------------------------------------------

	public bool assignNextField(Vector3 direction) {
		if(!nextFloor && nextFieldAvailable(direction)) {
			nextFloor = getFloor(endPosition + direction);
			return nextFloor.assign(gameObject);
		}
		return false;
	}

	// ------------------------------------------------------------------------

	public bool nextFieldAvailable(Vector3 direction) {
		RaycastHit hit;

		// check if something is standing on the field
		Debug.DrawRay (endPosition + Vector3.up * 0.25f, direction, Color.blue, 0.5f);
		if(Physics.Raycast(endPosition + Vector3.up * 0.25f, direction, out hit, 1f)) {
			return false;
		}

		Floor floor = getFloor(endPosition + direction);
		if(!floor || (floor && !floor.isFree(gameObject))) {
			return false;
		}

		Lift lift = floor.gameObject.GetComponent<Lift>();
		if(lift && lift.isPlaying()) {
			return false;
		}
		return true;
	}


	// ------------------------------------------------------------------------

	public void set(Vector3 position) {
		smooth = false;
		endPosition = position;
		startPosition = position;
		transform.position = position;
	}

	// ------------------------------------------------------------------------

	public bool push(GameObject by) {
		// prevent from adding the same pusher multiple times per step
		if(!Walking && !pushedBy.Contains(by) && nextFieldAvailable(by.transform.forward)) {
			pushedBy.Add(by);
			return true;
		}
		return false;
	}

	// ------------------------------------------------------------------------

	public bool move(Vector3 t) {
		if(Walking || pushedBy.Count > 0)
			return false;
		moveQueue.Enqueue(t);
		return true;
	}

	// ------------------------------------------------------------------------

	public void returnToOldPosition() {
		Debug.Log("name: " + gameObject.name + " / " + startPosition + " / " + endPosition + " / " + transform.position);
		endPosition = startPosition;
		startPosition = transform.position;
		startTime = 0;
	}

	// ------------------------------------------------------------------------

	public void lift() {
		if(Walking) {
			return;
		}
		Lift lift = getLift();
		if(lift) {
			if(lift.isUp)
				lift.down();
			else
				lift.up();
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
			if(hit.collider.tag == "Lift") {
				return hit.collider.gameObject.GetComponent<Lift>();
			}
		}
		return null;
	}

	// ------------------------------------------------------------------------

	public Floor getFloor(Vector3 position) {
		RaycastHit hit;
		if(Physics.Raycast(position + Vector3.up * 0.25f, Vector3.down, out hit, 0.3f)) {
			if(hit.collider.tag == "Floor" || hit.collider.tag == "Lift") {
				return hit.collider.gameObject.GetComponent<Floor>();
			}
		}
		return null;
	}

	// ------------------------------------------------------------------------

	public bool Walking {
		get {
			var distance = Vector3.Distance(transform.position, endPosition);

			#if UNITY_DEBUG
			var distanceTotal = Vector3.Distance(startPosition, endPosition);
			if(distanceTotal > (1 + deadZone)) {
				Debug.LogError("Distance > 1: " + gameObject.name + " / " + distanceTotal);
				Debug.Break();
			}
			Debug.Log("Object is walking " + gameObject.name + " " + (distance > deadZone) + " (" + distance + " / " + distanceTotal + ")");
			#endif

			if(distance > deadZone) {
				return true;
			} else {
				startPosition = smoothVector(startPosition);
				endPosition = smoothVector(endPosition);
				transform.position = smoothVector(transform.position);
				startPosition = endPosition;
				startTime = 0f;
			}
			return false;
		}
	}

	// ------------------------------------------------------------------------

	private Vector3 smoothVector(Vector3 v) {
		if(smooth) {
			v.x = Mathf.RoundToInt(v.x);
			v.y = Mathf.RoundToInt(v.y);
			v.z = Mathf.RoundToInt(v.z);
		}
		return v;
	}
}
