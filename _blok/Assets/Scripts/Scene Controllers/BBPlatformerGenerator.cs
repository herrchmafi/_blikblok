using UnityEngine;
using System.Collections;

public class BBPlatformerGenerator : MonoBehaviour {
	public Transform groundTileFab;
	public Transform wallTileFab;
	public Vector2 groundTileLayout;
	public Vector2 wallTileLayout;
	
	private GameObject platforms;
	private GameObject ground;
	
	// Use this for initialization
	void Start () {
		this.platforms = GameObject.FindGameObjectWithTag(BBSceneConstants.platformsTag);
		this.ground = new GameObject();
		this.ground.name = "Ground";
		this.ground.transform.parent = this.platforms.transform;
		this.GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void GenerateMap() {
		//Create Tiles
		for (int x = 0; x < this.groundTileLayout.x; x++) {
			for (int y = 0; y < this.groundTileLayout.y; y++) {
				//Position starts at top left and goes to bottom right
				Vector3 wallTilePosition = new Vector3(-this.groundTileLayout.x / 2 + this.groundTileFab.localScale.x / 2 + x, -this.groundTileLayout.y / 2 + this.groundTileFab.transform.localScale.y / 2 + y, BBSceneConstants.ground);
				Transform wallTile = (Transform)Instantiate(this.groundTileFab, wallTilePosition, Quaternion.identity);
				wallTile.parent = this.ground.transform;
			}
		}
		for (int x = 0; x < this.wallTileLayout.x; x++) {
			for (int y = 0; y < this.wallTileLayout.y; y++) {
			
			}
		}
	}
}
