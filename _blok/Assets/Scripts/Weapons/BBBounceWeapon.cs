using UnityEngine;
using System.Collections;

public class BBBounceWeapon : BBWeapon {
	public LayerMask collisionMask;
	public Vector3 dirVect;
	public float velocity;
	
	// Update is called once per frame
	public virtual void Update () {
		transform.Translate(this.dirVect * this.velocity * Time.deltaTime);
	}

	public virtual void OnTriggerEnter(Collider collider) {
		GameObject collidedObject = collider.gameObject;
		if (BBUnityComponentsHelper.IsInLayerMask(collidedObject, this.collisionMask)) {
			this.dirVect = BBPhysicsHelper.reflectDir(this.dirVect.normalized, gameObject, collidedObject);
		}	
	} 
}
