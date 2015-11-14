using UnityEngine;
using System.Collections;

public class PushingCannon : Actor {
	public bool rotating = false;
	private float timeBeforeRotation = 2.2f;
	private float lastTime = 0;
	private bool hasPushedInThisFrame = false;
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
		} else {
			spotlight.fadeOn = true;
			playAudio();
		}

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
		if(rotating && Time.time - lastTime > timeBeforeRotation / gameController.gameSpeed) {
			if(Debug.isDebugBuild) {
				Debug.Log("RPN: rotated after " + (Time.time - lastTime) + "s");
			}
			gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, 90);
			lastTime = Time.time;
			hasPushedInThisFrame = false;
		}
	}
}