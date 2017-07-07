using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GetMainCameraUI : MonoBehaviour {

	public Canvas canvas;

	void Awake () {
		canvas.worldCamera = Camera.main;
	}
}
