using UnityEngine;
using System.Collections;

public class BBErrorHelper : MonoBehaviour {
	public static void DLog(string errorType, string errorMessage) {
		print("Error type: " + errorType + "Message: " + errorMessage);

	}
}
