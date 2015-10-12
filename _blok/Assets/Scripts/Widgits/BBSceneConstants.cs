using UnityEngine;
using System.Collections;

public class BBSceneConstants {
	public const float ground = -1.0f;
	
	private static Vector3 aStarColliderCenter = new Vector3(.0f, .0f, -1.5f);
	public static Vector3 AStarColliderCenter {
		get { return aStarColliderCenter; }
	}
}
