using UnityEngine;
using System.Collections;

public class Klondike : Actor {
	private bool isGrabbed = false;
	private float startTime;
	private Vector3 endScale = new Vector3(0,0,0);

	// ------------------------------------------------------------------------

	void Update() {
		if(isGrabbed) {
			float scale = Vector3.Distance(transform.localScale, endScale);
			if(scale > 0) {
				float scaleCovered = (Time.time - startTime) * 0.05f;
				float frac = scaleCovered / scale;
				transform.localScale = Vector3.Lerp(transform.localScale, endScale, frac);
			} else {
				gameObject.SetActive(false);
				GameObject.Find("Level").GetComponent<Conditions>().pickupKlondike();
			}
		}
	}

	// ------------------------------------------------------------------------

	public override void grabbed(GameObject by) {
		startTime = Time.time;
		isGrabbed = true;
	}
}