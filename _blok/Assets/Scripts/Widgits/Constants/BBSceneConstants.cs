using UnityEngine;
using System.Collections;

//This class contains constants that are used within scenes
public class BBSceneConstants {
	//Scene Tags
	public const string playerTag = "Player";
	public const string playersTag = "Players";
	public const string actionPlayerTag = "ActionPlayer";
	public const string gameControllerTag = "GameController";
	public const string enemyTag = "Enemy";
	public const string allyTag = "Ally";
	public const string deadTag = "Dead";
	public const string damageTag = "Damage";
	public const string platformsTag = "Platforms";
	
	//Names used within game object hierarchy (i.e. parents, chidren)
	public const string actionEntity = "Action Entity";
	public const string animatedEntity = "Animated Entity";
	
	//Input Elements
	public const string horizontalInput = "Horizontal";
	public const string verticalInput = "Vertical";
	public const string jumpInput = "Jump";
	public const string normalAttackInput = "NormalAttack";
	
	//ground is where the ground objects are spawned, collidedGround refers to where colliders hit, sprite ground is where the image is to work visually
	public const float collidedGround = -1.0f;
	public const float ground = -.0f;
	public const float spriteGround = .0f;
	
	private static Vector3 aStarColliderCenter = new Vector3(.0f, .0f, -1.5f);
	public static Vector3 AStarColliderCenter {
		get { return aStarColliderCenter; }
	}
	
	//Player
	public static readonly Vector3 actionPlayerOffset = new Vector3(.0f, .0f, 1.0f);
}
