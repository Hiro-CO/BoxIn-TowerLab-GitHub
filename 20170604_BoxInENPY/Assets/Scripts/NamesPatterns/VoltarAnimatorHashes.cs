using UnityEngine;
using System.Collections;

public class VoltarAnimatorHashes : MonoBehaviour {

	public int visible;
	public int instantVisible;
	public int instantInvisible;

	void Awake (){
		visible = Animator.StringToHash ("Visible");
		instantVisible = Animator.StringToHash ("InstantVisible");
		instantInvisible = Animator.StringToHash ("InstantInvisible");
	}
}
