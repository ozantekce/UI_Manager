using DG.Tweening;
using EditorTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIManager
{

    [System.Serializable]
    public class TweenAnimation : BaseUIAnimation
    {

        public override UIAnimationType AnimationType => UIAnimationType.Tween;

        private Sequence _sequence;

        [SerializeField] public bool Loop;

        [field: SerializeField, ShowIf(nameof(Loop))] public LoopType LoopType { get; private set; } = LoopType.Restart;
        [field: SerializeField, ShowIf(nameof(Loop))] public int LoopCount { get; private set; } = 0;

        [field: SerializeField] public List<TweenDataWrapper> Sequence { get; private set; } = new List<TweenDataWrapper>();


        public override IEnumerator Enumerator(UIElement element)
        {

            if (!element.gameObject.activeSelf)
            {
                element.gameObject.SetActive(true);
            }

            if (element.ElementInfo == null)
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
                    if (element.ElementInfo != null)
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

            foreach (TweenDataWrapper data in Sequence)
            {
                BaseTweenData baseTweenData = data.TweenData;
                if (baseTweenData.SequenceType == SequenceType.Append)
                {
                    _sequence.Append(baseTweenData.CreateTween(element));
                }
                else if (baseTweenData.SequenceType == SequenceType.Join)
                {
                    _sequence.Join(baseTweenData.CreateTween(element));
                }

            }

            if (Loop)
            {
                _sequence.SetLoops(LoopCount, LoopType);
            }

        }

        public override void Kill(bool complete = false)
        {
            _sequence.Kill(complete);
        }
        public override bool IsPlaying => _sequence != null && _sequence.active;
    }


}
