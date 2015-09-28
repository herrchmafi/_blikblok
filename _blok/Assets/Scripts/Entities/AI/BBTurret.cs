using UnityEngine;
using System.Collections;

public class BBTurret : MonoBehaviour {
	enum State {
		SPAWNING,
		LOADING,
		ATTACKING,
		REFRESHING,
		ROTATING
	};
	
	[SerializeField]
	private State currentState;
	
	public GameObject bullet;
	
	public int rotations = 4;
	public int currentRotation = 1;
	public float angleOffset = .0f;
	
	public int shotsPerRotation = 1;
	private int currentShot;
	
	public float secondsPerRotation = 1.0f;
	public float secondsForLoad = .2f;
	public float secondsForRefresh = .2f;
	
	private Quaternion prevRotation;
	private Quaternion targetRotation;
	private BBTimer timer;
	
	// Use this for initialization
	void Start () {
		this.DetermineRotations();
		this.currentState = State.ROTATING;
		this.timer = new BBTimer();
		this.timer.Start();
	}
	
	// Update is called once per frame
	void Update () {
		this.timer.Update();
		
		//State Logic
		//              			-------------   
		//              			|	    	| 
		//							v			|
		//SPAWNING ->  ROTATING -> LOADING -> ATTACKING -> REFRESHING
		//				^									|
		//				|									|
		//				-------------------------------------
		switch (this.currentState) {
			case State.SPAWNING:
				break;
			case State.LOADING:
				if (this.timer.Seconds >= this.secondsForLoad) {
					this.Fire();
				}
				break;
			case State.ATTACKING:
				if (this.currentShot >= this.shotsPerRotation) {
					this.currentShot = 0;
					this.currentState = State.REFRESHING;
					this.timer.Start();	
				} else {
					this.currentState = State.LOADING;
					this.timer.Start();
				}
				break;
			case State.REFRESHING:
				if (this.timer.Seconds >= this.secondsForRefresh) {
					this.currentState = State.ROTATING;
					this.timer.Reset();
				}
				break;
			case State.ROTATING:
				float u = this.timer.Seconds / this.secondsPerRotation;
				transform.rotation = Quaternion.Lerp(this.prevRotation, this.targetRotation, u);
				if (this.timer.Seconds >= this.secondsPerRotation) {
					this.timer.Reset();
					this.currentRotation = (this.currentRotation + 1) % this.rotations;
					this.DetermineRotations();
					this.currentState = State.LOADING;
				}	
				break;
		}
	}
	
	private void DetermineRotations() {
		float prevAngle = (float)this.currentRotation / this.rotations * BBPhysicsConstants.degreesPerRevolution;
		float targetAngle = (float)(this.currentRotation + 1) % this.rotations / this.rotations * BBPhysicsConstants.degreesPerRevolution;
		this.prevRotation = Quaternion.Euler(Vector3.forward * (prevAngle + angleOffset));
		this.targetRotation = Quaternion.Euler(Vector3.forward * (targetAngle + angleOffset));
	}
	
	private void Fire() {
		this.timer.Stop();
		GameObject bulletObject = (GameObject) Instantiate(this.bullet, transform.position + transform.up, transform.rotation);
		bulletObject.GetComponent<BBTurretBullet>().FireInDir(transform.up);
		this.currentState = State.ATTACKING;
		this.currentShot++;
	}
}
