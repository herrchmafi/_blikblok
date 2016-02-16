using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BBSpawnController : MonoBehaviour {
	private BBSpriteFactory spriteFactory;

	private BBTimer timer;

	private List<SpawnUnit> spawnTrack;
	public List<SpawnUnit> SpawnTrack {
		get { return this.spawnTrack; }
	}

	public struct SpawnUnit {
		private BBSpriteFactory.Sprite sprite;
		public BBSpriteFactory.Sprite Sprite {
			get { return this.sprite; }
		}
		private float spawnDelay;
		public float SpawnDelay {
			get { return this.spawnDelay; }
		}

		private Vector3 position;
		public Vector3 Position {
			get { return this.position; }
		}

		public SpawnUnit(BBSpriteFactory.Sprite sprite, float spawnDelay, Vector3 position) {
			this.sprite = sprite;
			this.spawnDelay = spawnDelay;
			this.position = position;
		}
	}

	void Awake() {
		this.timer = new BBTimer();
		this.spawnTrack = new List<SpawnUnit>();
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
				if (this.timer.Seconds >= spawnUnit.SpawnDelay) {
					this.spriteFactory.CreateSprite(spawnUnit.Sprite, spawnUnit.Position);
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

	public void LoadTrack(List<SpawnUnit> spawnTrack) {
		this.spawnTrack = spawnTrack;
		this.timer.Start();
	}
}
