using UnityEngine;
using System.Collections;

public class BBGroundTile : MonoBehaviour {
	public float changeSeconds = 1.0f;
	private BBTimer timer;
	
	private Vector3 toScale;
	private Vector3 fromScale;

	public BBCoordinate coordinate {
		get { return this.gridController.CoordinateFromWorldPoint(transform.position); }
	}

	private BBGridController gridController;
	// Use this for initialization
	void Start () {
		this.timer = new BBTimer();
		this.Expand();
		this.gridController = GameObject.FindGameObjectWithTag(BBSceneConstants.layoutControllerTag).GetComponent<BBGridController>();
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

	public void ChangeColor() {
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.material.color = Color.blue;
	}

	public void SpawnExplodeFab(Transform fab) {
		Transform explosionTransform = (Transform)Instantiate(fab, transform.position, Quaternion.identity);
		explosionTransform.gameObject.GetComponent<BBExplosion>().Explode(0);
	}

}
