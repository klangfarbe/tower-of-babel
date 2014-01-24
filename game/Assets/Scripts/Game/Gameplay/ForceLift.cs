using UnityEngine;
using System.Collections;

public class ForceLift : Actor {
	public bool triggerUp = false;

	private GameObject[] lifts;
	private static bool liftsActive = false;

	// ------------------------------------------------------------------------

	void Start() {
		lifts = GameObject.FindGameObjectsWithTag("Lift");
	}

	// ------------------------------------------------------------------------

	void FixedUpdate() {
		if(ForceLift.liftsActive) {
			foreach(GameObject lift in lifts) {
				if(lift.GetComponent<Lift>().isPlaying()) {
					return;
				}
			}
			ForceLift.liftsActive = false;
		}
	}

	// ------------------------------------------------------------------------

	public override void grabbed(GameObject by) {
		if(ForceLift.liftsActive)
			return;

		ForceLift.liftsActive = true;

		foreach(GameObject lift in lifts) {
			if(triggerUp)
				lift.GetComponent<Lift>().up();
			else
				lift.GetComponent<Lift>().down();
		}
	}
}