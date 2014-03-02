using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUINotification : BaseUIController {
	public Font font;
	public Texture2D txtBackground;

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
		ntyTextStyle.fontSize = 64;
		ntyTextStyle.font = font;
		ntyTextStyle.normal.textColor = Color.white;
		ntyTextStyle.alignment = TextAnchor.MiddleCenter;
//		ntyTextStyle.normal.background = txtBackground;
//		ntyTextStyle.border = new RectOffset(3,3,3,3);
		ntyTextStyle.padding = new RectOffset(25,25,25,25);
	}

	// ------------------------------------------------------------------------

	void OnGUI() {
		if(ar == null) {
			ar = new AspectRatio();
		}
		ar.initGuiScale();
		drawNotification();
	}

	// ------------------------------------------------------------------------

	public void notify(string message, float duration) {
		notifications.Enqueue(new Notification(message, duration));
		StartCoroutine(fadeNotificationText());
	}

	// ------------------------------------------------------------------------

	public void drawNotification() {
		if(currentNotification != null) {
			GUI.depth = 1000;
			GUI.DrawTexture(new Rect(-10, ar.sHeight / 2 - 100, ar.sWidth + 20, 200), txtBackground, ScaleMode.ScaleToFit);
			GUI.Label(new Rect(-10, ar.sHeight / 2 - 100, ar.sWidth + 20, 200), currentNotification.msg, ntyTextStyle);
		} else if (notifications.Count > 0) {
//			if()
			currentNotification = notifications.Dequeue();
			Color c = ntyTextStyle.normal.textColor;
			c.a = 0;
			ntyTextStyle.normal.textColor = c;
			Debug.Log("New notification " + currentNotification.msg + " / " + c + " / " + notifications.Count);
			StartCoroutine(fadeNotificationText());
		} else {
//			StartCoroutine(fadeOutNotificationBox());
		}
	}

	// ------------------------------------------------------------------------

	public IEnumerator fadeNotificationText() {
		if(currentNotification != null) {
			Color c = ntyTextStyle.normal.textColor;
			Debug.Log("Fade in text");
			yield return null;
			while(ntyTextStyle.normal.textColor.a < 1) {
				c.a += 1.2f * Time.deltaTime;
				c.a = Mathf.Clamp01(c.a);
				ntyTextStyle.normal.textColor = c;
				yield return null;
			}

			yield return new WaitForSeconds(currentNotification.duration);

			Debug.Log("fade out text");
			yield return null;
			while(ntyTextStyle.normal.textColor.a > 0) {
				c.a -= 1.2f * Time.deltaTime;
				c.a = Mathf.Clamp01(c.a);
				ntyTextStyle.normal.textColor = c;
				yield return null;
			}
			currentNotification = null;
		}
	}

	// ------------------------------------------------------------------------

	public void clearNotifications() {
		notifications.Clear();
	}
}
