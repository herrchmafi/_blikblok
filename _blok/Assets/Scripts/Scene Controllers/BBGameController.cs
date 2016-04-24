using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	
	private BBSpawnController spawnController;
	private BBPlatformerGenerator platformGenerator;
	
	private BBPlayerCameraController playerCameraController;

	private BBGridController gridController;
	
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
		this.platformGenerator = GameObject.FindGameObjectWithTag(BBSceneConstants.layoutControllerTag).GetComponent<BBPlatformerGenerator>();
		this.spawnController = GameObject.FindGameObjectWithTag(BBSceneConstants.spawnControllerTag).GetComponent<BBSpawnController>();
		this.gridController = GameObject.FindGameObjectWithTag(BBSceneConstants.layoutControllerTag).GetComponent<BBGridController>();
		this.playerCameraController = Camera.main.GetComponent<BBPlayerCameraController>();
		
		this.platformGenerator.GenerateMap();
		this.spawnController.LoadTrack(new List<BBSpawnController.SpawnUnit>() {
			this.spawnController.CreateSpawnUnit(BBSpriteFactory.Sprite.PLAYER, 0.0f, new BBCoordinate(13, 16), true, 0, new BBEntityStats("Raul", 100, 0, 4)),
			this.spawnController.CreateSpawnUnit(BBSpriteFactory.Sprite.PLAYER, 0.0f, new BBCoordinate(15, 16), true, 1, new BBEntityStats("Gladys", 100, 0, 4)),
			this.spawnController.CreateSpawnUnit(BBSpriteFactory.Sprite.PLAYER, 0.0f, new BBCoordinate(17, 16), true, 2, new BBEntityStats("Barf", 100, 0, 4)),
			this.spawnController.CreateSpawnUnit(BBSpriteFactory.Sprite.PLAYER, 0.0f, new BBCoordinate(19, 16), true, 3, new BBEntityStats("Tongue", 100, 0, 4))
		
		});
		this.players = GameObject.FindGameObjectsWithTag(BBSceneConstants.playerTag);
		this.UpdatePlayers();

		this.enemies = GameObject.FindGameObjectsWithTag(BBSceneConstants.enemyTag);
		this.neutrals = GameObject.FindGameObjectsWithTag(BBSceneConstants.neutralTag);
		this.allies = GameObject.FindGameObjectsWithTag(BBSceneConstants.allyTag);
		this.haters = GameObject.FindGameObjectsWithTag(BBSceneConstants.haterTag);
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
			if (player.GetComponent<BBBasePlayerController>().Number == this.mainPlayerNumber) {
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
