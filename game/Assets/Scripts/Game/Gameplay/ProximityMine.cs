using UnityEngine;
using System.Collections;

public class ProximityMine : Actor {
	private GameObject[] hits = new GameObject[4];
	private bool triggered = false;

	// ------------------------------------------------------------------------

	void FixedUpdate() {
		if(!Enable)
			return;

		Vector3 pos = transform.position + Vector3.up * 0.25f;

		Debug.DrawRay(pos, transform.forward * 1.5f, Color.yellow, 0.1f);
		Debug.DrawRay(pos, -transform.forward * 1.5f, Color.red, 0.1f);
		Debug.DrawRay(pos, transform.right * 1.5f, Color.green, 0.1f);
		Debug.DrawRay(pos, -transform.right * 1.5f, Color.blue, 0.1f);

		RaycastHit hit;
		if(Physics.Raycast(pos, transform.forward, out hit, 1.49f)) {
			hits[0] = hit.collider.gameObject;
			checkToTriggerMine(hit.collider.gameObject);
		} else {
			hits[0] = null;
		}
		if(Physics.Raycast(pos, -transform.forward, out hit, 1.49f)) {
			hits[1] = hit.collider.gameObject;
			checkToTriggerMine(hit.collider.gameObject);
		} else {
			hits[1] = null;
		}
		if(Physics.Raycast(pos, transform.right, out hit, 1.49f)) {
			hits[2] = hit.collider.gameObject;
			checkToTriggerMine(hit.collider.gameObject);
		} else {
			hits[2] = null;
		}
		if(Physics.Raycast(pos, -transform.right, out hit, 1.49f)) {
			hits[3] = hit.collider.gameObject;
			checkToTriggerMine(hit.collider.gameObject);
		} else {
			hits[3] = null;
		}

		if(!triggered)
			return;

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
		if(GameObject.Find("Level").GetComponent<LevelLoader>().Destroysfloor) {
			Physics.Raycast(pos, -transform.up, out hit, 0.3f);
			Destroy(hit.collider.gameObject);
		}
	}

	// ------------------------------------------------------------------------

	private void checkToTriggerMine(GameObject g) {
		MoveActor actor = g.GetComponent<MoveActor>();
		if(actor && actor.Walking) {
			triggered = true;
		}
	}

	// ------------------------------------------------------------------------

	public override bool zapped(GameObject by) {
		return false;
	}
}