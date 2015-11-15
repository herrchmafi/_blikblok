using UnityEngine;
using System.Collections;

public class BBLivingEntity : MonoBehaviour, BBIDamageable {
	
	public float health = 100.0f;
	public float defense;
	
	private BBDamageSpeech damageSpeech;
	
	private BBController3D controller;
	
	private BBKnockback knockback;
	
	public virtual void Start() {
		this.damageSpeech = transform.GetComponent<BBDamageSpeech>();
		this.controller = (transform.parent != null) ? gameObject.GetComponentInParent<BBController3D>() : gameObject.GetComponent<BBController3D>();
	}
	
	public virtual void Update() {
		if (this.knockback != null) {
			this.TakeKnockback(this.knockback);
		}
	}
	
	//Take hit without knockback using OnTrigger events w/o knockback
	public void TakeHit(float power, Collider collider) {
		float targetDamage = power - this.defense;
		this.damageSpeech.TakeHit(targetDamage);
		if (targetDamage < .0f) {
			targetDamage = .0f; 
		}
		this.health -= targetDamage;
		if (this.health <= .0f) {
			this.Die();
		}
	}
	
	//Take hit without knockback using OnTrigger events with knockback
	public void TakeHit(float power, Collider collider, BBKnockback knockback) {
		this.TakeHit(power, collider);
		if (this.controller != null) {
			this.knockback = knockback;
			this.knockback.Timer.Start();
		}
	}
	
	//Take hit without knockback using Unity's default physics
	public void TakeHit(float power, Collision collision) {
		float targetDamage = power - this.defense;
		this.damageSpeech.TakeHit(targetDamage);
		if (targetDamage < .0f) {
			targetDamage = .0f; 
		}
		this.health -= targetDamage;
		if (this.health <= .0f) {
			this.Die();
		}
	}
	
	public void TakeHit(float power, Collision collision, BBKnockback knockback) {
		this.TakeHit(power, collision);
		if (this.controller != null) {
			this.knockback = knockback;;
			this.knockback.Timer.Start();
		}
	}
	
	//Call when knockback isn't null from within update
	private void TakeKnockback(BBKnockback knockback) {
		this.knockback.Timer.Update();
		if (this.knockback.Timer.Seconds < this.knockback.Seconds) {
			this.controller.Move(this.knockback.Direction * this.knockback.Magnitude * Time.deltaTime);
		} else {
			this.knockback = null;
		}
	}
	
	//Call when health is below 0 (or equal)
	private void Die() {
		string tag = gameObject.tag;
		gameObject.tag = "Dead";
		//Notification sent notifying of death. GameController will handle the updates
		BBEventController.SendDeathNotification(tag);
		if (transform.parent != null) {
			Destroy(transform.parent.gameObject);
		} else {
			Destroy(gameObject);
		}
	}
}
