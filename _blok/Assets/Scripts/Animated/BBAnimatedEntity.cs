using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Designed for living animatable objects
public class BBAnimatedEntity : BBAnimated {
	public Transform hitFab;
	public Color hitColor;
	
	public Transform deathFab;
	
	private BBTransformer transformer;
	
	public Vector3 deathScaleVect;
	public float deathShakeSeconds;
	public float deathShakeMagnitude;
	public float deathShakePeriod;
	
	private BBLivingEntity livingEntity;
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		this.transformer = gameObject.GetComponent<BBTransformer>();
		this.livingEntity = GetComponentInParent<BBLivingEntity>();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}
	
	//This is called by parent class. A hit prefab is instantiated and the object flashes to corresponding hitColor
	public void TakeHit() {
		//	Basically, for players we need the offset whereas everything else doesn't have one. This is due to the hierarchy
		Vector3 offset = (transform.localPosition.z.Equals(Vector3.zero)) ? Vector3.zero : BBSceneConstants.actionPlayerOffset;
		Transform hitTransform = (Transform)Instantiate(this.hitFab, transform.parent.position + offset, transform.parent.rotation);
		hitTransform.parent = transform;
		this.colorChange.Renderer.material.color = this.colorChange.DefaultColor;
		this.colorChange.ChangeColors(new List<BBColorChange.ColorChange>() {
			new BBColorChange.ColorChange(this.hitColor, BBGraphicsConstants.toHitColorChangeTime),
			new BBColorChange.ColorChange(this.colorChange.DefaultColor, BBGraphicsConstants.afterHitColorChangeTime)
		});
	}
	
	public void Death() {
		this.transformer.Scale(this.deathScaleVect, this.deathShakeSeconds);
		this.transformer.Shake(this.deathShakeSeconds, this.deathShakeMagnitude, this.deathShakePeriod);
		this.SetAnimationState(BBActionPlayerController.State.IDLE);
		Transform deathTransform = (Transform)Instantiate(this.deathFab, transform.parent.position, transform.parent.rotation);
		deathTransform.GetComponent<BBExplosion>().Explode(this.deathShakeSeconds);
		deathTransform.parent = transform;
	}
	
	
}
