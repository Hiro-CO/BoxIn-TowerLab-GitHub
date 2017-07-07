using UnityEngine;
using System.Collections;

public class DisableWhenScorePercentageBelow : MonoBehaviour {

	public float scorePercentage = 70f;

	void Start () {
		if (PlaytableApiContainer.getScorePercentage (0) < scorePercentage) {
			gameObject.SetActive (false);	
		}
	}

}
