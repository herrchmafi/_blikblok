using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	[Range(0, 3)]
	public int mainPlayerNumber;
	private PlayerCameraController playerCameraController;
	// Use this for initialization
	void Start () {
		this.playerCameraController =  Camera.main.GetComponent<PlayerCameraController>();
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in players) {
			if (player.GetComponent<Player>().playerNumber == this.mainPlayerNumber) {
				this.playerCameraController.SetTargetPlayer(player);
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
