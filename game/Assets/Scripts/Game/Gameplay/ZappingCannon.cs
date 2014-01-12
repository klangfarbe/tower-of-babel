using UnityEngine;
using System.Collections;

public class ZappingCannon : Actor {
	public bool rotating = false;
	public int timeBeforeRotation = 2;
	private float lastTime;

	void Start() {
		lastTime = Time.time;
	}

	void Update() {
		if(!Enable)
			return;

		RaycastHit hit;

		if(base.raycast(out hit)) {
			Actor actor = hit.collider.gameObject.GetComponentInChildren<Actor>();
			if(actor) {
				actor.zapped(gameObject);
			}
		}
		if(rotating && Time.time - lastTime > timeBeforeRotation) {
			turnRight();
			lastTime = Time.time;
		}
	}
}