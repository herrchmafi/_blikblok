using UnityEngine;
using System.Collections;

public class BBHUDController : MonoBehaviour {
	public Transform playerHUDFab;
	//	TODO:// Temporary so I can see stuff. Will make more flexible later on
	public static readonly Vector2 playerHUDLeftOffset = new Vector3(500.0f, 600.0f);

	public void CreatePlayerHUD(int playerNumber) {
		Transform playerHUDTransform = (Transform)Instantiate(this.playerHUDFab, Vector3.zero, Quaternion.identity);
		playerHUDTransform.SetParent(transform);
		playerHUDTransform.position = playerHUDLeftOffset + new Vector2(playerNumber * playerHUDTransform.GetComponent<RectTransform>().rect.width, .0f);
	}
}
