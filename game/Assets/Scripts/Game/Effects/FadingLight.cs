using UnityEngine;
using System.Collections;

public class FadingLight : MonoBehaviour {
	private float fadeSpeed = 2f;			// How fast the light fades between intensities.
	private float highIntensity = 4f;		// The maximum intensity of the light whilst the alarm is on.
	private float lowIntensity = 0.0f;	   // The minimum intensity of the light whilst the alarm is on.
	private float changeMargin = 0.1f;	   // The margin within which the target intensity is changed.
	public bool fadeOn;					 // Whether or not the alarm is on.

	private float targetIntensity;		  // The intensity that the light is aiming for currently.

	// ------------------------------------------------------------------------

	void Awake () {
		light.intensity = 0f;
		targetIntensity = highIntensity;
	}

    // ------------------------------------------------------------------------

    void Update () {
        if(fadeOn) {
            light.intensity = Mathf.Lerp(light.intensity, targetIntensity, fadeSpeed * Time.deltaTime);
    		if(Mathf.Abs(targetIntensity - light.intensity) < changeMargin) {
                targetIntensity = targetIntensity == highIntensity ? lowIntensity : highIntensity;
    		}
        } else
            light.intensity = Mathf.Lerp(light.intensity, 0f, fadeSpeed * Time.deltaTime);
    }
}