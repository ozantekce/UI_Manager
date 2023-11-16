using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace UI_Manager
{

    [CreateAssetMenu(fileName = "Color", menuName = "UIManagerAnimation/Color")]

    public class ColorAnimation : UI_Animation
    {


        public float duration;
        public bool startColorIsCurrentFade;
        [HideInInspectorIf("startColorIsCurrentFade")]
        public Color colorStart;
        public Color colorEnd;

        public override IEnumerator Enumerator(UI_Element element)
        {

            element.gameObject.SetActive(true);
            Image image = element.GetComponent<Image>();

            Color currentColor = image.color;

            if (!startColorIsCurrentFade)
            {
                currentColor = colorStart;
            }

            image.color = currentColor;

            float timer = 0;
            Color deltaColor = (colorEnd - currentColor) / duration;
            while (timer < duration)
            {
                float deltaTime = Time.deltaTime;
                timer += deltaTime;

                currentColor += deltaTime * deltaColor;
                image.color = currentColor;

                yield return null;
            }

            image.color = colorEnd;

        }


    }

}

