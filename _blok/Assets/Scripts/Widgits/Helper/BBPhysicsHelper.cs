using UnityEngine;
using System.Collections;

public class BBPhysicsHelper {

	public static float ObjectGravity(float jumpHeight, float timeToJumpApex) {
		return (2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
	}
	
	public static float JumpVelocity(float gravity, float timeToJumpApex) {
		return -Mathf.Abs(gravity) * timeToJumpApex;
	}
	
	public static Vector3 reflectDir(Vector3 currentDir, Vector3 normalDir) {
		Vector3 u = (Vector3.Dot(currentDir, normalDir) / Vector3.Dot(normalDir, normalDir)) * normalDir;
		Vector3 w = currentDir - u;
		return w - u;
	}
	
}
