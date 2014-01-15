using UnityEngine;
using System.Collections;

public class Grabber : Actor {
	public override void fire() {
		base.fire();
		RaycastHit hit;
		if(!base.raycast(out hit))
			return;
		Actor actor = hit.collider.gameObject.GetComponentInChildren<Actor>();
		if(actor)
			actor.grabbed(gameObject);
	}
}