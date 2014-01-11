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

		MoveActor actor = target.GetComponent<MoveActor>();

		if(Input.GetKey(KeyCode.W)) {
			actor.move(target.transform.forward);
		} else if(Input.GetKeyDown(KeyCode.A)) {
			actor.turnLeft();
		} else if(Input.GetKey(KeyCode.S)) {
			actor.move(-target.transform.forward);
		} else if(Input.GetKeyDown(KeyCode.D)) {
			actor.turnRight();
		} else if(Input.GetKeyDown(KeyCode.Space)) {
			//actor.fire();
		} else if(Input.GetKeyDown(KeyCode.Q)) {
			actor.down();
		} else if(Input.GetKeyDown(KeyCode.E)) {
			actor.up();
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
