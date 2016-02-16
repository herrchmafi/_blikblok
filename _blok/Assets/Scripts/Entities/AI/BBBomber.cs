using UnityEngine;
using System.Collections;

public class BBBomber : BBLivingEntity {
	private BBGameController gameController;

	private Transform target;

	private BBTimer timer;
	private BBPathfinder pathFinder;

	// Use this for initialization
	public override void Start () {
		base.Start();
		this.gameController = GameObject.FindGameObjectWithTag(BBSceneConstants.gameControllerTag).GetComponent<BBGameController>();

		this.pathFinder = GetComponent<BBPathfinder>();
		this.timer = new BBTimer();

	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
		if (this.target == null) {
			if (this.gameController.Players.Length > 0) {
				this.target = this.gameController.Players[0].transform;
			}
		}
		if (Input.GetKeyDown("o")) {
			this.pathFinder.RequestPath(transform.position, this.target.position, this.Bounds2D, this.speed);
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (BBAIHelper.IsOpponent(gameObject, collider.gameObject)) {
			this.Die();
		}
	}
}
