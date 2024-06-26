using DG.Tweening;
using UnityEngine;

namespace UIManager
{
    [System.Serializable]
    public class MoveTweenData : BaseTweenData
    {
        public override TweenDataType DataType => TweenDataType.Move;
        public enum MoveFrom { FirstPosition, CurrentPosition, Position, DeltaPositionFromFirstPosition, DeltaPositionFromCurrentPosition, OtherTransformPosition }
        public enum MoveTo { FirstPosition, CurrentPosition, Position, DeltaPositionFromFirstPosition, DeltaPositionFromCurrentPosition, OtherTransformPosition }
        public enum Space { Local, World, Anchor }

        public Space space;
        public MoveFrom moveFrom;
        public MoveTo moveTo;

        [ShowInEnum(nameof(moveFrom), nameof(MoveFrom.Position))]
        public Vector3 startPosition;

        [ShowInEnum(nameof(moveFrom), nameof(MoveFrom.OtherTransformPosition))]
        public Transform startTransform;

        [ShowInEnum(nameof(moveFrom), nameof(MoveFrom.DeltaPositionFromFirstPosition), nameof(MoveFrom.DeltaPositionFromCurrentPosition))]
        public Vector3 startDeltaPosition;



        [ShowInEnum(nameof(moveTo), nameof(MoveTo.Position))]
        public Vector3 endPosition;

        [ShowInEnum(nameof(moveTo), nameof(MoveTo.OtherTransformPosition))]
        public Transform endTransform;

        [ShowInEnum(nameof(moveTo), nameof(MoveTo.DeltaPositionFromFirstPosition), nameof(MoveTo.DeltaPositionFromCurrentPosition))]
        public Vector3 endDeltaPosition;

        public override Tween CreateTweenImplementation(UIElement element)
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
                case MoveFrom.DeltaPositionFromFirstPosition:
                    posStart = GetPositionFromSpace(element, element.ElementInfo.Value.firstLocalPos, element.ElementInfo.Value.firstGlobalPos, element.ElementInfo.Value.firstAnchorPos) + startDeltaPosition;
                    break;
                case MoveFrom.DeltaPositionFromCurrentPosition:
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
                case MoveTo.DeltaPositionFromFirstPosition:
                    posEnd = GetPositionFromSpace(element, element.ElementInfo.Value.firstLocalPos, element.ElementInfo.Value.firstGlobalPos, element.ElementInfo.Value.firstAnchorPos) + endDeltaPosition;
                    break;
                case MoveTo.DeltaPositionFromCurrentPosition:
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

        private Vector3 GetPositionFromSpace(UIElement element, Vector3 localPosition, Vector3 worldPosition, Vector3 anchorPosition)
        {
            return space switch
            {
                Space.Local => localPosition,
                Space.World => worldPosition,
                Space.Anchor => anchorPosition,
                _ => Vector3.zero,
            };
        }

        private Tween CreatePositionTween(UIElement element, Vector3 startPos, Vector3 endPos)
        {
            switch (space)
            {
                case Space.Local:
                    element.transform.localPosition = startPos;
                    return loop ? element.transform.DOLocalMove(endPos, duration).SetEase(ease).SetLoops(loopCount, loopType) :
                        element.transform.DOLocalMove(endPos, duration).SetEase(ease);
                case Space.World:
                    element.transform.position = startPos;
                    return loop ? element.transform.DOMove(endPos, duration).SetEase(ease).SetLoops(loopCount, loopType) :
                        element.transform.DOMove(endPos, duration).SetEase(ease);
                case Space.Anchor:
                    element.RectTransform.anchoredPosition = startPos;
                    return loop ? element.RectTransform.DOAnchorPos(endPos, duration).SetEase(ease).SetLoops(loopCount, loopType) :
                        element.RectTransform.DOAnchorPos(endPos, duration).SetEase(ease);
                default:
                    return null;
            }
        }
    }
}
