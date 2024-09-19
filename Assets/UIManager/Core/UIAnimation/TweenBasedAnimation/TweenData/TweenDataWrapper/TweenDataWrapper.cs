using EditorTools;
using UnityEngine;

namespace UIManager
{

    [System.Serializable]
    public class TweenDataWrapper
    {
        [field: SerializeField] public TweenDataType DataType { get; private set; }

        [SerializeField, ShowInEnum(nameof(DataType), nameof(TweenDataType.Move))] private MoveTweenData _moveTweenData;
        [SerializeField, ShowInEnum(nameof(DataType), nameof(TweenDataType.Rotate))] private RotateTweenData _rotateTweenData;
        [SerializeField, ShowInEnum(nameof(DataType), nameof(TweenDataType.Scale))] private ScaleTweenData _scaleTweenData;
        [SerializeField, ShowInEnum(nameof(DataType), nameof(TweenDataType.Color))] private ColorTweenData _colorTweenData;
        [SerializeField, ShowInEnum(nameof(DataType), nameof(TweenDataType.Fade))] private FadeTweenData _fadeTweenData;
        [SerializeField, ShowInEnum(nameof(DataType), nameof(TweenDataType.Match))] private MatchWithTargetTweenData _matchWithTargetTweenData;



        public BaseTweenData TweenData
        {
            get
            {
                return DataType switch
                {
                    TweenDataType.Move => _moveTweenData,
                    TweenDataType.Rotate => _rotateTweenData,
                    TweenDataType.Scale => _scaleTweenData,
                    TweenDataType.Color => _colorTweenData,
                    TweenDataType.Fade => _fadeTweenData,
                    TweenDataType.Match => _matchWithTargetTweenData,
                    _ => null,
                };
            }
        }




    }

}