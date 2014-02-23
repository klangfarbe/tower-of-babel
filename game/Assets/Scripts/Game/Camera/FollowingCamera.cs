using UnityEngine;
using System.Collections;

public class FollowingCamera : MonoBehaviour {
	private GameObject target;
	private float rotationSpeed = 0.1f;
	private bool forcedUpdate = false;

	private float distance = 1f;
	private float height = 0.25f;
	private float angle = 5f;
	public  float startTime;
	private Vector3 endPosition;

	// ------------------------------------------------------------------------

	public void set(GameObject target, float distance, float height, float angle) {
		this.target = target;
		this.distance = distance;
		this.height = height;
		this.angle = angle;
		startTime = 0;
		forcedUpdate = true;
	}

	// ------------------------------------------------------------------------

	void LateUpdate () {
		if(!target) {
			return;
		}

		// calculate the correct offset position where the camera should look at
		var gegenkathethe = Mathf.Tan(angle * Mathf.Deg2Rad) * distance;
		var adjustedTargetPosition = target.transform.position + Vector3.up * height;
		var targetRotation = Quaternion.Euler (0, target.transform.eulerAngles.y, 0);
		endPosition = adjustedTargetPosition - targetRotation * Vector3.forward * distance + Vector3.up * gegenkathethe;

		if(forcedUpdate) {
			transform.position = endPosition;
			forcedUpdate = false;
		} else {
			if(Vector3.Distance(transform.position, endPosition) > 0) {
				startTime += Time.deltaTime * rotationSpeed;
				transform.position = Vector3.Slerp(transform.position - adjustedTargetPosition, endPosition - adjustedTargetPosition, startTime);
				transform.position += adjustedTargetPosition;
			}
			if(startTime > 1f) {
				startTime = 0;
			}
		}
		transform.LookAt(target.transform.position + Vector3.up * height);
	}

	// ------------------------------------------------------------------------

	public GameObject Target {
		set {
			this.target = value;
			if(value) {
				forcedUpdate = true;
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
	}
		get {
			return distance;
		}
	}

	// ------------------------------------------------------------------------

	public float Height {
		set {
			this.height = value;
			forcedUpdate = true;
		}
		get {
			return height;
		}
	}

	// ------------------------------------------------------------------------

	public float Angle {
		set {
			this.angle = value;
		}
		get {
			return angle;
		}
	}

	public void force() {
		forcedUpdate = true;
	}
}
