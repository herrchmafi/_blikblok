using UnityEngine;
using System.Collections;

public class BBEventController : MonoBehaviour {
	public delegate void PlayerDeath();
	public static event PlayerDeath OnPlayerDeath;
	// Use this for initialization
	
	public static void SendPlayerDeathNotification () {
		if (OnPlayerDeath != null) {
			OnPlayerDeath();
		}
	}
	
	
}
