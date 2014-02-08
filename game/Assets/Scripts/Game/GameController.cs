using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	private GameObject gameCamera;
	private CameraController cameraController;

	public GameObject activeObject;

	// ------------------------------------------------------------------------

	void Awake() {
		gameCamera = GameObject.Find("GameCam");
		cameraController = GameObject.Find("Controller").GetComponent<CameraController>();
	}

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

	public void actorForward() {
		Actor actor = getActor();
		if(actor) {
			actor.move(actor.gameObject.transform.forward);
		} else {
			cameraController.translateOverview(Vector3.up * 0.1f);
		}
	}

	// ------------------------------------------------------------------------

	public void actorBack() {
		Actor actor = getActor();
		if(actor) {
			actor.move(-actor.gameObject.transform.forward);
		} else {
			cameraController.translateOverview(-Vector3.up * 0.1f);
		}
	}

	// ------------------------------------------------------------------------

	public void actorLeft() {
		Actor actor = getActor();
		if(actor) {
			actor.turnLeft();
			gameCamera.GetComponent<FollowingCamera>().startTime = 0;
		} else { // overview is active
			cameraController.rotateOverview(-90);
		}
	}

	// ------------------------------------------------------------------------

	public void actorRight() {
		Actor actor = getActor();
		if(actor) {
			actor.turnRight();
			gameCamera.GetComponent<FollowingCamera>().startTime = 0;
		} else { // overview is active
			cameraController.rotateOverview(90);
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