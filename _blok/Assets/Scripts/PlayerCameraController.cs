using UnityEngine;
using System.Collections;

public class PlayerCameraController : MonoBehaviour {

	public Vector2 focusDimensions;
	public float cameraDist = -30.0f;
	public float baseFOV = 40.0f;
	public float expandDistThresholdFOV = 10.0f;
	public float expandTime = .3f;
	
	private GameObject targetPlayer;
	private Controller3D targetController;
	private FocusArea focusArea;
	
	public float verticalOffset;
	public float lookAheadDistX;
	public float lookSmoothTimeX;
	public float verticalSmoothTime;
	
	private float currentLookAheadX;
	private float targetLookAheadX;
	private float lookAheadDirX;
	private float smoothLookVelocityX;
	private float smoothVelocityY;
	
	private float expandFOVVelocity;
	
	private bool isLookAheadStopped;
	
	private GameObject[] players;
	// Use this for initialization
	void Start () {

	}
	
	void LateUpdate () {	
		//Determine camera positioning
		this.focusArea.Update(this.targetController.boxCollider.bounds);
		
		Vector2 focusPosition = this.focusArea.center + Vector2.up * this.verticalOffset;
		
		if (this.focusArea.velocity.x != 0) {
			this.lookAheadDirX = Mathf.Sign(this.focusArea.velocity.x);
			float inputX = this.targetController.PlayerInput.x;
			float inputY = this.targetController.PlayerInput.y;
			if (Mathf.Sign(inputX) == Mathf.Sign(this.focusArea.velocity.x) && this.targetController.PlayerInput.x != 0) {
				this.isLookAheadStopped = false;
				this.targetLookAheadX = this.lookAheadDirX * this.lookAheadDistX;
			} else {
				if (!this.isLookAheadStopped) {
					this.isLookAheadStopped = true;
					this.targetLookAheadX = this.currentLookAheadX + (this.lookAheadDirX * this.lookAheadDistX - this.currentLookAheadX) / 4.0f;
				}
			}
		}
		
		this.currentLookAheadX = Mathf.SmoothDamp(this.currentLookAheadX, this.targetLookAheadX, ref this.smoothLookVelocityX, this.lookSmoothTimeX);
		
		focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref this.smoothVelocityY, this.verticalSmoothTime);
		focusPosition += Vector2.right * this.currentLookAheadX;
		//Mostly arbitrary number. Negative to make camera in front of stage
		transform.position = (Vector3) focusPosition + Vector3.forward * cameraDist;
		
		//Determine FOV
		float maxDistance = .0f;
		foreach (GameObject player in this.players) {
			float currentDistance = Vector2.Distance(this.targetPlayer.transform.position, player.transform.position);
			if (currentDistance > maxDistance) {
				maxDistance = currentDistance;
			}
		}
		if (maxDistance > this.expandDistThresholdFOV) {
			float fovBuffer = (maxDistance - this.expandDistThresholdFOV) * 2.0f;
			Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, this.baseFOV + fovBuffer, ref this.expandFOVVelocity, this.expandTime);
		} else {
			Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, this.baseFOV, ref this.expandFOVVelocity, this.expandTime);
		}
	}
	
	void OnDrawGizmos() {
		Gizmos.color = new Color(1, 0, 0, .5f);
		Gizmos.DrawCube(this.focusArea.center, this.focusDimensions);
	}
	
	public void SetTargetPlayer(GameObject targetPlayer) {
		this.targetPlayer = targetPlayer;
		//Create array of non-main player objects
		GameObject[] tempPlayers = GameObject.FindGameObjectsWithTag("Player");
		this.players = new GameObject[tempPlayers.Length - 1];
		int index = 0;
		foreach (GameObject player in tempPlayers) {
			if (player != this.targetPlayer) {
				this.players[index] = player;
				index++;
			}
		}
		this.targetController = this.targetPlayer.GetComponent<Controller3D>();
		this.focusArea = new FocusArea(this.targetController.boxCollider.bounds, this.focusDimensions);
	}
	
	struct FocusArea {
		public Vector2 center;
		public Vector2 velocity;
		float left, right;
		float top, bottom;
		
		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x / 2;
			right = targetBounds.center.x + size.x / 2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;
			
			velocity = Vector2.zero;
			center = new Vector2((left + right) / 2, (top + bottom) / 2);
		}
	
		public void Update(Bounds targetBounds) {
			float shiftX = 0;
			//If out of bounds, then shift accordingly
			if (targetBounds.min.x < left) {
				shiftX = targetBounds.min.x - left;
			} else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;
			
			float shiftY = 0;
			//If out of bounds, then shift accordingly
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;
			} else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			top += shiftY;
			bottom += shiftY;
			center = new Vector2((left + right) / 2, (top + bottom) / 2);
			velocity = new Vector2(shiftX, shiftY);
		}
	}
}
