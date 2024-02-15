using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Manager
{
    [System.Serializable]
    [Flags]
    public enum MatchType
    {
        None = 0,
        Position = 1 << 0,
        Rotation = 1 << 1,
        Scale = 1 << 2,
        Color = 1 << 3,
        Alpha = 1 << 4
    }

    public class MatchTargetTweenData : BaseTweenData
    {

        public override TweenDataType DataType => TweenDataType.Match;

        public MatchType matchType = MatchType.Position | MatchType.Rotation | MatchType.Scale | MatchType.Color | MatchType.Alpha;

        public UI_Element targetElement; // Reference to the target UI element

        

        public override Tween CreateTweenImplementation(UI_Element element)
        {
            Sequence sequence = DOTween.Sequence();

            if ((matchType & MatchType.Position) == MatchType.Position)
            {
                Vector3 targetPosition = targetElement.transform.position;
                sequence.Join(element.transform.DOMove(targetPosition, duration).SetEase(ease));
            }

            if ((matchType & MatchType.Rotation) == MatchType.Rotation)
            {
                Quaternion targetRotation = targetElement.transform.rotation;
                sequence.Join(element.transform.DORotateQuaternion(targetRotation, duration).SetEase(ease));
            }

            if ((matchType & MatchType.Scale) == MatchType.Scale)
            {
                Vector3 targetScale = targetElement.transform.localScale;
                sequence.Join(element.transform.DOScale(targetScale, duration).SetEase(ease));
            }

            if ((matchType & MatchType.Color) == MatchType.Color)
            {
                Graphic graphic = element.GetComponent<Graphic>();
                Graphic targetGraphic = targetElement.GetComponent<Graphic>();
                if (graphic != null && targetGraphic != null)
                {
                    Color targetColor = targetGraphic.color;
                    sequence.Join(graphic.DOColor(targetColor, duration).SetEase(ease));
                }
            }

            if ((matchType & MatchType.Alpha) == MatchType.Alpha)
            {
                CanvasGroup canvasGroup = element.GetComponent<CanvasGroup>();
                CanvasGroup targetCanvasGroup = targetElement.GetComponent<CanvasGroup>();
                if (canvasGroup != null && targetCanvasGroup != null)
                {
                    float targetAlpha = targetCanvasGroup.alpha;
                    sequence.Join(canvasGroup.DOFade(targetAlpha, duration).SetEase(ease));
                }
            }

            sequence.SetLoops(loopCount, loopType);
            return sequence;
        }
    }
}
