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
	
	private Animator animator;
	// Use this for initialization
	void Start () {
		this.animator = gameObject.GetComponent<Animator>();
		this.currentState = State.IDLE;
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	public void Idle() {
		this.currentState = State.IDLE;
		this.SetAnimationState(this.currentState);
	}
	
	public void Walk() {
		this.currentState = State.WALKING;
		this.SetAnimationState(this.currentState);
	}
	
	public void Jump() {
		this.currentState = State.JUMPING;
		this.SetAnimationState(this.currentState);
		print ("Jump");
	}
	
	public void Look(Vector3 lookVect) {
		transform.rotation = Quaternion.Euler(lookVect);
	}
	
	
	private void SetAnimationState(State state) {
		this.animator.SetInteger("Movement_State", (int)state);
	}
	
}
