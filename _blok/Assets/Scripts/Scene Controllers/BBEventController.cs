using UnityEngine;
using System.Collections;

public class BBEventController : MonoBehaviour {
	public delegate void Death();
	public static event Death OnPlayerDeath;
	public static event Death OnEnemyDeath;
	public static event Death OnNeutralDeath;
	public static event Death OnAllyDeath;
	public static event Death OnHaterDeath;
	// Use this for initialization
	
	public static void SendDeathNotification (string tag) {
		if (tag.Equals(BBSceneConstants.playerTag) && OnPlayerDeath != null) {
			OnPlayerDeath();
		} else if (tag.Equals(BBSceneConstants.enemyTag) && OnEnemyDeath != null) {
			OnEnemyDeath();
		} else if (tag.Equals(BBSceneConstants.neutralTag) && OnNeutralDeath != null) {
			OnNeutralDeath();
		} else if (tag.Equals(BBSceneConstants.allyTag) && OnAllyDeath != null) {
			OnAllyDeath();
		} else if (tag.Equals(BBSceneConstants.haterTag) && OnHaterDeath!= null) {
			OnHaterDeath();
		} 	
	}
	
	public delegate void Spawn();
	public static event Spawn OnPlayerSpawn;
	public static event Spawn OnEnemySpawn;
	public static event Spawn OnNeutralSpawn;
	public static event Spawn OnAllySpawn;
	public static event Spawn OnHaterSpawn;
	
	public static void SendSpawnNotification (string tag) {
		if (tag.Equals(BBSceneConstants.playerTag) && OnPlayerDeath != null) {
			OnPlayerSpawn();
		} else if (tag.Equals(BBSceneConstants.enemyTag) && OnEnemyDeath != null) {
			OnEnemySpawn();
		} else if (tag.Equals(BBSceneConstants.neutralTag) && OnNeutralDeath != null) {
			OnNeutralSpawn();
		} else if (tag.Equals(BBSceneConstants.allyTag) && OnAllyDeath != null) {
			OnAllySpawn();
		} else if (tag.Equals(BBSceneConstants.haterTag) && OnHaterDeath!= null) {
			OnHaterSpawn();
		} 		 
	}
}
