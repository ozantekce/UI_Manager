using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Scale", menuName = "ScreenManagerAnimation/Scale")]
public class ScaleAnimation : UI_Animation
{
    public float duration;
    public bool startScaleIsCurrentScale;
    [HideInInspectorIf("startScaleIsCurrentScale")]
    public Vector3 startScale;
    public Vector3 endScale;

    public override IEnumerator Enumerator(UI_Element element)
    {

        element.gameObject.SetActive(true);

        Vector3 currentScale;
        if (startScaleIsCurrentScale)
        {
            currentScale = element.transform.localScale;
        }
        else
        {
            currentScale = startScale;
        }
        element.transform.localScale = currentScale;



        float timer = 0;
        Vector3 deltaScale = (endScale - currentScale) / duration;
        while (timer < duration)
        {
            float deltaTime = Time.deltaTime;
            timer += deltaTime;

            currentScale += deltaTime * deltaScale;
            element.transform.localScale = currentScale;
            yield return null;
        }

    }

}
