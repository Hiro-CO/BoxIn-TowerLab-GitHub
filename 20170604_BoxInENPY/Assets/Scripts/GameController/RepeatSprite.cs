using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]

public class RepeatSprite : MonoBehaviour {
	
	private bool hasLeft = false;
	private GameObject tileOnLeft;
	private bool hasRight = false;
	private GameObject tileOnRight;
	public int offset = 2;
	public bool isPatternSprite = true;
	public ObjectsPooler poolerToUse;
	private Camera cam;
	private Transform objectTransform;
	private float spriteWidth = 0f;
	
	void Awake(){
		cam = Camera.main;
		objectTransform = transform;
	}
	
	void Start () {
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = spriteRenderer.bounds.max.x - spriteRenderer.bounds.min.x;
	}
	
	void Update () {
		if(!hasLeft || !hasRight){
			
			//half the horizontal size of the screen
			//used to discover the camera border position
			//(because the camera.position return the camera center position)
			float cameraHorizontalSize = cam.orthographicSize * Screen.width/Screen.height;
			
			//half the width to discover the sprite border
			//(because the sprite.position return the center of the sprite)
			//the cameraHorizontalSize is used because on the if below we use the center of the camera
			float spriteRight = (objectTransform.position.x + spriteWidth/2) - cameraHorizontalSize;
			float spriteLeft = (objectTransform.position.x - spriteWidth/2) + cameraHorizontalSize;
			
			if(cam.transform.position.x >= spriteRight - offset
			   && !hasRight){
				tileOnRight = MakeTile(1);
				hasRight = true;
			}
			
			if(cam.transform.position.x <= spriteLeft + offset
			   && !hasLeft){
				tileOnLeft = MakeTile(-1);
				hasLeft = true;
			}
			CheckToDestroyThisTile();
		}

	}
	
	GameObject MakeTile(int direction){
		Vector3 newTilePosition = new Vector3 ( objectTransform.position.x + (spriteWidth * direction) , objectTransform.position.y, objectTransform.position.z);

		Transform newTile = poolerToUse.UsePooledObject (newTilePosition, objectTransform.rotation).transform;
		//Transform newTile = (Transform) Instantiate(objectTransform, newTilePosition, objectTransform.rotation);
		newTile.name = objectTransform.name;
		
		//if the sprite is not a pattern, we can reverse the scale, so the borders of the image 
		//will connect a little better (ideally we must use a pattern, this is just a small fix)
		if(!isPatternSprite){
			newTile.localScale = new Vector3(newTile.localScale.x*-1, newTile.localScale.y, newTile.localScale.z);
		}
		
		newTile.parent = objectTransform.parent;
		
		if (direction > 0) {
			RepeatSprite repeatSprite = newTile.GetComponent<RepeatSprite>();
			repeatSprite.SetHasLeft(true);
			repeatSprite.SetTileOnLeft(gameObject);
		} else {
			RepeatSprite repeatSprite = newTile.GetComponent<RepeatSprite>();
			repeatSprite.SetHasRight(true);
			repeatSprite.SetTileOnRight(gameObject);
		}
		
		return newTile.gameObject;
	}

	void CheckToDestroyThisTile(){
		if(!hasLeft || !hasRight){
			//half the horizontal size of the screen
			//used to discover the camera border position
			//(because the camera.position return the camera center position)
			float cameraHorizontalSize = cam.orthographicSize * Screen.width/Screen.height;
			
			//half the width to discover the sprite border
			//(because the sprite.position return the center of the sprite)
			//the cameraHorizontalSize is used because on the if below we use the center of the camera
			float spriteRight = (objectTransform.position.x + spriteWidth/2);
			float spriteLeft = (objectTransform.position.x - spriteWidth/2);
			
			if(spriteRight < cam.transform.position.x  - cameraHorizontalSize - offset ){
				DestroyThisTile();					
			}				

			if(spriteLeft > cam.transform.position.x + cameraHorizontalSize + offset ){
				DestroyThisTile();
			}
		}
	}

	void DestroyThisTile(){
		/*
		Destroy(gameObject);
		*/
		gameObject.SetActive (false);
		SetHasLeft (false);
		SetHasRight (false);
		if(tileOnRight != null){
			tileOnRight.GetComponent<RepeatSprite>().SetHasLeft(false);
		}
		if(tileOnLeft != null){
			tileOnLeft.GetComponent<RepeatSprite>().SetHasRight(false);
		}
	}
	
	void SetHasLeft(bool hasLeft){
		this.hasLeft = hasLeft;
	}
	
	void SetHasRight(bool hasRight){
		this.hasRight = hasRight;
	}
	
	void SetTileOnLeft(GameObject tileOnLeft){
		this.tileOnLeft = tileOnLeft;
	}
	
	void SetTileOnRight(GameObject tileOnRight){
		this.tileOnRight = tileOnRight;
	}
}
