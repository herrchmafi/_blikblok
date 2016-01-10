using UnityEngine;
using System.Collections;

public class HTNode : BBIHeapItem<HTNode> {
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
	
	private HTNode parent;
	public HTNode Parent {
		get { return this.parent; }
		set { this.parent = value; }
	}
	
	private HTVector2Int coordinate;
	public HTVector2Int Coordinate {
		get { return this.coordinate; }
	}
	
	public int CompareTo(HTNode compareNode) {
		int compare = this.FCost.CompareTo(compareNode.FCost);
		if (compare == 0) {
			compare = hCost.CompareTo (compareNode.hCost);
		}
		return -compare;
	}
	
	public HTNode (bool isWalkable, Vector3 worldPos, HTVector2Int coordinate) {
		this.isWalkable = isWalkable;
		this.worldPos = worldPos;
		this.coordinate = coordinate;
	}
	
}
