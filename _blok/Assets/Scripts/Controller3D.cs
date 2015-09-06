using UnityEngine;
using System.Collections;

public class Controller3D : RaycastController {

	private CollisionInfo collisionInfo;
	public CollisionInfo CollInfo {
		get { return this.collisionInfo; }
	}
	public struct CollisionInfo {
		public bool isLeft, isRight;
		public bool isTop, isBottom;
		public bool isFront, isBack;
		//Left is -1, right is 1
		public int faceDir;
		
		public void Reset() {
			isLeft = isRight = false;
			isTop = isBottom = false;
			isFront = isBack = false;
		}
	}
	
	public override void Start() {
		base.Start ();
		this.collisionInfo.faceDir = 1;
	}
	
	void Update() {
	
	}
	
	public void Move(Vector3 distVect) {
		this.UpdateRaycastOrigins ();
		this.collisionInfo.Reset ();
		if (distVect.x != 0) {
			this.collisionInfo.faceDir = (int)Mathf.Sign(distVect.x);
		}
		
		this.HandleHorizontalCollisions (ref distVect);
		this.HandleVerticalCollisions (ref distVect);
		this.HandleNormalCollisions (ref distVect);
		transform.Translate(distVect);
	}
	
	
	private void HandleHorizontalCollisions (ref Vector3 distVect) {
		float dirX = this.collisionInfo.faceDir;
		float rayLength = Mathf.Abs (distVect.x) + skinWidth;
		
		if (Mathf.Abs(distVect.x) < skinWidth) {
			rayLength = 2 * skinWidth;
		}
		
		for (int i = 0; i < this.horizontalRayCount; i++) {
			Vector3 rayOrigin = (dirX == -1) ? this.raycastOrigins.leftBottomFront : this.raycastOrigins.rightBottomFront;
			rayOrigin += Vector3.forward * (horizontalRaySpacing * i);
			for (int j = 0; j < this.normalRayCount; j++) {
				RaycastHit hit;
				//Debug.DrawRay(rayOrigin, dirX * Vector3.right * rayLength, Color.red);
				if (Physics.Raycast(rayOrigin, dirX * Vector3.right, out hit, rayLength, this.collisionMask)) {
					if (hit.distance == 0) {
						continue;
					}
					distVect.x = (hit.distance - skinWidth) * dirX;
					rayLength = hit.distance;
					
					this.collisionInfo.isLeft = dirX == -1;
					this.collisionInfo.isRight = dirX == 1;
				}
				rayOrigin += Vector3.up * (normalRaySpacing);
			}
		}
	}
	
	private void HandleVerticalCollisions (ref Vector3 distVect) {
		float dirY = Mathf.Sign(distVect.y);
		float rayLength = Mathf.Abs (distVect.y) + skinWidth;

		for (int i = 0; i < this.verticalRayCount ; i++) {
			Vector3 rayOrigin = (dirY == -1) ? this.raycastOrigins.leftBottomFront : this.raycastOrigins.leftTopFront;
			rayOrigin += Vector3.right * (this.verticalRaySpacing * i);
			for (int j = 0; j < this.horizontalRayCount; j++) {
				RaycastHit hit;
				//Debug.DrawRay(rayOrigin, dirY * Vector3.up * rayLength, Color.red);
				if (Physics.Raycast(rayOrigin, dirY * Vector3.up, out hit, rayLength, this.collisionMask)) {
					if (hit.distance == 0) {
						continue;
					}
					distVect.y = (hit.distance - skinWidth) * dirY;
					rayLength = hit.distance;
					
					this.collisionInfo.isBottom = dirY == -1;
					this.collisionInfo.isTop = dirY == 1;
				}
				rayOrigin += Vector3.forward * horizontalRaySpacing;

			}
		}
	}
	
	private void HandleNormalCollisions (ref Vector3 distVect) {
		float dirZ = Mathf.Sign(distVect.z);
		float rayLength = Mathf.Abs (distVect.z) + skinWidth;
		
		for (int i = 0; i < this.normalRayCount; i++) {
			Vector3 rayOrigin = (dirZ == -1) ? this.raycastOrigins.leftBottomFront : this.raycastOrigins.leftBottomBack;
			rayOrigin += Vector3.up * (this.normalRaySpacing * i);
			for (int j = 0; j < this.verticalRayCount; j++) {
				RaycastHit hit;
				Debug.DrawRay(rayOrigin, dirZ * Vector3.forward * rayLength, Color.red);
				if (Physics.Raycast(rayOrigin, dirZ * Vector3.forward, out hit, rayLength, this.collisionMask)) {
					if (hit.distance == 0) {
						continue;
					}
					distVect.z = (hit.distance - skinWidth) * dirZ;
					rayLength = hit.distance;
					
					this.collisionInfo.isFront = dirZ == -1;
					this.collisionInfo.isBack = dirZ == 1;
				}
				rayOrigin += Vector3.right * this.normalRaySpacing;
			}
		}
	}
	
	
	
	
	
	
	
	
	
	
	
}
