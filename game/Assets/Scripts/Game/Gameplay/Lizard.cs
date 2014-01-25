using UnityEngine;
using System.Collections;

public class Lizard : Actor {
	MoveActor actor = null;
	Vector3 direction;
	Floor floorToDestroy = null;

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

		destroyFloor();

		if(actor.assignNextField(direction)) {
			actor.move(direction);
		} else if(actor.assignNextField(-direction) && actor.nextFloor != floorToDestroy){
			StartCoroutine(changeDirection());
		}
	}

	// ------------------------------------------------------------------------

	public IEnumerator changeDirection() {
		yield return new WaitForSeconds(0.2f);
		direction = -direction;
		floorToDestroy = actor.getFloor();
	}

	// ------------------------------------------------------------------------

	private void destroyFloor() {
		if(floorToDestroy && actor.getFloor() != floorToDestroy) {
			ScaleAnimation anim = floorToDestroy.GetComponent<ScaleAnimation>();
			if(anim.scale() > 0) {
				anim.run();
			} else {
				Destroy(floorToDestroy.gameObject);
				floorToDestroy = null;
			}
		}
	}
}