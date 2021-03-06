﻿using UnityEngine;
using System.Collections;

public class BBController3D : RaycastController {

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
	
	public void Move(Vector3 distVect) {		
		this.UpdateRaycastOrigins ();
		this.collisionInfo.Reset ();
		if (distVect.x != 0) {
			this.collisionInfo.faceDir = (int)Mathf.Sign(distVect.x);
		}
		if (Mathf.Abs(distVect.x) > 0) {
			this.HandleHorizontalCollisions (ref distVect);
		}
		if (Mathf.Abs(distVect.y) > 0) {
			this.HandleVerticalCollisions (ref distVect);
		}
		if (Mathf.Abs (distVect.x) > 0 && Mathf.Abs (distVect.y) > 0) {
			this.HandleCornerCollisions(ref distVect);
		}
		this.HandleNormalCollisions (ref distVect);
		transform.Translate(distVect);
	}
	
	public void LookAt(Vector3 lookVect) {
		Vector3 heightCorrectedVect = new Vector3(lookVect.x, lookVect.y, transform.position.z);
		print(heightCorrectedVect);
		transform.LookAt(heightCorrectedVect);
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
			//Shoots rays based on direction of X-movement. Will adjust according to where hits are detected
			for (int j = 0; j < this.normalRayCount; j++) {
				RaycastHit hit;
				if (Physics.Raycast(rayOrigin, dirX * Vector3.right, out hit, rayLength, this.collisionMask)) {
					if (hit.distance == 0) {
						continue;
					}
					distVect.x = (hit.distance - skinWidth) * dirX;
					rayLength = hit.distance;
					
					this.collisionInfo.isLeft = dirX == -1;
					this.collisionInfo.isRight = dirX == 1;
				}
				rayOrigin += Vector3.up * this.normalRaySpacing;
			}
		}
	}
	
	private void HandleVerticalCollisions (ref Vector3 distVect) {
		float dirY = Mathf.Sign(distVect.y);
		float rayLength = Mathf.Abs (distVect.y) + skinWidth;
		//Shoots rays based on direction of y-movement. Will adjust according to where hits are detected
		for (int i = 0; i < this.verticalRayCount ; i++) {
			Vector3 rayOrigin = (dirY == -1) ? this.raycastOrigins.leftBottomFront : this.raycastOrigins.leftTopFront;
			rayOrigin += Vector3.right * (this.verticalRaySpacing * i);
			for (int j = 0; j < this.horizontalRayCount; j++) {
				RaycastHit hit;
				if (Physics.Raycast(rayOrigin, dirY * Vector3.up, out hit, rayLength, this.collisionMask)) {
					if (hit.distance == 0) {
						continue;
					}
					distVect.y = (hit.distance - skinWidth) * dirY;
					rayLength = hit.distance;
					
					this.collisionInfo.isBottom = dirY == -1;
					this.collisionInfo.isTop = dirY == 1;
					
				}
				rayOrigin += Vector3.forward * this.horizontalRaySpacing;

			}
		}
	}
	
	private void HandleCornerCollisions(ref Vector3 distVect) {
		float dirX = Mathf.Sign(distVect.x);
		float dirY = Mathf.Sign(distVect.y);
		float rayLength = BBMathHelper.PythagoreanLength(distVect.x, distVect.y) + skinWidth;
		Vector3 rayOrigin;
		if (distVect.x > 0) {
			rayOrigin = (distVect.y > 0) ? this.raycastOrigins.rightTopFront : this.raycastOrigins.rightBottomFront;
		} else {
			rayOrigin = (distVect.y > 0) ? this.raycastOrigins.leftTopFront : this.raycastOrigins.leftBottomFront;
		}
		for (int i = 0; i < this.diagonalRayCount; i++) {
			RaycastHit hit;
			Debug.DrawRay(rayOrigin, new Vector3(distVect.x, distVect.y) * rayLength, Color.red);
			if (Physics.Raycast(rayOrigin, new Vector3(distVect.x, distVect.y), out hit, rayLength, this.collisionMask)) {
				if (hit.distance == 0) {
					continue;
				}
				float dirAngle = BBMathHelper.TanAngle(distVect.x, distVect.y);
				float xHit = hit.distance * Mathf.Sin(dirAngle);
				float yHit = hit.distance * Mathf.Cos(dirAngle);
				distVect.x = (xHit - skinWidth) * dirX;
				distVect.y = (yHit - skinWidth) * dirY;
				rayLength = hit.distance;
				
				this.collisionInfo.isLeft = dirX == -1;
				this.collisionInfo.isRight = dirX == 1;
				this.collisionInfo.isBottom = dirY == -1;
				this.collisionInfo.isTop = dirY == 1;
			}
			rayOrigin += Vector3.forward * this.diagonalRaySpacing;
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
				//Debug.DrawRay(rayOrigin, dirZ * Vector3.forward * rayLength, Color.red);
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
