using UnityEngine;
using System;
using System.Collections;

public class Conditions : MonoBehaviour {
	public int klondikesToGather = 0;
	public int klondikesGathered = 0;
	public int robotsToDestroy = 0;
	public int robotsDestroyed = 0;
	public int timelimit = 0;
	public float startTime;
	public bool levelStarted = false;
	private bool timeout = false;

	private GameController gameController;

	// ------------------------------------------------------------------------

	void Update () {
		if(levelStarted) {
			checkWinningConditions();
		}
	}

	// ------------------------------------------------------------------------

	void Start() {
		gameController = GameObject.Find("Controller").GetComponent<GameController>();
	}

	// ------------------------------------------------------------------------

	private void checkWinningConditions() {
		if(klondikesToGather == 0 && robotsToDestroy == 0) {
			return;
		}

		if(timelimit > 0 && Time.time - startTime >= timelimit) {
			levelStarted = false;
			timeout = true;
			StartCoroutine(gameController.levelFailed());
		}

		// find necessary objects to complete the level
		GameObject psh = GameObject.Find("PSH");
		GameObject zap = GameObject.Find("ZAP");
		GameObject grb = GameObject.Find("GRB");

		if(!psh && !zap && !grb) {
			levelStarted = false;
			StartCoroutine(gameController.levelFailed());
		}

		if(!grb && klondikesGathered < klondikesToGather) {
			levelStarted = false;
			StartCoroutine(gameController.levelFailed());
		}

		if(!zap && robotsDestroyed < robotsToDestroy) {
			levelStarted = false;
			StartCoroutine(gameController.levelFailed());
		}

		if(klondikesGathered == klondikesToGather && robotsDestroyed == robotsToDestroy) {
			levelStarted = false;
			StartCoroutine(gameController.levelCompleted());
		}
	}

	// ------------------------------------------------------------------------

	public void startLevel() {
		if(levelStarted)
			return;

		if(Debug.isDebugBuild)
			Debug.Log("Level started");

		levelStarted = true;
		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Actor")) {
			if(Debug.isDebugBuild)
				Debug.Log("Enabling gameobject " + g.name);

			Actor actor = g.GetComponent<Actor>();
			if(actor) {
				actor.Enable = true;
			}
		}
		startTime = Time.time;
	}

	// ------------------------------------------------------------------------

	public void init(int klondikes, int robots, int timelimit) {
		levelStarted = false;
		timeout = false;
		klondikesGathered = 0;
		klondikesToGather = klondikes;
		robotsDestroyed = 0;
		robotsToDestroy = robots;
		this.timelimit = timelimit;
	}

	// ------------------------------------------------------------------------

	public string getRemainingTime() {
		if(!(timelimit > 0)) {
			return "";
		}
		if(timeout) {
			return "0:00";
		}

		TimeSpan timeSpan;
		if(!levelStarted) {
			timeSpan = TimeSpan.FromSeconds(timelimit);
		} else {
			float delta = Time.time - startTime;
			timeSpan = TimeSpan.FromSeconds(timelimit - delta);
		}
		return string.Format("{0:D}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
	}

	// ------------------------------------------------------------------------

	public string getConditionsText() {
		return klondikesGathered + "/" + klondikesToGather + " Klondikes\n"
			+ robotsDestroyed + "/" + robotsToDestroy + " Robots\n";
	}

	// ------------------------------------------------------------------------

	public void pickupKlondike() {
		if(klondikesToGather == 0 || klondikesToGather == klondikesGathered)
			return;
		klondikesGathered++;

		if(Debug.isDebugBuild)
			Debug.Log("Conditions: " + klondikesGathered + "/" + klondikesToGather + " Klondikes collected");
	}
	// ------------------------------------------------------------------------

	public void destroyRobot() {
		if(robotsToDestroy == 0 || robotsToDestroy == robotsDestroyed)
			return;
		robotsDestroyed++;

		if(Debug.isDebugBuild)
			Debug.Log("Conditions: " + robotsDestroyed + "/" + robotsToDestroy + " Robots destroyed");
	}
}
