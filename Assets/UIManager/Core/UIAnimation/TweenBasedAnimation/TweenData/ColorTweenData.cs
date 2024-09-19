using DG.Tweening;
using EditorTools;
using UnityEngine;
using UnityEngine.UI;

namespace UIManager
{
    [System.Serializable]
    public class ColorTweenData : BaseTweenData
    {
        public override TweenDataType DataType => TweenDataType.Color;


        [field: SerializeField] public ColorFrom ColorFrom { get; private set; }
        [field: SerializeField] public ColorTo ColorTo { get; private set; }

        [field: SerializeField, ShowInEnum(nameof(ColorFrom), nameof(ColorFrom.SpecificColor))]
        public Color StartSpecificColor { get; private set; }

        [field: SerializeField, ShowInEnum(nameof(ColorFrom), nameof(ColorFrom.OtherObjectColor))]
        public Graphic StartOtherGraphic { get; private set; }



        [field: SerializeField, ShowInEnum(nameof(ColorTo), nameof(ColorTo.SpecificColor))]
        public Color EndSpecificColor { get; private set; }

        [field: SerializeField, ShowInEnum(nameof(ColorTo), nameof(ColorTo.OtherObjectColor))]
        public Graphic EndOtherGraphic { get; private set; }


        public override Tween CreateTweenImplementation(UIElement element)
        {
            Tween tween = null;
            Color startColor = Color.white;
            Color endColor = Color.white;
            Graphic graphic = element.GetComponent<Graphic>();

            // Handle start color
            switch (ColorFrom)
            {
                case ColorFrom.InitialColor:
                    startColor = element.ElementInfo.Value.firstColor;
                    break;
                case ColorFrom.CurrentColor:
                    startColor = graphic.color;
                    break;
                case ColorFrom.SpecificColor:
                    startColor = StartSpecificColor;
                    break;
                case ColorFrom.OtherObjectColor:
                    startColor = StartOtherGraphic != null ? StartOtherGraphic.color : Color.white;
                    break;
            }

            // Handle end color
            switch (ColorTo)
            {
                case ColorTo.InitialColor:
                    endColor = element.ElementInfo.Value.firstColor;
                    break;
                case ColorTo.CurrentColor:
                    endColor = graphic.color;
                    break;
                case ColorTo.SpecificColor:
                    endColor = EndSpecificColor;
                    break;
                case ColorTo.OtherObjectColor:
                    endColor = EndOtherGraphic != null ? EndOtherGraphic.color : Color.white;
                    break;
            }

            // Create the color tween
            if (graphic != null)
            {
                if (Loop)
                {
                    tween = graphic.DOColor(endColor, Duration).SetEase(Ease).SetLoops(LoopCount, LoopType).From(startColor);
                }
                else
                {
                    tween = graphic.DOColor(endColor, Duration).SetEase(Ease).From(startColor);
                }

            }

            return tween;
        }
    }

    public enum ColorFrom { InitialColor, CurrentColor, SpecificColor, OtherObjectColor }
    public enum ColorTo { InitialColor, CurrentColor, SpecificColor, OtherObjectColor }
}
