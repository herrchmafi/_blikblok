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
		Transform sourceTransform = transform;
		Transform targetTransform = collidedObject.transform;
		BoxCollider sourceCollider = gameObject.GetComponent<BoxCollider>();
		BoxCollider targetCollider = collider.gameObject.GetComponent<BoxCollider>();
		Vector3 sourceColliderSize = (sourceCollider != null) ? sourceCollider.size : Vector3.zero;
		Vector3 targetColliderSize = (targetCollider != null) ? targetCollider.size : Vector3.zero;
		this.dirVect = BBPhysicsHelper.reflectDir(this.dirVect.normalized, gameObject, collidedObject);
		}	
	} 

}
