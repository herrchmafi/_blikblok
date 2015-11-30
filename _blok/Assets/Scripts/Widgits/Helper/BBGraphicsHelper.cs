using UnityEngine;
using System.Collections;

public class BBGraphicsHelper {
	public static Color NormalizedColor(Vector4 colorVect) {
		return new Color(colorVect.x/ BBGraphicsConstants.colorNormalize, colorVect.y / BBGraphicsConstants.colorNormalize, colorVect.z / BBGraphicsConstants.colorNormalize, colorVect.w);
	}
	
	public static Color OpaqueColor(Color color) {
		return new Color(color.r, color.g, color.b, 1.0f);
	}
}
