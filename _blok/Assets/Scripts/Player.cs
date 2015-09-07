using UnityEngine;
using System.Collections;
[RequireComponent (typeof (BoxCollider))]
public class Player : MonoBehaviour {
	[Range(0, 3)]
	public int playerNumber;
	public float maxSpeed = 5.0f;
	
	public float accelerationTime;
	
	public float jumpHeight = 4.0f;
	public float timeToJumpApex = 2.0f;
	
	private float gravity;
	private float jumpVelocity;
	
	private Vector3 velocityVect;
	private float velocityXSmoothing;
	private float velocityYSmoothing;
	private float velocityZSmoothing;

	private Controller3D controller;
	
	private Rigidbody rigidBody;
	
	// Use this for initialization
	void Start () {
		this.controller = gameObject.GetComponent<Controller3D>();
		this.gravity = PhysicsHelper.ObjectGravity(this.jumpHeight, this.timeToJumpApex);
		this.jumpVelocity = PhysicsHelper.JumpVelocity(this.gravity, this.timeToJumpApex);
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 input = new Vector2(Input.GetAxisRaw ("Horizontal" + playerNumber), Input.GetAxisRaw ("Vertical" + playerNumber));
		Vector3 targetVelocityXY = new Vector3(input.x * this.maxSpeed, input.y * this.maxSpeed);
		this.velocityVect.x = Mathf.SmoothDamp(this.velocityVect.x, targetVelocityXY.x, ref this.velocityXSmoothing, this.accelerationTime);
		this.velocityVect.y = Mathf.SmoothDamp(this.velocityVect.y, targetVelocityXY.y, ref this.velocityYSmoothing, this.accelerationTime); 

		if (Input.GetButtonDown("Jump" + playerNumber)) {
			if (this.controller.CollInfo.isBack) {
				this.velocityVect.z = this.jumpVelocity;
			}	
		}
		this.velocityVect.z += this.gravity * Time.deltaTime;
		this.controller.Move(this.velocityVect * Time.deltaTime, input);
		if (this.controller.CollInfo.isBack) {
			this.velocityVect.z = .0f;
		}
	}
	

}
