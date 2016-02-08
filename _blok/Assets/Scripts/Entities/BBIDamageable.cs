using UnityEngine;
using System.Collections;

public interface BBIDamageable {
	void TakeHit(int power, Collider collider);
	void TakeHit(int power, Collider collider, BBKnockback knockback);
}
