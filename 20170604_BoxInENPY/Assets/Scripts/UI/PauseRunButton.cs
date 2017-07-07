using UnityEngine;
using System.Collections;

public class PauseRunButton : MonoBehaviour {

	public MoveRect menuMoveRect;
	public MoveRect openedMenuMoveRect;
	public ImageUISpriteSwap imageUISpriteSwap;
	public int imageUINumber;

	public void Clicked(){
		if(!menuMoveRect.getChanging() && !openedMenuMoveRect.getChanging()){
			menuMoveRect.GoToStart();
			openedMenuMoveRect.GoToStart();
			imageUISpriteSwap.SwapToSprite(imageUINumber);
		}
	}
}
