using UnityEngine;
using System.Collections;

public class ScoreAnimatorHashes : MonoBehaviour {

	public int gotNote;

	void Awake (){
		gotNote = Animator.StringToHash ("GotNote");
	}
}
