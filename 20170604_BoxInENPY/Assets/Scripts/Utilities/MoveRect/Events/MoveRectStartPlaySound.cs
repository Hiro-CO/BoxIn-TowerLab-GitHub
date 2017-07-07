using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MoveRectStartPlaySound : MoveRectReachStartEvent {

	public AudioNames audio;

	public override void ExecuteEvents(){
		AudioManager.PlaySound (audio.ToString (), Vector3.zero);
	}
}
