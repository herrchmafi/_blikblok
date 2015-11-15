using UnityEngine;
using System.Collections;
using Pathfinding;

public class BBBomber : BBLivingEntity {
	private BBGameController gameController;
	private Seeker seeker;
	private Path path;
	
	public float speed = 2.0f;
	public float nextWaypointDistance = .0f;
	
	private Transform targetTransform;
	private Vector2 previousTargetPosition;
		
	private float updateTime = .25f;
	private BBTimer targetTimer;
	
	int currentWaypoint;
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		this.gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<BBGameController>();
		transform.GetComponent<BoxCollider>().center = BBSceneConstants.AStarColliderCenter;

		this.targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
		this.previousTargetPosition = this.targetTransform.position;
		
		this.seeker = gameObject.GetComponent<Seeker>();
		this.seeker.StartPath(transform.position, this.targetTransform.position);
		this.seeker.pathCallback += OnPathComplete;
		
		this.targetTimer = new BBTimer();
		this.targetTimer.Start();
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
		this.targetTimer.Update();
		if (this.targetTimer.Seconds >= this.updateTime) {
			this.targetTimer.Reset();
			if (this.previousTargetPosition != (Vector2)this.targetTransform.position) {
				this.seeker.StartPath(transform.position, this.targetTransform.position);
				this.previousTargetPosition = this.targetTransform.position;
			} 
		}
		
		if (this.path == null) {
			return;
		}
		if (this.currentWaypoint >= path.vectorPath.Count) {
			return;
		}

		Vector3 dir = (this.path.vectorPath[this.currentWaypoint] - transform.position).normalized;
		Vector3 dist = dir * this.speed * Time.deltaTime;
		transform.Translate(dist);
		
		if (Vector3.Distance (transform.position, path.vectorPath[this.currentWaypoint]) < this.nextWaypointDistance) {
			this.currentWaypoint++;
			return;
		}
	
	}
	
	void OnDisable() {
		this.seeker.pathCallback -= OnPathComplete;
	}
	
	public void OnPathComplete(Path path) {
		if (!path.error) {
			this.path = path;
			this.currentWaypoint = 0;
		}
	}
}
