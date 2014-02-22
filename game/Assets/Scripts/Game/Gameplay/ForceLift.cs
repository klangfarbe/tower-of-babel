using UnityEngine;
using System.Collections;

public class ForceLift : Actor {
	public bool triggerUp = false;

	public static bool liftsActive = false;

	// ------------------------------------------------------------------------

	void Start() {
		ForceLift.liftsActive = false;
	}

	// ------------------------------------------------------------------------

	void FixedUpdate() {
		if(ForceLift.liftsActive) {
			foreach(GameObject lift in GameObject.FindGameObjectsWithTag("Lift")) {
				if(lift.GetComponent<Lift>().isPlaying()) {
					return;
				}
			}
			ForceLift.liftsActive = false;
		}
	}

	// ------------------------------------------------------------------------

	public override bool grabbed(GameObject by) {
		if(ForceLift.liftsActive)
			return false;

		ForceLift.liftsActive = true;

		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Lift")) {
			Lift lift = g.GetComponent<Lift>();
			if(lift.getCarriedElement()) // skip if something stands on the lift
				continue;
			if(triggerUp)
				lift.up();
			else
				lift.down();
		}
		return true;
	}
}