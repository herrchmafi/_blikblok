using UnityEngine;
using System.Collections;

public class BBBounceBullet : BBBounceWeapon {
	public LayerMask reflectLayerMask;	
	public int bouncesUntilExplosion = 3;
	private float bounces;
	
	public override void Update() {
		base.Update();
	}

	public void Init(Vector3 dir, GameObject originObject) {
		base.Init(originObject);
		this.dirVect = dir;
	}
		
	public override void OnTriggerEnter(Collider collider) {
		base.OnTriggerEnter(collider);
		GameObject collidedObject = collider.gameObject;
		//	Increase bounce count whenever bounce collision happens. Will destroy once bounce count is matched
		if (BBUnityComponentsHelper.IsInLayerMask(collider.gameObject, this.reflectLayerMask)) {
			this.bounces++;
		} else {
			//	This variable gets set to null as soon as the object leaves the origin. This 
			if (this.OriginObject != null && this.OriginObject.Equals(collidedObject)) {
				return;
			}
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
