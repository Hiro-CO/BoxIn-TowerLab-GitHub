using UnityEngine;
using System.Collections;

public class MainMenuRunButton : MonoBehaviour {

	public goToScene goToSceneScriptTutorial;
	public goToScene goToSceneScriptGame;
	//public ShowInterstitial showInterstitial;
	public ImageUISpriteSwap imageSwap;
	public int spriteToGo = 3;

	public void PressedTheRunButton(){
		imageSwap.SwapToSprite(spriteToGo);
		//showInterstitial.ShowTheInterstitial();

		if(PersistenceController.control.getFinishedTheTutorial()){
			goToSceneScriptGame.selectScene();
		}else{
			goToSceneScriptTutorial.selectScene();
		}
	}
}
