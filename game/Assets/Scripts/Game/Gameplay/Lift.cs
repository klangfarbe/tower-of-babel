using UnityEngine;
using System.Collections;

public class Lift : MonoBehaviour {
	public bool isUp = false;

	private float speed = 0.6f;
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
//			getCarriedElement();
			updateElementPosition();
		}
	}

	// ------------------------------------------------------------------------

	private void updateElementPosition() {
		RaycastHit hit;
		//Debug.DrawRay (transform.position + offsetVector, Vector3.down * 1.5f, Color.red);
		if(element && Physics.Raycast(transform.position + offsetVector, Vector3.down, out hit, 1.5f, 1 << 8)) {
			//Debug.Log("Element: " + element.name + " / " + hit.collider.gameObject.name);
			MoveActor actor = element.GetComponentInChildren<MoveActor>();
			if(actor) {
				actor.set(hit.point);
			}
		}
	}

	// ------------------------------------------------------------------------

	public void up() {
		if(isUp || isPlaying())
			return;
		getCarriedElement();
		endScale = new Vector3(1, 34.33f, 1);
		isUp = !isUp;
		startTime = 0;
		startScale = transform.localScale;
		gameObject.GetComponent<AudioSource>().Play();
	}

	// ------------------------------------------------------------------------

	public void down() {
		if(!isUp || isPlaying())
			return;
		getCarriedElement();
		endScale = new Vector3(1, 1, 1);
		isUp = !isUp;
		startTime = 0;
		startScale = transform.localScale;
		gameObject.GetComponent<AudioSource>().Play();
	}

	// ------------------------------------------------------------------------

	public bool getCarriedElement() {
		element = null;
		RaycastHit hit;
		// Debug.DrawRay(transform.position, Vector3.up, Color.green, 1f);
		Debug.DrawRay(transform.position + new Vector3(-0.49f, 0, 0), Vector3.up, Color.red, 1f);
		Debug.DrawRay(transform.position + new Vector3(0.49f, 0, 0), Vector3.up, Color.yellow, 1f);
		Debug.DrawRay(transform.position + new Vector3(0, 0, -0.49f), Vector3.up, Color.blue, 1f);
		Debug.DrawRay(transform.position + new Vector3(0, 0, 0.49f), Vector3.up, Color.green, 1f);
		if(
			Physics.Raycast(transform.position, Vector3.up, out hit, 1.5f)
			|| Physics.Raycast(transform.position + new Vector3(-0.49f, 0, 0), Vector3.up, out hit, 1.5f)
			|| Physics.Raycast(transform.position + new Vector3(0.49f, 0, 0), Vector3.up, out hit, 1.5f)
			|| Physics.Raycast(transform.position + new Vector3(0, 0, -0.49f), Vector3.up, out hit, 1.5f)
			|| Physics.Raycast(transform.position + new Vector3(0, 0, 0.49f), Vector3.up, out hit, 1.5f)
			) {
//			Debug.Log("Lift element: " + hit.collider.gameObject.name + " " + hit.collider.tag);
			if(hit.collider.tag == "Actor" || hit.collider.tag == "Player")
				element = hit.collider.gameObject;
			return true;
		}
		return false;
	}

	// ------------------------------------------------------------------------

	public bool isPlaying() {
		return endScale != transform.localScale;
	}
}
