using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BBDrawWeapon : BBWeapon {
	public Transform explosionFab;

	private BBGridController gridController;

	private BBCoordinate originCoordinate;

	private BBCoordinate[] path;

	private HashSet<Vector3> visitedVectors;
	
	// Update is called once per frame
	void Update () {

	}

	public void Init(GameObject originObject) {
		base.Init(originObject);
		this.gridController = GameObject.FindGameObjectWithTag(BBSceneConstants.layoutControllerTag).GetComponent<BBGridController>();
		this.originCoordinate = this.gridController.CoordinateFromWorldPoint(originObject.transform.position);
		this.path = new BBCoordinate[] {
			BBCoordinate.CompoundCoordinate(new BBCoordinate[] { this.originCoordinate, new BBCoordinate(1, 1) }),
			BBCoordinate.CompoundCoordinate(new BBCoordinate[] { this.originCoordinate, new BBCoordinate(0, 2) }),
			BBCoordinate.CompoundCoordinate(new BBCoordinate[] { this.originCoordinate, new BBCoordinate(-1, 1) }),
			BBCoordinate.CompoundCoordinate(new BBCoordinate[] { this.originCoordinate, new BBCoordinate(-2, 0) }),
			BBCoordinate.CompoundCoordinate(new BBCoordinate[] { this.originCoordinate, new BBCoordinate(-1, -1) }),
			BBCoordinate.CompoundCoordinate(new BBCoordinate[] { this.originCoordinate, new BBCoordinate(0, -2) }),
			BBCoordinate.CompoundCoordinate(new BBCoordinate[] { this.originCoordinate, new BBCoordinate(1, -1) })
		};
		transform.GetComponent<BBPathFollow>().Path = this.path;
	}

	private void Fire(Vector3 worldPoint) {
		BBGroundTile tile = this.gridController.TileAtCoordinate(this.gridController.CoordinateFromWorldPoint(worldPoint));
		if (tile != null) {
			print("Explode");
			tile.SpawnExplodeFab(this.explosionFab);
		}
	}

	public void HitPathPoint(int index) {
		this.Fire(this.gridController.WorldPointFromCoordinate(this.path[index]));
		if (index == this.path.Length - 1) {
			Destroy(gameObject);
		}
	}
}
