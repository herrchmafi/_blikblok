using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BBColorChange : MonoBehaviour {
	private BBTimer timer;
	
	public struct ColorChange {
		private float colorChangeSeconds;
		public float ColorChangeSeconds {
			get { return this.colorChangeSeconds; }
		}	
		private Color toColor;
		public Color ToColor {
			get { return this.toColor; }
		}
		
		public ColorChange (Color toColor, float colorChangeSeconds) {
			this.toColor = toColor;
			this.colorChangeSeconds = colorChangeSeconds;
		}
	}
	
	private SpriteRenderer renderer;
	public SpriteRenderer Renderer {
		get { return this.renderer; }
	}

	private Color defaultColor;
	public Color DefaultColor {
		get { return this.defaultColor; }
	}
	
	private List<ColorChange> colorChanges;
	
	private int colorIndex;
	private Color currentColor;

	// Use this for initialization
	void Start () {
		this.renderer = gameObject.GetComponent<SpriteRenderer>();
		this.defaultColor = this.renderer.material.color;
		this.timer = new BBTimer();
	}
	
	// Update is called once per frame
	void Update () {
		//Color Changing
		if (this.timer.IsTiming) {
			this.timer.Update();
			ColorChange colorChange = this.colorChanges[this.colorIndex];
			if (this.timer.Seconds >= colorChange.ColorChangeSeconds) {
				this.colorIndex++;
				//If last color change has finished
				if (this.colorIndex >= this.colorChanges.Count) {
					this.renderer.material.color = this.colorChanges[this.colorChanges.Count - 1].ToColor;
					this.Stop();
					return;
				} else {
					colorChange = this.colorChanges[this.colorIndex];
					this.currentColor = this.colorChanges[this.colorIndex - 1].ToColor;
					this.timer.Reset();
				}
			}
			this.renderer.material.color = Color.Lerp(this.currentColor, colorChange.ToColor, this.timer.Seconds / colorChange.ColorChangeSeconds);
		}
	}
	
	public void ChangeColors(List<ColorChange> colorChanges) {
		if (colorChanges.Count > 0) {
			this.currentColor = this.renderer.material.color;
			this.colorChanges = colorChanges;
			if (this.timer.IsTiming) {
				this.Reset();
			} else {
				 this.timer.Start();
			}
		}
	}
	
	private void Stop() {
		this.timer.Stop();
		this.colorIndex = 0;
	}
	
	private void Reset() {
		this.timer.Reset();
		this.colorIndex = 0;
	}
	
}
