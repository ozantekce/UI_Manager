using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIManager
{

    [System.Serializable]
    public class TweenAnimation : BaseUIAnimation
    {

        public override UIAnimationType AnimationType => UIAnimationType.Tween;

        public bool loop;

        [ShowIf(nameof(loop))] public LoopType loopType = LoopType.Restart;
        [ShowIf(nameof(loop))] public int loopCount = 0;

        public List<TweenDataWrapper> sequence = new List<TweenDataWrapper>();


        private Sequence _sequence;
        

        public override IEnumerator Enumerator(UIElement element)
        {

            if (!element.gameObject.activeSelf)
            {
                element.gameObject.SetActive(true);
            }

            if(element.ElementInfo == null)
            {
                float oldAlpha;
                CanvasGroup canvasGroup = element.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = element.gameObject.AddComponent<CanvasGroup>();
                }
                oldAlpha = canvasGroup.alpha;
                while (true)
                {
                    canvasGroup.alpha = 0;
                    yield return null;
                    if(element.ElementInfo != null)
                    {
                        canvasGroup.alpha = oldAlpha;
                        break;
                    }
                }

            }



            ConvertSequence(element);

            while (IsPlaying)
            {
                yield return null;
            }
            
        }

        public void ConvertSequence(UIElement element)
        {
            _sequence = DOTween.Sequence(element.transform);

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

            if (loop)
            {
                _sequence.SetLoops(loopCount, loopType);
            }

        }

        public override void Kill(bool complete = false)
        {
            _sequence.Kill(complete);
        }
        public override bool IsPlaying => _sequence != null && _sequence.active;
    }


}
