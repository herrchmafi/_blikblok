using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BBGridController : MonoBehaviour {
	public bool isDisplayingGridGizmos;
	
	public LayerMask unwalkableMask;
	public Vector2 gridDimensions;

	private Vector2 gridWorldSize {
		get { return this.gridDimensions * this.nodeDiameter; }
	}

	public float nodeRadius;
	public BBNode[,] grid;

	private List<BBNode> uninhabitedNodes = new List<BBNode>();
	public List<BBNode> UnhibitedNodes {
		get { return this.uninhabitedNodes; }
		set { this.uninhabitedNodes = value; }
	}
	private float nodeDiameter;
	private BBCoordinate gridSize;
	
	public int MaxSize {
		get { return this.gridSize.X * this.gridSize.Y; }
	}
	
	void Awake() {
		this.nodeDiameter = this.nodeRadius * 2;
		this.gridSize = new BBCoordinate(Mathf.RoundToInt(this.gridWorldSize.x / this.nodeDiameter), Mathf.RoundToInt(this.gridWorldSize.y / this.nodeDiameter));
		this.CreateGrid();
	}
	
	public void CreateGrid() {
		this.grid = new BBNode[this.gridSize.X, this.gridSize.Y];
		Vector3 worldBottomLeft = transform.position - Vector3.right * this.gridWorldSize.x / 2 - Vector3.up * this.gridWorldSize.y / 2;
		//	Creates path grid starting from bottom left
		for (int x = 0; x < this.gridSize.X / this.nodeDiameter; x++) {
			for (int y = 0; y < this.gridSize.Y / this.nodeDiameter; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x + this.nodeRadius) 
				+ Vector3.up * (y + this.nodeRadius)
				+ BBSceneConstants.collidedGroundVect;
				bool isWalkable = !(Physics.CheckSphere(worldPoint, this.nodeRadius - .01f, this.unwalkableMask));
				BBNode newNode = new BBNode(isWalkable, worldPoint, new BBCoordinate(x, y), 0);
				grid[x, y] = newNode;
				if (isWalkable) {
					this.uninhabitedNodes.Add(newNode);
				}
			}
		}
	}
	
	public List<BBNode> GetNeighbours(BBNode node) {
		List<BBNode> neighbhours = new List<BBNode>();
		//	Checks adjacent nodes and adds them to list if within grid
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0) { continue; }
				BBCoordinate checkCoordinate = new BBCoordinate(node.Coordinate.X + x, node.Coordinate.Y + y);
				if (this.IsCoordinateInBounds(checkCoordinate)) {
					neighbhours.Add(grid[checkCoordinate.X, checkCoordinate.Y]);
				}
			}
		}
		return neighbhours;
	}

	public bool IsDiagonalMove(BBNode startNode, BBNode targetNode) {
		return (startNode.Coordinate.X != targetNode.Coordinate.X && startNode.Coordinate.Y != targetNode.Coordinate.Y);
	}

	public bool IsCoordinateInBounds(BBCoordinate coordinate) {
		return (coordinate.X >= 0 && coordinate.Y < this.gridSize.X) && (coordinate.Y >= 0 && coordinate.Y < this.gridSize.Y);
	}

	//	Checks if adjacent horizontal and vertical nodes would be cut off during a diagonal move
	public bool IsDiagonalMoveValid(BBNode startNode, BBNode targetNode, BBCoordinate bound) {
		//Set Indices accordingly and check if valid
		int horizontalX = (targetNode.Coordinate.X < startNode.Coordinate.X) ? startNode.Coordinate.X - bound.X : startNode.Coordinate.X + bound.X;
		if ((horizontalX < 0) || (horizontalX >= this.gridSize.X)) { return false; } 
		int verticalY = (targetNode.Coordinate.Y < startNode.Coordinate.Y) ? startNode.Coordinate.Y - bound.Y : startNode.Coordinate.Y + bound.Y;
		if ((verticalY < 0) || (verticalY >= this.gridSize.Y)) { return false; }
		BBCoordinate horizontalIndex = new BBCoordinate(horizontalX, startNode.Coordinate.Y);
		BBCoordinate verticalIndex = new BBCoordinate(startNode.Coordinate.X, verticalY);
		return (grid[horizontalIndex.X, horizontalIndex.Y].IsWalkable
		&& grid[verticalIndex.X, verticalIndex.Y].IsWalkable);
	}

	public BBCoordinate NearestOpenCoordinate(BBCoordinate coordinate) {
		BBNode nearestOpenNode = this.NearestOpenNode(coordinate);
		if (nearestOpenNode != null) {
			return nearestOpenNode.Coordinate;
		} else {
			return null; 
		}
	}

	public BBNode NearestOpenNode(BBCoordinate coordinate) {
		if (this.isOpenNodeAtCoordinate(coordinate)) { return this.NodeFromCoordinate(coordinate); }
		Queue<BBCoordinate> coordinatesToSearch = new Queue<BBCoordinate>();
		coordinatesToSearch.Enqueue(coordinate);
		//	BFS for open node	
		while (coordinatesToSearch.Count > 0) {
			HashSet<BBCoordinate> searchedCoordinates = new HashSet<BBCoordinate>();
			BBCoordinate coor = coordinatesToSearch.Dequeue();
			if (this.isOpenNodeAtCoordinate(coor)) {
				print("Final " + coor.X + ", " + coor.Y);
				return this.NodeFromCoordinate(coor);
			}
			BBCoordinate left = new BBCoordinate(coor.X - 1, coor.Y);
			this.OpenNodeHelper(left, searchedCoordinates, coordinatesToSearch);
			BBCoordinate top = new BBCoordinate(coor.X, coor.Y + 1);
			this.OpenNodeHelper(top, searchedCoordinates, coordinatesToSearch);
			BBCoordinate right = new BBCoordinate(coor.X + 1, coor.Y);
			this.OpenNodeHelper(right, searchedCoordinates, coordinatesToSearch);
			BBCoordinate bottom = new BBCoordinate(coor.X, coor.Y - 1);
			this.OpenNodeHelper(bottom, searchedCoordinates, coordinatesToSearch);
			searchedCoordinates.Add(coor);
		}
		return null;
	}

	//	Used in conjunction with NearestOpenNode to reduce boilerplate
	private void OpenNodeHelper(BBCoordinate coordinate, HashSet<BBCoordinate> searchedCoordinates, Queue<BBCoordinate> coordinatesToSearch) {
		if (this.IsCoordinateInBounds(coordinate)) {
			print("Testing " + coordinate.X + ", " + coordinate.Y);
			if (!searchedCoordinates.Contains(coordinate) && !coordinatesToSearch.Contains(coordinate)) {
				print("Enqueing " + coordinate.X + ", " + coordinate.Y);
				coordinatesToSearch.Enqueue(coordinate);
			}
		}
	}

	public bool isOpenNode(BBNode node) {
		return this.isOpenNodeAtCoordinate(node.Coordinate);
	}

	public bool isOpenNodeAtCoordinate(BBCoordinate coordinate) {
		if (coordinate.X >= this.gridWorldSize.x|| coordinate.Y >= this.gridWorldSize.y) {
			return false;
		}
		BBNode node = this.NodeFromCoordinate(coordinate);
		return (node.IsWalkable && node.InhabitedCount == 0);
	}
	
	//	Converts world position to grid coordinate node
	public BBNode NodeFromWorldPoint(Vector3 worldPos) {
		float percentX = Mathf.Clamp01((worldPos.x + this.gridWorldSize.x / 2) / this.gridWorldSize.x);
		float percentY = Mathf.Clamp01((worldPos.y + this.gridWorldSize.y / 2) / this.gridWorldSize.y);
		
		int x = Mathf.RoundToInt((this.gridSize.X - 1) * percentX);
		int y = Mathf.RoundToInt((this.gridSize.Y - 1) * percentY);
		
		return this.grid[x, y];
	}

	public BBNode NodeFromCoordinate(BBCoordinate coordinate) {
		return this.grid[coordinate.X, coordinate.Y];
	}

	public Vector3 WorldPointFromCoordinate(BBCoordinate coordinate) {
		Vector3 center = transform.position;
		return new Vector3(center.x + (coordinate.X - (this.gridWorldSize.x / 2)), center.y + (coordinate.Y - (this.gridWorldSize.y / 2)), BBSceneConstants.collidedGround);
	}

	public Vector3 WorldPointFromNode(BBNode node) {
		return WorldPointFromCoordinate(node.Coordinate);
	}

	public BBCoordinate CoordinateFromWorldPoint(Vector2 worldPos) {
		return this.NodeFromWorldPoint(worldPos).Coordinate;
	}
	
	void OnDrawGizmos() {
		if (!Application.isPlaying) { return; }
		Gizmos.DrawWireCube(transform.position, new Vector3(this.gridSize.X, this.gridSize.Y, 0));
		if (this.grid != null && this.isDisplayingGridGizmos) {
			foreach (BBNode node in grid) {
				Gizmos.color = (node.IsWalkable) ? Color.white : Color.red;
				if (node.InhabitedCount > 0) {
					Gizmos.color = Color.blue;
				}
				Gizmos.DrawCube(node.WorldPos, Vector3.one * (this.nodeDiameter - .1f));
			}
		}
	}

}