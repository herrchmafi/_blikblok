using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HTPathfinding : MonoBehaviour {
	public Transform seeker;
	
	public Transform target;
	
	
	private int diagonalMultiplier = 14;
	private int straightMultiplier = 10;

	private HTGrid grid;
	
	void Awake() {
		this.grid = GetComponent<HTGrid>();
	}
	
	void Update() {
		this.FindPath(this.seeker.position, this.target.position);
	}

	public void FindPath(Vector2 startPos, Vector2 targetPos) {
		HTNode startNode = this.grid.NodeFromWorldPoint(startPos);
		HTNode targetNode = this.grid.NodeFromWorldPoint(targetPos);
		
		BBHeap<HTNode> openSet = new BBHeap<HTNode>(this.grid.MaxSize);
		HashSet<HTNode>	closedSet = new HashSet<HTNode>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			HTNode currentNode = openSet.RemoveFirst();

			closedSet.Add(currentNode);

			if (currentNode == targetNode) {
				this.RetracePath(startNode, currentNode);
				return;
			}
			foreach (HTNode neighbour in grid.GetNeighbours(currentNode)) {
				if (!neighbour.IsWalkable || closedSet.Contains(neighbour)) { continue; }
				// Cost is the cost to the current position + cost to next node
				int costToNeighbour = currentNode.GCost + this.GetDistance(currentNode, neighbour);
				if (costToNeighbour < neighbour.GCost || !openSet.Contains(neighbour)) {
					neighbour.GCost = costToNeighbour;
					neighbour.HCost = GetDistance(neighbour, targetNode);
					neighbour.Parent = currentNode;
					
					if (!openSet.Contains(neighbour)) {
						openSet.Add (neighbour);
					} else {
						openSet.UpdateItem(neighbour);
					}
				} 
			}
		}
	}
	
	private void RetracePath(HTNode startNode, HTNode endNode) {
		List<HTNode> path = new List<HTNode>();
		HTNode currentNode = endNode;
		
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.Parent;
		}
		path.Reverse();
		
		grid.path = path;
	}
	
	private int GetDistance(HTNode a, HTNode b) {
		int distX = Mathf.Abs(a.Coordinate.X - b.Coordinate.X);
		int distY = Mathf.Abs(a.Coordinate.Y - b.Coordinate.Y);
		
		if (distX > distY) {
			return this.diagonalMultiplier * distY + this.straightMultiplier * (distX - distY);
		}
		return this.diagonalMultiplier * distX + this.straightMultiplier * (distY - distX);
	}
}
