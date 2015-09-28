using UnityEngine;
using System.Collections;

public interface BBIDamageable {

	void TakeHit(float power, Collision coll);
}
