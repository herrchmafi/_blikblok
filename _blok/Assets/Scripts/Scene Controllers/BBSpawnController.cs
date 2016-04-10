using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BBSpawnController : MonoBehaviour {
	private BBSpriteFactory spriteFactory;

	private BBGridController gridController;

	private BBTimer timer;

	private List<SpawnUnit> spawnTrack;

	public struct SpawnUnit {
		private BBSpriteFactory.Sprite sprite;
		public BBSpriteFactory.Sprite Sprite {
			get { return this.sprite; }
		}

		private float spawnSeconds;
		public float SpawnSeconds {
			get { return this.spawnSeconds; }
		}

		private BBCoordinate coordinate;
		public BBCoordinate Coordinate {
			get { return this.coordinate; }
		}

		private bool atOpenCoordinate;
		public bool AtOpenCoordinate {
			get { return this.atOpenCoordinate; }
		}

		private int number;
		public int Number {
			get { return this.number; }
		}

		private BBEntityStats stats;
		public BBEntityStats Stats {
			get { return this.stats; }
		}

		//	If number is not needed, just feed it a negative number. This will be the convention we use instead of assigning it random values
		public SpawnUnit(BBSpriteFactory.Sprite sprite, float spawnDelaySeconds, float spawnOffsetSeconds, BBCoordinate coordinate, bool atOpenCoordinate, int number, BBEntityStats stats) {
			this.sprite = sprite;
			this.spawnSeconds = spawnDelaySeconds + spawnOffsetSeconds;
			this.coordinate = coordinate;
			this.atOpenCoordinate = atOpenCoordinate;
			this.number = number;
			this.stats = stats;
		}
	}

	void Awake() {
		this.timer = new BBTimer();
		this.spawnTrack = new List<SpawnUnit>();
		this.gridController = GameObject.FindGameObjectWithTag(BBSceneConstants.layoutControllerTag).GetComponent<BBGridController>();
	}

	// Use this for initialization
	void Start () {
		this.spriteFactory = GetComponent<BBSpriteFactory>();
	}

	void Update() {
		if (this.timer.IsTiming) {
			this.timer.Update();
			HashSet<SpawnUnit> spawnedUnits = new HashSet<SpawnUnit>();
			foreach (SpawnUnit spawnUnit in this.spawnTrack) {
				if (this.timer.Seconds >= spawnUnit.SpawnSeconds) {
					if (spawnUnit.AtOpenCoordinate) {
						BBCoordinate nearestCoordinate = this.gridController.NearestOpenCoordinate(spawnUnit.Coordinate);
						if (nearestCoordinate == null) { continue; }
						this.spriteFactory.CreateSprite(spawnUnit.Sprite, this.gridController.WorldPointFromCoordinate(nearestCoordinate), spawnUnit.Number, spawnUnit.Stats);
					} else {
						this.spriteFactory.CreateSprite(spawnUnit.Sprite, this.gridController.WorldPointFromCoordinate(spawnUnit.Coordinate), spawnUnit.Number, spawnUnit.Stats);
					}
					spawnedUnits.Add(spawnUnit);
				}
			}
			foreach (SpawnUnit completedUnit in spawnedUnits) {
				this.spawnTrack.Remove(completedUnit);
			}
			if (this.spawnTrack.Count == 0) {
				this.timer.Stop();
			}
		}
	}

	//	Place wherever you want
	public SpawnUnit CreateSpawnUnit(BBSpriteFactory.Sprite sprite, float spawnDelaySeconds, BBCoordinate coordinate, bool atValidCoordinate, int number, BBEntityStats stats) {
		BBNode referencedNode = this.gridController.grid[coordinate.X, coordinate.Y];
		return new SpawnUnit(sprite, spawnDelaySeconds, this.timer.Seconds, coordinate, atValidCoordinate, number, stats);
	}

	public void LoadTrack(List<SpawnUnit> spawnTrack) {
		this.spawnTrack = spawnTrack;
		this.timer.Start();
	}
}
