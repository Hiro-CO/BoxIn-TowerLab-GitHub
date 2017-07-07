using UnityEngine;
using System.Collections;

public class EnemyAnimatorHashes : MonoBehaviour {

	public int dead;
	
	void Awake() {
		dead = Animator.StringToHash("Dead");
	}
}
