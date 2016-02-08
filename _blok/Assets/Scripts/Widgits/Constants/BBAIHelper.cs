using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BBAIHelper {

	public static bool IsOpponent(GameObject source, GameObject target) {
		string sourceTag = source.tag;
		HashSet<string> opponents = new HashSet<string>();
		if (sourceTag.Equals(BBSceneConstants.playerTag)) {
			opponents = BBAIConstants.playerOpponents;
		} else if (sourceTag.Equals(BBSceneConstants.enemyTag)) {
			opponents = BBAIConstants.enemyOpponents;
		} else if (sourceTag.Equals(BBSceneConstants.neutralTag)) {
			opponents = BBAIConstants.neutralOpponents;
		} else if (sourceTag.Equals(BBSceneConstants.allyTag)) {
			opponents = BBAIConstants.allyOpponents;
		} else if (sourceTag.Equals(BBSceneConstants.haterTag)) {
			opponents = BBAIConstants.haterOpponents;
		}
		return opponents.Contains(target.tag);
	}
}
