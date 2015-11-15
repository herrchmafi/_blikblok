using UnityEngine;
using System.Collections;

public class BBBounceBullet : BBBounceWeapon {
	public LayerMask reflectLayerMask;	
	public float bouncesUntilExplosion = 3;
	private float bounces;
	
	public override void Update() {
		base.Update();
	}
	
	public void FireInDir(Vector3 dir) {
		this.dirVect = dir;
	}
		
	public override void OnTriggerEnter(Collider collider) {
		base.OnTriggerEnter(collider);
		if (BBUnityComponentsHelper.IsInLayerMask(collider.gameObject, this.reflectLayerMask)) {
			this.bounces++;
		} else {
			BBIDamageable damageableObject = collider.gameObject.GetComponent<BBIDamageable>();
			if (damageableObject != null) {
				damageableObject.TakeHit(this.power, GetComponent<Collider>());
				Destroy(gameObject);
			}
		}
		 if (this.bounces == this.bouncesUntilExplosion) {
			Destroy(gameObject);
		}
		
	}
}
