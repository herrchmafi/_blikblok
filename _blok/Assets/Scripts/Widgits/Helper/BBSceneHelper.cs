using UnityEngine;
using System.Collections;

public class BBSceneHelper {
	public static Vector3 CollidedGroundVector3(Vector2 vector) {
		return new Vector3(vector.x, vector.y, BBSceneConstants.collidedGround);
	}

}
