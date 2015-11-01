using UnityEngine;
using System.Collections;

public class BBDamageSpeech : BBSpeech {
	public float textDisplayDuration = 1.0f;
	private BBTimer timer;
	
	private float amount;
	void OnGUI() {
		if (this.timer.IsTiming) {
			Vector3 point = (Vector3)Camera.main.WorldToScreenPoint(transform.position + this.textOffset);
			this.textArea.x = point.x;
			this.textArea.y = Screen.height - point.y - this.textArea.height;
			string damageText = "" + this.amount;
			GUI.Label (this.textArea, damageText);
		}
	}
	// Use this for initialization
	void Start () {
		this.timer = new BBTimer();
	}
	
	// Update is called once per frame
	void Update () {
		this.timer.Update();
		if (this.timer.Seconds >= this.textDisplayDuration) {
			this.timer.Stop();
		}
	}
	
	public void TakeHit(float amount) {
		if (!this.timer.IsTiming) {
			this.amount = amount;
			this.timer.Start();
		}
	}
}
