using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	private GameObject gameCamera;
	private CameraController cameraController;
	private GameGUI gui;

	public GameObject activeObject;

	private string version = "0.2b32";

	// ------------------------------------------------------------------------

	void Awake() {
		GameObject.Find("Version").GetComponent<GUIText>().text = version;

		gameCamera = GameObject.Find("GameCam");
		cameraController = GameObject.Find("Controller").GetComponent<CameraController>();
		gui = GameObject.Find("Controller").GetComponent<GameGUI>();
	}

	// ------------------------------------------------------------------------

	void Start() {
		checkUpdate();
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
		gui.notify("Level completed!");
		yield return new WaitForSeconds(3);
		gui.notify("");
		GameObject.Find("Level").GetComponent<LevelLoader>().next();
	}

	// ------------------------------------------------------------------------

	public IEnumerator levelFailed() {
		gui.notify("Level failed!");
		yield return new WaitForSeconds(3);
		gui.notify("");
		GameObject.Find("Level").GetComponent<LevelLoader>().reload();
	}

	// ------------------------------------------------------------------------

	public void levelPause() {
		Time.timeScale = 0f;
	}

	// ------------------------------------------------------------------------

	public void levelUnpause() {
		Time.timeScale = 1f;
	}

	// ------------------------------------------------------------------------

	public void checkUpdate() {
		gui.notify("Checking for update");
		StartCoroutine(wwwcall());
	}

	private IEnumerator wwwcall() {
		yield return new WaitForSeconds(1);
		WWW www = new WWW("http://tob.guzumi.de/version.txt");
		yield return www;
		if(www.text != version) {
			gui.notify("New version " + www.text);
		} else {
			gui.notify("No new version available");
		}
		yield return new WaitForSeconds(3);
		Debug.Log("Clear text");
		gui.notify("");
	}

}