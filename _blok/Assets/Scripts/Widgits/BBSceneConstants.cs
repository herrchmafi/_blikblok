using UnityEngine;
using System.Collections;

public class BBSceneConstants {
	//Scene Tags
	public const string playerTag = "Player";
	public const string gameControllerTag = "GameController";
	public const string baseTag = "Base"; 
	public const string damageTag = "Damage";
	public const string deadTag = "Dead";
	public const string enemyTag = "Enemy";
	public const string allyTag = "Ally";
	
	public const string actionPlayer = "Action Player";
	public const string animatedPlayer = "Animated Player";
	
	public const float ground = -1.0f;
	
	private static Vector3 aStarColliderCenter = new Vector3(.0f, .0f, -1.5f);
	public static Vector3 AStarColliderCenter {
		get { return aStarColliderCenter; }
	}
	
	
	//Player
	public static readonly Vector3 actionPlayerOffset = new Vector3(.0f, .0f, 1.0f);
}
