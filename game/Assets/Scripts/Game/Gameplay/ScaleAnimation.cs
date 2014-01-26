using UnityEngine;
using System.Collections;

public class ScaleAnimation : MonoBehaviour {
	private bool startAnim = false;
	private float startTime;
	public Vector3 endScale = new Vector3(0,0,0);
	public float duration = 1;
	private float speed;

	// ------------------------------------------------------------------------

	void Update() {
		if(startAnim && scale() > 0) {
			float scaleCovered = Time.deltaTime * speed;//(Time.time - startTime) * speed;
			float frac = scaleCovered / scale();
			transform.localScale = Vector3.Lerp(transform.localScale, endScale, frac);
		}
		if(startAnim && scale() <= 0) {
			startAnim = false;
			Debug.Log(gameObject.name + ": Scale animation end at " + Time.time);
		}
	}

	// ------------------------------------------------------------------------

	float calculateSpeed() {
		return scale() / duration;
	}

	// ------------------------------------------------------------------------

	public void run() {
		if(startAnim)
			return;
		startTime = Time.time;
		startAnim = true;
		speed = calculateSpeed();
		Debug.Log(gameObject.name + ": Scale animation start at " + Time.time + " with speed " + speed);
	}

	// ------------------------------------------------------------------------

	public float scale() {
		return Vector3.Distance(transform.localScale, endScale);
	}
}