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
	public Texture texPause;

	private CameraController cameraController;
	private SpiderCamera spider;

	private float sWidth = 1024f;
	private float sHeight = 768f;

	private GUIStyle btnStyle = new GUIStyle();

	// ------------------------------------------------------------------------

	void OnGUI() {
		calculateAspectRatio();

		float xFactor = Screen.width / sWidth;
		float yFactor = Screen.height / sHeight;
		GUIUtility.ScaleAroundPivot(new Vector2(xFactor, yFactor), Vector2.zero);

	#if UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE
		mobileGUI();
	#else
	//	mobileGUI();
//		standaloneGUI();
	#endif
	}

	// ------------------------------------------------------------------------

	void standaloneGUI() {
		GUILayout.BeginArea(new Rect(sWidth / 2 - 305, sHeight - 80, 610, 70));
		GUILayout.BeginHorizontal();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
	}

	// ------------------------------------------------------------------------

	void mobileGUI() {
		btnStyle.fixedHeight = 96;
		btnStyle.fixedWidth = 96;
		btnStyle.margin.bottom = 10;
		btnStyle.margin.right = 10;

		float btnFullWidth = btnStyle.fixedWidth + btnStyle.margin.right;
		float btnFullHeight = btnStyle.fixedHeight + btnStyle.margin.bottom;

		// Overview buttons on the left side of the screen
		GUILayout.BeginArea(new Rect(10, sHeight - btnFullHeight * 4, btnFullWidth, btnFullHeight * 4));
		GUILayout.BeginVertical();
        if(GUILayout.Button(texMap, btnStyle)) {
        	cameraController.activateOverview();
        }
        if(GUILayout.Button(texGrabber, btnStyle)) {
        	cameraController.activateGrabber();
        }
        if(GUILayout.Button(texPusher, btnStyle)) {
        	cameraController.activatePusher();
        }
        if(GUILayout.Button(texZapper, btnStyle)) {
        	cameraController.activateZapper();
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();

		// Overview buttons on the left side of the screen
		GUILayout.BeginArea(new Rect(sWidth - btnFullWidth * 3, sHeight - btnFullHeight * 2, btnFullWidth * 3, btnFullHeight * 2));
		GUILayout.BeginHorizontal();
        if(GUILayout.Button(texFire, btnStyle) && spider.Target) {
			Actor actor = spider.Target.GetComponent<Actor>();
			actor.fire();
        }
        if(GUILayout.Button(texUpDown, btnStyle) && spider.Target) {
			Actor actor = spider.Target.GetComponent<Actor>();
			actor.lift();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if(GUILayout.Button(texLeft, btnStyle) && spider.Target) {
			Actor actor = spider.Target.GetComponent<Actor>();
			actor.turnLeft();
        }
        if(GUILayout.Button(texForward, btnStyle) && spider.Target) {
			Actor actor = spider.Target.GetComponent<Actor>();
			actor.move(spider.Target.transform.forward);
        }
        if(GUILayout.Button(texRight, btnStyle) && spider.Target) {
			Actor actor = spider.Target.GetComponent<Actor>();
			actor.turnRight();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        // Pause button
        if(GUI.Button(new Rect(sWidth - btnFullWidth, 10, btnFullWidth, btnFullHeight), texPause, btnStyle)) {
			StartCoroutine(GameObject.Find("Level").GetComponent<Conditions>().levelFailed());
        }
	}

	// ------------------------------------------------------------------------

	void Start() {
		cameraController = GameObject.Find("Cameras").GetComponent<CameraController>();
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
