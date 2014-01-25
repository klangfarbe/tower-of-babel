using UnityEngine;
using System.Collections;

public class Freezer : Actor {
	public float cooldownTime = 3f;
	public float freezeTime = 3f;

	// ------------------------------------------------------------------------

	void FixedUpdate() {
		if(!Enable)
			return;
	}
}