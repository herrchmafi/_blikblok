using UnityEngine;
using System.Collections;

public class BBBasher : MonoBehaviour {
	enum State {
		SPAWNING,
		LOADING,
		ATTACKING,
		REFRESHING
	}
	[SerializeField]
	private State currentState;
	
	public float secondsForLoad = .2f;
	public float secondsForRefresh = .2f;

	private BBGameController gameController;
	private GameObject target;
	// Use this for initialization
	void Start () {
		this.gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<BBGameController>();
		this.currentState = State.LOADING;
	}
	
	// Update is called once per frame
	void Update () {
//		//State Logic
//		//              			-------------   
//		//              			|	    	| 
//		//							v			|
//		//SPAWNING ->  LOADING -> ATTACKING -> REFRESHING
//		//				^									|
//		//				|									|
//		//				-------------------------------------
//		switch (this.currentState) {
//			case State.SPAWNING:
//				break;
//			case State.LOADING:
//				break;
//			case StateMachineBehaviour.
//		}	
	}
	
	private void SetTarget() {
		float maxDist = .0f;
		foreach (GameObject player in this.gameController.Players) {
			float playerDist = Vector3.Distance(transform.position, player.transform.position);
			if (playerDist > maxDist) {
				maxDist = playerDist;
				this.target = player;
			}
		}
	}
	
}
