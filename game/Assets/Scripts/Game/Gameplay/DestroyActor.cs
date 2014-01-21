using UnityEngine;
using System.Collections;

public class DestroyActor : MonoBehaviour {
	public bool isWinningCondition = true;

	// ------------------------------------------------------------------------

	public void destroy() {
		GameObject prefab = (GameObject) Resources.Load("Explosion");
		Instantiate(prefab, transform.position, prefab.transform.rotation);
		gameObject.SetActive(false);
		if(isWinningCondition)
			GameObject.Find("Level").GetComponent<Conditions>().destroyRobot();
		releaseFloor();
		Destroy(gameObject);
	}

	// ------------------------------------------------------------------------

	public void releaseFloor() {
		RaycastHit hit;
		Debug.DrawRay(transform.position +  Vector3.up * 0.25f, Vector3.down * 0.3f, Color.green, 0.5f);
		if(Physics.Raycast(transform.position + Vector3.up * 0.25f, Vector3.down, out hit, 0.3f))
			if(hit.collider.tag == "Floor" || hit.collider.tag == "Lift")
				hit.collider.gameObject.GetComponent<Floor>().release(gameObject);
	}
}