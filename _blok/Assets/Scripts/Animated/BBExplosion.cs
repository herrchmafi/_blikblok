using UnityEngine;
using System.Collections;

public class BBExplosion : MonoBehaviour {
	private BBTimer timer;
	public float presplosionSeconds;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (this.timer != null) {
			this.timer.Update();
			if (this.timer.Seconds >= this.presplosionSeconds) {
				transform.GetComponent<BBAnimated>().SetAnimationTrigger(BBAnimatorConstants.ExplodeTrigger);
				this.timer.Stop();
			}
		}
	}
	
	public void Explode(float presplosionSeconds) {
		this.presplosionSeconds = presplosionSeconds;
		this.timer = new BBTimer();
		this.timer.Start();
	}
	
	public void Explode() {
		this.timer = new BBTimer();
		this.timer.Start();
	}
}
