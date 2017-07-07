using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

namespace Playmove
{
    public class RegisterDropdownListNames : RegisterDropdownListGeneric
    {
        private List<PYNamesManager.NameData> _allNames;
        private List<PYNamesManager.NameData> AllNames
        {
            get
            {
                if (_allNames == null)
                    _allNames = new List<PYNamesManager.NameData>();
                return _allNames;
            }
            set
            {
                _allNames = value;
            }
        }

        public bool ListEmpty { get { return FilterNames().Count == 0; } }


        private BoxCollider2D _collider;
        private BoxCollider2D Collider
        {
            get
            {
                if (!_collider)
                {
                    _collider = GetComponent<BoxCollider2D>();
                    if (!_collider)
                        _collider = gameObject.AddComponent<BoxCollider2D>();
                }
                return _collider;
            }
        }

        private bool _itensCreated;
        private string _textWaitingToFilter;

        private void Awake()
        {
            _itensHolderPosition = ItensHolder.transform.localPosition;
        }

        #region PyOpenable

        public override void Open()
        {
            base.Open();

            if (AllNames.Count == 0)
            {
                PYNamesManager.LoadNames(NamesLoaded);
            }
            else
            {
                if (State != OpenableState.Opened)
                    OpenWindow();
                if (_itensCreated)
                {
                    ShowListView(NamesManagerPopup.Instance.TypedName);
                }
                else
                {
                    AnimateListEnter(() => StartCoroutine("UpdateListNamesRoutine", FilterNames()));
                }
            }

            Opened();
        }

        public override void Close()
        {
            base.Close();

            WindowAnimation.Stop();
            WindowAnimation
                .SetDuration(0.2f)
                .SetEaseType(Ease.Type.InBack);
            WindowAnimation.Reverse(() => Closed());
        }

        #endregion

        public override void Initialize()
        {
            Collider.size = _maskSize;
            Collider.offset = MaskTransform.localPosition;
            ContentDrag.Content = ItensHolder;

            gameObject.SetActive(true);

            ClearFilter();

            transform.localScale = new Vector3(1, 0, 1);
        }

        public override void AddFilter(string name)
        {
            base.AddFilter(name);
        }

        public override void RemoveFilter(string name)
        {
            base.RemoveFilter(name);
            _filterList.Remove(name);
        }

        private IEnumerator CreateItens()
        {
            int index = 0;
            while (index < _listQuantity)
            {
                CreateItem(index);
                index++;
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }

        private RegisterDropdownItem CreateItem(int indexPosition)
        {
            RegisterDropdownItem newName = ((NameRegister)ListItemPrefab).CreateName((ItensHolder.position + (Vector3.down * indexPosition * _elementsDistance)), ItensHolder, _createdItens.Count);
            newName.Mask = _mask;
            newName.transform.localScale = Vector3.one;
            _createdItens.Add(newName);
            return newName;
        }

        public bool ContaisName(string typedName)
        {
            bool exist = false;
            for (int i = 0; i < AllNames.Count; i++)
            {
                if (AllNames[i].Name == typedName)
                {
                    exist = true;
                    for (int j = 0; j < _filterList.Count; j++)
                    {
                        if (_filterList[j] == typedName)
                        {
                            exist = false;
                            break;
                        }
                    }
                    break;
                }
            }

            return exist;
        }

        public int GetClassId()
        {
            return AllNames.Find(name => name.Name == LabelText).ClassId;
        }

        public void UpdateListView(string text)
        {
            if (!IsOpen)
                Open();
            else
            {
                if (_itensCreated)
                    AnimateListExit(() => ShowListView(text));
                else
                    _textWaitingToFilter = text;
            }
        }

        public void AddName(PYNamesManager.NameData name)
        {
            AllNames.Add(name);
        }

        private void ShowListView(string filterText)
        {
            UpdateListNames(FilterNames(filterText));
            AnimateListEnter(CalculateDragLimit);
            GetComponent<ContentControl>().UpPositionForced();
            Invoke("UpdateForcedBar", 0.1f);
        }

        private void UpdateForcedBar()
        {
            GetComponent<ContentControl>().UpPositionForced();
        }

        private List<PYNamesManager.NameData> ConvertList(List<RegisterDropdownItem> list)
        {
            List<PYNamesManager.NameData> newList = new List<PYNamesManager.NameData>();
            foreach (RegisterDropdownItem item in list)
                newList.Add(((NameRegister)item).NameData);
            return newList;
        }

        private void NamesLoaded(List<PYNamesManager.NameData> names)
        {
            AllNames =
                (from name in names
                 orderby name.Name ascending
                 orderby name.LastUpdated descending
                 select name).ToList();

            OpenWindow();
            AnimateListEnter(() => StartCoroutine("UpdateListNamesRoutine", FilterNames()));
        }

        private List<PYNamesManager.NameData> FilterNames()
        {
            List<PYNamesManager.NameData> list = new List<PYNamesManager.NameData>(); ;
            bool add = true;
            for (int i = 0; i < AllNames.Count; i++)
            {
                add = true;
                for (int j = 0; j < _filterList.Count; j++)
                {
                    if (AllNames[i].Name == _filterList[j])
                    {
                        add = false;
                        break;
                    }
                }
                if (add)
                    list.Add(AllNames[i]);
            }

            return list;
        }
        private List<PYNamesManager.NameData> FilterNames(string text)
        {
            List<PYNamesManager.NameData> list = FilterNames();
            if (!string.IsNullOrEmpty(text))
            {
                list = list.Where(x => x.Name.StartsWith(text)).OrderBy(n => n.Name).ToList();
                if (list.Count == 0)
                    list = new List<PYNamesManager.NameData>();
            }

            return list;
        }

        private new void Reposition()
        {
            int i = 0;
            foreach (RegisterDropdownItem name in _createdItens)
            {
                name.transform.position = ItensHolder.position + (Vector3.down * i * _elementsDistance);
                i++;
            }
        }

        private void UpdateListNames(List<PYNamesManager.NameData> names)
        {
            for (int i = 0; i < _listQuantity; i++)
            {
                if (i < names.Count)
                    ((NameRegister)_createdItens[i]).EnterName(names[i]);
                else
                    ((NameRegister)_createdItens[i]).RemoveName();
            }
        }

        private IEnumerator UpdateListNamesRoutine(List<PYNamesManager.NameData> names)
        {
            int index = 0;
            while (index < _listQuantity)
            {
                NameRegister nameItem;
                if (index >= _createdItens.Count)
                    nameItem = (NameRegister)CreateItem(index);
                else
                {
                    nameItem = (NameRegister)_createdItens[index];
                    nameItem.Reposition((ItensHolder.position + (Vector3.down * index * _elementsDistance)));
                }

                if (index < names.Count)
                    nameItem.EnterName(names[index]);
                else
                    nameItem.RemoveName();

                index++;
                yield return null;
            }

            _itensCreated = true;

            Reposition();
            CalculateDragLimit();

            if (!string.IsNullOrEmpty(_textWaitingToFilter))
                UpdateListView(_textWaitingToFilter);
        }
    }
}