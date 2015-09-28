using UnityEngine;
using System.Collections;
[RequireComponent (typeof (BoxCollider))]
public class BBPlayer : BBLivingEntity {
	[Range(0, 3)]
	public int playerNumber;
	
	public float maxSpeed = 5.0f;
	
	public float accelerationTime;
	
	public float jumpHeight = 4.0f;
	public float timeToJumpApex = 2.0f;
	
	enum MovementState {
		SPAWNING = 0,
		IDLE = 1,
		WALKING = 2,
		JUMPING = 3,
	}
	[SerializeField]
	private MovementState currentState;
	
	private float gravity;
	private float jumpVelocity;
	
	private Vector3 velocityVect;
	private float velocityXSmoothing;
	private float velocityYSmoothing;
	private float velocityZSmoothing;

	private BBController3D controller;
	
	private Rigidbody rigidBody;
	
	private Animator animator;
	
	// Use this for initialization
	void Start () {
		this.gravity = BBPhysicsHelper.ObjectGravity(this.jumpHeight, this.timeToJumpApex);
		this.jumpVelocity = BBPhysicsHelper.JumpVelocity(this.gravity, this.timeToJumpApex);
		this.controller = gameObject.GetComponent<BBController3D>();
		this.animator = gameObject.GetComponent<Animator>();
		this.currentState = MovementState.IDLE;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 input = new Vector2(Input.GetAxisRaw ("Horizontal" + playerNumber), Input.GetAxisRaw ("Vertical" + playerNumber));
		Vector3 targetVelocityXY = new Vector3(input.x * this.maxSpeed, input.y * this.maxSpeed);
		this.velocityVect.x = Mathf.SmoothDamp(this.velocityVect.x, targetVelocityXY.x, ref this.velocityXSmoothing, this.accelerationTime);
		this.velocityVect.y = Mathf.SmoothDamp(this.velocityVect.y, targetVelocityXY.y, ref this.velocityYSmoothing, this.accelerationTime); 
		
		//Jumping Logic
		if (this.controller.CollInfo.isBack) {
			if (Input.GetButtonDown("Jump" + this.playerNumber)) {
				this.currentState = MovementState.JUMPING;
				this.velocityVect.z = this.jumpVelocity;
			} else if ((Mathf.Abs(input.x) > 0 || Mathf.Abs(input.y) > 0)) {
				this.currentState = MovementState.WALKING;
			} else {
				this.currentState = MovementState.IDLE;
			}	
		}
		
		//Set Animator State
		this.animator.SetInteger("Movement_State", (int)this.currentState);
		
		this.velocityVect.z += this.gravity * Time.deltaTime;
		this.controller.Move(this.velocityVect * Time.deltaTime, input);
		if (this.controller.CollInfo.isBack) {
			this.velocityVect.z = .0f;
		}
	}
	

	
	void OnCollisionEnter(Collision collision) {

	}
}
