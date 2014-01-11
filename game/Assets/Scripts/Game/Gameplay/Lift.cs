using UnityEngine;
using System.Collections;

public class Lift : MonoBehaviour {
	public bool isUp = false;
	public float speed = 0.05f;
	private Vector3 offsetVector = new Vector3(0, 1.2f, 0);

	private float startTime;
	private Vector3 endScale;
	private GameObject element;

	// -----------------------------------------------------------------------------------------------------------------

	void Start() {
		endScale = transform.localScale;
	}

	// -----------------------------------------------------------------------------------------------------------------

	void Update () {
		if(isPlaying()) {
			Vector3 startScale = transform.localScale;
			float journeyLength = Vector3.Distance(startScale, endScale);
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			transform.localScale = Vector3.Lerp(startScale, endScale, fracJourney);
			updateElementPosition();
		} else {
			element = null;
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	private void updateElementPosition() {
		if(!element)
			return;
		RaycastHit hit;
		Debug.DrawRay (transform.position + offsetVector, Vector3.down * 1.5f, Color.red);
		if(Physics.Raycast(transform.position + offsetVector, Vector3.down, out hit, 1.5f, 1 << 8)) {
			element.GetComponentInChildren<MoveActor>().set(hit.point);
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void up() {
		if(isUp || isPlaying())
			return;
		endScale = new Vector3(1, 34.33f, 1);
		getCarriedElement();
		isUp = !isUp;
		startTime = Time.time;
	}

	// -----------------------------------------------------------------------------------------------------------------

	public void down() {
		if(!isUp || isPlaying())
			return;
		endScale = new Vector3(1, 1, 1);
		getCarriedElement();
		isUp = !isUp;
		startTime = Time.time;
	}

	// -----------------------------------------------------------------------------------------------------------------

	private void getCarriedElement() {
		RaycastHit hit;
//		Debug.DrawRay(transform.position, Vector3.up, Color.green, 1f);
		if(Physics.Raycast(transform.position, Vector3.up, out hit, 1.5f)) {
			Debug.Log("Lift element: " + hit.collider.gameObject.name + " " + hit.collider.tag);
			element = hit.collider.gameObject;
		}
	}

	// -----------------------------------------------------------------------------------------------------------------

	private bool isPlaying() {
		return endScale != transform.localScale;
	}
}
