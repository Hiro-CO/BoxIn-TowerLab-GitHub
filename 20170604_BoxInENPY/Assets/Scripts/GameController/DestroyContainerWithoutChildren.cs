using UnityEngine;
using System.Collections;

public class DestroyContainerWithoutChildren : MonoBehaviour {
	private Transform objectTransform;

	void Awake(){
		objectTransform = transform;
	}

	void Update () {
		if(!hasChildren()){
			gameObject.SetActive (false);
		}
	}

	bool hasChildren(){		
		if(objectTransform.childCount > 0){
			return true;
		}else{
			return false;
		}
	}

}
