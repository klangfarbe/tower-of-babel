using UnityEngine;
using System.Collections;

public class ZappingCannon : Actor {
	public bool rotating = false;
	private float timeBeforeRotation = 2.2f;
	private float lastTime = 0;
	private bool hasZappedInThisFrame = false;
	private FadingLight spotlight;

	// ------------------------------------------------------------------------

	void Start() {
		spotlight = GetComponentInChildren<FadingLight>();
	}

	// ------------------------------------------------------------------------

	void FixedUpdate() {
		if(!Enable) {
			spotlight.fadeOn = false;
			return;
		} else
			spotlight.fadeOn = true;

		if(lastTime == 0) {
			lastTime = Time.time;
		}

		RaycastHit hit;

		// push one time per turn if rotating
		if(base.raycast(out hit)) {
			Actor actor = hit.collider.gameObject.GetComponentInChildren<Actor>();
			if(!hasZappedInThisFrame && actor && actor.zapped(gameObject)) {
				hasZappedInThisFrame = true;
			}
			if(!rotating) {
				hasZappedInThisFrame = false;
			}
		}
		if(rotating && Time.time - lastTime > timeBeforeRotation / gameController.gameSpeed) {
			if(Debug.isDebugBuild) {
				Debug.Log("RZN: rotated after " + (Time.time - lastTime) + "s");
			}
			gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, 90);
			lastTime = Time.time;
			hasZappedInThisFrame = false;
		}
	}
}