using UnityEngine;
using System.Collections;

public class BoxesAnimatorHashes : MonoBehaviour {

	public int collide;
	
	void Awake() {
		collide = Animator.StringToHash("Collide");
	}
}
