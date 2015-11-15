using UnityEngine;
using System.Collections;

public class Grabber : Actor {
	public override bool fire() {
		base.fire();

		if(isOnMovingLift()) {
			return false;
		}

		RaycastHit hit;
		if(!base.raycast(out hit))
			return false;

		if(Debug.isDebugBuild) {
			Debug.Log("Trying to grab " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
		}

		Actor actor = hit.collider.gameObject.GetComponentInChildren<Actor>();
		if(!actor || !actor.grabbed(gameObject)) {
			if(Debug.isDebugBuild) {
				Debug.Log("Could not grab " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
			}
			return false;
		} else if(Debug.isDebugBuild) {
			Debug.Log("Grabbed " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
		}
		return true;
	}
}