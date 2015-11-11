using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUINotification : MonoBehaviour {
	public Font font;
	public Texture2D txtBackground;

	private AspectRatio ar;
	private Color color;
	private GUIStyle ntyTextStyle = new GUIStyle();

	public class Notification {
		public string msg;
		public float duration;
		public System.Guid id;

		public Notification(string msg, float duration, System.Guid id) {
			this.msg = msg;
			this.duration = duration;
			this.id = id;
		}
	}
	private Queue<Notification> notifications = new Queue<Notification>();
	private Notification currentNotification = null;

	// ------------------------------------------------------------------------

	void Awake() {
		ntyTextStyle.fontSize = 64;
		ntyTextStyle.font = font;
		ntyTextStyle.wordWrap = true;
		ntyTextStyle.normal.textColor = Color.white;
		ntyTextStyle.alignment = TextAnchor.MiddleCenter;
		ntyTextStyle.normal.background = txtBackground;
		ntyTextStyle.border = new RectOffset(3,3,3,3);
		ntyTextStyle.padding = new RectOffset(25,25,25,25);
		color = GUI.color;
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
		System.Guid id = System.Guid.NewGuid();
		notifications.Enqueue(new Notification(message, duration, id));
		StartCoroutine(fadeNotificationText(id));
	}

	// ------------------------------------------------------------------------

	public void drawNotification() {
		if(currentNotification != null) {
			GUI.depth = -1000;
			GUI.color = color;
			GUI.Label(new Rect(-10, ar.sHeight / 2 - 100, ar.sWidth + 20, 200), currentNotification.msg, ntyTextStyle);
		} else if (notifications.Count > 0) {
			currentNotification = notifications.Dequeue();
			color.a = 0;
			GUI.color = color;
			StartCoroutine(fadeNotificationText(currentNotification.id));
		}
	}

	// ------------------------------------------------------------------------

	public IEnumerator fadeNotificationText(System.Guid id) {
			color = GUI.color;
			yield return null;

			while(currentNotification != null && currentNotification.id == id && color.a < 1) {
				color.a += 1.2f * Time.deltaTime;
				color.a = Mathf.Clamp01(color.a);
				yield return null;
			}

			yield return new WaitForSeconds(currentNotification != null ? currentNotification.duration : 0);

			while(currentNotification != null && currentNotification.id == id && color.a > 0) {
				color.a -= 1.2f * Time.deltaTime;
				color.a = Mathf.Clamp01(color.a);
				yield return null;
			}

			if(currentNotification != null && currentNotification.id == id)
				currentNotification = null;
	}

	// ------------------------------------------------------------------------

	public void clearNotifications() {
		notifications.Clear();
		currentNotification = null;
		color.a = 0;
		GUI.color = color;
	}
}
