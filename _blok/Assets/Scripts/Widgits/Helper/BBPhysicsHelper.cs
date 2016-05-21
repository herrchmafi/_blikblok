using UnityEngine;
using System.Collections;

public class BBPhysicsHelper {

	public static float ObjectGravity(float jumpHeight, float timeToJumpApex) {
		return (2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
	}

	public static float JumpVelocity(float gravity, float timeToJumpApex) {
		return -Mathf.Abs(gravity) * timeToJumpApex;
	}

	// Determines the reflection direction between two objects given the sources current direction
	public static Vector3 reflectDir(Vector3 currentDir, GameObject source, GameObject target) {
		Vector3 normalDir = target.transform.up;
		Vector3 u = (Vector3.Dot(currentDir, normalDir) / Vector3.Dot(normalDir, normalDir)) * normalDir;
		Vector3 w = currentDir - u;
		Vector3 reflectDir = w - u;
		PositionRelation relation = BBPhysicsHelper.MinPositionRelationBetween(source, target);
		return (relation == PositionRelation.TOP || relation == PositionRelation.BOTTOM) ? reflectDir : -reflectDir;
	}

	public static Vector3 u(Vector3 currentDir, Vector3 normalDir) {
		return (Vector3.Dot(currentDir, normalDir) / Vector3.Dot(normalDir, normalDir)) * normalDir;
	}

	public static Vector3 w(Vector3 currentDir, Vector3 normalDir) {
		Vector3 u = (Vector3.Dot(currentDir, normalDir) / Vector3.Dot(normalDir, normalDir)) * normalDir;
		Vector3 w = currentDir - u;
		return w;
	}

	public enum PositionRelation {
		TOP, BOTTOM,
		LEFT, RIGHT
	}

	private struct SideBounds {
		private float left;
		public float Left {
			get { return this.left; }
		}

		private float bottom;
		public float Bottom {
			get { return this.bottom; }
		}

		private float right;
		public float Right {
			get { return this.right; }
		}

		private float top;
		public float Top {
			get { return this.top; }
		}

		public SideBounds(float _left, float _bottom, float _right, float _top) {
			this.left = _left;
			this.bottom = _bottom;
			this.right = _right;
			this.top = _top;
		}
	}

	//	Determines which side source object is from target. Goes by closests side
	public static PositionRelation MinPositionRelationBetween(GameObject source, GameObject target) {
		BoxCollider sourceCollider = source.GetComponent<BoxCollider>();
		BoxCollider targetCollider = target.GetComponent<BoxCollider>();

		Vector3 sourceColliderSize = (sourceCollider != null) ? sourceCollider.size : Vector3.zero;
		Vector3 targetColliderSize = (targetCollider != null) ? targetCollider.size : Vector3.zero;

		Transform sourceTransform = source.transform;
		Transform targetTransform = target.transform;
		SideBounds sourceBounds = new SideBounds(
									sourceTransform.position.x - sourceTransform.localScale.x * sourceColliderSize.x / 2,
									sourceTransform.position.y - sourceTransform.localScale.y * sourceColliderSize.y / 2,
									sourceTransform.position.x + sourceTransform.localScale.x * sourceColliderSize.x / 2,
									sourceTransform.position.y + sourceTransform.localScale.y * sourceColliderSize.y / 2
									);
		SideBounds targetBounds = new SideBounds (
									targetTransform.position.x - targetTransform.localScale.x * targetColliderSize.x / 2,
									targetTransform.position.y - targetTransform.localScale.y * targetColliderSize.y / 2,
									targetTransform.position.x + targetTransform.localScale.x * targetColliderSize.x / 2,
									targetTransform.position.y + targetTransform.localScale.y * targetColliderSize.y / 2
									);
		// Find smallest differenced between side and just go with it

		PositionRelation minRelation = PositionRelation.LEFT;
		float minMagnitude = Mathf.Abs(sourceBounds.Left - targetBounds.Right);

		float rightMagnitude = Mathf.Abs(sourceBounds.Right - targetBounds.Left);
		if (rightMagnitude < minMagnitude) {
			minMagnitude = rightMagnitude;
			minRelation = PositionRelation.RIGHT;
		} 

		float bottomMagnitude = Mathf.Abs(sourceBounds.Bottom - targetBounds.Top);
		if (bottomMagnitude < minMagnitude) {
			minMagnitude = bottomMagnitude;
			minRelation = PositionRelation.BOTTOM;
		}

		float topMagnitude = Mathf.Abs(sourceBounds.Top - targetBounds.Bottom);
		if (topMagnitude < minMagnitude) {
			minRelation = PositionRelation.TOP;
		}
		return minRelation;
	}
	
}
