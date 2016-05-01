using UnityEngine;
using System.Collections;
[RequireComponent (typeof (BoxCollider))]
public class BBBasePlayerController : MonoBehaviour {

	[SerializeField]
	private int number;
	public int Number {
		get { return this.number; }
		set { this.number = value; }
	}
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
	
	private Vector2 playerInput;
	public Vector2 PlayerInput {
		get { return this.playerInput; }
	}
	
	private BBActionPlayerController actionPlayerController;
	
	// Use this for initialization
	void Start () {
		this.gravity = BBPhysicsHelper.ObjectGravity(this.jumpHeight, this.timeToJumpApex);
		this.jumpVelocity = BBPhysicsHelper.JumpVelocity(this.gravity, this.timeToJumpApex);
		this.controller = gameObject.GetComponent<BBController3D>();
		transform.parent = GameObject.FindGameObjectWithTag(BBSceneConstants.playersTag).transform;
	}

	public void Init(int number, BBEntityStats stats) {
		this.number = number;
		this.actionPlayerController = transform.Find(BBSceneConstants.actionEntity).GetComponent<BBActionPlayerController>();
		this.actionPlayerController.Init(number, stats);
		GameObject.FindGameObjectWithTag(BBSceneConstants.canvasControllerTag).GetComponent<BBCanvasController>().SyncPlayerStat(this.number, stats);
	}
	
	// Update is called once per frame
	void Update () {
		if (tag.Equals(BBSceneConstants.deadTag)) { return; }
		this.playerInput = new Vector2(Input.GetAxisRaw (BBSceneConstants.horizontalInput + this.number), Input.GetAxisRaw (BBSceneConstants.verticalInput + this.number));
		Vector3 targetVelocityXY = new Vector3(this.playerInput.x * this.maxSpeed, this.playerInput.y * this.maxSpeed);
		this.velocityVect.x = Mathf.SmoothDamp(this.velocityVect.x, targetVelocityXY.x, ref this.velocityXSmoothing, this.accelerationTime);
		this.velocityVect.y = Mathf.SmoothDamp(this.velocityVect.y, targetVelocityXY.y, ref this.velocityYSmoothing, this.accelerationTime); 
		
		//Jumping Logic
		if (this.controller.CollInfo.isBack) {
			if (Input.GetButtonDown(BBSceneConstants.jumpInput + this.number)) {
				this.actionPlayerController.Jump();
				this.velocityVect.z = this.jumpVelocity;
			} else if ((Mathf.Abs(this.playerInput.x) > 0 || Mathf.Abs(this.playerInput.y) > 0)) {
				this.actionPlayerController.Walk();
			} else {
				this.actionPlayerController.Idle();
			}	
		}
		
		this.velocityVect.z += this.gravity * Time.deltaTime;
		this.controller.Move(this.velocityVect * Time.deltaTime);
		if (this.controller.CollInfo.isBack) {
			this.velocityVect.z = .0f;
		}
		//Looking
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = Camera.main.transform.position.z - transform.position.z;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		Vector2 distToMouse = new Vector2(mousePos.x - screenPos.x, mousePos.y - screenPos.y);
		
		float angleBetweenPosAndMouse = Mathf.Atan2(distToMouse.y, distToMouse.x) * Mathf.Rad2Deg + BBPhysicsConstants.dirOffset;
		this.actionPlayerController.Look(new Vector3(0, 0, angleBetweenPosAndMouse));
		
		//Player input
		if (Input.GetButtonDown(BBSceneConstants.normalAttackInput)) {
			this.actionPlayerController.NormalAttack();
		} else if(Input.GetButtonDown(BBSceneConstants.specialAttackInput)) {
			this.actionPlayerController.SpecialAttack();
		}
	}
}
