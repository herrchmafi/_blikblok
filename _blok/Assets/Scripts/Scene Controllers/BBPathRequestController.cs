using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BBPathRequestController : MonoBehaviour {

	private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	private PathRequest currentPathRequest;
	
	private static BBPathRequestController instance;
	private HTPathfinding pathfinding;
	
	private bool isProcessingPath;
	
	struct PathRequest {
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Vector2 finalSize;

		public Action<Vector3[], bool> callback;
		
		public PathRequest(Vector3 pathStart, Vector3 pathEnd, Vector2 finalSize, Action<Vector3[], bool> callback) {
			this.pathStart = pathStart;
			this.pathEnd = pathEnd;
			this.finalSize = finalSize;
			this.callback = callback;
		}
	}
	
	void Awake() {
		BBPathRequestController.instance = this;
		this.pathfinding = GetComponent<HTPathfinding>();
	}
	
	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Vector2 finalSize, Action<Vector3[], bool> callback) {
		PathRequest newRequest = new PathRequest(pathStart, pathEnd, finalSize, callback);
		BBPathRequestController.instance.pathRequestQueue.Enqueue(newRequest);
		BBPathRequestController.instance.TryProcessNext();
	}
	
	private void TryProcessNext() {
		if (!this.isProcessingPath && this.pathRequestQueue.Count > 0) {
			this.currentPathRequest = this.pathRequestQueue.Dequeue();
			this.isProcessingPath = true;
			this.pathfinding.StartFindPath(this.currentPathRequest.pathStart, this.currentPathRequest.pathEnd);
		}
	}
	
	public void FinishedProcessingPath(Vector3[] path, bool success) {
		this.currentPathRequest.callback(path, success);
		this.isProcessingPath = false;
		this.TryProcessNext();
	}
}
