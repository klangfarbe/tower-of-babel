using UnityEngine;
using System.Collections;

public class Freezer : Actor {
	private float cooldownTime = 9f;
	private float freezeTime = 9.5f;
	private bool blocked = false;

	// ------------------------------------------------------------------------

	public override bool grabbed(GameObject by) {
		if(blocked)
			return false;
		blocked = true;

		if(Debug.isDebugBuild) {
			Debug.Log(gameObject.name + ": Freeze start at " + Time.time);
		}

		playAudio(0);

		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Actor")) {
			Actor a = g.GetComponent<Actor>();
			if(a)
				a.Enable = false;
		}
		StartCoroutine(unfreeze());
		return true;
	}

	// ------------------------------------------------------------------------

	IEnumerator unfreeze() {
		yield return new WaitForSeconds(freezeTime / gameController.gameSpeed);

		if(Debug.isDebugBuild) {
			Debug.Log(gameObject.name + ": Unfreeze at " + Time.time);
		}

		playAudio(1);

		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Actor")) {
			Actor a = g.GetComponent<Actor>();
			if(a)
				a.Enable = true;
		}
		StartCoroutine(cooldown());
	}

	// ------------------------------------------------------------------------

	IEnumerator cooldown() {
		yield return new WaitForSeconds(cooldownTime / gameController.gameSpeed);

		if(Debug.isDebugBuild) {
			Debug.Log(gameObject.name + ": Cooldown ended at " + Time.time);
		}
		blocked = false;
	}
}