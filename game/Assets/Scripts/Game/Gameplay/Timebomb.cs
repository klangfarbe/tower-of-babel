using UnityEngine;
using System.Collections;

public class Timebomb : Actor {
	private GameObject[] hits = new GameObject[4];
	private float timebombspeed;
	private float actorSpeed = 1.6f;

	// ------------------------------------------------------------------------

	void Start() {
		// fast = 8 fields
		// med = 12 fields
		// slow =24 fields
		timebombspeed = (float)GameObject.Find("Level").GetComponent<LevelLoader>().Timebombspeed;
		if(timebombspeed == 0) {
			timebombspeed = actorSpeed * 24;
		}
		else if(timebombspeed == 1) {
			timebombspeed = actorSpeed * 12;
		}
		else if(timebombspeed == 2) {
			timebombspeed = actorSpeed * 8;
		}
		GetComponent<ScaleAnimation>().duration = timebombspeed;
	}

	// ------------------------------------------------------------------------

	void FixedUpdate() {
		if(!Enable) {
			GetComponent<ScaleAnimation>().pause();
			return;
		}

		GetComponent<ScaleAnimation>().run();

		if(timebombspeed > 0) {
			timebombspeed -= Time.fixedDeltaTime;
		} else {
			explode();
		}

	}

	// ------------------------------------------------------------------------

	void explode() {
		Vector3 pos = transform.position + Vector3.up * 0.25f;

		if(Debug.isDebugBuild) {
			Debug.DrawRay(pos, transform.forward * 1.5f, Color.yellow, 0.1f);
			Debug.DrawRay(pos, -transform.forward * 1.5f, Color.red, 0.1f);
			Debug.DrawRay(pos, transform.right * 1.5f, Color.green, 0.1f);
			Debug.DrawRay(pos, -transform.right * 1.5f, Color.blue, 0.1f);
		}

		RaycastHit hit;
		if(Physics.Raycast(pos, transform.forward, out hit, 1.49f)) {
			hits[0] = hit.collider.gameObject;
		} else {
			hits[0] = null;
		}
		if(Physics.Raycast(pos, -transform.forward, out hit, 1.49f)) {
			hits[1] = hit.collider.gameObject;
		} else {
			hits[1] = null;
		}
		if(Physics.Raycast(pos, transform.right, out hit, 1.49f)) {
			hits[2] = hit.collider.gameObject;
		} else {
			hits[2] = null;
		}
		if(Physics.Raycast(pos, -transform.right, out hit, 1.49f)) {
			hits[3] = hit.collider.gameObject;
		} else {
			hits[3] = null;
		}

		foreach(GameObject g in hits) {
			if(g) {
				DestroyActor destroy = g.GetComponent<DestroyActor>();
				if(!destroy) {
					destroy = g.AddComponent<DestroyActor>() as DestroyActor;
				}
				destroy.destroy();
			}
		}
		GetComponent<DestroyActor>().destroy();
	}

	// ------------------------------------------------------------------------

	public override bool zapped(GameObject by) {
		return false;
	}
}