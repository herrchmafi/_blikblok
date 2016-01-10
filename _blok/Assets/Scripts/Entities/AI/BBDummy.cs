using UnityEngine;
using System.Collections;

public class BBDummy : BBLivingEntity {

	public Transform target;
	public float speed = 5.0f;
	private Vector3[] path;
	private int targetIndex;

	// Use this for initialization
	public override void Start () {
		base.Start();
		BBPathRequestController.RequestPath(transform.position, target.position, OnPathFound);
	}
	
	public void OnPathFound(Vector3[] newPath, bool isPathSuccess) {
		if (isPathSuccess) {
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}
	
	IEnumerator FollowPath() {
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
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
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
