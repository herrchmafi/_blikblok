using UnityEngine;
using System.Collections;

public class BBEventController : MonoBehaviour {
	public delegate void Death();
	public static event Death OnPlayerDeath;
	public static event Death OnEnemyDeath;
	public static event Death OnAllyDeath;
	// Use this for initialization
	
	public static void SendDeathNotification (string tag) {
		if (tag.Equals(BBSceneConstants.playerTag) && OnPlayerDeath != null) {
			OnPlayerDeath();
		} else if (tag.Equals(BBSceneConstants.enemyTag) && OnEnemyDeath != null) {
			OnEnemyDeath();
		} else if (tag.Equals(BBSceneConstants.allyTag) && OnAllyDeath != null) {
			OnAllyDeath();
		}	
	}
}
