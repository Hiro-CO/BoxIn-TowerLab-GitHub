using UnityEngine;
using System.Collections;

public class PrevNextSlide : MonoBehaviour {

	public RectTransform[] rectTransforms;
	int activeObjectIndex = 0;
	public PrevNextRects[] prevNextRects;
	public bool useScreenTransition = true;
	public ObjectsPooler[] objectsPoolers;
	public MainTagEnemy[] tagsSequence;

	void Start(){
		if (useScreenTransition) {
			foreach (ObjectsPooler pooler in objectsPoolers) {
				pooler.DeactivateAllPooledObjects ();
			}
			EnemyDataManager.control.FilterEnemies (tagsSequence [0]);
			int enemiesToSelect = 5;
			if (tagsSequence [activeObjectIndex] == MainTagEnemy.Seasons) {
				enemiesToSelect = 4;
			}
			EnemyDataManager.control.ChooseFromFilteredEnemies (enemiesToSelect);
		}
	}

	public void ClickedNext(){
		if (!prevNextRects[activeObjectIndex].moveRectsGoToLeft .getChanging () && !prevNextRects[activeObjectIndex].moveRectsGoToRight .getChanging ()) {
			prevNextRects[activeObjectIndex].moveRectsGoToLeft .GoToEnd ();

			if (activeObjectIndex < rectTransforms.Length - 1) {
				activeObjectIndex++;
			} else {
				activeObjectIndex = 0;
			}
			Vector2 anchorMinNew = rectTransforms [activeObjectIndex].anchorMin;
			anchorMinNew.x = 1;
			Vector2 anchorMaxNew = rectTransforms [activeObjectIndex].anchorMax;
			anchorMaxNew.x = 2;
			rectTransforms [activeObjectIndex].anchorMin = anchorMinNew;
			rectTransforms [activeObjectIndex].anchorMax = anchorMaxNew;
			rectTransforms [activeObjectIndex].gameObject.SetActive (true);
			prevNextRects [activeObjectIndex].moveRectsGoToRight.GoToStart ();

			if (useScreenTransition) {
				PrefabsInAllScenes.control.screenTransition.SetActive (true);
				foreach (ObjectsPooler pooler in objectsPoolers) {
					pooler.DeactivateAllPooledObjects ();
				}
				EnemyDataManager.control.FilterEnemies (tagsSequence [activeObjectIndex]);

				int enemiesToSelect = 5;
				if (tagsSequence [activeObjectIndex] == MainTagEnemy.Seasons) {
					enemiesToSelect = 4;
				}
				EnemyDataManager.control.ChooseFromFilteredEnemies (enemiesToSelect);
			}
		}
	}

	public void ClickedPrev(){
		if (!prevNextRects[activeObjectIndex].moveRectsGoToLeft .getChanging () && !prevNextRects[activeObjectIndex].moveRectsGoToRight .getChanging ()) {
			prevNextRects[activeObjectIndex].moveRectsGoToRight .GoToEnd ();

			if (activeObjectIndex > 0) {
				activeObjectIndex--;
			} else {
				activeObjectIndex = rectTransforms.Length - 1;
			}
			Vector2 anchorMinNew = rectTransforms [activeObjectIndex].anchorMin;
			anchorMinNew.x = -1;
			Vector2 anchorMaxNew = rectTransforms [activeObjectIndex].anchorMax;
			anchorMaxNew.x = 0;
			rectTransforms [activeObjectIndex].anchorMin = anchorMinNew;
			rectTransforms [activeObjectIndex].anchorMax = anchorMaxNew;
			rectTransforms [activeObjectIndex].gameObject.SetActive (true);
			prevNextRects[activeObjectIndex].moveRectsGoToLeft .GoToStart ();

			if (useScreenTransition) {
				PrefabsInAllScenes.control.screenTransition.SetActive (true);
				foreach (ObjectsPooler pooler in objectsPoolers) {
					pooler.DeactivateAllPooledObjects ();
				}
				EnemyDataManager.control.FilterEnemies (tagsSequence [activeObjectIndex]);

				int enemiesToSelect = 5;
				if (tagsSequence [activeObjectIndex] == MainTagEnemy.Seasons) {
					enemiesToSelect = 4;
				}
				EnemyDataManager.control.ChooseFromFilteredEnemies (enemiesToSelect);
			}
		}
	}
}
