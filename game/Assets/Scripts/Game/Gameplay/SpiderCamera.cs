using UnityEngine;
using System.Collections;

public class SpiderCamera : MonoBehaviour {
	public GameObject target;
	public float distance = 1;

	// -----------------------------------------------------------------------------------------------------------------

	void Update() {
		if(!target) {
			return;
		}

		Actor actor = target.GetComponent<Actor>();

		if(Input.GetKeyDown(KeyCode.W)) {
			actor.moveForward();
		} else if(Input.GetKeyDown(KeyCode.A)) {
			actor.turnLeft();
		} else if(Input.GetKeyDown(KeyCode.S)) {
			actor.moveBack();
		} else if(Input.GetKeyDown(KeyCode.D)) {
			actor.turnRight();
		} else if(Input.GetKeyDown(KeyCode.Space)) {
			actor.fire();
		} else if(Input.GetKeyDown(KeyCode.Q)) {
			actor.activateLiftDown();
		} else if(Input.GetKeyDown(KeyCode.E)) {
			actor.activateLiftUp();
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	void LateUpdate () {
		if(!target) {
			return;
		}

		Transform t = target.transform.Find("LookAt");

		var currentRotation = Quaternion.Euler (0, t.eulerAngles.y, 0);
		transform.position = t.position;
		transform.position -= currentRotation * Vector3.forward * distance;
		transform.LookAt(t);
	}
}
