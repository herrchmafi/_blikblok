using UnityEngine;
using System.Collections;

public class BBCoordinate {
	private int x;
	public int X {
		get { return this.x; }
		set { this.x = value; }
	}
	
	private int y;
	public int Y {
		get { return this.y; }
		set { this.y = value; }
	}
	
	public BBCoordinate(int x, int y) {
		this.x = x;
		this.y = y;
	}
}
