using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using Playmove;

public class EventSystemDragthresholdOnScenes : MonoBehaviour {

	public EventSystem eventSystem;
	public string[] scenesNames;
	public int[] dragThresholds;
	public int defaultDragThreshold = 5;
	bool changed = false;

	//Awake is needed because the first scene of the object dont execute the OnLevelWasLoaded(), only the Awake().
	void Awake(){
		ChangeDragthresholdEvent ();
	}

	void OnLevelWasLoaded() {
		ChangeDragthresholdEvent ();
	}

	void ChangeDragthresholdEvent(){
		changed = false;
		for(int i=0; i<scenesNames.Length; i++){
			if (scenesNames[i] == SceneManager.GetActiveScene ().name && dragThresholds.Length > i) {
				eventSystem.pixelDragThreshold = dragThresholds [i];
				changed = true;
			}
		}
		if (!changed && eventSystem.pixelDragThreshold != defaultDragThreshold) {
			eventSystem.pixelDragThreshold = defaultDragThreshold;
		}
	}
}