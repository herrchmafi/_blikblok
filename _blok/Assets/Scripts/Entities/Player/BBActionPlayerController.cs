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

	public Transform normalFab;
	private Transform normalFabLocal;

	public Transform specialFab;

	private BBAnimatedEntity animatedPlayer;
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		this.currentState = State.IDLE;
		this.normalFabLocal = (Transform)Instantiate(this.normalFab, transform.position, transform.rotation);
		this.normalFabLocal.parent = transform;
		this.normalFabLocal.localPosition += Vector3.up;
		this.animatedPlayer = transform.FindChild(BBSceneConstants.animatedEntity).GetComponent<BBAnimatedEntity>();
	}
	
	// Update is called once per frame
	public override void Update () {
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
		this.normalFabLocal.GetComponent<BBMelee>().IsAttacking = true;
	}
	
	public void SpecialAttack() {
		Transform localSpecialFab = (Transform)Instantiate(this.specialFab, transform.position, Quaternion.identity);
		BBDrawWeapon draw = localSpecialFab.GetComponent<BBDrawWeapon>();
		draw.Init(gameObject);
//		BBBounceBullet bounceBullet = localSpecialFab.GetComponent<BBBounceBullet>();
//		bounceBullet.Init(transform.up, gameObject);
	}
	
	public void Look(Vector3 lookVect) {
		transform.rotation = Quaternion.Euler(lookVect);
	}
	
}
