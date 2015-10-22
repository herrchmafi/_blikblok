using UnityEngine;
using System.Collections;

public class BBTimer  {
	//To use:
	//Instantiate timer, call on start function, update in Update()
	private bool isTiming = false;
	public bool IsTiming {
		get { return this.isTiming; }
	}
	
	private float seconds = .0f;
	public float Seconds {
		get { return this.seconds; }
	}
	
	public const float secondsPerMinute = 60.0f;
	public float Minutes {
		get { return this.seconds / secondsPerMinute; }
	}
	
	public const float secondsPerHour = 3600.0f;
	public float Hours {
		get { return this.seconds / secondsPerHour; }
	}
	
	public void Update () {
		if (this.isTiming) {
			this.seconds += Time.deltaTime;
		}
	}
	public void Start() {
		this.isTiming = true;
	}
	
	public void Reset() {
		this.seconds = .0f;
	}
	
	public void Pause() {
		this.isTiming = false;
	}
	public void Stop() {
		this.Pause();
		this.Reset();
	}
}
