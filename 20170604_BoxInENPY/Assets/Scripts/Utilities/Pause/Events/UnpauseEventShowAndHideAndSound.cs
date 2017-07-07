using UnityEngine;
using System.Collections;

public class UnpauseEventShowAndHideAndSound : UnpauseEvents {

	public GameObject[] objectsToShow;
	public GameObject[] objectsToHide;
	public SetMuteSound setMute;
	public MusicMuteChange musicMuteChange;

	public override void ExecuteEvents () {
		ShowObjects(objectsToShow);
		HideObjects(objectsToHide);
		setMute.RefreshVolume();
		musicMuteChange.Changed();
	}

	public void ShowObjects(GameObject[] objects){
		foreach(GameObject objectToShow in objects){
			objectToShow.SetActive(true);
		}
	}
	
	public void HideObjects(GameObject[] objects){
		foreach(GameObject objectToHide in objects){
			objectToHide.SetActive(false);
		}
	}
}
