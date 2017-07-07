using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MoveRectStartGoToScene : MoveRectReachStartEvent {

	public string sceneName;

	public override void ExecuteEvents(){
		SceneManager.LoadScene(sceneName);
	}
}
