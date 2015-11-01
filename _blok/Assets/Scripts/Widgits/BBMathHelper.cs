using UnityEngine;
using System.Collections;

public class BBMathHelper {


//     /|
// 	 /	| b	
//	-----
//	   a
	public static float PythagoreanLength(float a, float b) {
		return Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
	}
	
	public static float TanAngle(float a, float b) {
		if (a == 0) { return .0f; }
		return Mathf.Tan(b / a);
	}
}
