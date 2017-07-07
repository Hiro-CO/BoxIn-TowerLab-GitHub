using UnityEngine;
using System.Collections;

public class FollowRect : MonoBehaviour {

	public RectTransform rectFollower;
	public RectTransform rectFollowed;

	void LateUpdate () {
		if (rectFollower.anchorMin != rectFollowed.anchorMin) {
			rectFollower.anchorMin = rectFollowed.anchorMin;
		}
		if (rectFollower.anchorMax != rectFollowed.anchorMax) {
			rectFollower.anchorMax = rectFollowed.anchorMax;
		}
	}
}
