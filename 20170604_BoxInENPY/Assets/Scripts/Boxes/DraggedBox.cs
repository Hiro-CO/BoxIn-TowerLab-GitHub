using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DraggedBox : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public SpriteRenderer spriteRendererFrente;
	public Text text;
	BoxMovement origin;
	BoxMovement destiny;

	public void StartDragging(BoxMovement originNew, Sprite spriteNew, Sprite spriteFrenteNew, int enemyIndex){
		origin = originNew;
		destiny = null;

		gameObject.SetActive (true);
		spriteRenderer.sprite = spriteNew;
		spriteRendererFrente.sprite = spriteFrenteNew;

		if (EnemyDataManager.control.getChosenEnemies ().Count > enemyIndex) {
			text.text = EnemyDataManager.control.getChosenEnemies () [enemyIndex].nameEn.ToUpper();
		}
	}

	public void EndDragging(){
		if (destiny != null) {
			origin.SwapPosition (destiny.gameObject);
		}
		destiny = null;
		gameObject.SetActive (false);
	}

	void OnTriggerEnter2D (Collider2D other) {
		if(other.tag == Tags.playerBoxDrag){
			if (other != origin) {
				if(destiny != null){
					destiny.OtherBoxEndHovering ();
					destiny = null;
				}
				destiny = other.gameObject.GetComponent<BoxMovement> ();
				destiny.OtherBoxStartHovering ();
			}
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if(other.tag == Tags.playerBox){
			if (other == destiny && destiny != null) {
				destiny.OtherBoxEndHovering ();
				destiny = null;
			}
		}
	}
}
