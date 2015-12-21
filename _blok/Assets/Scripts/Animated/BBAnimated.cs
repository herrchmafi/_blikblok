using UnityEngine;
using System.Collections;

//This class is the base class for animatable objects
[RequireComponent(typeof(BBColorChange))]
public class BBAnimated : MonoBehaviour {
	protected BBColorChange colorChange;
	
	private Animator animator;
		
	// Use this for initialization
	public virtual void Start () {
		this.animator = gameObject.GetComponent<Animator>();
		this.colorChange = gameObject.GetComponent<BBColorChange>();
	}
	
	// Update is called once per frame
	public virtual void Update () {

	}
	
	public void SetAnimationState(BBActionPlayerController.State state) {
		if (this.animator != null) {
			this.animator.SetInteger(BBAnimatorConstants.StateParam, (int)state);
		}
	}
	
	public void SetAnimationTrigger(string trigger) {
		if (this.animator == null) {
			this.animator = gameObject.GetComponent<Animator>();
		}
		this.animator.SetTrigger(trigger);
	}
	
}
