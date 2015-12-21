using UnityEngine;
using System.Collections;

//Timed transformations
public class BBTransformer : MonoBehaviour {
	private BBTimer positionTimer, rotateTimer, scaleTimer, shakeTimer;
	private float positionSeconds, rotateSeconds, scaleSeconds, shakeSeconds;
	private Vector3 fromPosition, toPosition, originalPosition;
	//Only z axis
	private float fromAngle, toAngle, originalAngle;
	private Vector3 fromScale, toScale, originalScale;
	
	private float shakeMagnitude;
	private float shakePeriod;
	
	// Use this for initialization
	void Start () {
		this.positionTimer = new BBTimer();
		this.rotateTimer = new BBTimer();
		this.scaleTimer = new BBTimer();
		this.shakeTimer = new BBTimer();
		
		this.originalPosition = transform.position;
		this.originalAngle = transform.rotation.z;
		this.originalScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.positionTimer.IsTiming) {
			this.positionTimer.Update();
			if (this.positionTimer.Seconds >= this.positionSeconds) {
				transform.position = this.toPosition;
				this.positionTimer.Stop();
			} else {
				transform.position = Vector3.Lerp(this.fromPosition, this.toPosition, this.positionTimer.Seconds / this.positionSeconds);
			}
		}
		if (this.rotateTimer.IsTiming) {
			this.rotateTimer.Update();
			if (this.rotateTimer.Seconds >= this.rotateSeconds) {
				transform.eulerAngles = new Vector3(.0f, .0f, this.toAngle); 
				this.positionTimer.Stop();
			} else {
				transform.eulerAngles = new Vector3(.0f, .0f, Mathf.MoveTowardsAngle(this.fromAngle, this.toAngle, this.rotateTimer.Seconds / this.rotateSeconds));
			}
		}
		if (this.scaleTimer.IsTiming) {
			this.scaleTimer.Update();
			if (this.scaleTimer.Seconds >= this.scaleSeconds) {
				transform.localScale = this.toScale;
				this.positionTimer.Stop();
			} else {
				transform.localScale = Vector3.Lerp(this.fromScale, this.toScale, this.scaleTimer.Seconds / this.scaleSeconds);
			}
		}
		if (this.shakeTimer.IsTiming) {
			this.shakeTimer.Update();
			if (this.shakeTimer.Seconds >= this.shakeSeconds) {
				transform.position = originalPosition;
				this.shakeTimer.Stop();
			} else {
				float shakeX = this.shakeMagnitude * Mathf.Sin(2 * Mathf.PI * this.shakeTimer.Seconds / this.shakePeriod);
				transform.position = this.originalPosition + transform.right * shakeX;
			}
		}
	}
	
	//Iterpolate from current position to specified position in specified time
	public void Position(Vector3 toPosition, float seconds) {
		this.originalPosition = transform.position;
		this.fromPosition = this.originalPosition;
		this.toPosition = toPosition;
		this.positionSeconds = seconds;
		this.positionTimer.Start();
	}
	
	//Interpolate from specified position
	public void Position(Vector3 fromPosition, Vector3 toPosition, float seconds) {
		this.fromPosition = fromPosition;
		this.toPosition = toPosition;
		this.positionSeconds = seconds;
		this.positionTimer.Start();
	}
	
	//Rotate to angle from current angle
	public void Rotate(float toAngle, float seconds) {
		this.originalAngle = transform.eulerAngles.z;
		this.fromAngle = this.originalAngle;
		this.toAngle = toAngle;
		this.rotateSeconds = seconds;
		this.rotateTimer.Start();
	}
	
	public void Rotate(float fromAngle, float toAngle, float seconds) {
		this.fromAngle = fromAngle;
		this.toAngle = toAngle;
		this.rotateSeconds = seconds;
		this.rotateTimer.Start();
	}
	
	public void Scale(Vector3 toScale, float seconds) {
		this.fromScale = this.originalScale;
		this.toScale = toScale;
		this.scaleSeconds = seconds;
		this.scaleTimer.Start();
	}
	
	public void Scale(Vector3 fromScale, Vector3 toScale, float seconds) {
		this.fromScale = fromScale;
		this.toScale = toScale;
		this.scaleSeconds = seconds;
		this.scaleTimer.Start();
	}
	
	//Supplemental Methods
	public void Shake(float seconds, float magnitude, float period) {
		this.originalPosition = transform.position;
		this.shakeSeconds = seconds;
		this.shakeMagnitude = magnitude;
		this.shakePeriod = period;
		this.shakeTimer.Start();
	}
	
}
