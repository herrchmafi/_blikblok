using UnityEngine;
using System.Collections;

public class BBWeapon : MonoBehaviour {
	public int power;
	private GameObject originObject;
	public GameObject OriginObject {
		get { return this.originObject; }
		set { this.originObject = value; }
	}

	public virtual void Init(GameObject originObject) {
		this.originObject = originObject;
	}
	
}
