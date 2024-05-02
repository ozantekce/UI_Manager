using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UIManager
{
    [System.Serializable]
    public class ColorTweenData : BaseTweenData
    {
        public override TweenDataType DataType => TweenDataType.Color;
        public enum ColorFrom { InitialColor, CurrentColor, SpecificColor, OtherObjectColor }
        public enum ColorTo { InitialColor, CurrentColor, SpecificColor, OtherObjectColor }

        public ColorFrom colorFrom;
        public ColorTo colorTo;

        [ShowInEnum(nameof(colorFrom), nameof(ColorFrom.SpecificColor))]
        public Color startSpecificColor;

        [ShowInEnum(nameof(colorFrom), nameof(ColorFrom.OtherObjectColor))]
        public Graphic startOtherGraphic;



        [ShowInEnum(nameof(colorTo), nameof(ColorTo.SpecificColor))]
        public Color endSpecificColor;

        [ShowInEnum(nameof(colorTo), nameof(ColorTo.OtherObjectColor))]
        public Graphic endOtherGraphic;

        public override Tween CreateTweenImplementation(UIElement element)
        {
            Tween tween = null;
            Color startColor = Color.white;
            Color endColor = Color.white;
            Graphic graphic = element.GetComponent<Graphic>();

            // Handle start color
            switch (colorFrom)
            {
                case ColorFrom.InitialColor:
                    startColor = element.ElementInfo.Value.firstColor;
                    break;
                case ColorFrom.CurrentColor:
                    startColor = graphic.color;
                    break;
                case ColorFrom.SpecificColor:
                    startColor = startSpecificColor;
                    break;
                case ColorFrom.OtherObjectColor:
                    startColor = startOtherGraphic != null ? startOtherGraphic.color : Color.white;
                    break;
            }

            // Handle end color
            switch (colorTo)
            {
                case ColorTo.InitialColor:
                    endColor = element.ElementInfo.Value.firstColor;
                    break;
                case ColorTo.CurrentColor:
                    endColor = graphic.color;
                    break;
                case ColorTo.SpecificColor:
                    endColor = endSpecificColor;
                    break;
                case ColorTo.OtherObjectColor:
                    endColor = endOtherGraphic != null ? endOtherGraphic.color : Color.white;
                    break;
            }

            // Create the color tween
            if (graphic != null)
            {
                if (loop)
                {
                    tween = graphic.DOColor(endColor, duration).SetEase(ease).SetLoops(loopCount, loopType).From(startColor);
                }
                else
                {
                    tween = graphic.DOColor(endColor, duration).SetEase(ease).From(startColor);
                }

            }

            return tween;
        }
    }
}
