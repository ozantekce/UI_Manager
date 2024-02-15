using DG.Tweening;
using System;
using UnityEngine;

namespace UI_Manager
{

    [System.Serializable]
    public abstract class BaseTweenData
    {
        
        public SequenceType sequenceType = SequenceType.Append;

        public LoopType loopType = LoopType.Restart;

        public int loopCount = 1;

        public float duration = 1;

        public Ease ease = Ease.Linear;

        public abstract TweenDataType DataType { get; }



        public Tween CreateTween(UI_Element element)
        {
            return CreateTweenImplementation(element);
        }

        public abstract Tween CreateTweenImplementation(UI_Element element);


        public static BaseTweenData Create(TweenDataType dataType)
        {

            if (dataType == TweenDataType.Move) return new MoveTweenData();
            else if (dataType == TweenDataType.Rotate) return new RotateTweenData();
            else if (dataType == TweenDataType.Scale) return new ScaleTweenData();
            else if (dataType == TweenDataType.Color) return new ColorTweenData();
            else if (dataType == TweenDataType.Fade) return new FadeTweenData();
            else if (dataType == TweenDataType.Match) return new MatchTargetTweenData();
            else return null;
        }


    }


    [System.Serializable]
    public class TweenDataWrapper : ISerializationCallbackReceiver
    {
        public TweenDataType dataType;

        [SerializeReference] private BaseTweenData _tweenData = new MoveTweenData();

        public BaseTweenData TweenData
        {
            get => _tweenData;
            set => _tweenData = value;
        }

        public void OnAfterDeserialize()
        {

        }

        public void OnBeforeSerialize()
        {
            if (_tweenData == null || _tweenData.DataType != dataType)
            {
                _tweenData = BaseTweenData.Create(dataType);
            }
        }


    }





    public enum TweenDataType { Move, Rotate, Scale, Color, Fade, Match }
    public enum SequenceType { Append, Join }


}
