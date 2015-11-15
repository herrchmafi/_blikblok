using UnityEngine;
using System.Collections;

public class BBGroundTile : MonoBehaviour {
	public float changeSeconds = 1.0f;
	private BBTimer timer;
	
	private Vector3 toScale;
	private Vector3 fromScale;
	// Use this for initialization
	void Start () {
		this.timer = new BBTimer();
		this.Expand();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.timer.IsTiming) {
			this.timer.Update();
			if (this.timer.Seconds >= changeSeconds) {
				transform.localScale = this.toScale;
				this.timer.Stop();
			} else {
				float u = BBMotionHelper.EaseSine(this.timer.Seconds / this.changeSeconds);
				transform.localScale = Vector3.Lerp(this.fromScale, this.toScale, u);
			}
		}
	}
	
	public void Expand() {
		this.toScale = Vector3.one;
		this.fromScale = Vector3.zero;
		this.timer.Start();
	}
	
	public void Contract() {
		this.toScale = Vector3.zero;
		this.fromScale = Vector3.one;
		this.timer.Start();
	}
}
