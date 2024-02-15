#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UI_Manager;
using System.Collections.Generic;


[CustomEditor(typeof(AnimationComponent))]
public class AnimationComponentEditor : Editor
{/*
    public override void OnInspectorGUI()
    {
        AnimationComponent component = (AnimationComponent)target;

        // Draw the default inspector
        DrawDefaultInspector();

        // Check for changes and update the animation type if necessary
        if (GUI.changed)
        {
            // Record object state for undo

            Dictionary<(AnimationExecuteTime, int), UI_Animation> changes = new Dictionary<(AnimationExecuteTime, int), UI_Animation>();

            Dictionary<AnimationExecuteTime, List<UI_Animation>> dic = component.GetAnimations();
            foreach (var pair in dic)
            {
                List<UI_Animation> currentAnimations = pair.Value;

                for (int i = 0; i < currentAnimations.Count; i++)
                {
                    UI_Animation currentAnimation = currentAnimations[i];
                    bool changed = false;


                    if (currentAnimation == null)
                    {
                        currentAnimation = UI_Animation.Create(UI_AnimationType.Tween);
                        currentAnimation.currentType = UI_AnimationType.Tween;
                        changed = true;
                    }
                    else if (currentAnimation.AnimationType != currentAnimation.currentType)
                    {
                        UI_AnimationType currentType = currentAnimation.currentType;
                        currentAnimation = UI_Animation.Create(currentType);
                        currentAnimation.currentType = currentType;
                        changed = true;
                    }

                    if (changed)
                    {
                        changes.Add((pair.Key, i), currentAnimation);
                    }

                    // Handling TweenAnimation separately
                    HandleTweenAnimation(currentAnimation, component, ref changed);
                }
            }

            // Apply changes
            foreach (var item in changes)
            {
                component.SetAnimation(item.Key.Item1, item.Value, item.Key.Item2);
            }
        }
    }

    private void HandleTweenAnimation(UI_Animation animation, AnimationComponent component, ref bool changed)
    {
        if (animation is TweenAnimation tweenAnimation)
        {
            List<BaseTweenData> list = tweenAnimation.tweens;

            for (int j = 0; j < list.Count; j++)
            {
                BaseTweenData currentData = list[j];

                if (currentData == null || currentData.DataType != currentData.currentType)
                {
                    TweenDataType currentType = currentData?.currentType ?? TweenDataType.Move;
                    currentData = BaseTweenData.Create(currentType);
                    currentData.currentType = currentType;
                    list[j] = currentData;
                    changed = true;
                }
            }
        }

        if (changed)
        {
            EditorUtility.SetDirty(component);
        }
    }
    */
}
#endif
