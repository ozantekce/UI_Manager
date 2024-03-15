using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

namespace UIManager
{

    [System.Serializable]
    public class TweenAnimation : BaseUIAnimation
    {

        public override UIAnimationType AnimationType => UIAnimationType.Tween;

        public LoopType loopType = LoopType.Restart;

        public int loopCount = 0;

        public List<TweenDataWrapper> sequence = new List<TweenDataWrapper>();


        private Sequence _sequence;
        

        public override IEnumerator Enumerator(UIElement element)
        {

            if (!element.gameObject.activeSelf)
            {
                element.gameObject.SetActive(true);
            }

            while(element.ElementInfo == null)
            {
                yield return null;
            }

            ConvertSequence(element);

            while (IsPlaying)
            {
                yield return null;
            }
            
        }

        public void ConvertSequence(UIElement element)
        {
            _sequence = DOTween.Sequence();

            foreach (TweenDataWrapper data in sequence)
            {
                BaseTweenData baseTweenData = data.TweenData;
                if(baseTweenData.sequenceType == SequenceType.Append)
                {
                    _sequence.Append(baseTweenData.CreateTween(element));
                }
                else if(baseTweenData.sequenceType == SequenceType.Join)
                {
                    _sequence.Join(baseTweenData.CreateTween(element));
                }

            }

            _sequence.SetLoops(loopCount, loopType);
        }

        public override void Kill()
        {
            _sequence.Kill();
        }
        public override bool IsPlaying => _sequence != null && _sequence.active;
    }


}
