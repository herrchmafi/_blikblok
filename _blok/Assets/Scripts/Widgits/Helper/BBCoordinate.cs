using UnityEngine;
using System.Collections;

public class BBCoordinate {
	public int x , y;

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
			x += coordinates[i].x;
			y += coordinates[i].y;
		}
		return new BBCoordinate(x, y);
	}
}
