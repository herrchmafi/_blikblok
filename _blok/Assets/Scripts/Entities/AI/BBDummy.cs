using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BBPathfinder))]
public class BBDummy : BBLivingEntity {
	
	private Bounds bounds;

	private BBGameController gameController;

	private BBPathfinder pathFinder;
	private Transform target;
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		this.bounds = GetComponent<Collider>().bounds;
		this.gameController = GameObject.FindGameObjectWithTag(BBSceneConstants.gameControllerTag).GetComponent<BBGameController>();
		this.pathFinder = GetComponent<BBPathfinder>();
	}
	
	public override void Update() {
		base.Update();
		if (this.target == null) {
			if (this.gameController.Players.Length > 0) {
				this.target = this.gameController.Players[0].transform;
			}
		}
		if (Input.GetKeyDown("p")) {
			this.pathFinder.RequestPath(transform.position, this.target.position, this.Bounds2D, this.Stats.Speed);
		}
	}
	

}
