using UnityEngine;
using System.Collections;

namespace Playmove
{
    public class ScoreRecordItem : RecordItem
    {
        [Header("ScoreRecordItem")]
        public PYText PositionText;
        public PYText PointsText;

        #region Socore Board Texts Animations
        private PYAnimation _positionTextAnim;
        private PYAnimation PositionTextAnim
        {
            get
            {
                if (_positionTextAnim == null)
                    _positionTextAnim = PositionText.GetComponent<PYAnimation>();
                return _positionTextAnim;
            }
        }

        private PYAnimation _pointsTextAnim;
        private PYAnimation PointsTextAnim
        {
            get
            {
                if (_pointsTextAnim == null)
                    _pointsTextAnim = PointsText.GetComponent<PYAnimation>();
                return _pointsTextAnim;
            }
        }
        #endregion

        public void SetItem(string position, string name, string points, System.Action callback = null)
        {
            if (_hasSetted)
                ReverseAnimations();
            _hasSetted = true;

            _callback = callback;
            StartCoroutine(setItemRoutine(position, name, points));
        }

        IEnumerator setItemRoutine(string position, string name, string points)
        {
            if (NameTextAnim is PYTweenAnimation)
                yield return new WaitForSeconds(((PYTweenAnimation)NameTextAnim).AnimationData.Duration);
            else
                yield return null;

            PositionText.Text = position;
            NameText.Text = name;
            PointsText.Text = points;

            PlayAnimations(true);
        }

        protected override void PlayAnimations(bool hasCallback)
        {
            base.PlayAnimations(hasCallback);

            System.Action callback = null;
            if (hasCallback) callback = CompletedTextAnimations;

            PositionTextAnim.Stop();
            PositionTextAnim.Play(callback);
            PointsTextAnim.Stop();
            PointsTextAnim.Play(callback);
        }

        protected override void ReverseAnimations(bool hasCallback = false)
        {
            base.ReverseAnimations(hasCallback);

            System.Action callback = null;
            if (hasCallback) callback = CompletedTextAnimations;

            PositionTextAnim.Stop();
            PositionTextAnim.Reverse(callback);
            PointsTextAnim.Stop();
            PointsTextAnim.Reverse(callback);
        }
    }
}