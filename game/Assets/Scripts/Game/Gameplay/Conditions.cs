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
	public GUIText conditions;
	public GUIText infotext;
	public GUIText remainingTimeText;

	// ------------------------------------------------------------------------

	void Update () {
		if(levelStarted) {
			updateRemainingTimeText();
			checkWinningConditions();
		}
	}

	// ------------------------------------------------------------------------

	private void checkWinningConditions() {
		if(timelimit > 0 && Time.time - startTime > timelimit) {
			StartCoroutine(levelFailed());
		}

		// find necessary objects to complete the level
		GameObject psh = GameObject.Find("PSH");
		GameObject zap = GameObject.Find("ZAP");
		GameObject grb = GameObject.Find("GRB");

		if(!psh && !zap && !grb) {
			StartCoroutine(levelFailed());
		}

		if(!grb && klondikesGathered < klondikesToGather) {
			StartCoroutine(levelFailed());
		}

		if(!zap && robotsDestroyed < robotsToDestroy) {
			StartCoroutine(levelFailed());
		}

		if(klondikesGathered == klondikesToGather && robotsDestroyed == robotsToDestroy) {
			StartCoroutine(levelCompleted());
		}
	}

	// ------------------------------------------------------------------------

	public IEnumerator levelFailed() {
		Debug.Log("Level failed!");
		levelStarted = false;
		infotext.text = "Level failed!";
		yield return new WaitForSeconds(2);
		GameObject.Find("Level").GetComponent<LevelLoader>().build();
	}

	// ------------------------------------------------------------------------

	public IEnumerator levelCompleted() {
		Debug.Log("Level completed!");
		levelStarted = false;
		infotext.text = "Level completed!";
		yield return new WaitForSeconds(2);
		GameObject.Find("Level").GetComponent<LevelLoader>().next();
	}

	// ------------------------------------------------------------------------

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

	// ------------------------------------------------------------------------

	public void init(int klondikes, int robots, int timelimit) {
		infotext.text = "";
		levelStarted = false;
		klondikesGathered = 0;
		klondikesToGather = klondikes;
		robotsDestroyed = 0;
		robotsToDestroy = robots;
		this.timelimit = timelimit;
		remainingTimeText.text = "";
		updateConditionsText();
		if(timelimit > 0) {
			TimeSpan timeSpan = TimeSpan.FromSeconds(timelimit);
			remainingTimeText.text = string.Format("{0:D}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
		}
	}

	// ------------------------------------------------------------------------

	public void updateRemainingTimeText() {
		float delta = Time.time - startTime;
		if(!(timelimit > 0) || timelimit - delta < 0) {
			return;
		}
		TimeSpan timeSpan = TimeSpan.FromSeconds(timelimit - delta);
		remainingTimeText.text = string.Format("{0:D}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
	}

	// ------------------------------------------------------------------------

	public void updateConditionsText() {
		conditions.text = klondikesGathered + "/" + klondikesToGather + " Klondikes collected\n"
			+ robotsDestroyed + "/" + robotsToDestroy + " Robots destroyed";
	}

	// ------------------------------------------------------------------------

	public void pickupKlondike() {
		if(klondikesToGather == 0 || klondikesToGather == klondikesGathered)
			return;
		klondikesGathered++;
		updateConditionsText();
		Debug.Log("Conditions: " + klondikesGathered + "/" + klondikesToGather + " Klondikes collected");
	}

	// ------------------------------------------------------------------------

	public void destroyRobot() {
		if(robotsToDestroy == 0 || robotsToDestroy == robotsDestroyed)
			return;
		robotsDestroyed++;
		updateConditionsText();
		Debug.Log("Conditions: " + robotsDestroyed + "/" + robotsToDestroy + " Robots destroyed");
	}
}
