using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BBDrawWeapon : BBWeapon {
	private BBGridController gridController;

	private BBCoordinate originCoordinate;
	private List<BBCoordinate> path;

	// Use this for initialization
	void Start () {
		this.path = new List<BBCoordinate>() {
			new BBCoordinate(2, 0),
			new BBCoordinate(1, 1),
			new BBCoordinate(0, 2),
			new BBCoordinate(-1, 1),
			new BBCoordinate(-2, 0),
			new BBCoordinate(-1, -1),
			new BBCoordinate(0, -2),
			new BBCoordinate(1, -1),
		};
	}
	
	// Update is called once per frame
	void Update () {
		foreach (BBCoordinate coordinate in this.path) {
			BBCoordinate compoundCoordinate = BBCoordinate.CompoundCoordinate(new BBCoordinate[] { coordinate, this.originCoordinate });		
			BBGroundTile tile = this.gridController.TileAtCoordinate(compoundCoordinate);
			if (tile != null) {
				tile.ChangeColor();
	//				tile.GetComponent
			}
		}
		Destroy(gameObject);
	}

	public void Init(GameObject originObject) {
		base.Init(originObject);
		this.gridController = GameObject.FindGameObjectWithTag(BBSceneConstants.layoutControllerTag).GetComponent<BBGridController>();
		this.originCoordinate = this.gridController.CoordinateFromWorldPoint(originObject.transform.position);

	}
}
