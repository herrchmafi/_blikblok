﻿using UnityEngine;
using System.Collections;

public class BBLivingEntity : MonoBehaviour, BBIDamageable {
	private BBEntityStats stats;
	public BBEntityStats Stats {
		get { return this.stats; }
		set { this.stats = value; }
	}
	protected int number;
	
	private BBDamageSpeech damageSpeech;
	
	protected BBController3D controller;
	
	private BBKnockback knockback;

	protected BBAnimatedEntity animatedEntity;

	public BBGridController gridController;
	private BBNode previousInhabitedNode;

	private int boundX, boundY;
	public BBCoordinate Bounds2D {
		get { return new BBCoordinate(this.boundX, this.boundY); }
	}
	
	public virtual void Start() {
		this.damageSpeech = transform.GetComponent<BBDamageSpeech>();
		this.controller = (transform.parent != null) ? gameObject.GetComponentInParent<BBController3D>() : gameObject.GetComponent<BBController3D>();
		this.animatedEntity = transform.FindChild(BBSceneConstants.animatedEntity).GetComponent<BBAnimatedEntity>();
		this.gridController = GameObject.FindGameObjectWithTag(BBSceneConstants.layoutControllerTag).GetComponent<BBGridController>();
		BoxCollider collider = GetComponent<BoxCollider>();
		this.boundX = (int)(collider.size.x * transform.localScale.x);
		this.boundY = (int)(collider.size.y * transform.localScale.y);
	}

	public void Init(int number, BBEntityStats stats) {
		this.number = number;
		this.stats = stats;
	}

	public virtual void Update() {
		if (this.knockback != null) {
			this.TakeKnockback(this.knockback);
		}
		BBNode currentInhabitedNode = this.gridController.NodeFromWorldPoint(transform.position);
		//	If previous node not set, set and increment inhabited count
		if (this.previousInhabitedNode == null) {
			this.previousInhabitedNode = currentInhabitedNode;
			this.previousInhabitedNode.InhabitedCount++;
			if (this.gridController.UnhibitedNodes.Contains(this.previousInhabitedNode)) {
				this.gridController.UnhibitedNodes.Remove(this.previousInhabitedNode);
			}
		} else if (!currentInhabitedNode.Equals(this.previousInhabitedNode)) {
		//If moving to new node, decrement other count and increase own
			this.previousInhabitedNode.InhabitedCount--;
			if (this.previousInhabitedNode.InhabitedCount < 0) {
				BBErrorHelper.DLog(BBErrorConstants.InvalidValueUpdate, "Inhabited node count went below zero");
			}
			if (this.previousInhabitedNode.InhabitedCount == 0 && !this.gridController.UnhibitedNodes.Contains(this.previousInhabitedNode)) {
				this.gridController.UnhibitedNodes.Add(this.previousInhabitedNode);
			}
			this.previousInhabitedNode = currentInhabitedNode;
			if (this.gridController.UnhibitedNodes.Contains(this.previousInhabitedNode)) {
				this.gridController.UnhibitedNodes.Remove(this.previousInhabitedNode);
			}
			this.previousInhabitedNode.InhabitedCount++;
		}
	}
	
	//Take hit without knockback using OnTrigger events w/o knockback
	public void TakeHit(int power, Collider collider) {
		this.animatedEntity.TakeHit();
		int targetDamage = power - this.stats.Defense;
		this.damageSpeech.TakeHit(targetDamage);
		if (targetDamage < 0) {
			targetDamage = 0; 
		}
		this.stats.Health -= targetDamage;
		if (this.stats.Health <= .0f) {
			this.Die();
		}
	}
	
	//Take hit without knockback using OnTrigger events with knockback
	public void TakeHit(int power, Collider collider, BBKnockback knockback) {
		this.TakeHit(power, collider);
		if (this.controller != null && !gameObject.tag.Equals(BBSceneConstants.deadTag)) {
			this.knockback = knockback;
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
	protected void Die() {
		string tag = gameObject.tag;
		gameObject.tag = BBSceneConstants.deadTag;
		if (transform.parent != null) {
			transform.parent.gameObject.tag = BBSceneConstants.deadTag; 
		}
		BBEventController.SendDeathNotification(tag);
		//Notification sent notifying of death. GameController will handle the updates
		this.animatedEntity.Death();
	}
	
	public void Destruction() {
		if (transform.parent != null) {
			Destroy(transform.parent.gameObject);
		} else {
			Destroy(gameObject);
		}
	}
	
}
