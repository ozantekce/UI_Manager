using DG.Tweening;
using UnityEngine;

namespace UI_Manager
{
    [System.Serializable]
    public class ScaleTweenData : BaseTweenData
    {
        public override TweenDataType DataType => TweenDataType.Scale;
        public enum ScaleFrom { FirstScale, CurrentScale, Scale, DeltaScale, OtherTransformScale }
        public enum ScaleTo { FirstScale, CurrentScale, Scale, DeltaScale, OtherTransformScale }

        public ScaleFrom scaleFrom;
        public ScaleTo scaleTo;

        [ShowInEnum(nameof(scaleFrom), nameof(ScaleFrom.Scale))]
        public Vector3 startScale;

        [ShowInEnum(nameof(scaleTo), nameof(ScaleTo.Scale))]
        public Vector3 endScale;

        [ShowInEnum(nameof(scaleFrom), nameof(ScaleFrom.OtherTransformScale))]
        public Transform startTransform;

        [ShowInEnum(nameof(scaleTo), nameof(ScaleTo.OtherTransformScale))]
        public Transform endTransform;

        [ShowInEnum(nameof(scaleFrom), nameof(ScaleFrom.DeltaScale))]
        public Vector3 startDeltaScale;

        [ShowInEnum(nameof(scaleTo), nameof(ScaleTo.DeltaScale))]
        public Vector3 endDeltaScale;

        public override Tween CreateTweenImplementation(UI_Element element)
        {
            Tween tween = null;
            Vector3 scaleStart = Vector3.zero;
            Vector3 scaleEnd = Vector3.zero;

            // Handle start scale
            switch (scaleFrom)
            {
                case ScaleFrom.FirstScale:
                    scaleStart = element.ElementInfo.Value.firstLocalScale;
                    break;
                case ScaleFrom.CurrentScale:
                    scaleStart = element.transform.localScale;
                    break;
                case ScaleFrom.Scale:
                    scaleStart = startScale;
                    break;
                case ScaleFrom.DeltaScale:
                    scaleStart = element.transform.localScale + startDeltaScale;
                    break;
                case ScaleFrom.OtherTransformScale:
                    scaleStart = startTransform.localScale;
                    break;
            }

            // Handle end scale
            switch (scaleTo)
            {
                case ScaleTo.FirstScale:
                    scaleEnd = element.ElementInfo.Value.firstLocalScale;
                    break;
                case ScaleTo.CurrentScale:
                    scaleEnd = element.transform.localScale;
                    break;
                case ScaleTo.Scale:
                    scaleEnd = endScale;
                    break;
                case ScaleTo.DeltaScale:
                    scaleEnd = element.transform.localScale + endDeltaScale;
                    break;
                case ScaleTo.OtherTransformScale:
                    scaleEnd = endTransform.localScale;
                    break;
            }

            // Create the tween
            element.transform.localScale = scaleStart;
            tween = element.transform.DOScale(scaleEnd, duration).SetEase(ease).SetLoops(loopCount, loopType);

            return tween;
        }
    }
}
