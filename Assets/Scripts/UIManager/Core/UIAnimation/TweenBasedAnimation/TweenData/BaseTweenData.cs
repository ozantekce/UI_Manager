using DG.Tweening;

namespace UIManager
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

        public Tween CreateTween(UIElement element)
        {
            return CreateTweenImplementation(element);
        }

        public abstract Tween CreateTweenImplementation(UIElement element);


        public static BaseTweenData Create(TweenDataType dataType)
        {
            if (dataType == TweenDataType.Move) return new MoveTweenData();
            else if (dataType == TweenDataType.Rotate) return new RotateTweenData();
            else if (dataType == TweenDataType.Scale) return new ScaleTweenData();
            else if (dataType == TweenDataType.Color) return new ColorTweenData();
            else if (dataType == TweenDataType.Fade) return new FadeTweenData();
            else if (dataType == TweenDataType.Match) return new MatchWithTargetTweenData();
            else return null;
        }


    }


}
