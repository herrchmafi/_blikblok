using UnityEngine;
using System.Collections;
using Pathfinding;

public class BBBomber : BBLivingEntity {
	private BBGameController gameController;
	
	private BBTimer targetTimer;
	
	
	// Use this for initialization
	public override void Start () {
		base.Start();
		this.gameController = GameObject.FindGameObjectWithTag(BBSceneConstants.gameControllerTag).GetComponent<BBGameController>();

	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
	}
}
