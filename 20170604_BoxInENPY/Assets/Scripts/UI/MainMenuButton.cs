using UnityEngine;
using System.Collections;

public class MainMenuButton : MonoBehaviour {

	public MoveRect moveRectMenu;
	public MoveRect moveRectThis;
	public MoveRect moveRectOther;
	public int spriteImageNumber = 999;
	public ImageUISpriteSwap imageSwap;

	public void ClickedOnButton () {
		if(!moveRectThis.gameObject.activeSelf){
			if(!moveRectOther.gameObject.activeSelf){
				moveRectOther.gameObject.SetActive(true);
			}

			if(spriteImageNumber != 999 
			   && (!moveRectMenu.getChanging() || !moveRectMenu.blockMoveOnChanging) 
			   && (! moveRectOther.getChanging() || !moveRectOther.blockMoveOnChanging)){
				imageSwap.SwapToSprite(spriteImageNumber);
			}
			moveRectMenu.GoToStart();
			moveRectOther.GoToStart();

		}
	}
}
