using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BBGameController : MonoBehaviour {
	[Range(0, 3)]
	public int mainPlayerNumber;

	[Range(1, 4)]
	public int playerCount;

	public Transform playerHUD1;
	public Transform playerHUD4;
	
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

		Transform playerHUD = null;
		switch (this.playerCount) {
		case 1:
			playerHUD = Instantiate(this.playerHUD1, Vector3.zero, Quaternion.identity) as Transform;
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			playerHUD = Instantiate(this.playerHUD4, Vector3.zero, Quaternion.identity) as Transform;
			break;
		default:
			BBErrorHelper.DLog(BBErrorConstants.InvalidSwitchInput, "Player count is invalid");
			break;
		}
		playerHUD.parent = GameObject.FindGameObjectWithTag(BBSceneConstants.controllersTag).transform;

		this.platformGenerator.GenerateMap();
		this.spawnController.LoadTrack(new List<BBSpawnController.SpawnUnit>() {
			this.spawnController.CreateSpawnUnit(BBSpriteFactory.Sprite.PLAYER, 0.0f, new BBCoordinate(13, 16), true, new BBEntityStats("Raul", 100, 0, 4), 0),
			this.spawnController.CreateSpawnUnit(BBSpriteFactory.Sprite.BOMBER, 1.0f, new BBCoordinate(17, 18), true, new BBEntityStats(BBSceneConstants.enemy, 20, 0, 0)),
			this.spawnController.CreateSpawnUnit(BBSpriteFactory.Sprite.TURRET, 1.0f, new BBCoordinate(19, 18), true, new BBEntityStats(BBSceneConstants.enemy, 20, 0, 0)),
			this.spawnController.CreateSpawnUnit(BBSpriteFactory.Sprite.DUMMY, 1.0f, new BBCoordinate(22,20), true, new BBEntityStats(BBSceneConstants.enemy, 20, 0, 0))
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
