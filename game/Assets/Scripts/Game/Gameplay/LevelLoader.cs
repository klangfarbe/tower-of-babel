using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

// 3rd party
using LitJson;

public class LevelLoader : MonoBehaviour {

	// ------------------------------------------------------------------------
	// Attributes
	// ------------------------------------------------------------------------

	public int level = 0;
	public float floorOffset = -0.03f;

	public Material patterncolor1;
	public Material patterncolor2;
	public Material groundcolor;

	public GUIText levelName;
	public GUIText levelNr;

	public GameObject mainCamera;
	public GameObject spiderCamera;

	private JsonData levelData;
	private int maxColumns = 0;
	private int maxRows = 0;
	private int maxFloors = 0;

	// the current fields to build
	private int floor;
	private int row;
	private int column;

	private int instanceCounter = 0;

	private List<UnityEngine.Object> objects = new List<UnityEngine.Object>();

	// ------------------------------------------------------------------------
	// Methods
	// ------------------------------------------------------------------------

	void Start() {
		build();
	}

	// ------------------------------------------------------------------------

	void Update() {
		if(Input.GetKeyUp(KeyCode.LeftArrow)) {
			prev();
		}
		if(Input.GetKeyUp(KeyCode.RightArrow)) {
			next();
		}
	}

	// ------------------------------------------------------------------------

	public void next() {
		if(level < 116) {
			level++;
			build();
		}
	}

	// ------------------------------------------------------------------------

	public void prev() {
		if(level > 0) {
			level--;
			build();
		}
	}

	// ------------------------------------------------------------------------

	public void build() {
		Debug.Log("Building level...");
		clearScene();
		loadResource();
		setColors();

		// create Objects
		for (floor = 0; floor < 4; floor++) { // level
			for (row = 0; row < 8; row++) { //row
				for (column = 0; column < 8; column++) { // column
					buildPosition ();
				}
			}
		}
		calculateLevelCenter();
		setLevelName();
		activateCamera();

		Conditions conditions = GameObject.Find("Level").GetComponent<Conditions>();
		conditions.init((int)levelData["conditions"]["klondikes"],
						(int)levelData["conditions"]["robots"],
						(int)levelData["conditions"]["timelimit"]);

		Behaviour behaviour = GameObject.Find("Level").GetComponent<Behaviour>();
		behaviour.destroysfloor = (bool)levelData["behaviour"]["destroysfloor"];
		behaviour.cameras = (bool)levelData["behaviour"]["cameras"];
		behaviour.timebombspeed = (int)levelData["behaviour"]["timebombspeed"];

	}

	// ------------------------------------------------------------------------

	void activateCamera() {
		SwitchCamera camera = GameObject.Find("Cameras").GetComponent<SwitchCamera>();
		camera.activateGrabber();
	}

	// ------------------------------------------------------------------------

	void setColors() {
		patterncolor1.color = hexToColor(levelData["fx"]["patterncolor1"].ToString().Substring(2));
		patterncolor2.color = hexToColor(levelData["fx"]["patterncolor2"].ToString().Substring(2));
		groundcolor.color = hexToColor(levelData["fx"]["groundcolor1"].ToString().Substring(2));
		setSkycolor(mainCamera);
		setSkycolor(spiderCamera);
	}

	// ------------------------------------------------------------------------

	void setSkycolor(GameObject cam) {
		GradientBackground script = cam.GetComponent<GradientBackground>();
		script.CreateBackground(
			hexToColor(levelData["fx"]["skycolor1"].ToString().Substring(2)),
			hexToColor(levelData["fx"]["skycolor2"].ToString().Substring(2))
		);
	}

	// ------------------------------------------------------------------------

	void setLevelName() {
		levelName.text = levelData["title"].ToString();
		levelNr.text = "Level: " + (level + 1).ToString();
	}

	// ------------------------------------------------------------------------

	void clearScene() {
		foreach(UnityEngine.Object o in objects) {
			Debug.Log("Destroyed " + o);
			Destroy(o);
		}
		objects.Clear();
		maxFloors = 0;
		maxColumns = 0;
		maxRows = 0;
		levelData = null;
		instanceCounter = 0;
	}

	// ------------------------------------------------------------------------

	void loadResource() {
		Debug.Log("Loading level data...");
		TextAsset json = Resources.Load("Level/" + string.Format("{0:d3}", level)) as TextAsset;
		levelData = JsonMapper.ToObject(json.text);
	}

	// ------------------------------------------------------------------------

	Color hexToColor(string hex) {
		//Debug.Log(hex);
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r,g,b, 255);
	}

	// ------------------------------------------------------------------------

	void updateMaximumLevelDimensions() {
		maxFloors = floor > maxFloors ? floor : maxFloors;
		maxRows = row > maxRows ? row : maxRows;
		maxColumns = column > maxColumns ? column : maxColumns;
	}

	// ------------------------------------------------------------------------

	void calculateLevelCenter() {
		float x = maxColumns / 2f;
		float y = maxFloors / 2f;
		float z = maxRows / 2f;

		// Transform the camera and lights and scale the level border bounding box
		GameObject.Find("MainCameraParent").transform.position = new Vector3(x, y, z);
	}

	// ------------------------------------------------------------------------

	void buildPosition () {
		try {
			JsonData field = levelData ["elements"] [floor.ToString()] [row.ToString()] [column];
			Debug.Log("Building " + floor + "/" + row + "/" + column + ": " + field["f"].ToString() + ", " + field["o"].ToString());
			updateMaximumLevelDimensions();
			buildFloor();
			buildObject();
		} catch (Exception e) {
//			Debug.LogException(e);
		}
	}

	// ------------------------------------------------------------------------

	void buildObject() {
		string type = levelData ["elements"] [floor.ToString()] [row.ToString()] [column] ["o"].ToString();
		if(type != "---") {
			GameObject instance = createInstance(type);
			instance.transform.parent = GameObject.Find("Actors").transform;
		}
	}

	// ------------------------------------------------------------------------

	void buildFloor() {
		var type = getFloorTypeAt(floor, row, column);
		string pattern = "";

		// select correct pattern
		if(type == "FLR" || type == "BOX") {
			if (row % 2 == 0) {
				pattern = (column % 2 == 0) ? "1" : "2";
			} else {
				pattern = (column % 2 != 0) ? "1" : "2";
			}
		}
		GameObject instance = createInstance(type + pattern, floorOffset);
		instance.name = type + "_" + floor + "_" + row + "_" + column;

		// Delete the FLR in case a BOX is below the FLR
		if(floor > 0) {
			var floorTypeBelow = getFloorTypeAt(floor - 1, row, column);
			if((type == "FLR" && floorTypeBelow == "BOX")
				|| (type == "---" && floorTypeBelow == "LFD")
				|| (type == "---" && floorTypeBelow == "LFU")) {
				Destroy(instance);
			}
		}

		if(type == "BOX" && floor < 3 && getFloorTypeAt(floor + 1, row, column) == "FLR") {
			instance.transform.localScale += new Vector3(0, -floorOffset, 0);
		}

		instance.transform.parent = GameObject.Find("World").transform;
	}

	// ------------------------------------------------------------------------


	GameObject createInstance(string type, float floorOffset = 0.0f) {
		GameObject prefab = (GameObject) Resources.Load(type);
		GameObject instance = (GameObject)Instantiate(prefab, new Vector3 (column, floor + floorOffset, row), prefab.transform.rotation);
		instance.name = type;
		if(instance.tag == "Actor") {
			instance.name = type + ++instanceCounter;
		}
		objects.Add(instance);
		return instance;
	}

	// ------------------------------------------------------------------------

	string getFloorTypeAt(int floor, int row, int column) {
		try {
			return levelData ["elements"] [floor.ToString()] [row.ToString()] [column] ["f"].ToString();
		} catch(Exception e) {
			Debug.LogException(e);
		}
		return null;
	}
}