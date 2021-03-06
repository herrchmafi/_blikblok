﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//	This class contains constants that are used within scenes
public class BBSceneConstants {
	//	Scene Tags
	public const string untaggedTag = "Untagged";
	public const string playerTag = "Player";
	public const string playersTag = "Players";
	public const string actionPlayerTag = "ActionPlayer";
	public const string gameControllerTag = "GameController";
	public const string enemyTag = "Enemy";
	public const string neutralTag = "Neutral";
	public const string allyTag = "Ally";
	public const string haterTag = "Hater";
	public const string deadTag = "Dead";
	public const string damageTag = "Damage";
	public const string platformsTag = "Platforms";
	public const string astarTag = "Astar";
	public const string spawnControllerTag = "SpawnController";
	public const string layoutControllerTag = "LayoutController";
	public const string canvasControllerTag = "CanvasController";
	public const string controllersTag = "Controllers";
	
	//	Names
	public const string actionEntity = "Action Entity";
	public const string animatedEntity = "Animated Entity";
	public const string enemy = "Enemy";
	
	//	Input Elements
	public const string horizontalInput = "Horizontal";
	public const string verticalInput = "Vertical";
	public const string jumpInput = "Jump";
	public const string normalAttackInput = "NormalAttack";
	public const string specialAttackInput = "SpecialAttack";
	
	//	Ground is where the ground objects are spawned, collidedGround refers to where colliders hit, sprite ground is where the image is to work visually
	public const float collidedGround = -1.0f;
	public static Vector3 collidedGroundVect = new Vector3(.0f, .0f, BBSceneConstants.collidedGround);
	public const float ground = .0f;
	public const float spriteGround = .0f;
		
	//	Player
	//	This is the animated entity in relation to the action player
	public static readonly Vector3 actionPlayerOffset = Vector3.forward;

	//Entity
	public enum NumberConventions {
		DEFAULTNUMBER = -1,
		NONUMBER = -2
	}
}
