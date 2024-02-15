using UnityEngine;
using UnityEngine.UI;

namespace UI_Manager
{
    [System.Serializable]
    public struct ElementInfo
    {

        #region Transform

        #region Transform_Global
        public Vector3 firstGlobalPos;
        public Quaternion firstGlobalRot;
        public Vector3 firstGlobalScale;
        #endregion

        #region Transform_Local
        public Vector3 firstLocalPos;
        public Quaternion firstLocalRot;
        public Vector3 firstLocalScale;
        #endregion

        #region Anchor
        public Vector2 firstAnchorPos;
        #endregion

        #endregion

        #region Color
        public Color firstColor;
        #endregion

        #region Fade
        public float firstAlpha;
        #endregion


        public ElementInfo(UI_Element element)
        {
            // Capture the initial global transform properties
            firstGlobalPos = element.transform.position;
            firstGlobalRot = element.transform.rotation;
            firstGlobalScale = element.transform.lossyScale;

            // Capture the initial local transform properties
            firstLocalPos = element.transform.localPosition;
            firstLocalRot = element.transform.localRotation;
            firstLocalScale = element.transform.localScale;

            // Capture the initial anchor position if the element has a RectTransform
            RectTransform rectTransform = element.GetComponent<RectTransform>();
            firstAnchorPos = rectTransform != null ? rectTransform.anchoredPosition : Vector2.zero;

            // Capture the initial color if the element has a Graphic component
            Graphic graphic = element.GetComponent<Graphic>();
            firstColor = graphic != null ? graphic.color : Color.white;

            // Capture the initial alpha value from the CanvasGroup component
            CanvasGroup canvasGroup = element.GetComponent<CanvasGroup>();
            firstAlpha = canvasGroup != null ? canvasGroup.alpha : 1f;
        }

    }


}
