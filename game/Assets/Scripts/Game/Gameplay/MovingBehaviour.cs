using UnityEngine;
using System.Collections;

public class MovingBehaviour : Actor {
	MoveActor actor = null;

	// ------------------------------------------------------------------------

	void Start() {
		actor = GetComponent<MoveActor>();
	}

	// ------------------------------------------------------------------------
	// prepare movement: try to go to you right field
	void FixedUpdate() {
		if(!Enable)
			return;

		if(actor.Walking || actor.nextFloor) {
			return;
		}

		if(actor.nextFieldAvailable(transform.right)) {
			actor.turnRight();
			actor.move(transform.forward);
		} else if(actor.nextFieldAvailable(transform.forward)) {
			actor.move(transform.forward);
		} else if(actor.nextFieldAvailable(-transform.right)) {
			actor.turnLeft();
			actor.move(transform.forward);
		} else if(actor.nextFieldAvailable(-transform.forward)) {
			actor.turnRight();
			actor.turnRight();
			actor.move(transform.forward);
		}
	}
}