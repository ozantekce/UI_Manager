using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Fade", menuName = "ScreenManagerAnimation/Fade")]

public class FadeAnimation : UI_Animation
{


    public float duration;
    public bool startFadeIsCurrentFade;
    [HideInInspectorIf("startFadeIsCurrentFade")]
    public float fadeStart;
    public float fadeEnd;
    public bool  withChilds;

    public override IEnumerator Enumerator(UI_Element element)
    {

        element.gameObject.SetActive(true);
        Image image = element.GetComponent<Image>();


        Image[] childs = null;
        if (withChilds)
        {
            childs = element.GetComponentsInChildren<Image>();
        }


        Color currentColor = image.color;
        
        if(!startFadeIsCurrentFade)
        {
            currentColor.a = fadeStart;
        }

        image.color = currentColor;

        if(childs != null )
        {
            foreach (Image childImage in childs)
            {
                Color childCurrentColor = childImage.color;
                
                if (!startFadeIsCurrentFade)
                {
                    childCurrentColor.a = fadeStart;
                }

                childImage.color = childCurrentColor;
            }
        }

        float timer = 0;
        float deltaFade = (fadeEnd - currentColor.a)/duration;
        while (timer<duration)
        {
            float deltaTime = Time.deltaTime;
            timer += deltaTime;

            currentColor.a += deltaTime * deltaFade;
            image.color = currentColor;

            if (childs != null)
            {
                foreach (Image childImage in childs)
                {
                    Color cc = childImage.color;
                    cc.a = currentColor.a;
                    childImage.color = cc;
                }
            }


            yield return null;
        }

    }


}
