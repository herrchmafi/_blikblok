using UnityEngine;
using System.Collections;

public class BBEntityStats {

	private string name;
	public string Name {
		get { return this.name; }
	}

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

	public BBEntityStats(string name, int health, int defense, float speed) {
		this.name = name;
		this.health = health;
		this.defense = defense;
		this.speed = speed;
	}
}
