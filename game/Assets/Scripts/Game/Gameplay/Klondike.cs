using UnityEngine;
using System.Collections;

public class Klondike : Actor {
	private bool isGrabbed = false;
	private ScaleAnimation animation;

	// ------------------------------------------------------------------------

	void Update() {
		if(isGrabbed && animation.scale() == 0) {
			GameObject.Find("Level").GetComponent<Conditions>().pickupKlondike();
			Destroy(gameObject);
		}
	}

	// ------------------------------------------------------------------------

	public override void grabbed(GameObject by) {
		isGrabbed = true;
		animation = gameObject.GetComponent<ScaleAnimation>();
		animation.run();
	}
}