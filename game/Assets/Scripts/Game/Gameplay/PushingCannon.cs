using UnityEngine;
using System.Collections;

public class PushingCannon : Actor {
	public bool rotating = false;
	public int timeBeforeRotation = 2;

	private float lastTime;
	private bool hasPushedInThisFrame = false;

	// ------------------------------------------------------------------------

	void Start() {
		lastTime = Time.time;
	}

	// ------------------------------------------------------------------------

	void FixedUpdate() {
		if(!Enable)
			return;

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
			turnRight();
			lastTime = Time.time;
			hasPushedInThisFrame = false;
		}
	}
}