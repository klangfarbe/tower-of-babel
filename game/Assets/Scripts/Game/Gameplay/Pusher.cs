using UnityEngine;
using System.Collections;

public class Pusher : Actor {
	public override bool fire() {
		base.fire();

		if(isOnMovingLift()) {
			return false;
		}

		RaycastHit hit;
		if(!base.raycast(out hit))
			return false;

		if(Debug.isDebugBuild) {
			Debug.Log("Trying to push " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
		}

		Actor actor = hit.collider.gameObject.GetComponentInChildren<Actor>();
		if(!actor || !actor.pushed(gameObject)) {
			if(Debug.isDebugBuild) {
				Debug.Log("Could not push " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
			}
			return false;
		} else if(Debug.isDebugBuild) {
			Debug.Log("Pushed " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
		}
		return true;
	}
}