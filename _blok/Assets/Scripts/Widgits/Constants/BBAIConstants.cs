using System.Collections;
using System.Collections.Generic;

public class BBAIConstants {

	public static readonly HashSet<string> playerOpponents = new HashSet<string>() {
		BBSceneConstants.enemyTag,
		BBSceneConstants.haterTag
	};
	public static readonly HashSet<string> enemyOpponents = new HashSet<string>() {
		BBSceneConstants.playerTag,
		BBSceneConstants.neutralTag,
		BBSceneConstants.allyTag,
		BBSceneConstants.haterTag
	};
	public static readonly HashSet<string> neutralOpponents = new HashSet<string>() {
	};
	public static readonly HashSet<string> allyOpponents = new HashSet<string>() {
		BBSceneConstants.enemyTag,
		BBSceneConstants.haterTag
	};
	public static readonly HashSet<string> haterOpponents = new HashSet<string>() {
		BBSceneConstants.playerTag,
		BBSceneConstants.neutralTag,
		BBSceneConstants.allyTag,
		BBSceneConstants.enemyTag,
		BBSceneConstants.haterTag
	};
		
}
