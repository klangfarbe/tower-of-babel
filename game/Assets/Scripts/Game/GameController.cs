using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject activeObject;

	// ------------------------------------------------------------------------

	public void actorFire() {
		Actor actor = getActor();
		if(actor) {
			actor.fire();
		}
	}

	// ------------------------------------------------------------------------

	public void actorLift() {
		Actor actor = getActor();
		if(actor) {
			actor.lift();
		}
	}

	// ------------------------------------------------------------------------

	public void actorLeft() {
		Actor actor = getActor();
		if(actor) {
			actor.turnLeft();
			var gameCamera = GameObject.Find("GameCam");
			gameCamera.GetComponent<FollowingCamera>().startTime = 0;
		}
	}

	// ------------------------------------------------------------------------

	public void actorForward() {
		Actor actor = getActor();
		if(actor) {
			actor.move(actor.gameObject.transform.forward);
		}
	}

	// ------------------------------------------------------------------------

	public void actorBack() {
		Actor actor = getActor();
		if(actor) {
			actor.move(-actor.gameObject.transform.forward);
		}
	}

	// ------------------------------------------------------------------------

	public void actorRight() {
		Actor actor = getActor();
		if(actor) {
			actor.turnRight();
			var gameCamera = GameObject.Find("GameCam");
			gameCamera.GetComponent<FollowingCamera>().startTime = 0;

		}
	}

	// ------------------------------------------------------------------------

	private Actor getActor() {
		if(activeObject == null) {
			return null;
		}
		return activeObject.GetComponent<Actor>();
	}

	// ------------------------------------------------------------------------

	public IEnumerator levelCompleted() {
		yield return new WaitForSeconds(5);
	}

	// ------------------------------------------------------------------------

	public IEnumerator levelFailed() {
		yield return new WaitForSeconds(5);
	}

	// ------------------------------------------------------------------------

	public void levelPause() {

	}

	// ------------------------------------------------------------------------


}