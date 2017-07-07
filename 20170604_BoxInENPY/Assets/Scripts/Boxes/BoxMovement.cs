using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxMovement : MonoBehaviour {

	public static List<BoxMovement> allBoxes = new List<BoxMovement>();
	public int enemyIndex = 0;
	public SpriteRenderer spriteRendererUsed;
	Sprite spriteUsed;
	public SpriteRenderer spriteRendererFrenteUsed;
	Sprite spriteFrenteUsed;
	Color startingColor;
	public Color colorOnDrag;
	public Animator animator;
	public BoxesAnimatorHashes boxHashes;
	public BoxEnableDisable boxEnableDisable;

	public void Awake(){
		for(int i=0; i < allBoxes.Count; i++){
			if (allBoxes[i] == null) {
				BoxMovement boxToRemove = allBoxes [i];
				allBoxes.Remove (boxToRemove);
			}
		}

		if (!allBoxes.Contains (this)) {
			allBoxes.Add (this);
		}
		spriteUsed = spriteRendererUsed.sprite;
		spriteFrenteUsed = spriteRendererFrenteUsed.sprite;
		startingColor = spriteRendererUsed.color;
	}

	public void StartDragging(){
		spriteRendererUsed.color = colorOnDrag;
		spriteRendererFrenteUsed.color = colorOnDrag;
	}

	public void EndDragging(){
		spriteRendererUsed.color = startingColor;
		spriteRendererFrenteUsed.color = startingColor;
	}

	public void OtherBoxStartHovering(){
		
	}

	public void OtherBoxEndHovering(){

	}

	public void SwapPosition(GameObject target){
		Vector3 pos = transform.position;
		transform.position = target.transform.position;
		target.transform.position = pos;
	}

	public Sprite getSpriteUsed(){
		return spriteUsed;
	}

	public Sprite getSpriteFrenteUsed(){
		return spriteFrenteUsed;
	}

	public void enableBox(){
		
	}

	public void disableBox(){
	
	}

	public void KilledEnemy(){
		animator.SetBool (boxHashes.collide, true);
	}
}
