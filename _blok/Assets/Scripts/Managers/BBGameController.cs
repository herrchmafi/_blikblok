using UnityEngine;
using System.Collections;

public class BBGameController : MonoBehaviour {
	[Range(0, 3)]
	public int mainPlayerNumber;
	private GameObject[] players;
	public GameObject[] Players {
		get { return this.players; }
	}
	private GameObject mainPlayer;
	public GameObject MainPlayer {
		get { return this.mainPlayer; }
	}
	private BBPlayerCameraController playerCameraController;

	// Use this for initialization
	void Start () {
		this.playerCameraController =  Camera.main.GetComponent<BBPlayerCameraController>();
		this.players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in this.players) {
			if (player.GetComponent<BBPlayer>().playerNumber == this.mainPlayerNumber) {
				this.mainPlayer = player;
				this.playerCameraController.SetTargetPlayer(player);
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnEnable() {
		BBEventController.OnPlayerDeath += UpdatePlayers;
	}
	
	void OnDisable() {
		BBEventController.OnPlayerDeath -= UpdatePlayers;
	}
	
	private void UpdatePlayers() {
		this.players = GameObject.FindGameObjectsWithTag("Player");
		if (this.players.Length == 0) {
			print("Everybody is dead!");
			return;
		}
		if (this.mainPlayer.tag.Equals("Dead")) {
			this.mainPlayer = this.players[0];
			this.playerCameraController.SetTargetPlayer(this.mainPlayer);
		}
	}
}
