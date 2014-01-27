using UnityEngine;
using System.Collections;

public interface ScaleAnimationCallback {
	void scaleAnimationFinished();
}

// ----------------------------------------------------------------------------

public class ScaleAnimation : MonoBehaviour {
	private bool startAnim = false;
	private float startTime;
	private float speed;

	private Vector3 originalScale;
	private Vector3 targetScale;

	public bool autoReverse = false;
	public float duration = 1;
	public Vector3 endScale = new Vector3(0,0,0);

	private ScaleAnimationCallback callback = null;

	// ------------------------------------------------------------------------

	void Start() {
		originalScale = transform.localScale;
	}

	// ------------------------------------------------------------------------

	void Update() {
		if(startAnim && scale() > 0) {
			float scaleCovered = Time.deltaTime * speed;
			float frac = scaleCovered / scale();
			transform.localScale = Vector3.Lerp(transform.localScale, targetScale, frac);
		}
		if(startAnim && scale() <= 0) {
			if(autoReverse && transform.localScale != originalScale) {
				targetScale = originalScale;
			} else {
				startAnim = false;
				if(callback != null)
					callback.scaleAnimationFinished();
	//			Debug.Log(gameObject.name + ": Scale animation end at " + Time.time);
			}
		}
	}

	// ------------------------------------------------------------------------

	float calculateSpeed() {
		return scale() / duration;
	}

	// ------------------------------------------------------------------------

	public void run(ScaleAnimationCallback callback) {
		this.callback = callback;
		run();
	}

	// ------------------------------------------------------------------------

	public void run() {
		if(startAnim)
			return;
		startTime = Time.time;
		startAnim = true;
		targetScale = endScale;
		speed = calculateSpeed();
	//	Debug.Log(gameObject.name + ": Scale animation start at " + Time.time + " with speed " + speed);
	}

	// ------------------------------------------------------------------------

	public float scale() {
		return Vector3.Distance(transform.localScale, targetScale);
	}

	// ------------------------------------------------------------------------

	public bool isPlaying() {
		return startAnim;
	}
}