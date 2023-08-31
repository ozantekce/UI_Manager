using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnityAnimation", menuName = "ScreenManagerAnimation/UnityAnimation")]

public class UnityAnimation : UI_Animation
{

    [SerializeField]
    private AnimationClip clip;

    private static Dictionary<I_UI_Element, Animation> keyValuePairs;

    private Animation animation;

    private WaitForEndOfFrame waitForEndOfFrame;
    public override IEnumerator Enumerator(I_UI_Element element)
    {

        if (clip == null) yield break;

        clip.legacy = true;

        if (keyValuePairs == null)
        {
            keyValuePairs = new Dictionary<I_UI_Element, Animation>();
        }
        if (!keyValuePairs.ContainsKey(element))
        {
            Animation animation = element.MonoBehaviour.gameObject.GetComponent<Animation>();
            if (animation == null)
                animation = element.MonoBehaviour.gameObject.AddComponent<Animation>();
            keyValuePairs.Add(element, animation);
        }

        animation = keyValuePairs[element];
        if (animation.GetClip(clip.name) == null)
        {
            animation.AddClip(clip, clip.name);
        }
        element.MonoBehaviour.gameObject.SetActive(true);
        animation.Play(clip.name);

        yield return waitForEndOfFrame;

        animation.Play(clip.name);
        while (animation.IsPlaying(clip.name))
        {
            if (element.Status == UIElementStatus.Opened
                || element.Status == UIElementStatus.Closed
                )
            {
                animation.Stop();
                yield break;
            }
            yield return waitForEndOfFrame;
        }

    }

}