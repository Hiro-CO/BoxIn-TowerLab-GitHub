using UnityEngine;
using System.Collections.Generic;

namespace Playmove
{
    public class ScoreboardWindow : MonoBehaviour
    {
        public Transform ContentObj;
        public GameObject RecordItemObj;

        public float LineSpace = 0.6f;
        public string EmptyRecordText = "---";

        public int AmountRecordsItemPerPage = 10;
        public TagManager.GameDifficulty InitialDifficulty = TagManager.GameDifficulty.Easy;

        public PYButton ButtonDelete;
        public PYButton[] ButtonsDifficulties;
        public PYButton[] ButtonsNavigation;

        private int _currentDifficulty = 0;
        private PYPagination _paginator;

        private List<ScoreRecordItem> _recordsItems;
        private List<Student> _students = new List<Student>();

        private int _counterSettingRecordItems = 0;

        // Use this for initialization
        void Start()
        {
            // Just for tests!
			/*
            PYScoreData.DeleteAll();
            for (int i = 0; i < 10; i++)
            {
                PYScoreData.RegisterStudent("MMMMMMMMMMMMMMMMMM" + i, 100 * (i == 0 ? 1 : i), (TagManager.GameDifficulty)Random.Range(1, 4));
            }
            */
            //end test

            CreateRecordItems();

            // Create new Pagination with default values
            _paginator = new PYPagination(0, AmountRecordsItemPerPage, 1);

            _currentDifficulty = (int)InitialDifficulty;
            Show(_currentDifficulty);
        }

		public void ChangeDifficulty(int newDifficulty){
			int difficulty = newDifficulty;
			_currentDifficulty = difficulty;
			Show(_currentDifficulty);
		}

        public void Show(int difficulty)
        {
            EnableDifficultyButtons(false);
            EnableNavigationButtons(false);

            UpdateButtonDeleteState();

            _currentDifficulty = difficulty;
            Refresh();
        }

        public void Refresh()
        {
            PYScoreData.OrganizeScores((TagManager.GameDifficulty)_currentDifficulty + 1);
            _students = PYScoreData.Students;

            _paginator.TotalElements = _students.Count;
            _paginator.NavigateToPage(1);

            AnimateRecordItems();
        }

        public void NavigateUp()
        {
            if (_students.Count == 0)
                return;

            EnableNavigationButtons(false);

            _paginator.NavigateLeft();
            AnimateRecordItems();
        }

        public void NavigateDown()
        {
            if (_students.Count == 0)
                return;

            EnableNavigationButtons(false);

            _paginator.NavigateRight();
            AnimateRecordItems();
        }

        void AnimateRecordItems()
        {
            for (int x = 0; x < _paginator.Indexes.Count; x++)
            {
                if (_paginator.Indexes[x] != -1 &&
                    _students.Count > _paginator.Indexes[x])
                {
                    _recordsItems[x].SetItem(
                        (_paginator.Indexes[x] + 1).ToString(),
                        _students[_paginator.Indexes[x]].Name,
                        _students[_paginator.Indexes[x]].Score[_currentDifficulty].ToString(),
                        CompletedSettingRecordItems);
                }
                else
                {
                    _recordsItems[x].SetItem(EmptyRecordText, EmptyRecordText, EmptyRecordText, CompletedSettingRecordItems);
                }
            }
        }

        // When all recordItems has been set
        void CompletedSettingRecordItems()
        {
            _counterSettingRecordItems++;
            if (_counterSettingRecordItems >= _recordsItems.Count)
            {
                _counterSettingRecordItems = 0;

                UpdateButtonsNavitionState();
                UpdateButtonDeleteState();

                EnableDifficultyButtons(true);
                // Disable the current difficulty button
                //ButtonsDifficulties[_currentDifficulty].IsEnabled = false;
            }
        }

        void CreateRecordItems()
        {
            // If we already have all the necessary recordItens created just return
            if (ContentObj.childCount >= AmountRecordsItemPerPage) return;

            _recordsItems = new List<ScoreRecordItem>();
            RecordItemObj.name = "RecordItem0";
            _recordsItems.Add(RecordItemObj.GetComponent<ScoreRecordItem>());

            for (int x = 0; x < AmountRecordsItemPerPage - 1; x++)
            {
                GameObject item = (GameObject)Instantiate(RecordItemObj);
                item.name = "RecordItem" + (x + 1);
                item.transform.SetParent(ContentObj);
                item.transform.localPosition = RecordItemObj.transform.localPosition - (Vector3.up * LineSpace) * (x + 1);

                _recordsItems.Add(item.GetComponent<ScoreRecordItem>());
            }
        }

        void EnableNavigationButtons(bool isEnabled)
        {
            foreach (PYButton btn in ButtonsNavigation)
                btn.IsEnabled = isEnabled;
        }

        void EnableDifficultyButtons(bool isEnable)
        {
            foreach (PYButton btn in ButtonsDifficulties)
                btn.IsEnabled = isEnable;
        }

        void UpdateButtonDeleteState()
        {
            ButtonDelete.IsEnabled = PYScoreData.Students.Count != 0;
        }

        void UpdateButtonsNavitionState()
        {
            foreach (PYButton btn in ButtonsNavigation)
                btn.IsEnabled = _students.Count != 0 && _paginator.TotalPages > 1;
        }
    }
}