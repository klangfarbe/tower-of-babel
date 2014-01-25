using UnityEngine;
using System.Collections;

public class PushingCannon : Actor {
	public bool rotating = false;
	public int timeBeforeRotation = 2;

	private float lastTime = 0;
	private bool hasPushedInThisFrame = false;

	// ------------------------------------------------------------------------

	void FixedUpdate() {
		if(!Enable)
			return;

		if(lastTime == 0) {
			lastTime = Time.time;
		}

		RaycastHit hit;

		// push one time per turn if rotating
		if(base.raycast(out hit)) {
			Actor actor = hit.collider.gameObject.GetComponentInChildren<Actor>();
			if(actor && !hasPushedInThisFrame) {
				actor.pushed(gameObject);
				hasPushedInThisFrame = true;
			}
			if(!rotating) {
				hasPushedInThisFrame = false;
			}
		}
		if(rotating && Time.time - lastTime > timeBeforeRotation) {
			gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, 90);
			lastTime = Time.time;
			hasPushedInThisFrame = false;
		}
	}
}