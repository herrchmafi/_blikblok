using UnityEngine;
using System.Collections;

public class BBDummy : BBLivingEntity {
	
	public float speed = 5.0f;
	
	private float pathUpdateTime = .25f;
	private BBTimer timer;

	public Transform target;
	private int targetIndex;
	
	private Vector3[] path;
	
	private Bounds bounds;
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		this.bounds = GetComponent<Collider>().bounds;
		this.timer = new BBTimer();
		this.timer.Start();
	}
	
	public override void Update() {
		base.Update();
		this.timer.Update();
		if (this.target != null && this.timer.Seconds >= this.pathUpdateTime) {
			BBPathRequestController.RequestPath(transform.position, this.target.position, OnPathFound);
			this.timer.Reset();
		}
	}
	
	private void OnPathFound(Vector3[] newPath, bool isPathSuccess) {
		if (isPathSuccess) {
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}
	
	private IEnumerator FollowPath() {
		Vector3 currentWaypoint = path[0];
		
		while (true) {
			if (transform.position == currentWaypoint) {
				this.targetIndex++;
				if (this.targetIndex >= this.path.Length) {
					yield break;
				}
				currentWaypoint = this.path[this.targetIndex];
			}
			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
			yield return null;
		}
	}
	
	public void OnDrawGizmos() {
		if (this.path != null) {
			for (int i = this.targetIndex; i < this.path.Length; i++) {
				Gizmos.color = Color.black;
				Gizmos.DrawCube(this.path[i], Vector3.one);
				
				if (i == this.targetIndex) {
					Gizmos.DrawLine(transform.position, path[i]);
				} else {
					Gizmos.DrawLine(path[i - 1], path[i]);
				}
			}
		}
	}
}
