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
	}
}