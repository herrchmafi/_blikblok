using UnityEngine;
using System.Collections;

public class BBExplosion : MonoBehaviour {
	private BBTimer timer;
	private float presplosionSeconds;

	private int power;
	
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
	
	public void Explode(float presplosionSeconds, int power) {
		this.presplosionSeconds = presplosionSeconds;
		this.power = power;
		this.timer = new BBTimer();
		this.timer.Start();
	}

	public void ExplodeResult() {
		Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(.5f, .5f, .5f));
		foreach (Collider collider in colliders) {
			BBIDamageable damagable = collider.gameObject.GetComponent<BBIDamageable>();
			if (damagable != null) {
				damagable.TakeHit(this.power, null);
			}
		}

		if (transform.parent != null) {
			Transform parent = transform.parent;
			transform.parent = null;
			//Destroy hierarchy
			while (parent.parent != null) {
				parent = parent.parent;
			}
			Destroy(parent.gameObject);
		}
	}
}
