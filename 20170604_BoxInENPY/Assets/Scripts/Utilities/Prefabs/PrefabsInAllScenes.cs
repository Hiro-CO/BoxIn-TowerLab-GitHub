using UnityEngine;
using System.Collections;

public class PrefabsInAllScenes : MonoBehaviour {

	public static PrefabsInAllScenes control;
	public GameObject screenTransition;

	void Awake () {
		if(control == null){
			DontDestroyOnLoad(this);
			control = this;
		}else{
			Destroy(gameObject);
		}
	}

	public void ScreenTransition(){
		screenTransition.SetActive (true);
	}
}
