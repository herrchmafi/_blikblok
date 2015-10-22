using UnityEngine;
using System.Collections;

public class BBAnimatedPlayer : MonoBehaviour {
	private Animator animator;
	// Use this for initialization
	void Start () {
		this.animator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetAnimationState(BBActionPlayerController.State state) {
		this.animator.SetInteger("Movement_State", (int)state);
	}
}
