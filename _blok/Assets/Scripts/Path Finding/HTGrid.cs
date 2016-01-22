using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HTGrid : MonoBehaviour {
	public bool isDisplayingGridGizmos;
	
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;

	public float nodeRadius;
	public HTNode[,] grid;
	
	private float nodeDiameter;
	private HTVector2Int gridSize;
	
	public int MaxSize {
		get { return this.gridSize.X * this.gridSize.Y; }
	}
	
	void Awake() {
		this.nodeDiameter = this.nodeRadius * 2;
		this.gridSize = new HTVector2Int(Mathf.RoundToInt(this.gridWorldSize.x / this.nodeDiameter), Mathf.RoundToInt(this.gridWorldSize.y / this.nodeDiameter));
		this.CreateGrid();
	}
	
	public void CreateGrid() {
		this.grid = new HTNode[this.gridSize.X, this.gridSize.Y];
		Vector3 worldBottomLeft = transform.position - Vector3.right * this.gridWorldSize.x / 2 - Vector3.up * this.gridWorldSize.y / 2;
		//Creates path grid starting from bottom left
		for (int x = 0; x < this.gridSize.X; x++) {
			for (int y = 0; y < this.gridSize.Y; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * this.nodeDiameter + this.nodeRadius) 
				+ Vector3.up * (y * this.nodeDiameter + this.nodeRadius)
				+ BBSceneConstants.collidedGroundVect;
				bool isWalkable = !(Physics.CheckSphere(worldPoint, this.nodeRadius - .01f, this.unwalkableMask));
				int movementPenalty = 0;
				this.grid[x, y] = new HTNode(isWalkable, worldPoint, new HTVector2Int(x, y), movementPenalty);
			}
		}
	}
	
	public List<HTNode> GetNeighbours(HTNode node) {
		List<HTNode> neighbhours = new List<HTNode>();
		//Checks adjacent nodes and adds them to list if within grid
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0) { continue; }
				int checkX = node.Coordinate.X + x;
				int checkY = node.Coordinate.Y + y;
				if ((checkX >= 0 && checkX < this.gridSize.X) && (checkY >= 0 && checkY < this.gridSize.Y)) {
					neighbhours.Add(grid[checkX, checkY]);
				}
			}
		}
		return neighbhours;
	}

	public bool IsDiagonalMove(HTNode startNode, HTNode targetNode) {
		return (startNode.Coordinate.X != targetNode.Coordinate.X && startNode.Coordinate.Y != targetNode.Coordinate.Y);
	}

	//Checks if adjacent horizontal and vertical nodes would be cut off during a diagonal move
	public bool IsDiagonalMoveValid(HTNode startNode, HTNode targetNode, HTVector2Int bound) {
		//Set Indices accordingly and check if valid
		int horizontalX = (targetNode.Coordinate.X < startNode.Coordinate.X) ? startNode.Coordinate.X - bound.X : startNode.Coordinate.X + bound.X;
		if ((horizontalX < 0) || (horizontalX >= this.gridSize.X)) { return false; } 
		int verticalY = (targetNode.Coordinate.Y < startNode.Coordinate.Y) ? startNode.Coordinate.Y - bound.Y : startNode.Coordinate.Y + bound.Y;
		if ((verticalY < 0) || (verticalY >= this.gridSize.Y)) { return false; }
		HTVector2Int horizontalIndex = new HTVector2Int(horizontalX, startNode.Coordinate.Y);
		HTVector2Int verticalIndex = new HTVector2Int(startNode.Coordinate.X, verticalY);
		return (grid[horizontalIndex.X, horizontalIndex.Y].IsWalkable
		&& grid[verticalIndex.X, verticalIndex.Y].IsWalkable);
	}
	
	//Converts world position to grid coordinate node
	public HTNode NodeFromWorldPoint(Vector3 worldPos) {
		float percentX = Mathf.Clamp01((worldPos.x + this.gridWorldSize.x / 2) / this.gridWorldSize.x);
		float percentY = Mathf.Clamp01((worldPos.y + this.gridWorldSize.y / 2) / this.gridWorldSize.y);
		
		int x = Mathf.RoundToInt((this.gridSize.X - 1) * percentX);
		int y = Mathf.RoundToInt((this.gridSize.Y - 1) * percentY);
		
		return grid[x, y];
	}
	
	void OnDrawGizmos() {
		if (!Application.isPlaying) { return; }
		Gizmos.DrawWireCube(transform.position, new Vector3(this.gridSize.X, this.gridSize.Y, 0));
		if (this.grid != null && this.isDisplayingGridGizmos) {
			foreach (HTNode node in grid) {
				Gizmos.color = (node.IsWalkable) ? Color.white : Color.red;
				Gizmos.DrawCube(node.WorldPos, Vector3.one * (this.nodeDiameter -.1f));
				
			}
		}
	}
	
	
}