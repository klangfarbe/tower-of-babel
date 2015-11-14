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

		instantiateParticle("FRZ_activated", freezeTime / gameController.gameSpeed);

		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Actor")) {
			Actor a = g.GetComponent<Actor>();
			if(a)
				a.Enable = false;
		}

		if(Debug.isDebugBuild) {
			Debug.Log(gameObject.name + ": Freeze start at " + Time.time);
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

		foreach(GameObject g in GameObject.FindGameObjectsWithTag("Actor")) {
			Actor a = g.GetComponent<Actor>();
			if(a)
				a.Enable = true;
		}

		instantiateParticle("FRZ_cooldown", freezeTime);

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

	// ------------------------------------------------------------------------

	private void instantiateParticle(string name, float duration) {
		GameObject prefab = (GameObject) Resources.Load(name);
		Object o = Instantiate(prefab, transform.position + Vector3.up * 0.1f, prefab.transform.rotation);
		Destroy(o, duration + 3f);
	}
}