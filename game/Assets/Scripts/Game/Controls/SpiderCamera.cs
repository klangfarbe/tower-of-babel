using UnityEngine;
using System.Collections;

public class SpiderCamera : MonoBehaviour {
	private GameObject target;
	private float rotationSpeed = 0.8f;
	private bool forcedUpdate = false;

	private float distance = 1f;
	private float height = 0.25f;
	private float angle = 0.1f;
	private float startTime;
	private Vector3 endPosition;
	private Quaternion lastRotation;

	// ------------------------------------------------------------------------


//		Actor actor = target.GetComponent<Actor>();
//
//		if(Input.GetKeyDown(KeyCode.UpArrow)) {
//			actor.move(target.transform.forward);
//		} else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
//			actor.turnLeft();
//			startTime = 0;
//		} else if(Input.GetKeyDown(KeyCode.DownArrow)) {
//			actor.move(-target.transform.forward);
//		} else if(Input.GetKeyDown(KeyCode.RightArrow)) {
//			actor.turnRight();
//			startTime = 0;
//		} else if(Input.GetKeyDown(KeyCode.Space)) {
//			actor.fire();
//		} else if(Input.GetKeyDown(KeyCode.Return)) {
//			actor.lift();
//		}

	public void set(GameObject target, float distance, float height, float angle) {
		this.target = target;
		this.distance = distance;
		this.height = height;
		this.angle = 1f - angle;
		startTime = 0;
		forcedUpdate = true;
	}

	// ------------------------------------------------------------------------

	void LateUpdate () {
		if(!target) {
			return;
		}

		var currentRotation = Quaternion.Euler (0, target.transform.eulerAngles.y, 0);
		endPosition = target.transform.position - currentRotation * Vector3.forward * distance + Vector3.up * height;

		if(lastRotation != currentRotation) {
			startTime = 0;
			lastRotation = currentRotation;
		}

		if(forcedUpdate) {
			transform.position = endPosition;
//			Debug.Log("Camera: " + transform.position + " Spider: " + target.transform.position);
			forcedUpdate = false;
		} else {
			Vector3 startPosition = transform.position;
			if(Vector3.Distance(startPosition, endPosition) > 0) {
				startTime += Time.deltaTime * rotationSpeed;
				transform.position = Vector3.Slerp(startPosition - target.transform.position, endPosition - target.transform.position, startTime);
				transform.position += target.transform.position;
			}
		}
		transform.LookAt(target.transform.position + Vector3.up * height * angle);
	}

	// ------------------------------------------------------------------------

	public GameObject Target {
		set {
			this.target = value;
			if(value) {
				forcedUpdate = true;
				lastRotation = Quaternion.Euler(0, target.transform.eulerAngles.y, 0);
			}
		}
		get {
			return target;
		}
	}

	// ------------------------------------------------------------------------

	public float Distance {
		set {
			this.distance = value;
			startTime = 0;
		}
		get {
			return distance;
		}
	}

	// ------------------------------------------------------------------------

	public float Height {
		set {
			this.height = value;
			startTime = 0;
		}
		get {
			return height;
		}
	}
}
