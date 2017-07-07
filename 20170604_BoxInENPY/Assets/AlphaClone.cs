using UnityEngine;
using System.Collections;
using Playmove;

public class AlphaClone : MonoBehaviour {

	public PYTextBox origin;
	public SpriteRenderer destiny;

	void LateUpdate () {
		if (destiny.color.a != 1f && origin.Text == "") {
			Color newColor = destiny.color;
			newColor.a = origin.Color.a;
			destiny.color = newColor;
		} 
		if (destiny.color.a != 0f && origin.Text != "") {
			Color newColor = destiny.color;
			newColor.a = 0f;
			destiny.color = newColor;
		}
	}
}
