#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace UIManager
{
    public partial class UIManager
    {

    /*
        public void FindAndStoreUIElements()
        {
            _elements = FindAllUIElementsInScene();
            //EditorUtility.SetDirty(this);
        }

        private List<UIElement> FindAllUIElementsInScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            List<UIElement> elements = new List<UIElement>();
            foreach (GameObject rootGameObject in currentScene.GetRootGameObjects())
            {
                MonoBehaviour[] subs = rootGameObject.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (MonoBehaviour entity in subs)
                {
                    if (entity is UIElement ui_Element)
                    {
                        elements.Add(ui_Element);
                    }
                }
            }
            return elements;

        }

        */

    }
}

#endif