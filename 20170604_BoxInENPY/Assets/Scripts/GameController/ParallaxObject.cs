using UnityEngine;
using System.Collections;

public class ParallaxObject : MonoBehaviour {

	public Transform backgrounds;
	public float smooth = 1;
	private float parallaxValues;
	private Transform cameraPosition;
	private Vector3 cameraPrevPosition;

	void Awake(){
		backgrounds = transform;
		cameraPosition = Camera.main.transform;
	}

	void Start () {
		parallaxValues = backgrounds.position.z * -1;

		cameraPrevPosition = cameraPosition.position;
	}
	
	void Update () {
		float parallaxX = (cameraPrevPosition.x - cameraPosition.position.x) * parallaxValues;
		//float parallaxY = (cameraPrevPosition.y - cameraPosition.position.y) * parallaxValues;

		float backgroundTargetPosX = backgrounds.position.x + parallaxX;
		//float backgroundTargetPosY = backgrounds.position.y + parallaxY;

		//Vector3 targetPosition = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds.position.z);
		Vector3 targetPosition = new Vector3(backgroundTargetPosX, backgrounds.position.y, backgrounds.position.z);
		backgrounds.position = Vector3.Lerp(backgrounds.position, targetPosition, smooth * Time.deltaTime);
		//backgrounds.position = targetPosition;

		cameraPrevPosition = cameraPosition.position;
	}
}
