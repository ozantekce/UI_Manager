using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI_Manager
{

    [System.Serializable]
    public class TweenAnimation : UI_Animation
    {

        public override UI_AnimationType AnimationType => UI_AnimationType.Tween;

        public LoopType loopType = LoopType.Restart;

        public int loopCount = 0;

        public List<TweenDataWrapper> sequence = new List<TweenDataWrapper>();


        private Sequence _sequence;
        

        public override IEnumerator Enumerator(UI_Element element)
        {

            if(!element.gameObject.activeSelf) element.gameObject.SetActive(true);

            while(element.ElementInfo == null)
            {
                yield return null;
            }

            ConvertSequence(element);

            while (_sequence != null && _sequence.active)
            {
                yield return null;
            }
            
        }

        public Sequence ConvertSequence(UI_Element element)
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

            return _sequence;
        }

        public override void Kill()
        {
            _sequence.Kill();
        }
    }


}
