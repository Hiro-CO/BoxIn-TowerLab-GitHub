using UnityEngine;
using System.Collections;

public class ButtonGoTopressed : MonoBehaviour {

	public Sprite btnPressed;
	public SpriteRenderer spriteRenderer;
	public MusicPlay musicPlay;

	public void GoToPressed(){
		spriteRenderer.sprite = btnPressed;
		//musicPlay.PlayTheMusic ();
	}
}
