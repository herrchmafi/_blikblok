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
	private BBPlatformerGenerator platformGenerator;
	
	private BBPlayerCameraController playerCameraController;
	
	private GameObject[] enemies;
	public GameObject[] Enemies {
		get { return this.enemies; }
	}
	
	private GameObject[] neutrals;
	public GameObject[] Neutrals {
		get { return this.neutrals; }
	}

	private GameObject[] allies;
	public GameObject[] Allies {
		get { return this.allies; }
	}

	private GameObject[] haters;
	public GameObject[] Haters {
		get { return this.haters; }
	}
	
	// Use this for initialization
	void Start () {
		this.platformGenerator = transform.GetComponent<BBPlatformerGenerator>();
		this.spriteFactory = transform.GetComponent<BBSpriteFactory>();
		this.playerCameraController = Camera.main.GetComponent<BBPlayerCameraController>();
		
		this.platformGenerator.GenerateMap();
		
		this.spriteFactory.CreateSprite(BBSpriteFactory.Sprite.PLAYER, new Vector3(.0f, .0f, BBSceneConstants.collidedGround));
		this.players = GameObject.FindGameObjectsWithTag(BBSceneConstants.playerTag);
		this.UpdatePlayers();

		this.enemies = GameObject.FindGameObjectsWithTag(BBSceneConstants.enemyTag);
		this.neutrals = GameObject.FindGameObjectsWithTag(BBSceneConstants.neutralTag);
		this.allies = GameObject.FindGameObjectsWithTag(BBSceneConstants.allyTag);
		this.haters = GameObject.FindGameObjectsWithTag(BBSceneConstants.haterTag);
		
	}
	
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnEnable() {
		BBEventController.OnPlayerDeath += UpdatePlayers;
		BBEventController.OnEnemyDeath += UpdateEnemies;
		BBEventController.OnNeutralDeath += UpdateNeutrals;
		BBEventController.OnAllyDeath += UpdateAllies;
		BBEventController.OnHaterDeath += UpdateHaters;
		
		BBEventController.OnPlayerSpawn += UpdatePlayers;
		BBEventController.OnEnemySpawn += UpdateEnemies;
		BBEventController.OnNeutralSpawn += UpdateNeutrals;
		BBEventController.OnAllySpawn += UpdateAllies;
		BBEventController.OnHaterSpawn += UpdateHaters;
	}
	
	void OnDisable() {
		BBEventController.OnPlayerDeath -= UpdatePlayers;
	}
	
	private void UpdatePlayers() {
		this.players = GameObject.FindGameObjectsWithTag(BBSceneConstants.playerTag);
		if (this.players.Length == 0) {
			return;
		}
		//Sets camera target
		foreach (GameObject player in this.players) {
			if (player.GetComponent<BBBasePlayerController>().playerNumber == this.mainPlayerNumber) {
				this.mainPlayer = player;
				this.playerCameraController.SetTargetPlayer(player);
				break;
			}
		}
		//If the main player died, set a new camera target
		if (this.mainPlayer.tag.Equals(BBSceneConstants.deadTag)) {
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

	private void UpdateNeutrals() {
		this.neutrals = GameObject.FindGameObjectsWithTag(BBSceneConstants.neutralTag);
		if (this.neutrals.Length == 0) {
			return;
		}
	}

	private void UpdateAllies() {
		this.allies = GameObject.FindGameObjectsWithTag(BBSceneConstants.allyTag);
		if (this.allies.Length == 0) {
			return;
		}
	}

	private void UpdateHaters() {
		this.haters = GameObject.FindGameObjectsWithTag(BBSceneConstants.haterTag);
		if (this.haters.Length == 0) {
			return;
		}
	}
	
}
