using UnityEngine;
using System.Collections.Generic;

namespace Playmove
{
    public class RegisterWindow : PYScoreNavigation
    {
        private List<RecordItem> _entryItems = new List<RecordItem>();

        protected override void Start()
        {
            base.Start();

            // Just for tests!
            PYScoreData.DeleteAll();
            for (int i = 0; i < 15; i++)
            {
				PYScoreData.RegisterStudent("MMMMMMMMMMMMMMMMMM" + i, 100 * (i == 0 ? 1 : i), PYGameManager.Instance.GameDifficulty);
            }

            Show();
        }

        public override void Refresh(int? page = null)
        {
			_students = PYScoreData.Students;
            base.Refresh();
        }

        protected override void CreateRecordItems()
        {
            base.CreateRecordItems();

            for (int x = 0; x < _recordItems.Count; x++)
                _entryItems.Add(_recordItems[x].GetComponent<RecordItem>());
        }

        protected override void AnimateRecordItems()
        {
            for (int x = 0; x < _paginator.Indexes.Count; x++)
            {
                if (_paginator.Indexes[x] != -1 &&
                    _students.Count > _paginator.Indexes[x])
                {
                    _entryItems[x].SetItem(_students[_paginator.Indexes[x]].Name, CompletedSettingRecordItems);
                }
                else
                {
                    _entryItems[x].SetItem(EmptyRecordText, CompletedSettingRecordItems);
                }
            }
        }

        protected override void CompletedSettingRecordItems()
        {
            _counterSettingRecordItems++;
            if (_counterSettingRecordItems >= AmountRecordsItemPerPage)
            {
                _counterSettingRecordItems = 0;
                UpdateButtonsNavigationState();
            }
        }
    }
}