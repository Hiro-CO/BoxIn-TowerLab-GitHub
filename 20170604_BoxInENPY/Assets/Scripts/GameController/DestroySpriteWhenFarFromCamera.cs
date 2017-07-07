using UnityEngine;
using System.Collections;

public class DestroySpriteWhenFarFromCamera : MonoBehaviour {
	public float offset = 6f;
	private Camera cam;
	private Transform objectTransform;
	private float spriteWidth;
	private float spriteHeight;

	void Awake(){
		cam = Camera.main;
		objectTransform = transform;
	}
	
	void Start () {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = spriteRenderer.bounds.max.x - spriteRenderer.bounds.min.x;
		spriteHeight = spriteRenderer.bounds.max.y - spriteRenderer.bounds.min.y;
	}
	
	void Update () {
		CheckToDestroyThisSprite();
	}

	void CheckToDestroyThisSprite(){
			//half the horizontal size of the screen
			//used to discover the camera border position
			//(because the camera.position return the camera center position)
			float cameraHorizontalSize = cam.orthographicSize * Screen.width/Screen.height;
			float cameraVerticalSize = 2f * cam.orthographicSize;
			
			//half the width to discover the sprite border
			//(because the sprite.position return the center of the sprite)
			//the cameraHorizontalSize is used because on the if below we use the center of the camera
			float spriteRight = (objectTransform.position.x + spriteWidth/2);
			float spriteLeft = (objectTransform.position.x - spriteWidth/2);
			float spriteUp = (objectTransform.position.y + spriteHeight/2);
			float spriteDown = (objectTransform.position.y - spriteHeight/2);
			
			if(spriteRight < cam.transform.position.x  - cameraHorizontalSize - offset ){
				DestroyThisSprite();					
			}				
			
			if(spriteLeft > cam.transform.position.x + cameraHorizontalSize + offset ){
				DestroyThisSprite();
			}

			if(spriteUp > cam.transform.position.y  + cameraVerticalSize + offset ){
				DestroyThisSprite();					
			}

			if(spriteDown < cam.transform.position.y - cameraVerticalSize - offset ){
				DestroyThisSprite();
			}
	}

	void DestroyThisSprite(){
		//Destroy(gameObject);
		gameObject.SetActive(false);
	}

}
