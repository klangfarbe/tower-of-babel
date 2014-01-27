using UnityEngine;
using System.Collections;

public class Lizard : Actor, ScaleAnimationCallback {
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
		floorToDestroy = actor.getFloor(transform.position);
		if(!floorToDestroy.gameObject.name.StartsWith("FLR")) {
			floorToDestroy = null;
		}
	}

	// ------------------------------------------------------------------------

	private void destroyFloor() {
		if(floorToDestroy && actor.getFloor(transform.position) != floorToDestroy) {
			ScaleAnimation anim = floorToDestroy.GetComponent<ScaleAnimation>();
			if(anim.scale() > 0) {
				anim.run(this);
			}
		}
	}

 	// ------------------------------------------------------------------------

	public void scaleAnimationFinished() {
		if(floorToDestroy)
			Destroy(floorToDestroy.gameObject);
		floorToDestroy = null;
	}
}