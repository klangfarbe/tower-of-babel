using UnityEngine;
using System.Collections;

public class MoveActor : MonoBehaviour {
	public float speed = 0.05f;
	private bool walking = false;
	public Vector3 endPosition;
	private float startTime;

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

	public bool targetFieldIsFree(Vector3 direction) {
		RaycastHit hit;

		// check if something is standing on the field
		Debug.DrawRay (transform.position + Vector3.up * 0.25f, direction, Color.blue, 0.5f);
		if(Physics.Raycast(transform.position + Vector3.up * 0.25f, direction, out hit, 1f)) {
			Debug.Log("Hit " + hit.collider.gameObject.name);
			return false;
		}

		// check if floor is available
		Debug.DrawRay(transform.position + direction + Vector3.up * 0.25f, Vector3.down * 0.3f, Color.green, 0.5f);
		if(Physics.Raycast(transform.position + direction + Vector3.up * 0.25f, Vector3.down, out hit, 0.3f)) {
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

	public void move(Vector3 t) {
		if(walking || !targetFieldIsFree (t))
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
			Debug.Log("Hit: " + hit.collider.gameObject.name + " " + hit.collider.tag);
			if(hit.collider.tag == "lift") {
				return hit.collider.gameObject.transform.parent.gameObject.GetComponentInChildren<Lift>();
			}
		}
		return null;
	}

	// -----------------------------------------------------------------------------------------------------------------

	void OnTriggerEnter(Collider other) {
		Debug.Log("OnTriggerEnter: " + other.gameObject.name);
	}

	// -----------------------------------------------------------------------------------------------------------------

	void OnTriggerStay(Collider other) {
		Debug.Log("OnTriggerStay: " + other.gameObject.name);
	}

	// -----------------------------------------------------------------------------------------------------------------

	void OnTriggerExit(Collider other) {
		Debug.Log("OnTriggerExit: " + other.gameObject.name);
	}
}
