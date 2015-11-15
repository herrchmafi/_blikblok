using UnityEngine;
using System.Collections;

public class BBSpriteFactory : MonoBehaviour {
	public Transform basePlayerFab;
	public Transform turretFab;
	public Transform dummyFab;

	public enum Sprite {
		PLAYER,
		TURRET,
		DUMMY
	}
	
	public void CreateSprite(Sprite sprite, Vector3 position) {
		switch (sprite) {
			case Sprite.PLAYER:
				Transform player = Instantiate(this.basePlayerFab, position, Quaternion.identity) as Transform;
				player.parent = GameObject.FindGameObjectWithTag(BBSceneConstants.playersTag).transform;
				break;
			case Sprite.TURRET:
				Instantiate(this.turretFab, position, Quaternion.identity);
				break;
			case Sprite.DUMMY:
				Instantiate(this.dummyFab, position, Quaternion.identity);
				break;
		}
	}
}
