using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Designed for living animatable objects
public class BBAnimatedEntity : BBAnimated {
	public Transform hitFab;
	public Color hitColor;
	
	// Use this for initialization
	public override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}
	
	//This is called by parent class. A hit prefab is instantiated and the object flashes to corresponding hitColor
	public void TakeHit() {
		Transform hitTransform = (Transform)Instantiate(this.hitFab, transform.parent.position, transform.parent.rotation);
		hitTransform.parent = transform;
		this.colorChange.Renderer.material.color = this.colorChange.DefaultColor;
		this.colorChange.ChangeColors(new List<BBColorChange.ColorChange>() {
			new BBColorChange.ColorChange(this.hitColor, BBGraphicsConstants.toHitColorChangeTime),
			new BBColorChange.ColorChange(this.colorChange.DefaultColor, BBGraphicsConstants.afterHitColorChangeTime)
		});
	}
	
	
}
