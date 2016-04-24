using UnityEngine;
using System.Collections;

public class BBSpawnExplosionResult : MonoBehaviour, BBExplosionResult {
	public Transform spawnTransform;
	private BBSpriteFactory factory;

	private int spawnNumber = (int)BBSceneConstants.NumberConventions.DEFAULTNUMBER;
	public int SpawnNumber {
		get { return this.spawnNumber; }
		set { this.spawnNumber = value; }
	}

	 private BBEntityStats spawnStats;
	 public BBEntityStats SpawnStats {
	 	get { return this.spawnStats; }
	 	set { this.spawnStats = value; }
	 }
	// Use this for initialization
	void Start () {
		this.factory = GameObject.FindGameObjectWithTag(BBSceneConstants.gameControllerTag).GetComponent<BBSpriteFactory>(); 	
	}
	
	public void ExplosionResult() {
		Transform spawn = Instantiate(this.spawnTransform, transform.position, Quaternion.identity) as Transform;
		if (spawn.tag.Equals(BBSceneConstants.playerTag)) {
			spawn.GetComponent<BBBasePlayerController>().Init(this.spawnNumber, this.spawnStats);
		} else {
			spawn.GetComponent<BBLivingEntity>().Init(this.spawnNumber, this.spawnStats);
		}
		BBEventController.SendSpawnNotification(spawn.tag);
		Destroy(gameObject);
	}
		
}
