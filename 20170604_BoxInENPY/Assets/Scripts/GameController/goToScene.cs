using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class goToScene : MonoBehaviour {
	public string sceneName;

	public void selectScene (){
		SceneManager.LoadScene(sceneName);
	}
}
