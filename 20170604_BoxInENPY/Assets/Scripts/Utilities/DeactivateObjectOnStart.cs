using UnityEngine;
using System.Collections;

public class DeactivateObjectOnStart : MonoBehaviour {

	void Start () {
		gameObject.SetActive(false);
	}
}
