using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class RaycastController : MonoBehaviour {
	
	public LayerMask collisionMask;
	
	protected const float skinWidth = .015f;
	public int horizontalRayCount;
	public int verticalRayCount;
	public int diagonalRayCount;
	public int normalRayCount;
	
	protected float horizontalRaySpacing;
	protected float verticalRaySpacing;
	protected float diagonalRaySpacing;
	protected float normalRaySpacing;
	
	public BoxCollider boxCollider;
	
	protected RaycastOrigins raycastOrigins;
	
	//Prevent null reference exceptions because this is referenced in other scripts' Start methods
	public virtual void Awake() {
		this.boxCollider = GetComponent<BoxCollider> ();
	}
	
	public virtual void Start() {
		this.CalculateRaySpacing ();
	}
	//Calculate references to specific positions of gameobject bounds
	protected void UpdateRaycastOrigins() {
		Bounds bounds = boxCollider.bounds;
		bounds.Expand (skinWidth * -2);
		
		raycastOrigins.leftBottomFront = new Vector3 (bounds.min.x, bounds.min.y, bounds.min.z);
		raycastOrigins.rightBottomFront = new Vector3 (bounds.max.x, bounds.min.y, bounds.min.z);
		raycastOrigins.leftTopFront = new Vector3 (bounds.min.x, bounds.max.y, bounds.min.z);
		raycastOrigins.rightTopFront = new Vector3 (bounds.max.x, bounds.max.y, bounds.min.z);
		raycastOrigins.leftBottomBack = new Vector3 (bounds.min.x, bounds.min.y, bounds.max.z);
	}
	
	//Determine spacing of rays based on ray count of respective axis
	protected void CalculateRaySpacing() {
		Bounds bounds = boxCollider.bounds;
		bounds.Expand (skinWidth * -2);
		
		this.horizontalRayCount = Mathf.Clamp (this.horizontalRayCount, 2, int.MaxValue);
		this.verticalRayCount = Mathf.Clamp (this.verticalRayCount, 2, int.MaxValue);
		this.normalRayCount = Mathf.Clamp (this.normalRayCount, 2, int.MaxValue);
		
		this.horizontalRaySpacing = bounds.size.z / (this.horizontalRayCount - 1);
		this.verticalRaySpacing = bounds.size.x / (this.verticalRayCount - 1);
		this.normalRaySpacing = bounds.size.y / (this.normalRayCount - 1);
		this.diagonalRaySpacing = bounds.size.z / (this.diagonalRayCount - 1);
	}
	
	protected struct RaycastOrigins {
		public Vector3 leftTopFront, rightTopFront;
		public Vector3 leftBottomFront, rightBottomFront;
		public Vector3 leftBottomBack;
	}
}
