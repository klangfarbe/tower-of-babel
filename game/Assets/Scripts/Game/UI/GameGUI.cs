using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameGUI : BaseUIController {
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

	public Font font;

	public Texture2D textWndBackground;

	private bool drawPauseMenu = false;

	private GUIStyle btnStyle = new GUIStyle();
	private GUIStyle wndStyle = new GUIStyle();
	private GUIStyle wndBtnStyle = new GUIStyle();
	private GUIStyle cndTextStyle = new GUIStyle();
	private GUIStyle ntyTextStyle = new GUIStyle();

	public class Notification {
		public string msg;
		public float duration;

		public Notification(string msg, float duration) {
			this.msg = msg;
			this.duration = duration;
		}
	}
	private Queue<Notification> notifications = new Queue<Notification>();
	private Notification currentNotification = null;

	// ------------------------------------------------------------------------

	void Awake() {
		setupGUIStyles();
	}

	// ------------------------------------------------------------------------

	void OnGUI() {
		if(ar == null) {
			ar = new AspectRatio();
		}
		ar.initGuiScale();
		drawNotification();
		mobileGUI();
	}

	// ------------------------------------------------------------------------

	private void setupGUIStyles() {
		wndStyle.normal.background = textWndBackground;
		wndStyle.border = new RectOffset(25,25,25,25);
		wndStyle.padding = new RectOffset(25,25,25,25);

		btnStyle.fixedHeight = 128;
		btnStyle.fixedWidth = 128;
		btnStyle.margin.bottom = 10;
		btnStyle.margin.right = 10;

		wndBtnStyle.fontSize = 64;
		wndBtnStyle.font = font;
		wndBtnStyle.normal.textColor = Color.white;
		wndBtnStyle.alignment = TextAnchor.UpperCenter;
		wndBtnStyle.margin = new RectOffset(25,25,25,25);

		cndTextStyle.fontSize = 36;
		cndTextStyle.font = font;
		cndTextStyle.normal.textColor = Color.white;
		cndTextStyle.alignment = TextAnchor.UpperLeft;

		ntyTextStyle.fontSize = 64;
		ntyTextStyle.font = font;
		ntyTextStyle.normal.textColor = Color.white;
		ntyTextStyle.alignment = TextAnchor.UpperCenter;

	}

	// ------------------------------------------------------------------------

	void mobileGUI() {
		float btnFullWidth = btnStyle.fixedWidth + btnStyle.margin.right;
		float btnFullHeight = btnStyle.fixedHeight + btnStyle.margin.bottom;

		// Overview buttons on the left side of the screen
		GUILayout.BeginArea(new Rect(10, ar.sHeight - btnFullHeight * 4, btnFullWidth, btnFullHeight * 4));
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

		// Overview buttons on the right side of the screen
		if(!cameraController.mapActive) {
			GUILayout.BeginArea(new Rect(ar.sWidth - btnFullWidth * 3, ar.sHeight - btnFullHeight * 2, btnFullWidth * 3, btnFullHeight * 2));
			GUILayout.BeginHorizontal();
			if(GUILayout.Button(texFire, btnStyle)) {
				gameController.actorFire();
			}
			if(GUILayout.Button(texForward, btnStyle)) {
				gameController.actorForward();
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			if(GUILayout.Button(texLeft, btnStyle)) {
				gameController.actorLeft();
			}
			if(GUILayout.Button(texUpDown, btnStyle)) {
				gameController.actorLift();
			}
			if(GUILayout.Button(texRight, btnStyle)) {
				gameController.actorRight();
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		// Pause button
		if(GUI.Button(new Rect(ar.sWidth - btnFullWidth, 10, btnFullWidth, btnFullHeight), texPause, btnStyle)) {
			drawPauseMenu = true;
		}

		// conditions
		GUI.Label(new Rect(10, 10, 150, 50), conditions.getConditionsText(), cndTextStyle);

		// remaining Time
		GUI.Label(new Rect(ar.sWidth / 2 - 100, 10, 200, 150), conditions.getRemainingTime(), wndBtnStyle);

		if(drawPauseMenu) {
			GUI.ModalWindow(0, new Rect(ar.sWidth / 2 - 480, ar.sHeight / 2 - 210, 960, 420), pauseMenu, "", wndStyle);
		}
	}

	// ------------------------------------------------------------------------

	void pauseMenu(int windowId) {
		gameController.levelPause();

		GUILayout.BeginVertical(new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true) });
		if(GUILayout.Button("Restart Level", wndBtnStyle, GUILayout.ExpandWidth(true))) {
			drawPauseMenu = false;
			gameController.levelUnpause();
			StartCoroutine(gameController.levelRestart());
		}
		if(GUILayout.Button("Previous Level", wndBtnStyle, GUILayout.ExpandWidth(true))) {
			GameObject.Find("Level").GetComponent<LevelLoader>().prev();
		}
		if(GUILayout.Button("Next Level", wndBtnStyle, GUILayout.ExpandWidth(true))) {
			GameObject.Find("Level").GetComponent<LevelLoader>().next();
		}
		if(GUILayout.Button("Resume Game", wndBtnStyle, GUILayout.ExpandWidth(true))) {
			drawPauseMenu = false;
			gameController.levelUnpause();
		}
		GUILayout.EndVertical();
//
//		if(GUILayout.Button("Check for update", cndTextStyle, GUILayout.ExpandWidth(true))) {
//			drawPauseMenu = false;
//  			gameController.checkUpdate();
//		}
	}

	// ------------------------------------------------------------------------

	public void notify(string message, float duration) {
		notifications.Enqueue(new Notification(message, duration));
		StartCoroutine(fadeNotificationText());
	}

	// ------------------------------------------------------------------------

	public void drawNotification() {
		if(currentNotification != null) {
			GUI.Label(new Rect(ar.sWidth / 2 - 300, ar.sHeight / 2 - 200, 600, 400), currentNotification.msg, ntyTextStyle);
		} else if (notifications.Count > 0) {
			currentNotification = notifications.Dequeue();
			Color c = ntyTextStyle.normal.textColor;
			c.a = 0;
			ntyTextStyle.normal.textColor = c;
			Debug.Log("New notification " + currentNotification.msg + " / " + c + " / " + notifications.Count);
			StartCoroutine(fadeNotificationText());
		}
	}

	// ------------------------------------------------------------------------

	public IEnumerator fadeNotificationText() {
		if(currentNotification != null) {
			Color c = ntyTextStyle.normal.textColor;
			Debug.Log("Fade in text");
			yield return null;
			while(ntyTextStyle.normal.textColor.a < 1) {
				c.a += 1.5f * Time.deltaTime;
				c.a = Mathf.Clamp01(c.a);
				ntyTextStyle.normal.textColor = c;
				yield return null;
			}

			yield return new WaitForSeconds(currentNotification.duration);

			Debug.Log("fade out text");
			yield return null;
			while(ntyTextStyle.normal.textColor.a > 0) {
				c.a -= 1.5f * Time.deltaTime;
				c.a = Mathf.Clamp01(c.a);
				ntyTextStyle.normal.textColor = c;
				yield return null;
			}
			currentNotification = null;
		}
	}

	public void clearNotifications() {
		notifications.Clear();
	}
}
