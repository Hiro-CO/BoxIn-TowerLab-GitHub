using UnityEngine;
using System.Collections;

public class AlignSpriteToCamera : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public Camera camera;
	public bool alignOnStart = true;
	public bool alignUp = false;
	public bool alignRight = false;
	public bool alignDown = false;
	public bool alignLeft = false;

	void Start () {
		if(alignUp){setTop ();}
		if(alignRight){setRight ();}
		if(alignDown){setBottom ();}
		if(alignLeft){setLeft ();}

		if(alignUp){setTop ();}
		if(alignRight){setRight ();}
		if(alignDown && camera.transform.rotation.z != 0f){setBottomUpsideDown ();}
		if(alignLeft){setLeft ();}
	}

	public void setTop(){
		Vector3 newBounds = spriteRenderer.gameObject.transform.position;
		newBounds.y = -(camera.ScreenToWorldPoint(camera.rect.min).y + spriteRenderer.bounds.extents.y);

		spriteRenderer.gameObject.transform.position = newBounds;
	}
	public void setRight(){
		Vector3 newBounds = spriteRenderer.gameObject.transform.position;
		newBounds.x = -(camera.ScreenToWorldPoint(camera.rect.min).x + spriteRenderer.bounds.extents.x);

		spriteRenderer.gameObject.transform.position = newBounds;
	}
	public void setBottom(){
		Vector3 newBounds = spriteRenderer.gameObject.transform.position;
		newBounds.y = camera.ScreenToWorldPoint(camera.rect.min).y + spriteRenderer.bounds.extents.y;

		spriteRenderer.gameObject.transform.position = newBounds;
	}
	public void setBottomUpsideDown(){
		Vector3 newBounds = spriteRenderer.gameObject.transform.position;
		newBounds.y = -camera.ScreenToWorldPoint(camera.rect.max).y + spriteRenderer.bounds.extents.y;

		spriteRenderer.gameObject.transform.position = newBounds;
	}
	public void setLeft(){
		Vector3 newBounds = spriteRenderer.gameObject.transform.position;
		newBounds.x = camera.ScreenToWorldPoint(camera.rect.min).x + spriteRenderer.bounds.extents.x;

		spriteRenderer.gameObject.transform.position = newBounds;
	}
}
