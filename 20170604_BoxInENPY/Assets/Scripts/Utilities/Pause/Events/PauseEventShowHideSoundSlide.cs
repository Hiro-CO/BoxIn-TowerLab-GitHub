using UnityEngine;
using System.Collections;

public class PauseEventShowHideSoundSlide : PauseEvents {

	public GameObject[] objectsToShow;
	public GameObject[] objectsToHide;
	public SetMuteSound setMute;
	public MusicMuteChange musicMuteChange;
	public MoveRect[] moveRects;

	public override void ExecuteEvents () {
		ShowObjects(objectsToShow);
		HideObjects(objectsToHide);
		setMute.RefreshVolume();
		musicMuteChange.Changed();
		foreach(MoveRect moveRect in moveRects){
			moveRect.GoToEnd();
		}
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
