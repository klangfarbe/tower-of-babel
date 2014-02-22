using UnityEngine;
using System.Collections;
using System;

public class GameSettings : MonoBehaviour {
	public static GameSettings instance;
	public string version;

	private string nextScene;

	// ------------------------------------------------------------------------

	void Awake() {
		if(!instance) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(this);
		}
	}

	// ------------------------------------------------------------------------

	void Start() {
		version = ((TextAsset)Resources.Load("version")).text;
		Debug.Log("Game version " + version);

	#if UNITY_STANDALONE || UNITY_ANDROID || UNITY_MOBILE
		checkUpdate();
	#endif
	}

	// ------------------------------------------------------------------------

#if UNITY_STANDALONE || UNITY_ANDROID || UNITY_MOBILE
	public void checkUpdate() {
	//	if(Application.internetReachability) {

	//	}
		WWW www = null;
		try {
			www = new WWW("http://tob.guzumi.de/version.txt");
			while(!www.isDone)
				new WaitForSeconds(0.1f);
		} catch(System.Exception e) {
			Debug.LogException(e);
		}
		Debug.Log(www.text + " / " + www.error);
		if(www.error == "" && www.text != version) {
			//gui.notify("New version " + www.text);
			StartCoroutine(wwwcall());
		} else {
		//	gui.notify("");

		}
	}

	private IEnumerator wwwcall() {
		yield return new WaitForSeconds(3);
	//	gui.notify("");
	}
#endif
}