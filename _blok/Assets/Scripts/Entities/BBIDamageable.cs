using UnityEngine;
using System.Collections;

public interface BBIDamageable {
	void TakeHit(float power, Collider collider);
	void TakeHit(float power, Collider collider, BBKnockback knockback);
}
