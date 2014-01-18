using UnityEngine;
using System.Collections;

public class Lift : MonoBehaviour {
	public bool isUp = false;

	private float speed = 0.7f;
	private float startTime;

	private Vector3 offsetVector = new Vector3(0, 1.2f, 0);
	private Vector3 endScale;
	private Vector3 startScale;

	private GameObject element;

	// ------------------------------------------------------------------------

	void Start() {
		startScale = endScale = transform.localScale;
	}

	// ------------------------------------------------------------------------

	void Update () {
		if(isPlaying()) {
			startTime += Time.deltaTime * speed;
			transform.localScale = Vector3.Lerp(startScale, endScale, startTime);
			getCarriedElement();
			updateElementPosition();
		} else {
			element = null;
		}
	}

	// ------------------------------------------------------------------------

	private void updateElementPosition() {
		RaycastHit hit;
		Debug.DrawRay (transform.position + offsetVector, Vector3.down * 1.5f, Color.red);
		if(element && Physics.Raycast(transform.position + offsetVector, Vector3.down, out hit, 1.5f, 1 << 8)) {
			try {
				element.GetComponentInChildren<MoveActor>().set(hit.point);
			} catch(System.NullReferenceException e) {
				Debug.Log("update element position throws null reference " + e);
			}
		}
	}

	// ------------------------------------------------------------------------

	public void up() {
		if(isUp || isPlaying())
			return;
		endScale = new Vector3(1, 34.33f, 1);
		isUp = !isUp;
		startTime = 0;
		startScale = transform.localScale;
	}

	// ------------------------------------------------------------------------

	public void down() {
		if(!isUp || isPlaying())
			return;
		endScale = new Vector3(1, 1, 1);
		isUp = !isUp;
		startTime = 0;
		startScale = transform.localScale;
	}

	// ------------------------------------------------------------------------

	private void getCarriedElement() {
		RaycastHit hit;
//		Debug.DrawRay(transform.position, Vector3.up, Color.green, 1f);
		if(Physics.Raycast(transform.position, Vector3.up, out hit, 1.5f)) {
//			Debug.Log("Lift element: " + hit.collider.gameObject.name + " " + hit.collider.tag);
			element = hit.collider.gameObject;
		} else {
			element = null;
		}
	}

	// ------------------------------------------------------------------------

	public bool isPlaying() {
		return endScale != transform.localScale;
	}
}
