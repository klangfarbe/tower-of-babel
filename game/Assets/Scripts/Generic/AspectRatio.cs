using UnityEngine;
using System;

public class AspectRatio {
	public enum Aspect { AR_UNKNOWN, AR_3_2, AR_4_3, AR_16_9, AR_16_10 }

	public float sWidth = 1024f;
	public float sHeight = 768f;
	public float xFactor = 1;
	public float yFactor = 1;

	public AspectRatio() {
		calculateAspectRatio();
	}

	// ------------------------------------------------------------------------

	public void initGuiScale() {
		calculateAspectRatio();
		GUIUtility.ScaleAroundPivot(new Vector2(xFactor, yFactor), Vector2.zero);
	}

	// ------------------------------------------------------------------------

	public Aspect calculateAspectRatio () {
		Aspect a = Aspect.AR_UNKNOWN;

		var aspect = (float)Screen.width / (float)Screen.height;
		if(aspect >= 1.7f && aspect < 1.78f) { // 16:9
			sWidth = 1920;
			sHeight = 1080;
			a = Aspect.AR_16_9;
		} else if(aspect >= 1.25f && aspect < 1.35f) { // 4:3
			sWidth = 2048;
			sHeight = 1536;
			a = Aspect.AR_4_3;
		} else if(aspect >= 1.55f && aspect < 1.65f) { // 16:10
			sWidth = 1920;
			sHeight = 1200;
			a = Aspect.AR_16_10;
		} else if(aspect >= 1.45f && aspect < 1.53f) { // 3:2
			sWidth = 1280;
			sHeight = 854;
			a = Aspect.AR_3_2;
		}

		xFactor = Screen.width / sWidth;
		yFactor = Screen.height / sHeight;

		return a;
	}
}