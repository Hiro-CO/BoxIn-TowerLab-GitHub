using UnityEngine;
using System.Collections;

public class SparkleAnimatorHashes : MonoBehaviour {

	public int animationToUse;
	
	void Awake() {
		animationToUse = Animator.StringToHash("AnimationToUse");
	}
}
