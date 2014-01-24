using UnityEngine;
using System.Collections;

public class Worm : Actor {
	MoveActor actor = null;
	Vector3 direction;

	// ------------------------------------------------------------------------

	void Start() {
		actor = GetComponent<MoveActor>();
		direction = transform.forward;
	}

	// ------------------------------------------------------------------------

	void FixedUpdate() {
		if(!Enable)
			return;

		if(actor.Walking || actor.nextFloor) {
			return;
		}

		if(actor.assignNextField(direction)) {
			actor.move(direction);
		} else {
			StartCoroutine(changeDirection());
		}
	}

	// ------------------------------------------------------------------------

	public IEnumerator changeDirection() {
		yield return new WaitForSeconds(0.2f);
		direction = -direction;
	}

	// ------------------------------------------------------------------------

	public override void zapped(GameObject by) {
		direction = -direction;
		//actor.returnToOldPosition();
	}
}