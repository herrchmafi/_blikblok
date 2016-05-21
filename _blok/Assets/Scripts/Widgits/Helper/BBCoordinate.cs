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

	public string CoordinateString {
		get { return "x: " + this.x + ", y: " + this.y; }
	}
	
	public BBCoordinate(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public static BBCoordinate CompoundCoordinate(BBCoordinate[] coordinates) {
		int x = 0, y = 0;
		for (int i = 0; i < coordinates.Length; i++) {
			x += coordinates[i].X;
			y += coordinates[i].Y;
		}
		return new BBCoordinate(x, y);
	}
}
