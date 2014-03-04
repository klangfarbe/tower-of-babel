using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	private GameObject gameCamera;
	private CameraController cameraController;
	private GUINotification gui;

	public GameObject activeObject;
	public SceneFader sceneFader;

	// ------------------------------------------------------------------------

	void Awake() {
		gameCamera = GameObject.Find("GameCam");
		cameraController = GameObject.Find("Controller").GetComponent<CameraController>();
		gui = GameObject.Find("Controller").GetComponent<GUINotification>();
		sceneFader = SceneFader.create();
		sceneFader.startScene();
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
		gui.notify("Level completed!", 3);
		yield return new WaitForSeconds(4);
		sceneFader.endScene();
		while(sceneFader.Blending)
			yield return null;
		GameObject.Find("Level").GetComponent<LevelLoader>().next();
		sceneFader.startScene();
	}

	// ------------------------------------------------------------------------

	public IEnumerator levelFailed() {
		gui.notify("Level failed!", 3);
		yield return new WaitForSeconds(4);
		StartCoroutine(levelRestart());
	}

	// ------------------------------------------------------------------------

	public IEnumerator levelAbort() {
		sceneFader.endScene();
		while(sceneFader.Blending)
			yield return null;
		Application.LoadLevel("mainmenu");
	}

	// ------------------------------------------------------------------------

	public IEnumerator levelRestart() {
		sceneFader.endScene();
		while(sceneFader.Blending)
			yield return null;
		GameObject.Find("Level").GetComponent<LevelLoader>().reload();
		sceneFader.startScene();
	}

	// ------------------------------------------------------------------------

	public void levelPause() {
		Time.timeScale = 0f;
	}

	// ------------------------------------------------------------------------

	public void levelUnpause() {
		Time.timeScale = 1f;
	}
}