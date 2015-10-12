using UnityEngine;
using System.Collections;

public class BBMelee : BBWeapon {
	
	// Use this for initialization
	void Start () {
	 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject == this) {
			return;
		}
		BBIDamageable damageableObject = collision.gameObject.GetComponent<BBIDamageable>();
		if (damageableObject != null) {
			damageableObject.TakeHit(this.power, collision);
		}
		
	}
}
