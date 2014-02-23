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
		Actor actor = hit.collider.gameObject.GetComponentInChildren<Actor>();
		if(!actor || !actor.zapped(gameObject)) {

		}
	}
}