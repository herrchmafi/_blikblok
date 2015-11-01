using UnityEngine;
using System.Collections;

public class BBActionPlayerController : BBLivingEntity {

	public enum State {
		SPAWNING = 0,
		IDLE = 1,
		WALKING = 2,
		JUMPING = 3,
	}
	[SerializeField]
	private State currentState;
	public State CurrentState {
		get { return this.currentState; }
	}
	
	public Transform meleeFab;
	private Transform meleeFabLocal;
	
	private BBAnimatedPlayer animatedPlayer;
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		this.currentState = State.IDLE;
		this.meleeFabLocal = (Transform)Instantiate(this.meleeFab, transform.position, transform.rotation);
		this.meleeFabLocal.parent = transform;
		this.meleeFabLocal.localPosition += Vector3.down;
		this.animatedPlayer = transform.FindChild(BBSceneConstants.animatedPlayer).GetComponent<BBAnimatedPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		base.Update();
	}
	
	public void Idle() {
		this.currentState = State.IDLE;
		this.animatedPlayer.SetAnimationState(this.currentState);
	}
	
	public void Walk() {
		this.currentState = State.WALKING;
		this.animatedPlayer.SetAnimationState(this.currentState);
	}
	
	public void Jump() {
		this.currentState = State.JUMPING;
		this.animatedPlayer.SetAnimationState(this.currentState);
	}
	
	public void NormalAttack() {
		this.meleeFabLocal.GetComponent<BBMelee>().IsAttacking = true;
	}
	
	public void SpecialAttack() {
	
	}
	
	public void Look(Vector3 lookVect) {
		transform.rotation = Quaternion.Euler(lookVect);
	}
	
	
	
}
