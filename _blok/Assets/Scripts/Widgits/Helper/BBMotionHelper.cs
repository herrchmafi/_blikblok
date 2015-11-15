using UnityEngine;
using System.Collections;

public class BBMotionHelper {
	public static float EaseSine(float t) {
		return (Mathf.Sin(t * Mathf.PI - Mathf.PI / 2) + 1) / 2;
	}
}
