using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveActor : MonoBehaviour {
	public float speed = 0.05f;
	private bool walking = false;
	public Vector3 endPosition;
	private float startTime;

	private List<GameObject> pushedBy = new List<GameObject>();

	// -----------------------------------------------------------------------------------------------------------------

	void Start () {
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

	void FixedUpdate() {
		// select the nearest pusher which pushed the object
		if(pushedBy.Count > 0) {
			Vector3 v = transform.position;
			float distance = 9999;
			foreach(GameObject g in pushedBy) {
				float d = Vector3.Distance(transform.position, g.transform.position);
				if(d < distance) {
					distance = d;
					v = g.transform.forward;
				}
			}
			pushedBy.Clear();
			move(v);
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	public bool targetFieldIsFree(Vector3 direction) {
		RaycastHit hit;

		// check if something is standing on the field
		Debug.DrawRay (endPosition + Vector3.up * 0.25f, direction, Color.blue, 0.5f);
		if(Physics.Raycast(endPosition + Vector3.up * 0.25f, direction, out hit, 1f)) {
			return false;
		}

		// check if floor is available
		Debug.DrawRay(endPosition + direction + Vector3.up * 0.25f, Vector3.down * 0.3f, Color.green, 0.5f);
		if(Physics.Raycast(endPosition + direction + Vector3.up * 0.25f, Vector3.down, out hit, 0.3f)) {
			// Prevent from moving if lift still in animation
			if(hit.collider.tag == "Lift"
				&& hit.collider.gameObject.transform.parent.gameObject.GetComponentInChildren<Lift>().isPlaying()) {
				return false;
			}
			return true;
		}
		return false;
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void set(Vector3 position) {
		endPosition = position;
		transform.position = position;
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void pushed(GameObject by) {
		pushedBy.Add(by);
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void move(Vector3 t) {
		if(walking || pushedBy.Count != 0 || !targetFieldIsFree (t))
			return;
		endPosition += t;
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void up() {
		if(walking) {
			return;
		}
		Lift lift = getLift();
		if(lift) {
			lift.up();
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void down() {
		if(walking) {
			return;
		}
		Lift lift = getLift();
		if(lift) {
			lift.down();
		}
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

	Lift getLift() {
		RaycastHit hit;
//		Debug.DrawRay(transform.position + Vector3.up * 0.25f, Vector3.down * 0.3f, Color.green, 1f);
		if(Physics.Raycast(transform.position + Vector3.up * 0.25f, Vector3.down, out hit, 0.3f)) {
			if(hit.collider.tag == "Lift") {
				return hit.collider.gameObject.transform.parent.gameObject.GetComponentInChildren<Lift>();
			}
		}
		return null;
	}
}
