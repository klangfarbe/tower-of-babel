using UnityEngine;
using System.Collections;

public class GUIButtons : MonoBehaviour {
	public Texture texMap;
	public Texture texGrabber;
	public Texture texPusher;
	public Texture texZapper;
	public Texture texForward;
	public Texture texBack;
	public Texture texLeft;
	public Texture texRight;
	public Texture texUpDown;
	public Texture texFire;
	public GUISkin skin;

	private SwitchCamera cameraSelector;
	private SpiderCamera spider;

	private float sWidth = 1024f;
	private float sHeight = 768f;

	// ------------------------------------------------------------------------

	void OnGUI() {
		calculateAspectRatio();

		float xFactor = Screen.width / sWidth;
		float yFactor = Screen.height / sHeight;
		GUIUtility.ScaleAroundPivot(new Vector2(xFactor, yFactor), Vector2.zero);

		GUI.skin = skin;
		GUILayout.BeginArea(new Rect(sWidth / 2 - 305, sHeight - 80, 610, 70));
		GUILayout.BeginHorizontal();
        if(GUILayout.Button(texMap)) {
        	cameraSelector.activateOverview();
        }
        if(GUILayout.Button(texGrabber)) {
        	cameraSelector.activateGrabber();
        }
        if(GUILayout.Button(texPusher)) {
        	cameraSelector.activatePusher();
        }
        if(GUILayout.Button(texZapper)) {
        	cameraSelector.activateZapper();
        }
        if(GUILayout.Button(texFire) && spider.Target) {
			Actor actor = spider.Target.GetComponent<Actor>();
			actor.fire();
        }
        if(GUILayout.Button(texLeft) && spider.Target) {
			Actor actor = spider.Target.GetComponent<Actor>();
			actor.turnLeft();
			spider.startTime = 0;
        }
        if(GUILayout.Button(texForward) && spider.Target) {
			Actor actor = spider.Target.GetComponent<Actor>();
			actor.move(spider.Target.transform.forward);
        }
        if(GUILayout.Button(texRight) && spider.Target) {
			Actor actor = spider.Target.GetComponent<Actor>();
			actor.turnRight();
			spider.startTime = 0;
        }
        if(GUILayout.Button(texUpDown) && spider.Target) {
			Actor actor = spider.Target.GetComponent<Actor>();
			actor.lift();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
	}

	// ------------------------------------------------------------------------

	void Start() {
		cameraSelector = GameObject.Find("Cameras").GetComponent<SwitchCamera>();
		spider = GameObject.Find("SpiderCamera").GetComponent<SpiderCamera>();
	}

	// ------------------------------------------------------------------------

	void calculateAspectRatio () {
		// calculate aspect ratio
		var aspect = (float)Screen.width / (float)Screen.height;
		if(aspect >= 1.7f && aspect < 1.78f) {
			sWidth = 1280;
			sHeight = 720;
		} else if(aspect >= 1.3f && aspect < 1.34f) {
			sWidth = 1024;
			sHeight = 768;
		} else if(aspect >= 1.6f && aspect < 1.65f) {
			sWidth = 1280;
			sHeight = 800;
		}
		Debug.Log("Screen aspect ratio is " + aspect.ToString("F4") + " with size " + Screen.width + "x" + Screen.height);
	}
}
