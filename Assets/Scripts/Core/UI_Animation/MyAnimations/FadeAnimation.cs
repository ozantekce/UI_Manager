using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Fade", menuName = "ScreenManagerAnimation/Fade")]

public class FadeAnimation : UI_Animation
{


    public float duration;
    public float fadeStart;
    public float fadeEnd;
    public bool  withChilds;

    public override IEnumerator Enumerator(I_UI_Element element)
    {

        element.MonoBehaviour.gameObject.SetActive(true);
        Image image = element.MonoBehaviour.GetComponent<Image>();


        Image[] childs = null;
        if (withChilds)
        {
            childs = element.MonoBehaviour.GetComponentsInChildren<Image>();
        }


        Color c = image.color;
        c.a = fadeStart;
        image.color = c;

        if(childs != null )
        {
            foreach (Image childImage in childs)
            {
                Color cc = childImage.color;
                cc.a = fadeStart;
                childImage.color = cc;
            }
        }

        float timer = 0;
        float deltaFade = (fadeEnd - fadeStart)/duration;
        while (timer<duration)
        {
            float deltaTime = Time.deltaTime;
            timer += deltaTime;

            c.a += deltaTime * deltaFade;
            image.color = c;

            if (childs != null)
            {
                foreach (Image childImage in childs)
                {
                    Color cc = childImage.color;
                    cc.a = c.a;
                    childImage.color = cc;
                }
            }


            yield return null;
        }

    }


}
