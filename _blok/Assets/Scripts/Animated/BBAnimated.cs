using UnityEngine;
using System.Collections;

public class BBAnimated : MonoBehaviour {
	private Color defaultColor;
	
	private Animator animator;
	private SpriteRenderer renderer;
	
	//Color change struct holds a properties and functionality to change colors
	private struct ColorChange {
		private BBTimer timer;
		public BBTimer Timer {
			get { return this.timer; }
		}
		private float colorChangeSeconds;
	
		private Color fromColor, toColor;
		
		private Material material;
		
		//Go from color to color within time. Supply object's material you want to change
		public void Init(Color fromColor, Color toColor, float colorChangeSeconds, Material material) {
			this.fromColor = fromColor;
			this.toColor = toColor;
			this.colorChangeSeconds = colorChangeSeconds;
			this.material = material;
			this.timer = new BBTimer();
			this.timer.Start();
		}
		
		public void Update() {
			if (this.timer != null && this.timer.IsTiming) {
				this.timer.Update();
				if (this.timer.Seconds < this.colorChangeSeconds) {
					//this.material.color = Color.Lerp(this.fromColor, this.toColor, (this.timer.Seconds / this.colorChangeSeconds));
					this.material.color = Color.Lerp(Color.red, Color.green, (this.timer.Seconds / this.colorChangeSeconds));
				} else {
					this.material.color = this.toColor;
					this.timer.Stop();
				}
			}
		}  
	}
	private ColorChange colorChange;
	public Color testColor1, testColor2;
	// Use this for initialization
	public virtual void Start () {
		this.animator = gameObject.GetComponent<Animator>();
		this.renderer = transform.GetComponent<SpriteRenderer>();
		this.defaultColor = this.renderer.color;
		this.SetColorChange(testColor1, testColor2, 5.0f);

	}
	
	// Update is called once per frame
	public virtual void Update () {
		this.colorChange.Update();
	}
	
	public void SetAnimationState(BBActionPlayerController.State state) {
		this.animator.SetInteger(BBAnimatorConstants.StateParam, (int)state);
	}
	
	public void SetColorChange(Color fromColor, Color toColor, float colorChangeSeconds) {
		this.colorChange.Init(fromColor, toColor, colorChangeSeconds, this.renderer.material);
	}
}
