using UnityEngine;
using System.Collections;

public class BBCanvasController : MonoBehaviour {
	//	GameObject Names
	private const string playerHUD = "Player HUD";

	public void SyncPlayerStat(int number, BBEntityStats stats) {
		Transform targetHUD = transform.FindChild(playerHUD + " " + number);
		targetHUD.GetComponent<BBPlayerHUDController>().Stats = stats;
	}

}
