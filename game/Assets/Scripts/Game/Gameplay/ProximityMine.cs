using UnityEngine;
using System.Collections;

public class ProximityMine : Actor {
	private GameObject[] hits = new GameObject[4];

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
		if(Physics.Raycast(pos, transform.forward, out hit, 1.5f)) {
			hits[0] = hit.collider.gameObject;
		} else {
			hits[0] = null;
		}
		if(Physics.Raycast(pos, -transform.forward, out hit, 1.5f)) {
			hits[1] = hit.collider.gameObject;
		} else {
			hits[1] = null;
		}
		if(Physics.Raycast(pos, transform.right, out hit, 1.5f)) {
			hits[2] = hit.collider.gameObject;
		} else {
			hits[2] = null;
		}
		if(Physics.Raycast(pos, -transform.right, out hit, 1.5f)) {
			hits[3] = hit.collider.gameObject;
		} else {
			hits[3] = null;
		}

		bool triggered = false;
		foreach(GameObject g in hits) {
			if(g) {
				Debug.Log(g.name);
				DestroyActor destroy = g.GetComponent<DestroyActor>();
				if(destroy) {
					destroy.destroy();
					triggered = true;
				}
			}
		}
		if(triggered) {
			GetComponent<DestroyActor>().destroy();
			if(GameObject.Find("Level").GetComponent<Behaviour>().destroysfloor) {
				Physics.Raycast(pos, -transform.up, out hit, 0.3f);
				hit.collider.gameObject.SetActive(false);
			}
		}
	}

	// ------------------------------------------------------------------------

	public override void zapped(GameObject by) {
	}
}