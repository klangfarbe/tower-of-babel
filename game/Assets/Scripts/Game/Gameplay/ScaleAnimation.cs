using UnityEngine;
using System.Collections;

public class ScaleAnimation : MonoBehaviour {
	private bool startAnim = false;
	private float startTime;
	public Vector3 endScale = new Vector3(0,0,0);

	// ------------------------------------------------------------------------

	void Update() {
		if(startAnim && scale() > 0) {
			float scaleCovered = (Time.time - startTime) * 0.05f;
			float frac = scaleCovered / scale();
			transform.localScale = Vector3.Lerp(transform.localScale, endScale, frac);
		}
	}

	// ------------------------------------------------------------------------

	public void run() {
		if(startAnim)
			return;
		startTime = Time.time;
		startAnim = true;
	}

	// ------------------------------------------------------------------------

	public float scale() {
		return Vector3.Distance(transform.localScale, endScale);
	}
}