using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class DestroyObjectOnOtherScenes : MonoBehaviour {

	public static DestroyObjectOnOtherScenes control;
	public string[] scenesToUseThisGameObject;
	List<string> scenesToUseThisGameObjectList = new List<string>();

	void Awake () {
		for(int i=0; i< scenesToUseThisGameObject.Length; i++){
			scenesToUseThisGameObjectList.Add(scenesToUseThisGameObject[i]);
		}

		if(control == null){
			DontDestroyOnLoad(gameObject);
			control = this;
		}else{
			Destroy(gameObject);
		}
	}

	void OnLevelWasLoaded(int level) {		
		if(!scenesToUseThisGameObjectList.Contains(SceneManager.GetActiveScene().name)){
			Destroy(gameObject);
		}
	}
}