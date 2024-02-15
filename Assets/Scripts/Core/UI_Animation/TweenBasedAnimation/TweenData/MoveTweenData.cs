using DG.Tweening;
using UnityEngine;

namespace UI_Manager
{
    [System.Serializable]
    public class MoveTweenData : BaseTweenData
    {
        public override TweenDataType DataType => TweenDataType.Move;
        public enum MoveFrom { FirstPosition, CurrentPosition, Position, DeltaPosition, OtherTransformPosition }
        public enum MoveTo { FirstPosition, CurrentPosition, Position, DeltaPosition, OtherTransformPosition }
        public enum Space { Local, World, Anchor }

        public Space space;
        public MoveFrom moveFrom;
        public MoveTo moveTo;

        [ShowInEnum(nameof(moveFrom), nameof(MoveFrom.Position))]
        public Vector3 startPosition;

        [ShowInEnum(nameof(moveTo), nameof(MoveTo.Position))]
        public Vector3 endPosition;

        [ShowInEnum(nameof(moveFrom), nameof(MoveFrom.OtherTransformPosition))]
        public Transform startTransform;

        [ShowInEnum(nameof(moveTo), nameof(MoveTo.OtherTransformPosition))]
        public Transform endTransform;

        [ShowInEnum(nameof(moveFrom), nameof(MoveFrom.DeltaPosition))]
        public Vector3 startDeltaPosition;

        [ShowInEnum(nameof(moveTo), nameof(MoveTo.DeltaPosition))]
        public Vector3 endDeltaPosition;

        public override Tween CreateTweenImplementation(UI_Element element)
        {
            Tween tween = null;
            Vector3 posStart = Vector3.zero;
            Vector3 posEnd = Vector3.zero;

            // Handle start position
            switch (moveFrom)
            {
                case MoveFrom.FirstPosition:
                    posStart = GetPositionFromSpace(element, element.ElementInfo.Value.firstLocalPos, element.ElementInfo.Value.firstGlobalPos, element.ElementInfo.Value.firstAnchorPos);
                    break;
                case MoveFrom.CurrentPosition:
                    posStart = GetPositionFromSpace(element, element.transform.localPosition, element.transform.position, element.RectTransform.anchoredPosition);
                    break;
                case MoveFrom.Position:
                    posStart = startPosition;
                    break;
                case MoveFrom.DeltaPosition:
                    posStart = GetPositionFromSpace(element, element.transform.localPosition, element.transform.position, element.RectTransform.anchoredPosition) + startDeltaPosition;
                    break;
                case MoveFrom.OtherTransformPosition:
                    posStart = startTransform.position;
                    break;
            }

            // Handle end position
            switch (moveTo)
            {
                case MoveTo.FirstPosition:
                    posEnd = GetPositionFromSpace(element, element.ElementInfo.Value.firstLocalPos, element.ElementInfo.Value.firstGlobalPos, element.ElementInfo.Value.firstAnchorPos);
                    break;
                case MoveTo.CurrentPosition:
                    posEnd = GetPositionFromSpace(element, element.transform.localPosition, element.transform.position, element.RectTransform.anchoredPosition);
                    break;
                case MoveTo.Position:
                    posEnd = endPosition;
                    break;
                case MoveTo.DeltaPosition:
                    posEnd = GetPositionFromSpace(element, element.transform.localPosition, element.transform.position, element.RectTransform.anchoredPosition) + endDeltaPosition;
                    break;
                case MoveTo.OtherTransformPosition:
                    posEnd = endTransform.position;
                    break;
            }

            // Create the tween
            tween = CreatePositionTween(element, posStart, posEnd);

            return tween;
        }

        private Vector3 GetPositionFromSpace(UI_Element element, Vector3 localPosition, Vector3 worldPosition, Vector3 anchorPosition)
        {
            return space switch
            {
                Space.Local => localPosition,
                Space.World => worldPosition,
                Space.Anchor => anchorPosition,
                _ => Vector3.zero,
            };
        }

        private Tween CreatePositionTween(UI_Element element, Vector3 startPos, Vector3 endPos)
        {
            switch (space)
            {
                case Space.Local:
                    element.transform.localPosition = startPos;
                    return element.transform.DOLocalMove(endPos, duration).SetEase(ease).SetLoops(loopCount, loopType);
                case Space.World:
                    element.transform.position = startPos;
                    return element.transform.DOMove(endPos, duration).SetEase(ease).SetLoops(loopCount, loopType);
                case Space.Anchor:
                    element.RectTransform.anchoredPosition = startPos;
                    return element.RectTransform.DOAnchorPos(endPos, duration).SetEase(ease).SetLoops(loopCount, loopType);
                default:
                    return null;
            }
        }
    }
}
