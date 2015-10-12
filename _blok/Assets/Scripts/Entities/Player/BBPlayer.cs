using UnityEngine;
using System.Collections;
[RequireComponent (typeof (BoxCollider))]
public class BBPlayer : MonoBehaviour {
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

	private BBController3D controller;
	
	private BBGameController gameController;
	
	private BBActionPlayerController actionPlayerController;
	
	// Use this for initialization
	void Start () {
		this.gravity = BBPhysicsHelper.ObjectGravity(this.jumpHeight, this.timeToJumpApex);
		this.jumpVelocity = BBPhysicsHelper.JumpVelocity(this.gravity, this.timeToJumpApex);
		this.controller = gameObject.GetComponent<BBController3D>();
		this.gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<BBGameController>();
		this.actionPlayerController = transform.Find("Action Player").GetComponent<BBActionPlayerController>();
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
				this.actionPlayerController.Jump();
				this.velocityVect.z = this.jumpVelocity;
			} else if ((Mathf.Abs(input.x) > 0 || Mathf.Abs(input.y) > 0)) {
				this.actionPlayerController.Walk();
			} else {
				this.actionPlayerController.Idle();
			}	
		}
		
		this.velocityVect.z += this.gravity * Time.deltaTime;
		this.controller.Move(this.velocityVect * Time.deltaTime, input);
		if (this.controller.CollInfo.isBack) {
			this.velocityVect.z = .0f;
		}
		//Looking
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = Camera.main.transform.position.z - transform.position.z;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		Vector2 distToMouse = new Vector2(mousePos.x - screenPos.x, mousePos.y - screenPos.y);
		float angleBetweenPosAndMouse = Mathf.Atan2(distToMouse.y, distToMouse.x) * Mathf.Rad2Deg;
		this.actionPlayerController.Look(new Vector3(0, 0, angleBetweenPosAndMouse));
	}
	
	void OnCollisionEnter(Collision collision) {

	}
}
