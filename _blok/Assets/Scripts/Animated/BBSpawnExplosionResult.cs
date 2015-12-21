using UnityEngine;
using System.Collections;

public class BBSpawnExplosionResult : BBExplosionResult {
	public Transform spawnTransform;
	private BBSpriteFactory factory;
	// Use this for initialization
	void Start () {
		this.factory = GameObject.FindGameObjectWithTag(BBSceneConstants.gameControllerTag).GetComponent<BBSpriteFactory>(); 	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ExplodeResult() {
		Transform spawn = Instantiate(this.spawnTransform, transform.position, Quaternion.identity) as Transform;
		BBEventController.SendSpawnNotification(spawn.tag);
		Destroy(gameObject);
	}	
}
