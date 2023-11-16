using System.Collections;
using UnityEngine;


namespace UI_Manager
{

    [CreateAssetMenu(fileName = "Fade", menuName = "UIManagerAnimation/Fade")]

    public class FadeAnimation : UI_Animation
    {


        public float duration;
        public bool startFadeIsCurrentFade;
        [HideInInspectorIf("startFadeIsCurrentFade")]
        public float fadeStart;
        public float fadeEnd;


        public override IEnumerator Enumerator(UI_Element element)
        {

            element.gameObject.SetActive(true);
            CanvasGroup canvasGroup = element.gameObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = element.gameObject.AddComponent<CanvasGroup>();
            }


            float currentAlpha = canvasGroup.alpha;
            if (!startFadeIsCurrentFade) currentAlpha = fadeStart;
            canvasGroup.alpha = currentAlpha;

            float timer = 0;
            float deltaFade = (fadeEnd - currentAlpha) / duration;
            while (timer < duration)
            {
                float deltaTime = Time.deltaTime;
                timer += deltaTime;

                currentAlpha += deltaTime * deltaFade;
                canvasGroup.alpha = currentAlpha;

                yield return null;
            }

        }


    }


}


