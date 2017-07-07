using UnityEngine;
using System.Collections;

public class BtnVoltarWithAnimation : MonoBehaviour {

	public goToScene goToSceneScript;
	public Animator animator;
	public VoltarAnimatorHashes voltarHash;
	public GameObject OffButton;
	bool firstClick = false;

	public void ClickedOnButton(){
		if(!firstClick){
			animator.SetBool (voltarHash.instantInvisible, true);
			animator.SetBool (voltarHash.visible, true);
			firstClick = true;
			OffButton.SetActive(true);
		}else{
			goToSceneScript.selectScene ();			
		}
	}

	public void ClickedOnButtonConfirmation(){
		if(firstClick){
			goToSceneScript.selectScene ();			
		}
	}

	public void ClickedOffTheButton(){
		if(firstClick){
			animator.SetBool (voltarHash.instantVisible, true);
			animator.SetBool (voltarHash.visible, false);
			firstClick = false;
			OffButton.SetActive(false);
		}
	}
}
