using UnityEngine;
using System.Collections;

public class DisableWhenClickedOutsideUI : MonoBehaviour {

	public RectTransform buttonRect;
	Rect buttonArea;
	public Rect ButtonArea{ get{ return buttonArea; }}

	//Only works well because the object turns active after player clicks on this button
	//the ReverseOnActualPlayerNumber rotate the camera on Start, so this would not work if
	//the object were active from the beggining of the Scene.
	void Awake () {
		Recalculate (Camera.main.transform.rotation.eulerAngles.z);
	}

	void Update () {
		#if UNITY_EDITOR || UNITY_WEBGL
		if (Input.GetMouseButtonDown (0)) {
			CheckClickPosition(Input.mousePosition);
		}
		#else
		if(Input.touchCount > 0){
			CheckClickPosition(Input.touches[0].position);
		}
		#endif
	}

	void CheckClickPosition(Vector3 position){
		if (!ButtonArea.Contains (position)) {
			gameObject.SetActive (false);
		}
	}

	public void Recalculate(float rotationZ){
		if(rotationZ == 0){
			buttonArea = GetRectFromRectTransformUI(buttonRect);
		}else{
			buttonArea = GetRectFromRectTransformUIReverse(buttonRect);
		}
	}

	Rect GetRectFromRectTransformUI(RectTransform rectTransform){
		return new Rect(rectTransform.anchorMin.x*Screen.width,rectTransform.anchorMin.y*Screen.height, GetWidthFromRectTransformUI(rectTransform), GetHeightFromRectTransformUI(rectTransform));
	}

	Rect GetRectFromRectTransformUIReverse(RectTransform rectTransform){
		return new Rect((1-rectTransform.anchorMax.x)*Screen.width,(1-rectTransform.anchorMax.y)*Screen.height,GetWidthFromRectTransformUI(rectTransform), GetHeightFromRectTransformUI(rectTransform));
	}

	float GetWidthFromRectTransformUI(RectTransform rectTransform){
		//the rect is a percentage. the maximum number, 1, represents 100% from the screen width
		//so we multiply the screen width by the rect to get the real width of the button
		float rectWidth = rectTransform.anchorMax.x - rectTransform.anchorMin.x;
		rectWidth *= Screen.width;

		return rectWidth;
	}

	float GetHeightFromRectTransformUI(RectTransform rectTransform){
		//the rect is a percentage. the maximum number, 1, represents 100% from the screen width
		//so we multiply the screen width by the rect to get the real width of the button
		float rectHeight = rectTransform.anchorMax.y - rectTransform.anchorMin.y;
		rectHeight *= Screen.height;

		return rectHeight;
	}
}
