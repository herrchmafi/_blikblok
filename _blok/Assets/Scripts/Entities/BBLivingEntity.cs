using UnityEngine;
using System.Collections;

public class BBLivingEntity : MonoBehaviour, BBIDamageable {
	
	public float health = 100.0f;
	public float defense;
	
	public void TakeHit(float damage, Collision collision) {
		float targetDamage = damage - this.defense;
		if (targetDamage < .0f) {
			targetDamage = .0f; 
		}
		this.health -= targetDamage;
		if (this.health <= .0f) {
			this.Die();
		}
	}
	
	private void Die() {
		gameObject.tag = "Dead";
		BBEventController.SendPlayerDeathNotification();
		Destroy(gameObject);
	}
}
