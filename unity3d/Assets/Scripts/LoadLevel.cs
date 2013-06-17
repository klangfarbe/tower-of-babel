using UnityEngine;
using System.Collections.Generic;

using LitJson;
using System;
using System.IO;

public class LoadLevel : MonoBehaviour {
	public int level = 0;
	public float floorOffset = 0.0f;
	public GameObject floorPattern1;
	public GameObject floorPattern2;
	public GameObject boxPattern1;
	public GameObject boxPattern2;
	public GameObject liftDown;
	public GameObject liftUp;
	public Material materialPattern1;
	public Material materialPattern2;


	private JsonData levelData;
	private int maxWidth = 0;
	private int maxLength = 0;
	private int maxHeight = 0;

	private List<UnityEngine.Object> objects = new List<UnityEngine.Object>();

	// ------------------------------------------------------------------------

	void Start() {
		Debug.Log("Awakening scene...");
		build();
	}

	// ------------------------------------------------------------------------

	void Update() {
		if(Input.GetKeyUp(KeyCode.LeftArrow)) {
			if(level > 0) {
				level--;
				build();
			}
		}
		if(Input.GetKeyUp(KeyCode.RightArrow)) {
			if(level < 117) {
				level++;
				build();
			}
		}
	}

	// ------------------------------------------------------------------------

	public void build() {
		Debug.Log("Building level...");

		loadResource();
		setMaterialColors();
		clearScene();

		// create Objects
		for (int y = 0; y < 4; y++) {
			for (int x = 0; x < 8; x++) {
				for (int z = 0; z < 8; z++) {
					buildElement (x, y, z);
				}
			}
		}
		calculateLevelCenter();
	}

	// ------------------------------------------------------------------------

	void clearScene() {
		foreach(UnityEngine.Object o in objects) {
			Destroy(o);
		}
		objects.Clear();
		maxHeight = 0;
		maxWidth = 0;
		maxLength = 0;
		levelData = null;
	}

	// ------------------------------------------------------------------------
	
	void loadResource() {
		Debug.Log("Loading level data...");
		TextAsset json = Resources.Load("Level/" + string.Format("{0:d3}", level)) as TextAsset;
		levelData = JsonMapper.ToObject(json.text);
	}

	// ------------------------------------------------------------------------

	void setMaterialColors() {
		materialPattern1.color = hexToColor(levelData["fx"]["patterncolor1"].ToString().Substring(2));
		materialPattern2.color = hexToColor(levelData["fx"]["patterncolor2"].ToString().Substring(2));
		Debug.Log(materialPattern1.color.ToString());
		Debug.Log(materialPattern2.color.ToString());
	}

	// ------------------------------------------------------------------------

	Color hexToColor(string hex) {
		Debug.Log(hex);
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}

	// ------------------------------------------------------------------------

	void updateMaximumLevelDimensions(int x, int y, int z) {
		maxWidth = x > maxWidth ? x : maxWidth;
		maxHeight = y > maxHeight ? y : maxHeight;
		maxLength = z > maxLength ? z : maxLength;
	}

	// ------------------------------------------------------------------------

	void calculateLevelCenter() {

		Debug.Log(camera);
		float x = maxWidth / 2f;
		float y = maxHeight / 2f;
		float z = maxLength / 2f;
		Debug.Log("Position: " + y + ", field: " + x + ", " + z);

		// Transform the camera and lights
		GameObject.Find("MainCameraParent").transform.position = new Vector3(x, y, z);
		GameObject.Find("Lights").transform.position = new Vector3(x, maxHeight + 2, z);
	}

	// ------------------------------------------------------------------------
	
	void buildElement (int x, int y, int z) {
		try {
			JsonData row = levelData ["elements"] [y] [x];
			string f = row[z]["f"].ToString();

			UnityEngine.Object obj = null;

			if(f == "FLR" || f == "LFU") {
				obj = Instantiate (decideFloorObject(f, x, z), new Vector3 (x, y + floorOffset, z), Quaternion.identity);
			} else if(f == "BOX" || f == "LFD") {
				obj = Instantiate (decideFloorObject(f, x, z), new Vector3 (x, y + floorOffset + 0.5f, z), Quaternion.identity);
			}
			objects.Add(obj);
			updateMaximumLevelDimensions(x, y, z);
		} catch (Exception e) {
			// Ignore
		}
	}

	// ------------------------------------------------------------------------

	GameObject getObjectForPattern(string type, int pattern) {
		if(type == "FLR")
			return pattern == 1 ? floorPattern1 : floorPattern2;
		if(type == "BOX")
			return pattern == 1 ? boxPattern1 : boxPattern2;
		if(type == "LFD")
			return liftDown;
		if(type == "LFU")
			return liftUp;
		return null;
	}

	// ------------------------------------------------------------------------
	
	GameObject decideFloorObject (string f, int x, int z) {
		if (x % 2 == 0) {
			return (z % 2 == 0) ? getObjectForPattern(f, 1) : getObjectForPattern(f, 2);
		} else {
			return (z % 2 != 0) ? getObjectForPattern(f, 1) : getObjectForPattern(f, 2);
		}
	}
}
