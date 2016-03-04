using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BBPathfinding : MonoBehaviour {
	
	private BBPathRequestController requestManager;

	private BBGridController grid;
	
	private int diagonalMultiplier = 14;
	private int straightMultiplier = 10;
	
	void Awake() {
		this.requestManager = GetComponent<BBPathRequestController>();
		this.grid = GetComponent<BBGridController>();
	}
	
	
	public void StartFindPath(Vector3 startPos, Vector3 targetPos, BBCoordinate bound) {
		StartCoroutine(this.FindPath (startPos, targetPos, bound));
	}

	private IEnumerator FindPath(Vector3 startPos, Vector3 targetPos, BBCoordinate bound) {
	
		Vector3[] waypoints = new Vector3[0];
		bool isPathSuccess = false;
		
		BBNode startNode = this.grid.NodeFromWorldPoint(startPos);
		BBNode targetNode = this.grid.NodeFromWorldPoint(targetPos);
		
		if (startNode.IsWalkable && targetNode.IsWalkable) {
		
			BBHeap<BBNode> openSet = new BBHeap<BBNode>(this.grid.MaxSize);
			HashSet<BBNode>	closedSet = new HashSet<BBNode>();
			openSet.Add(startNode);
	
			while (openSet.Count > 0) {
				BBNode currentNode = openSet.RemoveFirst();
	
				closedSet.Add(currentNode);
	
				if (currentNode == targetNode) {
					isPathSuccess = true;
					break;
				}
				foreach (BBNode neighbour in grid.GetNeighbours(currentNode)) {
					if (!neighbour.IsWalkable || closedSet.Contains(neighbour)) { continue; }
					if (grid.IsDiagonalMove(currentNode, neighbour)) {
						if (!grid.IsDiagonalMoveValid(currentNode, neighbour, bound)) { 
							continue; 
						}
					}
					// Cost is the cost to the current position + cost to next node + penalty
					int costToNeighbour = currentNode.GCost + this.GetDistance(currentNode, neighbour) + neighbour.TerrainPenalty;
					if (costToNeighbour < neighbour.GCost || !openSet.Contains(neighbour)) {
						neighbour.GCost = costToNeighbour;
						neighbour.HCost = this.GetDistance(neighbour, targetNode);
						neighbour.Parent = currentNode;
				
						if (!openSet.Contains(neighbour)) {
							openSet.Add (neighbour);
						} else {
							openSet.UpdateItem(neighbour);
						}
					} 
				}
			}
			yield return null;
			if (isPathSuccess) {
				waypoints = this.RetracePath(startNode, targetNode);
			}
			this.requestManager.FinishedProcessingPath(waypoints, isPathSuccess);
		}
	}
	
	private Vector3[] RetracePath(BBNode startNode, BBNode endNode) {
		List<BBNode> path = new List<BBNode>();
		BBNode currentNode = endNode;
		
		//Bubble back up to original node
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.Parent;
		}
		Vector3[] waypoints = this.SimplifyPath(path);
		Array.Reverse(waypoints);
		return waypoints;
	}
	
	//Decreases path count for identical direction points
	private Vector3[] SimplifyPath(List<BBNode> path) {
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;
		
		for (int i = 1; i < path.Count; i++) {
			Vector2 directionNew = new Vector2(path[i - 1].Coordinate.X - path[i].Coordinate.X, path[i - 1].Coordinate.Y - path[i].Coordinate.Y);
			waypoints.Add(path[i].WorldPos);
		}
		return waypoints.ToArray();
	}
	
	private int GetDistance(BBNode a, BBNode b) {
		int distX = Mathf.Abs(a.Coordinate.X - b.Coordinate.X);
		int distY = Mathf.Abs(a.Coordinate.Y - b.Coordinate.Y);
		
		if (distX > distY) {
			return this.diagonalMultiplier * distY + this.straightMultiplier * (distX - distY);
		}
		return this.diagonalMultiplier * distX + this.straightMultiplier * (distY - distX);
	}
}
