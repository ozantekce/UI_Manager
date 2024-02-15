using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI_Manager
{
    public partial class UI_Manager : MonoBehaviour
    {

        private static UI_Manager s_Instance;


        [SerializeField] private List<UI_Element> _elements;

        private Dictionary<UIElementType, List<UI_Element>> _typeToElements;
        private Dictionary<string, UI_Element> _nameToElement;


        private Heap<Command> _commands;

        private void Awake()
        {
            if (s_Instance != null)
            {
                DestroyImmediate(this);
                return;
            }
            s_Instance = this;

            _typeToElements = new Dictionary<UIElementType, List<UI_Element>>();
            _nameToElement = new Dictionary<string, UI_Element>();
            _elements = new List<UI_Element>();

            _commands = new Heap<Command>();

            List<UI_Element> elements = FindAllUIElementsInScene();

            foreach (UI_Element uiElement in elements)
            {
                AddElement(uiElement);
                uiElement.ConfigurationsAwake();
            }

        }


        private void Start()
        {
            foreach (UI_Element uiElement in _elements)
            {
                uiElement.ConfigurationsStart();
            }

        }


        public void AddElement(UI_Element ui_Element)
        {
            if (_elements.Contains(ui_Element)) return;

            List<UI_Element> temp = _typeToElements.GetValueOrDefault(ui_Element.Type, new List<UI_Element>());
            temp.Add(ui_Element);
            _typeToElements[ui_Element.Type] = temp;
            if (!string.IsNullOrEmpty(ui_Element.Name)) _nameToElement[ui_Element.Name] = ui_Element;
            _elements.Add(ui_Element);

        }


        public void RemoveElement(UI_Element ui_Element)
        {
            if (!_elements.Contains(ui_Element)) return;

            _elements.Remove(ui_Element);
            _typeToElements[ui_Element.Type].Remove(ui_Element);
            if (_nameToElement.ContainsKey(ui_Element.Name)) _nameToElement.Remove(ui_Element.Name);
        }


        public E GetElement<E>(string name) where E : UI_Element { return (E)_nameToElement[name]; }


        //Optional
        private const bool ExecuteOneCommandPerFrame = false;

        private void Update()
        {
        ExecuteNextCommand:
            if (_commands.IsEmpty() || !_commands.Peek().IsReady)
                return;

            Command command = _commands.Remove();

            if (command.IsReady)
            {
                command.Execute();
                if (!ExecuteOneCommandPerFrame)
                {
                    goto ExecuteNextCommand;
                }
            }

        }


        public List<UI_Element> FindAllUIElementsInScene()
        {

            Scene currentScene = SceneManager.GetActiveScene();
            List<UI_Element> elements = new List<UI_Element>();
            foreach (GameObject rootGameObject in currentScene.GetRootGameObjects())
            {
                MonoBehaviour[] subs = rootGameObject.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (MonoBehaviour entity in subs)
                {
                    if (entity is UI_Element ui_Element)
                    {
                        elements.Add(ui_Element);
                    }
                }
            }

            return elements;

        }

        public void LoadScene(string sceneName) { LoadScene(sceneName, 0f); }

        public void LoadScene(string sceneName, float delay = 0) { AddCommand(new LoadSceneCommand(sceneName, delay)); }

        public void OpenUIElement(UI_Element element) { OpenUIElement(element, 0f, 0); }
        public void OpenUIElement(UI_Element element, float delay, int animIndex) { AddCommand(new OpenUIElementCommand(element, delay, animIndex)); }

        public void CloseUIElement(UI_Element element) { CloseUIElement(element, 0f, 0); }
        public void CloseUIElement(UI_Element element, float delay, int animIndex) { AddCommand(new CloseUIElementCommand(element, delay, animIndex)); }

        public void ForceOpenUIElement(UI_Element element)
        {
            element.Status = UIElementStatus.Opened;
            element.gameObject.SetActive(true);
        }

        public void ForceCloseUIElement(UI_Element element)
        {
            element.Status = UIElementStatus.Closed;
            element.gameObject.SetActive(true);
        }


        public void CloseAllUIElements(float delay, int animIndex)
        {
            foreach (UI_Element e in _elements)
            {
                CloseUIElement(e, delay, animIndex);
            }
        }

        public void CloseAllUIElements(UIElementType type, float delay, int animIndex)
        {
            if (_typeToElements.TryGetValue(type, out List<UI_Element> elements))
            {
                foreach (UI_Element e in elements)
                {
                    CloseUIElement(e, delay, animIndex);
                }
            }
        }


        public void QuitApplication(float delay = 0) { AddCommand(new QuitAppCommand(delay)); }



        private void AddCommand(Command command)
        {
            _commands.Insert(command);
        }



        #region GetterSetter

        public bool TryGetElements(UIElementType elementType, out List<UI_Element> elements)
        {
            if (_typeToElements.TryGetValue(elementType, out List<UI_Element> list))
            {
                elements = list;
                return true;
            }
            elements = null;
            return false;
        }

        #endregion



        public static UI_Manager Instance { get => s_Instance; }





    }


}

