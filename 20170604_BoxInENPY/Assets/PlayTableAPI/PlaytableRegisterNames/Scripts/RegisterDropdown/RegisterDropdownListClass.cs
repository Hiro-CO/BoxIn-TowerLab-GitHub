using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.Events;

namespace Playmove
{
    public class RegisterDropdownListClass : RegisterDropdownListGeneric
    {
        private bool _initialized;

        #region PyOpenable

        public override void Open()
        {
            base.Open();

            Initialize();
            OpenWindow();

            ContentControl.UpPosition();

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

        /// TODO: Instanciar elementos em tempo de execução
        /// TODO: Melhorar Paginação de para filtros
        public override void Initialize()
        {
            if (_initialized)
                return;
            _initialized = true;

            _itensHolderPosition = ItensHolder.transform.localPosition;
            BoxCollider2D coll = gameObject.AddComponent<BoxCollider2D>();
            coll.size = _maskSize;
            coll.offset = MaskTransform.localPosition;
            ContentDrag.Content = ItensHolder;

            List<string> classes = NamesManagerPopup.Instance.GetClassList();

            if (_createdItens.Count == 0)
            {
                for (int i = 0; i < classes.Count; i++)
                {
                    RegisterDropdownItem newItem = Instantiate(ListItemPrefab, ItensHolder.position + (Vector3.down * i * _elementsDistance), Quaternion.identity) as RegisterDropdownItem;
                    newItem.transform.SetParent(ItensHolder);
                    newItem.MyButton.onClick.AddListener(ItemClicked);
                    newItem.ListIndex = _createdItens.Count;
                    newItem.Mask = _mask;
                    _createdItens.Add(newItem);
                }
            }
            else
            {
                foreach (RegisterDropdownItem name in _createdItens)
                {
                    name.RemoveItem();
                }
            }

            SetItens(classes);
        }

        public int GetSelectedClassIndex()
        {
            return _createdItens.Where((item) => item.Text == LabelText).Select(item => item.ListIndex).SingleOrDefault();
        }

        public void SetClass(int id)
        {
            // LabelText deve ser atualizado pelo RegisterDropdownManager
            GetComponentInParent<RegisterDropdownManager>().LabelText = _createdItens[id].Text;
        }

        private void ItemClicked(PYButton newItem)
        {
            onItemClicked.Invoke(newItem.transform.parent.GetComponent<RegisterDropdownItem>());
        }

        //private void SetItens(List<RegisterDropdownItem> itens)
        //{
        //    for (int i = 0; i < _createdItens.Count; i++)
        //    {
        //        if (i < itens.Count)
        //        {
        //            _createdItens[i].SetItem(itens[i].Text);
        //        }
        //        else
        //        {
        //            _createdItens[i].RemoveItem();
        //        }
        //    }
        //    CalculateDragLimit();
        //}
        private void SetItens(List<string> itens)
        {
            for (int i = 0; i < _createdItens.Count; i++)
            {
                if (i < itens.Count)
                {
                    _createdItens[i].SetItem(itens[i]);
                }
                else
                {
                    _createdItens[i].RemoveItem();
                }
            }
            CalculateDragLimit();
        }
    }
}