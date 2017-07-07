using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Playmove;

public class EnableObjectsOnSpecificScenes : MonoBehaviour {

	public string[] scenesToEnableObject;
	public GameObject[] objectsToEnable;
	public SpriteRenderer[] spriteRenderersToHide;
	public SortingOrder[] sortingOrderScriptsToHide;
	public Collider2D[] collidersToDisable;
	public int sortNumberToShow = 30;
	public int sortNumberToHide = 0;

	void Awake(){
		EnableEvent ();
	}

	void OnLevelWasLoaded(int level) {
		EnableEvent ();
	}

	void EnableEvent(){
		bool enableCheck = false;
		foreach(string levelString in scenesToEnableObject){
			if(levelString == SceneManager.GetActiveScene().name){
				enableCheck = true;
			}
		}

		foreach(GameObject objectToEnable in objectsToEnable){
			objectToEnable.SetActive(enableCheck);
		}

		foreach(SpriteRenderer spriteRendererToHide in spriteRenderersToHide){
			if(enableCheck){
				spriteRendererToHide.sortingOrder = sortNumberToShow;
			}else{
				spriteRendererToHide.sortingOrder = sortNumberToHide;
			}
		}


		foreach(SortingOrder sortingOrderScriptToHide in sortingOrderScriptsToHide){
			if(enableCheck){
				sortingOrderScriptToHide.ChangeSortingOrder(sortNumberToShow);
			}else{
				sortingOrderScriptToHide.ChangeSortingOrder(sortNumberToHide);
			}
		}

		foreach (Collider2D col in collidersToDisable) {
			if(enableCheck){
				col.enabled = true;
			}else{
				col.enabled = false;
			}
		}

	}
}
