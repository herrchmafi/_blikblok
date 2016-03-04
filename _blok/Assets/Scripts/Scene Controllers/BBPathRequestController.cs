using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BBPathRequestController : MonoBehaviour {

	private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	private PathRequest currentPathRequest;
	
	private static BBPathRequestController instance;
	private BBPathfinding pathfinding;
	
	private bool isProcessingPath;
	
	struct PathRequest {
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public BBCoordinate bound;

		public Action<Vector3[], bool> callback;
		
		public PathRequest(Vector3 pathStart, Vector3 pathEnd, BBCoordinate bound, Action<Vector3[], bool> callback) {
			this.pathStart = pathStart;
			this.pathEnd = pathEnd;
			this.bound = bound;
			this.callback = callback;
		}
	}
	
	void Awake() {
		BBPathRequestController.instance = this;
		this.pathfinding = GetComponent<BBPathfinding>();
	}
	
	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, BBCoordinate bound, Action<Vector3[], bool> callback) {
		PathRequest newRequest = new PathRequest(pathStart, pathEnd, bound, callback);
		BBPathRequestController.instance.pathRequestQueue.Enqueue(newRequest);
		BBPathRequestController.instance.TryProcessNext();
	}
	
	private void TryProcessNext() {
		if (!this.isProcessingPath && this.pathRequestQueue.Count > 0) {
			this.currentPathRequest = this.pathRequestQueue.Dequeue();
			this.isProcessingPath = true;
			this.pathfinding.StartFindPath(this.currentPathRequest.pathStart, this.currentPathRequest.pathEnd, this.currentPathRequest.bound);
		}
	}
	
	public void FinishedProcessingPath(Vector3[] path, bool success) {
		this.currentPathRequest.callback(path, success);
		this.isProcessingPath = false;
		this.TryProcessNext();
	}
}
