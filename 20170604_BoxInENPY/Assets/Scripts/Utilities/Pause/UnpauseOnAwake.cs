using UnityEngine;
using System.Collections;

public class UnpauseOnAwake : MonoBehaviour {

	public PauseAndUnpause pause;

	void Awake () {
		pause.Unpause();
	}
}
