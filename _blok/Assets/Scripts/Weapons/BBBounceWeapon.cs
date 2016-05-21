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
		print("Source x:" +
									(sourceTransform.position.x - sourceTransform.localScale.x * sourceColliderSize.x / 2) + " y:" +
									(sourceTransform.position.y - sourceTransform.localScale.y * sourceColliderSize.y / 2) + " w:" +
									(sourceTransform.position.x + sourceTransform.localScale.x * sourceColliderSize.x / 2) + " h:" +
									(sourceTransform.position.y + sourceTransform.localScale.y * sourceColliderSize.y / 2)
									);
		print("Target x:" +
									(targetTransform.position.x - targetTransform.localScale.x * targetColliderSize.x / 2) + " y:" +
									(targetTransform.position.y - targetTransform.localScale.y * targetColliderSize.y / 2) + " w:" +
									(targetTransform.position.x + targetTransform.localScale.x * targetColliderSize.x / 2) + " h:" +
									(targetTransform.position.y + targetTransform.localScale.y * targetColliderSize.y / 2)
									);
			print(BBPhysicsHelper.MinPositionRelationBetween(gameObject, collidedObject));
			this.dirVect = BBPhysicsHelper.reflectDir(this.dirVect.normalized, gameObject, collidedObject);
		}	
	} 

}
