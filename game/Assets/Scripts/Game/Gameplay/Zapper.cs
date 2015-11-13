using UnityEngine;
using System.Collections;

public class Zapper : Actor {
	public override void fire() {
		base.fire();

		if(isOnMovingLift()) {
			return;
		}

		RaycastHit hit;
		if(!base.raycast(out hit))
			return;

		if(Debug.isDebugBuild) {
			Debug.Log("Trying to zap " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
		}

		Actor actor = hit.collider.gameObject.GetComponentInChildren<Actor>();
		if(!actor || !actor.zapped(gameObject)) {
			if(Debug.isDebugBuild) {
				Debug.Log("Could not zap " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
			}
		} else if(Debug.isDebugBuild) {
			Debug.Log("Zapped " + hit.collider.gameObject.name + " (" + hit.collider.tag + ")");
		}
	}
}