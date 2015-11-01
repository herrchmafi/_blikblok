using UnityEngine;
using System.Collections;

public class BBTurretBullet : BBWeapon {
	public LayerMask reflectLayerMask;
	
	public float velocity = 2.0f;
	
	public float bouncesUntilExplosion = 3;
	private float bounces;
	
	public void FireInDir(Vector3 dir) {
		Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
		rigidBody.velocity = this.velocity * dir;
	}
		
	void OnCollisionEnter(Collision collision) {
		if (BBUnityComponentsHelper.IsInLayerMask(collision.gameObject, this.reflectLayerMask)) {
			this.bounces++;
		} else {
			print(collision.gameObject.tag);
			BBIDamageable damageableObject = collision.gameObject.GetComponent<BBIDamageable>();
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
