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

	public float floorOffset = -0.03f;

	public Material patterncolor1;
	public Material patterncolor2;
	public Material groundcolor;

	private JsonData levelData;
	private int maxColumns = 0;
	private int maxRows = 0;
	private int maxFloors = 0;

	// the current fields to build
	private int floor;
	private int row;
	private int column;

	private int instanceCounter = 0;

	private GameObject objGrabber;
	private GameObject objPusher;
	private GameObject objZapper;
	private bool hasConverter;

	private List<UnityEngine.Object> objects = new List<UnityEngine.Object>();

	// ------------------------------------------------------------------------
	// Move the following with in the scene controller. The LevelLoader
	// should be generic and must not know anything about the number of
	// levels available
	// ------------------------------------------------------------------------

	public int level = 0;
	void Start() {
		if(PlayerPrefs.HasKey("lastLevel")) {
			level = PlayerPrefs.GetInt("lastLevel");
		}
		build("Level/" + string.Format("{0:d3}", level));
	}

	// ------------------------------------------------------------------------

	void Update() {
		if(Input.GetKeyUp(KeyCode.P)) {
			prev();
		}
		if(Input.GetKeyUp(KeyCode.N)) {
			next();
		}
	}

	// ------------------------------------------------------------------------

	public void next() {
		if(level < 116) {
			build("Level/" + string.Format("{0:d3}", ++level));
		}
		PlayerPrefs.SetInt("lastLevel", level);
	}

	// ------------------------------------------------------------------------

	public void prev() {
		if(level > 0) {
			build("Level/" + string.Format("{0:d3}", --level));
		}
		PlayerPrefs.SetInt("lastLevel", level);
	}

	// ------------------------------------------------------------------------

	public void reload() {
		build("Level/" + string.Format("{0:d3}", level));
	}

	// ------------------------------------------------------------------------
	// Level information
	// ------------------------------------------------------------------------

	public string Title {
		get { return levelData["title"].ToString(); }
	}

	public string Author {
		get { return levelData["author"].ToString(); }
	}

	public bool Destroysfloor {
		get { return (bool)levelData["behaviour"]["destroysfloor"]; }
	}

	public bool Cameras {
		get { return (bool)levelData["behaviour"]["cameras"]; }
	}

	public int Timebombspeed {
		get { return (int)levelData["behaviour"]["timebombspeed"]; }
	}

	public int MaxFloors {
		get { return maxFloors; }
	}

	public int MaxRows {
		get { return maxRows; }
	}

	public int MaxColumns {
		get { return maxColumns; }
	}

	public int Klondikes {
		get { return (int)levelData["conditions"]["klondikes"]; }
	}

	public int Robots {
		get { return (int)levelData["conditions"]["robots"]; }
	}

	public int Timelimit {
		get { return (int)levelData["conditions"]["timelimit"]; }
	}

	public bool HasConverter {
		get { return hasConverter; }
	}

	// ------------------------------------------------------------------------
	// Methods
	// ------------------------------------------------------------------------

	public void build(String resource) {
		if(Debug.isDebugBuild) {
			Debug.Log("Building level...");
		}
		clearScene();
		loadResource(resource);
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
		activateCamera();

		Conditions conditions = gameObject.GetComponent<Conditions>();
		conditions.init((int)levelData["conditions"]["klondikes"],
						(int)levelData["conditions"]["robots"],
						(int)levelData["conditions"]["timelimit"]);

		showLevelInformation();
	}

	// ------------------------------------------------------------------------
	// Private methods for building the level
	// ------------------------------------------------------------------------

	private void showLevelInformation() {
		GUINotification gui = GameObject.Find("Controller").GetComponent<GUINotification>();
		gui.clearNotifications();
		gui.notify(Title, 2f);

		string spiders = "";
		if(objGrabber) {
			spiders = "Grabber";
			if(objZapper && objPusher) {
				spiders += ", ";
			} else if(objZapper || objPusher) {
				spiders += " and ";
			}
		}
		if(objPusher) {
			spiders += "Pusher";
			if(objZapper) {
				spiders += " and ";
			}
		}
		if(objZapper) {
			spiders += "Zapper";
		}

		gui.notify("Available spiders:\n" + spiders, 2f);

		string goal = "";
		if(Klondikes > 0) {
			goal = "Collect " + Klondikes + " Klondike" + (Klondikes > 1 ? "s" : "");
			if(Robots > 0) {
				goal += " and destroy " + Robots + " robot" + (Robots > 1 ? "s" : "");
			}
		} else if(Robots > 0) {
			goal += "Destroy " + Robots + " robot" + (Robots > 1 ? "s" : "");
		}
		if(Timelimit > 0) {
			goal += " within " + gameObject.GetComponent<Conditions>().getRemainingTime() + " minutes";
		}
		gui.notify(goal, 2f);
	}

	// ------------------------------------------------------------------------

	void activateCamera() {
		CameraController camera = GameObject.Find("Controller").GetComponent<CameraController>();
		camera.init();
		if(Cameras) {
			camera.activateOverview();
		} else {
			if(objGrabber) camera.activateGrabber(objGrabber);
			else if(objPusher) camera.activatePusher(objPusher);
			else if(objZapper) camera.activateZapper(objZapper);
		}
	}

	// ------------------------------------------------------------------------

	void setColors() {
		patterncolor1.color = hexToColor(levelData["fx"]["patterncolor1"].ToString().Substring(2));
		patterncolor2.color = hexToColor(levelData["fx"]["patterncolor2"].ToString().Substring(2));
		groundcolor.color = hexToColor(levelData["fx"]["groundcolor1"].ToString().Substring(2));
		setSkycolor(GameObject.Find("GameCam"));
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

	void clearScene() {
		foreach(UnityEngine.Object o in objects) {
			if(Debug.isDebugBuild)
				Debug.Log("Destroyed " + o);
			Destroy(o);
		}
		objects.Clear();
		maxFloors = 0;
		maxColumns = 0;
		maxRows = 0;
		levelData = null;
		instanceCounter = 0;
		objZapper = null;
		objPusher = null;
		objGrabber = null;
		hasConverter = false;
	}

	// ------------------------------------------------------------------------

	void loadResource(String resource) {
		if(Debug.isDebugBuild)
			Debug.Log("Loading level data...");
		TextAsset json = Resources.Load(resource) as TextAsset;
		levelData = JsonMapper.ToObject(json.text);
	}

	// ------------------------------------------------------------------------

	Color hexToColor(string hex) {
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		if(Debug.isDebugBuild)
			Debug.Log("Converting #" + hex + " to RGB(" + r + "," + g + "," + b + ")");
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
		GameObject.Find("LevelCenter").transform.position = new Vector3(x, 0.1f, z);
		GameObject.Find("LevelCenter").transform.rotation = Quaternion.identity;

		// Adapt Bounding Box for Level Borders
		GameObject.Find("LevelBounds").transform.position = new Vector3(x, y, z);
		GameObject.Find("LevelBounds").transform.localScale = new Vector3(maxColumns + 1f, maxFloors + 2f, maxRows + 1f);
		GameObject.Find("LevelBounds").transform.rotation = Quaternion.identity;

		// Adapt lights to point to level center
		GameObject.Find("tr").transform.LookAt(GameObject.Find("LevelCenter").transform);
		GameObject.Find("tl").transform.LookAt(GameObject.Find("LevelCenter").transform);
		GameObject.Find("br").transform.LookAt(GameObject.Find("LevelCenter").transform);
		GameObject.Find("bl").transform.LookAt(GameObject.Find("LevelCenter").transform);
	}

	// ------------------------------------------------------------------------

	void buildPosition () {
		try {
			if(Debug.isDebugBuild) {
				JsonData field = levelData ["elements"] [floor.ToString()] [row.ToString()] [column];
				Debug.Log("Building " + floor + "/" + row + "/" + column + ": " + field["f"].ToString() + ", " + field["o"].ToString());
			}
			updateMaximumLevelDimensions();
			buildFloor();
			buildObject();
		} catch(ArgumentOutOfRangeException e) {
			// ignore
		} catch(KeyNotFoundException e) {
			// ignore
		} catch (Exception e) {
			if(Debug.isDebugBuild)
				Debug.LogException(e);
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

		if(type == "---")
			return;

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

		#if UNITY_DEBUG
			instance.GetComponentInChildren<MeshRenderer>().material = patterncolor1;
		#endif

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
		if(type == "GRB") {
			objGrabber = instance;
		}
		if(type == "PSH") {
			objPusher = instance;
		}
		if(type == "ZAP") {
			objZapper = instance;
		}
		if(type == "CVN" || type == "CVE") {
			hasConverter = true;
		}
		objects.Add(instance);
		return instance;
	}

	// ------------------------------------------------------------------------

	string getFloorTypeAt(int floor, int row, int column) {
		try {
			return levelData ["elements"] [floor.ToString()] [row.ToString()] [column] ["f"].ToString();
		} catch(KeyNotFoundException e) {
			// ignore
		} catch(Exception e) {
			if(Debug.isDebugBuild) {
				Debug.LogException(e);
			}
		}
		return null;
	}
}