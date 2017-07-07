using UnityEngine;
using System.Collections;

public class MoveRect : MonoBehaviour {

	public float moveX = 0.4f;
	public float moveY = 0f;
	public float speed = 5f;
	public bool useLerp = true;
	public bool moveOnAwake = false;
	public MoveRectReachStartEvent reachStartEvent;
	public MoveRectReachEndEvent reachEndEvent;
	public float deltaTimeWhenPaused = 0.01f;
	public bool blockMoveOnChanging = false;
	float timePassed = 0f;
	RectTransform origin;
	Vector2 startMinAnchor;
	Vector2 startMaxAnchor;
	Vector2 endMinAnchor;
	Vector2 endMaxAnchor;

	bool changing = false;
	bool toStartPosition = true;

	void Awake () {
		origin = GetComponent<RectTransform>();
		startMinAnchor = origin.anchorMin;
		startMaxAnchor = origin.anchorMax;

		endMinAnchor = startMinAnchor;
		endMinAnchor.x += moveX;
		endMinAnchor.y += moveY;

		endMaxAnchor = startMaxAnchor;
		endMaxAnchor.x += moveX;
		endMaxAnchor.y += moveY;

		if (moveOnAwake) {
			GoToEnd ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(changing){
			if(!toStartPosition){
				if(!V2Equal(origin.anchorMin, endMinAnchor) && !V2Equal(origin.anchorMax, endMaxAnchor)){
					float moved = 0f;
					if(Time.deltaTime > 0){
						moved = Time.deltaTime * speed;
					}else{
						moved = deltaTimeWhenPaused * speed;
					}
					if (useLerp) {
						origin.anchorMin = Vector2.Lerp (origin.anchorMin, endMinAnchor, moved);
						origin.anchorMax = Vector2.Lerp (origin.anchorMax, endMaxAnchor, moved);
					} else {
						Vector2 newAnchorMin = origin.anchorMin;
						Vector2 newAnchorMax = origin.anchorMax;
						if (origin.anchorMin.x - endMinAnchor.x < -0.0001 && endMinAnchor.x - startMinAnchor.x > 0f) {
							newAnchorMin.x = origin.anchorMin.x + moved;
							if (newAnchorMin.x > endMinAnchor.x) {
								newAnchorMin.x = endMinAnchor.x;
							}
							newAnchorMax.x = origin.anchorMax.x + moved;
							if (newAnchorMax.x > endMaxAnchor.x) {
								newAnchorMax.x = endMaxAnchor.x;
							}
						}
						if (origin.anchorMin.x - endMinAnchor.x > 0.0001 && endMinAnchor.x - startMinAnchor.x < 0f) {
							newAnchorMin.x = origin.anchorMin.x - moved;
							if (newAnchorMin.x < endMinAnchor.x) {
								newAnchorMin.x = endMinAnchor.x;
							}
							newAnchorMax.x = origin.anchorMax.x - moved;
							if (newAnchorMax.x < endMaxAnchor.x) {
								newAnchorMax.x = endMaxAnchor.x;
							}
						}
						if (origin.anchorMin.y - endMinAnchor.y < -0.0001 && endMinAnchor.y - startMinAnchor.y > 0f) {
							newAnchorMin.y = origin.anchorMin.y + moved;
							if (newAnchorMin.y > endMinAnchor.y) {
								newAnchorMin.y = endMinAnchor.y;
							}
							newAnchorMax.y = origin.anchorMax.y + moved;
							if (newAnchorMax.y > endMaxAnchor.y) {
								newAnchorMax.y = endMaxAnchor.y;
							}
						}
						if (origin.anchorMin.y - endMinAnchor.y > 0.0001 && endMinAnchor.y - startMinAnchor.y < 0f) {
							newAnchorMin.y = origin.anchorMin.y - moved;
							if (newAnchorMin.y < endMinAnchor.y) {
								newAnchorMin.y = endMinAnchor.y;
							}
							newAnchorMax.y = origin.anchorMax.y - moved;
							if (newAnchorMax.y < endMaxAnchor.y) {
								newAnchorMax.y = endMaxAnchor.y;
							}
						}
						origin.anchorMin = newAnchorMin;
						origin.anchorMax = newAnchorMax;
					}
					timePassed += moved;
				}else{
					if(reachEndEvent != null){reachEndEvent.ExecuteEvents();}
					if (!useLerp) {
						origin.anchorMin = endMinAnchor;
						origin.anchorMax = endMaxAnchor;
					}
					changing = false;
					timePassed = 0f;
				}
			}else{
				if(!V2Equal(origin.anchorMin, startMinAnchor) && !V2Equal(origin.anchorMax, startMaxAnchor)){
					float moved = 0f;
					if(Time.deltaTime > 0){
						moved = Time.deltaTime * speed;
					}else{
						moved = deltaTimeWhenPaused * speed;
					}
					if (useLerp) {
						origin.anchorMin = Vector2.Lerp (origin.anchorMin, startMinAnchor, moved);
						origin.anchorMax = Vector2.Lerp (origin.anchorMax, startMaxAnchor, moved);
					} else {
						Vector2 newAnchorMin = origin.anchorMin;
						Vector2 newAnchorMax = origin.anchorMax;

						if (origin.anchorMin.x - startMinAnchor.x < -0.0001 && endMinAnchor.x - startMinAnchor.x < 0f) {
							newAnchorMin.x = origin.anchorMin.x + moved;
							if (newAnchorMin.x > startMinAnchor.x) {
								newAnchorMin.x = startMinAnchor.x;
							}
							newAnchorMax.x = origin.anchorMax.x + moved;
							if (newAnchorMax.x > startMaxAnchor.x) {
								newAnchorMax.x = startMaxAnchor.x;
							}
						}
						if (origin.anchorMin.x - startMinAnchor.x > 0.0001 && endMinAnchor.x - startMinAnchor.x > 0f) {
							newAnchorMin.x = origin.anchorMin.x - moved;
							if (newAnchorMin.x < startMinAnchor.x) {
								newAnchorMin.x = startMinAnchor.x;
							}
							newAnchorMax.x = origin.anchorMax.x - moved;
							if (newAnchorMax.x < startMaxAnchor.x) {
								newAnchorMax.x = startMaxAnchor.x;
							}
						}
						if (origin.anchorMin.y - startMinAnchor.y < -0.0001 && endMinAnchor.y - startMinAnchor.y < 0f) {
							newAnchorMin.y = origin.anchorMin.y + moved;
							if (newAnchorMin.y > startMinAnchor.y) {
								newAnchorMin.y = startMinAnchor.y;
							}
							newAnchorMax.y = origin.anchorMax.y + moved;
							if (newAnchorMax.y > startMaxAnchor.y) {
								newAnchorMax.y = startMaxAnchor.y;
							}
						}
						if (origin.anchorMin.y - startMinAnchor.y > 0.0001 && endMinAnchor.y - startMinAnchor.y > 0f) {
							newAnchorMin.y = origin.anchorMin.y - moved;
							if (newAnchorMin.y < startMinAnchor.y) {
								newAnchorMin.y = startMinAnchor.y;
							}
							newAnchorMax.y = origin.anchorMax.y - moved;
							if (newAnchorMax.y < startMaxAnchor.y) {
								newAnchorMax.y = startMaxAnchor.y;
							}
						}
						origin.anchorMin = newAnchorMin;
						origin.anchorMax = newAnchorMax;
					}
					timePassed += moved;
				}else{
					if(reachStartEvent != null){reachStartEvent.ExecuteEvents();}

					if (!useLerp) {
						origin.anchorMin = startMinAnchor;
						origin.anchorMax = startMaxAnchor;
					}
					changing = false;
					timePassed = 0f;
				}
			}
		}
	}

	public void GoToStart(){
		if(!changing || !blockMoveOnChanging){
			timePassed = 0f;
			changing = true;
			toStartPosition = true;
		}
	}

	public void GoToEnd(){
		if(!changing || !blockMoveOnChanging){
			timePassed = 0f;
			changing = true;
			toStartPosition = false;
		}
	}

	public void ToggleMovement(){
		timePassed = 0f;
		changing = true;
		toStartPosition = !toStartPosition;
	}

	public void ToggleMovementOnlyWhenStopped(){
		if(!changing){
			timePassed = 0f;
			changing = true;
			toStartPosition = !toStartPosition;
		}
	}

	public bool V2Equal(Vector2 a, Vector2 b){
		return Vector2.SqrMagnitude(a - b) < 0.0001;
	}

	public bool getChanging(){
		return changing;
	}

	public bool getToStartPosition(){
		return toStartPosition;
	}
}
