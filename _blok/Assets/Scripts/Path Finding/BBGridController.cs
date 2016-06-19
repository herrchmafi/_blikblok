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
		get { return this.gridSize.x * this.gridSize.y; }
	}
	
	void Awake() {
		this.nodeDiameter = this.nodeRadius * 2;
		this.gridSize = new BBCoordinate(Mathf.RoundToInt(this.gridWorldSize.x / this.nodeDiameter), Mathf.RoundToInt(this.gridWorldSize.y / this.nodeDiameter));
		this.CreateGrid();
	}
	
	public void CreateGrid() {
		this.grid = new BBNode[this.gridSize.x, this.gridSize.y];
		Vector3 worldBottomLeft = transform.position - Vector3.right * this.gridWorldSize.x / 2 - Vector3.up * this.gridWorldSize.y / 2;
		//	Creates path grid starting from bottom left
		for (int x = 0; x < this.gridSize.x / this.nodeDiameter; x++) {
			for (int y = 0; y < this.gridSize.y / this.nodeDiameter; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x + this.nodeRadius) 
					+ Vector3.up * (y  + this.nodeRadius)
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
				BBCoordinate checkCoordinate = new BBCoordinate(node.Coordinate.x + x, node.Coordinate.y + y);
				if (this.IsCoordinateInBounds(checkCoordinate)) {
					neighbhours.Add(grid[checkCoordinate.x, checkCoordinate.y]);
				}
			}
		}
		return neighbhours;
	}

	public bool IsDiagonalMove(BBNode startNode, BBNode targetNode) {
		return (startNode.Coordinate.x != targetNode.Coordinate.x && startNode.Coordinate.y != targetNode.Coordinate.y);
	}

	public bool IsCoordinateInBounds(BBCoordinate coordinate) {
		return (coordinate.x >= 0 && coordinate.x < this.gridSize.x) && (coordinate.y >= 0 && coordinate.y < this.gridSize.y);
	}

	//	Checks if adjacent horizontal and vertical nodes would be cut off during a diagonal move
	public bool IsDiagonalMoveValid(BBNode startNode, BBNode targetNode, BBCoordinate bound) {
		//Set Indices accordingly and check if valid
		int horizontalX = (targetNode.Coordinate.x < startNode.Coordinate.x) ? startNode.Coordinate.x - bound.x : startNode.Coordinate.x + bound.x;
		if ((horizontalX < 0) || (horizontalX >= this.gridSize.x)) { return false; } 
		int verticalY = (targetNode.Coordinate.y < startNode.Coordinate.y) ? startNode.Coordinate.y - bound.y : startNode.Coordinate.y + bound.y;
		if ((verticalY < 0) || (verticalY >= this.gridSize.y)) { return false; }
		BBCoordinate horizontalIndex = new BBCoordinate(horizontalX, startNode.Coordinate.y);
		BBCoordinate verticalIndex = new BBCoordinate(startNode.Coordinate.x, verticalY);
		return (grid[horizontalIndex.x, horizontalIndex.y].IsWalkable
			&& grid[verticalIndex.x, verticalIndex.y].IsWalkable);
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
		if (this.IsOpenNodeAtCoordinate(coordinate)) { return this.NodeFromCoordinate(coordinate); }
		Queue<BBCoordinate> coordinatesToSearch = new Queue<BBCoordinate>();
		coordinatesToSearch.Enqueue(coordinate);
		//	BFS for open node
		HashSet<BBCoordinate> searchedCoordinates = new HashSet<BBCoordinate>();	
		while (coordinatesToSearch.Count > 0) {
			BBCoordinate coor = coordinatesToSearch.Dequeue();
			if (this.IsOpenNodeAtCoordinate(coor)) {
				return this.NodeFromCoordinate(coor);
			}
			BBCoordinate left = new BBCoordinate(coor.x - 1, coor.y);
			this.OpenNodeHelper(left, searchedCoordinates, coordinatesToSearch);
			BBCoordinate top = new BBCoordinate(coor.x, coor.y + 1);
			this.OpenNodeHelper(top, searchedCoordinates, coordinatesToSearch);
			BBCoordinate right = new BBCoordinate(coor.x + 1, coor.y);
			this.OpenNodeHelper(right, searchedCoordinates, coordinatesToSearch);
			BBCoordinate bottom = new BBCoordinate(coor.x, coor.y - 1);
			this.OpenNodeHelper(bottom, searchedCoordinates, coordinatesToSearch);
			searchedCoordinates.Add(coor);
		}
		return null;
	}

	//	Used in conjunction with NearestOpenNode to reduce boilerplate
	private void OpenNodeHelper(BBCoordinate coordinate, HashSet<BBCoordinate> searchedCoordinates, Queue<BBCoordinate> coordinatesToSearch) {
		if (!searchedCoordinates.Contains(coordinate) && !coordinatesToSearch.Contains(coordinate)) {
			if (this.IsCoordinateInBounds(coordinate)) {
				coordinatesToSearch.Enqueue(coordinate);
			}
		}
	
	}

	//	Determine valid nodes

	private bool IsCoordinateInGridBounds(BBCoordinate coordinate) {
		return !(coordinate.x >= this.gridWorldSize.x|| coordinate.y >= this.gridWorldSize.y);
	}

	public bool IsNodeAtCoordinate(BBCoordinate coordinate) {
		if (!this.IsCoordinateInBounds(coordinate)) {
			return false;
		}
		BBNode node = this.NodeFromCoordinate(coordinate);
		return node.IsWalkable;
	}
	public bool IsOpenNode(BBNode node) {
		return this.IsOpenNodeAtCoordinate(node.Coordinate);
	}

	public bool IsOpenNodeAtCoordinate(BBCoordinate coordinate) {
		if (!this.IsCoordinateInBounds(coordinate)) {
			return false;
		}
		BBNode node = this.NodeFromCoordinate(coordinate);
		return (node.IsWalkable && node.InhabitedCount == 0);
	}

	public BBGroundTile TileAtCoordinate(BBCoordinate coordinate) {
		if (!this.IsNodeAtCoordinate(coordinate)) {
			return null;
		}
		//	WorldPointFromCoordinate returns the collided ground. We want where the ground actually is for these calculations
		Collider[] colliders = Physics.OverlapSphere(this.WorldPointFromCoordinate(coordinate) - BBSceneConstants.collidedGroundVect, this.nodeRadius - .01f);
		foreach (Collider collider in colliders) {
			BBGroundTile tile = collider.gameObject.GetComponent<BBGroundTile>();
			if (tile != null) {
				return tile;
			}
		}
		return null;
	}
	
	//	Converts world position to grid coordinate node
	public BBNode NodeFromWorldPoint(Vector3 worldPos) {
		float percentX = Mathf.Clamp01((worldPos.x + this.gridWorldSize.x / 2) / this.gridWorldSize.x);
		float percentY = Mathf.Clamp01((worldPos.y + this.gridWorldSize.y / 2) / this.gridWorldSize.y);
		
		int x = Mathf.RoundToInt((this.gridSize.x - 1) * percentX);
		int y = Mathf.RoundToInt((this.gridSize.x - 1) * percentY);
		
		return this.grid[x, y];
	}

	public BBNode NodeFromCoordinate(BBCoordinate coordinate) {
		return this.grid[coordinate.x, coordinate.y];
	}

	public Vector3 WorldPointFromCoordinate(BBCoordinate coordinate) {
		Vector3 center = transform.position;
		return new Vector3(center.x + (coordinate.x - (this.gridWorldSize.x / 2)) + this.nodeRadius, center.y + (coordinate.y - (this.gridWorldSize.y / 2)) + this.nodeRadius, BBSceneConstants.collidedGround);
	}


	public Vector3 WorldPointFromNode(BBNode node) {
		return this.WorldPointFromCoordinate(node.Coordinate);
	}

	public BBCoordinate CoordinateFromWorldPoint(Vector2 worldPos) {
		return this.NodeFromWorldPoint(worldPos).Coordinate;
	}
	
	void OnDrawGizmos() {
		if (!Application.isPlaying) { return; }
		Gizmos.DrawWireCube(transform.position, new Vector3(this.gridSize.y, this.gridSize.y, 0));
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