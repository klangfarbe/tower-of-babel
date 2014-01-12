using UnityEngine;
using System.Collections;

public class Conditions : MonoBehaviour {

	public int klondikesToGather = 0;
	public int klondikesGathered = 0;
	public int robotsToDestroy = 0;
	public int robotsDestroyed = 0;
	public int timelimit = 0;
	public float startTime;
	public bool levelStarted = false;

	// -----------------------------------------------------------------------------------------------------------------

	void Update () {
		if(levelStarted) {
			checkWinningConditions();
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	private void checkWinningConditions() {
		if(timelimit > 0 && Time.time - startTime > timelimit) {
			Debug.Log("Level failed due to timeout");
			levelFailed();
		}

		// find necessary objects to complete the level
		GameObject psh = GameObject.Find("PSH");
		GameObject zap = GameObject.Find("ZAP");
		GameObject grb = GameObject.Find("GRB");

		if(!psh && !zap && !grb) {
			levelFailed();
		}

		if(!grb && klondikesGathered < klondikesToGather) {
			levelFailed();
		}

//		if(grb && klondikesGathered < klondikesToGather && ) {
//			levelFailed();
//		}

		if(!zap && robotsDestroyed < robotsToDestroy) {
			levelFailed();
		}

		if(klondikesGathered == klondikesToGather && robotsDestroyed == robotsToDestroy) {
			levelCompleted();
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	private void levelFailed() {
		Debug.Log("Level failed!");
		levelStarted = false;
	}

	// -----------------------------------------------------------------------------------------------------------------

	private void levelCompleted() {
		Debug.Log("Level completed!");
		levelStarted = false;
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void startLevel() {
		if(levelStarted)
			return;
		Debug.Log("Level started");
		levelStarted = true;
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Actor")) {
			Debug.Log("Enabling gameobject " + g.name);
			Actor actor = g.GetComponent<Actor>();
			if(actor) {
				actor.Enable = true;
			}
		}
		startTime = Time.time;
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void init(int klondikes, int robots, int timelimit) {
		levelStarted = false;
		klondikesGathered = 0;
		klondikesToGather = klondikes;
		robotsDestroyed = 0;
		robotsToDestroy = robots;
		this.timelimit = timelimit;
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void pickupKlondike() {
		if(klondikesToGather == 0)
			return;
		klondikesGathered++;
		Debug.Log("Conditions: " + klondikesGathered + "/" + klondikesToGather + " Klondikes collected");
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void destroyRobot() {
		if(robotsToDestroy == 0)
			return;
		robotsDestroyed++;
		Debug.Log("Conditions: " + robotsDestroyed + "/" + robotsToDestroy + " Objects destroyed");
	}
}
