using UnityEngine;
using System.Collections;

public class Grabber : Actor {
	public override void fire() {
		base.fire();

		if(isOnMovingLift()) {
			return;
		}

		RaycastHit hit;
		if(!base.raycast(out hit))
			return;

		if(Debug.isDebugBuild) {
			Debug.Log("Trying to grab " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
		}

		Actor actor = hit.collider.gameObject.GetComponentInChildren<Actor>();
		if(!actor || !actor.grabbed(gameObject)) {
			if(Debug.isDebugBuild) {
				Debug.Log("Could not grab " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
			}
		} else if(Debug.isDebugBuild) {
			Debug.Log("Grabbed " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
		}
	}
}