using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BBPlayerHUDController : MonoBehaviour {
	private const string name = "Name";
	private const string health = "Health";

	private BBEntityStats stats;
	public BBEntityStats Stats {
		set { this.stats = value; }
	}

	private Text nameText;
	private Text healthText;

	void Start() {
		this.nameText = transform.FindChild(name).GetComponent<Text>();
		this.healthText = transform.FindChild(health).GetComponent<Text>();
	}

	void Update() {
		if (stats != null) {
			this.nameText.text = "" + stats.Health;
			this.healthText.text = stats.Name;
		} else {
			this.nameText.text = "";
			this.healthText.text = "";
		}
	}
}
