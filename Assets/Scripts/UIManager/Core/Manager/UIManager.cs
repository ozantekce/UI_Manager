using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UIManager
{

    [DefaultExecutionOrder(-50)]
    public partial class UIManager : MonoBehaviour
    {

        public static UIManager Instance { get; private set; }

        [SerializeField, ReadOnly] private List<UIElement> _elements = new List<UIElement>();

        private readonly Dictionary<UIElementType, List<UIElement>> _typeToElements = new Dictionary<UIElementType, List<UIElement>>();
        private readonly Dictionary<string, UIElement> _nameToElement = new Dictionary<string, UIElement>();


        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(this);
                return;
            }

            Instance = this;
            _typeToElements.Clear();
            _nameToElement.Clear();

            List<UIElement> cachedElements = new List<UIElement>(_elements);
            _elements.Clear();

            foreach (UIElement uiElement in cachedElements)
            {
                if(uiElement != null)
                {
                    AddElement(uiElement);
                    uiElement.ConfigurationsAwake();
                }
            }

        }



        private void Start()
        {
            foreach (UIElement uiElement in _elements)
            {
                uiElement.ConfigurationsStart();
            }
        }


        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
            
            while (_commands.Count > 0)
            {
                BaseCommand command = _commands.Remove();
                command.OnManagerDestroy();
            }
            
        }


#if UNITY_EDITOR
        public void FindAndStoreUIElements()
        {
            _elements = FindAllUIElementsInScene();
            EditorUtility.SetDirty(this);
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

#endif


        public void AddElement(UIElement uiElement)
        {
            if (_elements.Contains(uiElement)) return;

            List<UIElement> typeElements = _typeToElements.GetValueOrDefault(uiElement.Type, new List<UIElement>());
            typeElements.Add(uiElement);
            _typeToElements[uiElement.Type] = typeElements;
            if (!string.IsNullOrEmpty(uiElement.Name))
            {
                _nameToElement[uiElement.Name] = uiElement;
            }
            _elements.Add(uiElement);

        }


        public void RemoveElement(UIElement uiElement)
        {
            if (!_elements.Contains(uiElement)) return;

            _elements.Remove(uiElement);
            _typeToElements[uiElement.Type].Remove(uiElement);
            if (_nameToElement.ContainsKey(uiElement.Name))
            {
                _nameToElement.Remove(uiElement.Name);
            }
        }


        public E GetElement<E>(string name) where E : UIElement 
        {
            if(ContainsElement(name))
                return (E)_nameToElement[name];
            else
                return null;
        }

        public bool TryGetElement<E>(string name, out E element) where E : UIElement
        {
            if (ContainsElement(name))
            {
                element = GetElement<E>(name);
                return true;
            }
            else
            {
                element = null;
                return false;
            }
        }

        public bool ContainsElement(string name)
        {
            return _nameToElement.ContainsKey(name);
        }



        public bool TryGetElements(UIElementType elementType, out List<UIElement> elements)
        {
            if (_typeToElements.TryGetValue(elementType, out List<UIElement> list))
            {
                elements = list;
                return true;
            }
            elements = null;
            return false;
        }


    }


}

