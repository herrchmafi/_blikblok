using UnityEngine;
using System.Collections;

public class BBDeathExplosionResult : BBExplosionResult {
	private 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ExplodeResult() {
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
}
