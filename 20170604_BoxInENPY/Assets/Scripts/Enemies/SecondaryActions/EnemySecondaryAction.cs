using UnityEngine;
using System.Collections;

public class EnemySecondaryAction : MonoBehaviour {

	public MusicNames musicToPlay;
	public static bool startedMusic = false;

	public virtual void PrepareAction(){
		if( !AudioManager.getIsPlaying (musicToPlay.ToString ()) ){
			AudioManager.StopAllMusic ();
			AudioManager.PlayMusic (musicToPlay.ToString (), Vector3.zero);
		}
	}

	public virtual void ExecuteAction(Vector3 dragPosition){
		
	}
}
