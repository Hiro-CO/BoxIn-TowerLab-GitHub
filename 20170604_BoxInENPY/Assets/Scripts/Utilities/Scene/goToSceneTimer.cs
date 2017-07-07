using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class goToSceneTimer : MonoBehaviour {
	public string sceneName;
	public float timeToGoToScene = 1f;
	float timeToGoToScenePassed = 0f;

	public void Update(){
		if (timeToGoToScenePassed < timeToGoToScene) {
			timeToGoToScenePassed += Time.deltaTime;
		} else {
			selectScene ();
		}
	}

	public void selectScene (){
		SceneManager.LoadScene(sceneName);
	}
}
