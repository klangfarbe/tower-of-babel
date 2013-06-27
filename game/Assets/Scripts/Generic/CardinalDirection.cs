using UnityEngine;

class CardinalDirection {
	private static Quaternion north = Quaternion.Euler(0,0,0);
	private static Quaternion east = Quaternion.Euler(0,90,0);
	private static Quaternion south = Quaternion.Euler(0,180,0);
	private static Quaternion west = Quaternion.Euler(0,270,0);

	// -------------------------------------------------------------------------

	public static Quaternion North {
		get {
			return north;
		}
	}

	// -------------------------------------------------------------------------

	public static Quaternion East {
		get {
			return east;
		}
	}

	// -------------------------------------------------------------------------

	public static Quaternion South {
		get {
			return south;
		}
	}

	// -------------------------------------------------------------------------

	public static Quaternion West {
		get {
			return west;
		}
	}
}