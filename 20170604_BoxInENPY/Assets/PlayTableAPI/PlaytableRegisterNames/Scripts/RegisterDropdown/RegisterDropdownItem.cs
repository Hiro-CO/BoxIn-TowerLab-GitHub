using UnityEngine;
using System.Collections;
using System;

namespace Playmove
{
    public class RegisterDropdownItem : MonoBehaviour
    {
        public RegisterDropdownItem()
        { }

        public RegisterDropdownItem(string text)
        {
            Text = text;
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        public int ListIndex { get; set; }

        public Renderer Mask;

        private PYTextBox _myText;
        public PYTextBox MyText
        {
            get
            {
                if (!_myText) _myText = GetComponentInChildren<PYTextBox>();
                return _myText;
            }
        }

        private BoxCollider2D _myCollider;
        public BoxCollider2D MyCollider
        {
            get
            {
                if (!_myCollider) _myCollider = GetComponentInChildren<BoxCollider2D>();
                return _myCollider;
            }
        }

        private PYButton _myButton;
        public PYButton MyButton
        {
            get
            {
                if (!_myButton) _myButton = GetComponentInChildren<PYButton>();
                return _myButton;
            }
        }

        #region Unity

        private void Update()
        {
            if (Mask == null) return;

            //MyCollider.enabled = (Mask.bounds.Intersects(MyText.GetComponent<Renderer>().bounds));
            MyCollider.enabled = ((MyCollider.bounds.min.y > Mask.bounds.min.y && MyCollider.bounds.max.y < Mask.bounds.max.y));
        }

        #endregion

        public void RemoveItem()
        {
            Text = MyText.Text = "";
            gameObject.SetActive(false);
        }

        internal void SetItem(string text)
        {
            gameObject.SetActive(true);
            Text = MyText.Text = text;
        }
    }
}