using UnityEngine;
using System.Collections;

public class SceneFader : MonoBehaviour {
	public Texture fadeTexture;
	private float fadeSpeed = 0.25f;
	private float fadeDir = -1;
	private bool fadeIn = true;
	private bool fadeOut = false;
	private Color blendColor;

	private string nextScene;

	// ------------------------------------------------------------------------

	void Awake() {
		blendColor = GUI.color;
		blendColor.a = 1f;
	}

	// ------------------------------------------------------------------------

	void OnGUI() {
		if(fadeIn || fadeOut) {
			blend();
			if(fadeIn && !Blending) {
    			fadeIn = false;
			}
			if(fadeOut && !Blending && nextScene != null) {
				//Debug.Log(blendColor);
				Debug.Log(Blending + " Loading level " + nextScene);
				Application.LoadLevel(nextScene);
				nextScene = null;
			}
    	}
	}

	// ------------------------------------------------------------------------

	void blend() {
		blendColor.a += fadeDir * Time.deltaTime * fadeSpeed;
    	blendColor.a = Mathf.Clamp01(blendColor.a);
		GUI.color = blendColor;
		GUI.depth = -1000;
    	GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
	}

	// ------------------------------------------------------------------------

	public void startScene() {
		fadeDir = -1;
		fadeIn = true;
		fadeOut = false;
	}

	// ------------------------------------------------------------------------

	public void endScene() {
		if(!fadeOut && !fadeIn && !Blending) {
			fadeDir = 1;
			blendColor.a = 0.01f;
			fadeOut = true;
		}
	}

	// ------------------------------------------------------------------------

	public void loadScene(string name) {
		endScene();
		nextScene = name;
	}

	// ------------------------------------------------------------------------

	public bool Blending {
		get {
			return (blendColor.a > 0f && blendColor.a < 1f);
		}
	}

	// ------------------------------------------------------------------------

	public static SceneFader create() {
		GameObject prefab = (GameObject)Resources.Load("SceneFader");
		GameObject instance = (GameObject)Instantiate(prefab, Vector3.zero, prefab.transform.rotation);
		return instance.GetComponent<SceneFader>();
	}
}
