using UnityEngine;
using System.Collections;

public class PushingCannon : Actor {
	public bool rotating = false;
	public int timeBeforeRotation = 2;

	private float lastTime;
	private bool pushed = false;

	// ------------------------------------------------------------------------

	void Start() {
		lastTime = Time.time;
	}

	// ------------------------------------------------------------------------

	void Update() {
		if(!Enable)
			return;

		RaycastHit hit;

		// push one time per turn if rotating
		if(base.raycast(out hit)) {
			Actor actor = hit.collider.gameObject.GetComponentInChildren<Actor>();
			if(actor && !pushed) {
				actor.pushed(gameObject);
				pushed = true;
			}
			if(!rotating) {
				pushed = false;
			}
		}
		if(rotating && Time.time - lastTime > timeBeforeRotation) {
			turnRight();
			lastTime = Time.time;
			pushed = false;
		}
	}
}