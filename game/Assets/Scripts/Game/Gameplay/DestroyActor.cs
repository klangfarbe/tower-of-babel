using UnityEngine;
using System.Collections;

public class DestroyActor : MonoBehaviour {
	public bool isWinningCondition = true;

	public void destroy() {
		gameObject.SetActive(false);
		if(isWinningCondition)
			GameObject.Find("Level").GetComponent<Conditions>().destroyRobot();
	}
}