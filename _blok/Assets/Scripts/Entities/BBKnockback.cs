using UnityEngine;
using System.Collections;

public class BBKnockback {
	private float magnitude;
	public float Magnitude {
		get { return this.magnitude; }
	}
	
	public float seconds;
	public float Seconds {
		get { return this.seconds; }
	}
	
	private Vector3 direction;
	public Vector3 Direction {
		get { return this.direction; }
	}
	
	private BBTimer timer = new BBTimer();
	public BBTimer Timer {
		get { return this.timer; }
	}
	
	public BBKnockback (float knockbackMagnitude, float knockbackTime, Vector3 direction) {
		this.magnitude = knockbackMagnitude;
		this.seconds = knockbackTime;
		this.direction = direction;
	}
}
