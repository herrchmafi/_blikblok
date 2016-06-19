using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BBPathFollow : MonoBehaviour {
	private BBCoordinate[] path;
	public BBCoordinate[] Path {
		get { return this.path; }
		set { this.path = value; }
	}

	public float speed = 1.0f;

	private int currentIndex = 0;
	public int CurrentIndex {
		get { return this.currentIndex; }
	}

	private Vector3 currentPosition;
	private float t = .0f;

	private BBTimer timer = new BBTimer();

	private BBGridController gridController;
	// Use this for initialization
	void Start () {
		this.currentPosition = transform.position;
		this.gridController = GameObject.FindGameObjectWithTag(BBSceneConstants.layoutControllerTag).GetComponent<BBGridController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.path != null) {
			if (!this.timer.IsTiming) {
				this.timer.Start();
			}
			this.t += this.speed * Time.deltaTime;
			if (this.t >= 1.0f) {
				this.t = .0f;
				this.currentPosition = this.gridController.WorldPointFromCoordinate(this.path[this.currentIndex]);
				gameObject.SendMessage("Fire", this.currentPosition);
				this.currentIndex++;
			}
			//	Reached endpoint, remove path
			if (this.currentIndex >= this.path.Length) {
				this.path = null;
				this.timer.Stop();
				return;
			}

			transform.position = Vector3.Lerp(this.currentPosition, this.gridController.WorldPointFromCoordinate(this.path[this.currentIndex]), this.t);
		}
	}

	public void StartPath(BBCoordinate[] path) {
		this.path = path;
		this.timer.Start();
	}
}
