using UnityEngine;
using System.Collections;

public class BBEntityStats {

	private int health;
	public int Health {
		get { return this.health; }
		set { this.health = value; }
	}

	private int defense;
	public int Defense {
		get { return this.defense; }
		set { this.defense = value; }
	}

	private float speed;
	public float Speed {
		get { return this.speed; }
		set { this.speed = value; }
	}

	public BBEntityStats(int health, int defense, float speed) {
		this.health = health;
		this.defense = defense;
		this.speed = speed;
	}
}
