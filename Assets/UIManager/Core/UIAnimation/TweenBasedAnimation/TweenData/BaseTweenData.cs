using DG.Tweening;
using EditorTools;
using UnityEngine;

namespace UIManager
{

    [System.Serializable]
    public abstract class BaseTweenData
    {

        public SequenceType SequenceType { get; private set; } = SequenceType.Append;

        [SerializeField] public bool Loop;
        [SerializeField] public bool SpeedBased;
        [field: SerializeField] public Ease Ease { get; private set; } = Ease.Linear;
        [field: SerializeField, ShowIf(nameof(Loop))] public LoopType LoopType { get; private set; } = LoopType.Restart;

        [field: SerializeField, ShowIf(nameof(Loop))] public int LoopCount { get; private set; } = 1;

        [field: SerializeField] public float Duration { get; private set; } = 1;



        public abstract TweenDataType DataType { get; }

        public Tween CreateTween(UIElement element)
        {
            Tween t = CreateTweenImplementation(element);
            if (t != null && SpeedBased)
            {
                t.SetSpeedBased();
            }
            return t;
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
