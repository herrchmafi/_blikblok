using UnityEngine;
using System.Collections;

public interface BBIDamageable {
	void TakeHit(float power, Collider collider);
	void TakeHit(float power, Collider collider, BBKnockback knockback);
	void TakeHit(float power, Collision collision); 
	void TakeHit(float power, Collision collision, BBKnockback knockback);
}
