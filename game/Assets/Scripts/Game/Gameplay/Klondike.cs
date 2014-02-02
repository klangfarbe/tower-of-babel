using UnityEngine;
using System.Collections;

public class Klondike : Actor {
	private bool isGrabbed = false;
	private ScaleAnimation anim;

	// ------------------------------------------------------------------------

	void Update() {
		if(isGrabbed && anim.scale() == 0) {
			GameObject.Find("Level").GetComponent<Conditions>().pickupKlondike();
			Destroy(gameObject);
		}
	}

	// ------------------------------------------------------------------------

	public override bool grabbed(GameObject by) {
		isGrabbed = true;
		anim = gameObject.GetComponent<ScaleAnimation>();
		anim.run();
		return true;
	}
}