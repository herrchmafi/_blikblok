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
	
	private BBSpriteFactory spriteFactory;
	
	private BBPlayerCameraController playerCameraController;
	
	private GameObject[] enemies;
	public GameObject[] Enemies {
		get { return this.enemies; }
	}
	
	private GameObject[] allies;
	public GameObject[] Allies {
		get { return this.allies; }
	}

	// Use this for initialization
	void Start () {
		this.spriteFactory = transform.GetComponent<BBSpriteFactory>();
		this.spriteFactory.CreateSprite(BBSpriteFactory.Sprite.PLAYER, new Vector3(.0f, .0f, BBSceneConstants.collidedGround - 10));
		this.players = GameObject.FindGameObjectsWithTag(BBSceneConstants.playerTag);
		this.playerCameraController = Camera.main.GetComponent<BBPlayerCameraController>();
		
		this.enemies = GameObject.FindGameObjectsWithTag(BBSceneConstants.enemyTag);
		
		this.allies = GameObject.FindGameObjectsWithTag(BBSceneConstants.allyTag);
		
		
		foreach (GameObject player in this.players) {
			if (player.GetComponent<BBBasePlayerController>().playerNumber == this.mainPlayerNumber) {
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
		BBEventController.OnEnemyDeath += UpdateEnemies;
		BBEventController.OnAllyDeath += UpdateAllies;
		
	}
	
	void OnDisable() {
		BBEventController.OnPlayerDeath -= UpdatePlayers;
	}
	
	private void UpdatePlayers() {
		this.players = GameObject.FindGameObjectsWithTag(BBSceneConstants.playerTag);
		if (this.players.Length == 0) {
			return;
		}
		if (this.mainPlayer.tag.Equals("Dead")) {
			this.mainPlayer = this.players[0];
			this.playerCameraController.SetTargetPlayer(this.mainPlayer);
		}
	}
	
	private void UpdateEnemies() {
		this.enemies = GameObject.FindGameObjectsWithTag(BBSceneConstants.enemyTag);
		if (this.enemies.Length == 0) {
			return;
		}
	}
	
	private void UpdateAllies() {
		this.allies = GameObject.FindGameObjectsWithTag(BBSceneConstants.allyTag);
		if (this.allies.Length == 0) {
			return;
		}
	}
	
	
	
	
	
}
