using UnityEngine;
using System.Collections;
[RequireComponent (typeof (BoxCollider))]
public class Player : MonoBehaviour {
	public float maxSpeed = 5.0f;
	
	private Vector3 velocityVect;
	
	public float accelerationTime;
	
	public float jumpHeight = 4.0f;
	public float timeToJumpApex = 2.0f;
	
	private float gravity;
	private float jumpVelocity;
	
	private float velocityXSmoothing;
	private float velocityYSmoothing;
	private float velocityZSmoothing;

	private Controller3D controller;
	
	private Rigidbody rigidBody;
	
	// Use this for initialization
	void Start () {
		this.controller = gameObject.GetComponent<Controller3D>();
		this.maxSpeed = this.maxSpeed;
		this.gravity = PhysicsHelper.ObjectGravity(this.jumpHeight, this.timeToJumpApex);
		this.jumpVelocity = PhysicsHelper.JumpVelocity(this.gravity, this.timeToJumpApex);
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 input = new Vector2(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		Vector3 targetVelocityXY = new Vector3(input.x * this.maxSpeed, input.y * this.maxSpeed);
		this.velocityVect.x = Mathf.SmoothDamp(this.velocityVect.x, targetVelocityXY.x, ref this.velocityXSmoothing, this.accelerationTime);
		this.velocityVect.y = Mathf.SmoothDamp(this.velocityVect.y, targetVelocityXY.y, ref this.velocityYSmoothing, this.accelerationTime); 

		if (Input.GetButtonDown("Jump")) {
			if (this.controller.CollInfo.isBack) {
				this.velocityVect.z = this.jumpVelocity;
			}	
		}
		this.velocityVect.z += this.gravity * Time.deltaTime;
		this.controller.Move(this.velocityVect * Time.deltaTime);
		if (this.controller.CollInfo.isBack) {
			this.velocityVect.z = .0f;
		}
	}
	

}
