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
    public bool  withChild;

    public override IEnumerator Enumerator(I_UI_Element element)
    {

        element.MonoBehaviour.gameObject.SetActive(true);
        Image image = element.MonoBehaviour.GetComponent<Image>();

        Color c = image.color;
        c.a = fadeStart;
        image.color = c;

        float timer = 0;
        float deltaFade = (fadeEnd - fadeStart)/duration;
        while (timer<duration)
        {
            float deltaTime = Time.deltaTime;
            timer += deltaTime;

            c.a += deltaTime * deltaFade;
            image.color = c;
            
            yield return null;
        }

    }


}
