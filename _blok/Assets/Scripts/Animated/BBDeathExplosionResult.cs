using UnityEngine;
using System.Collections;

public class BBDeathExplosionResult : MonoBehaviour, BBExplosionResult {
	public void ExplosionResult(int number) {
		if (transform.parent != null) {
			Transform parent = transform.parent;
			transform.parent = null;
			//Destroy hierarchy
			while (parent.parent != null) {
				parent = parent.parent;
			}
			Destroy(parent.gameObject);
		}
	}

	public void ExplosionResult() {
		this.ExplosionResult((int)BBSceneConstants.NumberConventions.DEFAULTNUMBER);
	}
}
