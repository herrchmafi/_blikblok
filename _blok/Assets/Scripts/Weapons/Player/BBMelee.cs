using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BBMelee : BBWeapon {
	private bool isAttacking;
	public bool IsAttacking {
		get { return this.isAttacking; }
		set { 
			this.isAttacking = value;
			if (this.isAttacking) {
				if (!this.timer.IsTiming) {
					print("Attack");
					this.timer.Start();
					this.attackedObjects = new HashSet<GameObject>();
				}
			} else {
				this.ResetAttack();
			}
		}
	}
	
	public float attackDuration = 1.0f;
	private BBTimer timer = new BBTimer();
	
	private HashSet<GameObject> attackedObjects;
	
	private BoxCollider boxCollider;
	// Use this for initialization
	void Start () {
		this.boxCollider = transform.GetComponent<BoxCollider>();
		this.boxCollider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		this.timer.Update();
		if (this.isAttacking) {
			this.boxCollider.enabled = this.isAttacking;
			if (this.timer.Seconds >= this.attackDuration) {
				this.ResetAttack();
			}
		}
	}
	
	void OnTriggerEnter(Collider collider) {
		GameObject attackedObject = collider.gameObject;
		//Do not attack parent
		if (collider.gameObject.Equals(transform.parent.gameObject) || !this.isAttacking) {
			return;
		}
		BBIDamageable damageableObject = attackedObject.GetComponent<BBIDamageable>();

		if (damageableObject != null && !this.attackedObjects.Contains(attackedObject)) {
			damageableObject.TakeHit(this.power, collider);
			print("Hit you");
			this.attackedObjects.Add(attackedObject);
		}
	}
	private void ResetAttack() {
		this.isAttacking = false;
		this.boxCollider.enabled = this.isAttacking;
		this.timer.Stop();
		this.attackedObjects = null;
	}
}
