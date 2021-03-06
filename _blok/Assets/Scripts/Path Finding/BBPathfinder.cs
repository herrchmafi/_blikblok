﻿using UnityEngine;
using System.Collections;

public class BBPathfinder : MonoBehaviour {

	public Transform target;
	private int targetIndex;

	private Vector3[] path;

	private float speed;

	private BBTimer timer;

	void Start() {
		
	}

	public void RequestPath(Vector3 startPos, Vector3 targetPos, BBCoordinate bound, float speed) {
		this.targetIndex = 0;
		this.speed = speed;
		BBPathRequestController.RequestPath(startPos, targetPos, bound, OnPathFound);
	}

	private void OnPathFound(Vector3[] newPath, bool isPathSuccess) {
		if (isPathSuccess) {
			path = newPath;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}
	
	private IEnumerator FollowPath() {
		if (path.Length == 0) {
			yield break;
		}
		Vector3 currentWaypoint = path[0];
		while (true) {
			if (transform.position == currentWaypoint) {
				this.targetIndex++;
				if (this.targetIndex >= this.path.Length) {
					yield break;
				}
				currentWaypoint = this.path[this.targetIndex];
			}
			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, this.speed * Time.deltaTime);
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
