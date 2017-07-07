using UnityEngine;
using System.Collections;

namespace Playmove
{
    public class RecordItem : MonoBehaviour
    {
        [Header("RecordItem")]

        public int AmountTextToSet = 1;
        public PYText NameText;

        protected PYAnimation _nameTextAnim;
        public PYAnimation NameTextAnim
        {
            get
            {
                if (_nameTextAnim == null)
                    _nameTextAnim = NameText.GetComponent<PYAnimation>();
                return _nameTextAnim;
            }
        }

        protected System.Action _callback;
        protected bool _hasSetted = false;
        protected int _animationCounterCompleted;

        public void SetItem(string name, System.Action callback = null)
        {
            _animationCounterCompleted = AmountTextToSet;
            if (_hasSetted)
                PlayAnimations(false);
            _hasSetted = true;

            _callback = callback;
            StartCoroutine(setItemRoutine(name));
        }

        IEnumerator setItemRoutine(string name)
        {
            if (NameTextAnim is PYTweenAnimation)
                yield return new WaitForSeconds(((PYTweenAnimation)NameTextAnim).AnimationData.Duration);
            else
                yield return null;

            NameText.Text = name;
            PlayAnimations(true);
        }

        protected virtual void PlayAnimations(bool hasCallback)
        {
            System.Action callback = null;
            if (hasCallback) callback = CompletedTextAnimations;

            NameTextAnim.Stop();
            NameTextAnim.Play(callback);
        }

        protected virtual void ReverseAnimations(bool hasCallback = false)
        {
            System.Action callback = null;
            if (hasCallback) callback = CompletedTextAnimations;

            NameTextAnim.Stop();
            NameTextAnim.Reverse(callback);
        }

        protected virtual void CompletedTextAnimations()
        {
            _animationCounterCompleted--;
            if (_animationCounterCompleted <= 0)
            {
                _animationCounterCompleted = AmountTextToSet;
                if (_callback != null)
                    _callback();
            }
        }
    }
}