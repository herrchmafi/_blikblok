using UnityEngine;
using System.Collections;

public class BBSpriteFactory : MonoBehaviour {
	public enum Sprite {
		PLAYER,
		TURRET,
		BOMBER,
		DUMMY,
		NONE
	}

	public Transform playerSpawnFab;
	public Transform turretSpawnFab;
	public Transform bomberSpawnFab;
	public Transform dummySpawnFab;

	private int playersSpawnedCount = 0;
	public int PlayersSpawnedCount {
		get { return this.playersSpawnedCount; }
	}
	
	public void CreateSprite(Sprite sprite, Vector3 position, int number) {
		Transform spawn = null;
		switch (sprite) {
			case Sprite.PLAYER:
				spawn = Instantiate(this.playerSpawnFab, position, Quaternion.identity) as Transform;
				spawn.GetComponent<BBExplosion>().Number = number;
				break;
			case Sprite.TURRET:
				spawn = Instantiate(this.turretSpawnFab, position, Quaternion.identity) as Transform;
				break;
			case Sprite.BOMBER:
				spawn = Instantiate(this.bomberSpawnFab, position, Quaternion.identity) as Transform;
				break;
			case Sprite.DUMMY:
				spawn = Instantiate(this.dummySpawnFab, position, Quaternion.identity) as Transform;
				break;
		}
		if (spawn != null) {
			BBExplosion explosion = spawn.GetComponent<BBExplosion>();
			if (explosion != null) {
				explosion.Explode(1.0f);
			}	
		}
	}

	
	public static string TagForSprite(Sprite sprite) {
		switch(sprite) {
			case Sprite.PLAYER:
				return BBSceneConstants.playerTag;
			case Sprite.TURRET:
				return BBSceneConstants.enemyTag;
			case Sprite.BOMBER:
				return BBSceneConstants.enemyTag;
			case Sprite.DUMMY:
				return BBSceneConstants.enemyTag;
			default:
				return BBSceneConstants.untaggedTag;
		}
	}
}
