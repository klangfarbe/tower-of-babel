using UnityEngine;
using System.Collections;

public class Pusher : Actor {
	public override void fire() {
		base.fire();

		if(isOnMovingLift()) {
			return;
		}

		RaycastHit hit;
		if(!base.raycast(out hit))
			return;

		if(Debug.isDebugBuild) {
			Debug.Log("Trying to push " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
		}

		Actor actor = hit.collider.gameObject.GetComponentInChildren<Actor>();
		if(!actor || !actor.pushed(gameObject)) {
			if(Debug.isDebugBuild) {
				Debug.Log("Could not push " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
			}
		} else if(Debug.isDebugBuild) {
			Debug.Log("Pushed " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
		}
	}
}