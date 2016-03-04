using UnityEngine;
using System.Collections;

public class BBNode : BBIHeapItem<BBNode> {
	private bool isWalkable;
	public bool IsWalkable {
		get { return this.isWalkable; }
	}
	
	private Vector3 worldPos;
	public Vector3 WorldPos {
		get { return this.worldPos; }
	}
	
	private int gCost;
	public int GCost {
		get { return this.gCost; }
		set { this.gCost = value; }
	}
	
	private int hCost;
	public int HCost {
		get { return this.hCost; }
		set { this.hCost = value; }
	}
	
	public int FCost {
		get { return this.gCost + this.hCost; }
	}
	
	private int heapIndex;
	public int HeapIndex {
		get { return this.heapIndex; }
		set { this.heapIndex = value; }
	}
	
	private BBNode parent;
	public BBNode Parent {
		get { return this.parent; }
		set { this.parent = value; }
	}
	
	private BBCoordinate coordinate;
	public BBCoordinate Coordinate {
		get { return this.coordinate; }
	}

	//	If living entity is currently on
	private int inhabitedCount;
	public int InhabitedCount {
		get { return this.inhabitedCount; }
		set { this.inhabitedCount = value; }
	}
	
	private int terrainPenalty;
	public int TerrainPenalty {
		get { return this.terrainPenalty; }
	}
	
	public int CompareTo(BBNode compareNode) {
		int compare = this.FCost.CompareTo(compareNode.FCost);
		if (compare == 0) {
			compare = hCost.CompareTo (compareNode.hCost);
		}
		return -compare;
	}
	
	public BBNode (bool isWalkable, Vector3 worldPos, BBCoordinate coordinate, int movementPenalty) {
		this.isWalkable = isWalkable;
		this.worldPos = worldPos;
		this.coordinate = coordinate;
	}
	
}
