using UnityEngine;
using System.Collections;

public class SpiderCamera : MonoBehaviour {
	private GameObject target;
	public float distance = 1;
	public float rotationSpeed = 0.8f;
	public Vector3 endPosition;

	public float startTime;
	private Transform lookAt;
	private bool forcedUpdate = false;

	// ------------------------------------------------------------------------

	void Update() {
		if(!target) {
			return;
		}

		Actor actor = target.GetComponent<Actor>();

		if(Input.GetKeyDown(KeyCode.UpArrow)) {
			actor.move(target.transform.forward);
		} else if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			actor.turnLeft();
			startTime = 0;
		} else if(Input.GetKeyDown(KeyCode.DownArrow)) {
			actor.move(-target.transform.forward);
		} else if(Input.GetKeyDown(KeyCode.RightArrow)) {
			actor.turnRight();
			startTime = 0;
		} else if(Input.GetKeyDown(KeyCode.Space)) {
			actor.fire();
		} else if(Input.GetKeyDown(KeyCode.Return)) {
			actor.lift();
		}
	}

	// ------------------------------------------------------------------------

	void LateUpdate () {
		if(!target) {
			return;
		}

		var currentRotation = Quaternion.Euler (0, lookAt.eulerAngles.y, 0);
		endPosition = lookAt.position - currentRotation * Vector3.forward * distance;

		if(forcedUpdate) {
			transform.position = endPosition;
//			Debug.Log("Camera: " + transform.position + " Spider: " + target.transform.position);
			forcedUpdate = false;
		} else {
			Vector3 startPosition = transform.position;
			if(Vector3.Distance(startPosition, endPosition) > 0) {
				startTime += Time.deltaTime * rotationSpeed;
				transform.position = Vector3.Slerp(startPosition - lookAt.position, endPosition - lookAt.position, startTime);
				transform.position += lookAt.position;
			}
		}
		transform.LookAt(lookAt);
	}

	// ------------------------------------------------------------------------

	public GameObject Target {
		set {
			this.target = value;
			if(value) {
				forcedUpdate = true;
				lookAt = value.transform.Find("LookAt");
			}
		}
		get {
			return target;
		}
	}
}
