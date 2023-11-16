using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI_Manager
{
    public class UI_Manager : MonoBehaviour
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

        public void LoadScene(string sceneName) { LoadScene(sceneName, 0); }

        public void LoadScene(string sceneName, float delay = 0) { AddCommand(new LoadSceneCommand(sceneName, delay)); }

        public void OpenUIElement(UI_Element element) { OpenUIElement(element, 0); }
        public void OpenUIElement(UI_Element element, float delay = 0) { AddCommand(new OpenUIElementCommand(element, delay)); }

        public void CloseUIElement(UI_Element element) { CloseUIElement(element, 0f); }
        public void CloseUIElement(UI_Element element, float delay = 0) { AddCommand(new CloseUIElementCommand(element, delay)); }

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


        public void CloseAllUIElements(float delay = 0)
        {
            foreach (UI_Element e in _elements)
            {
                CloseUIElement(e, delay);
            }
        }

        public void CloseAllUIElements(UIElementType type, float delay = 0)
        {
            if (_typeToElements.TryGetValue(type, out List<UI_Element> elements))
            {
                foreach (UI_Element e in elements)
                {
                    CloseUIElement(e, delay);
                }
            }
        }


        public void QuitApplication(float delay = 0) { AddCommand(new QuitAppCommand(delay)); }



        private void AddCommand(Command command, params Command[] subs)
        {
            if (subs != null && subs.Length > 0) command.AddSubCommands(subs);
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




        #region Commands
        private abstract class Command : IComparable<Command>
        {

            protected UI_Element _element;
            private float _delay;
            private bool _waitUntilTerminated;

            private bool _isTerminated;
            private float _createTime;
            private List<Command> _subCommands;
            public Command(UI_Element element, float delay = 0)
            {
                this._element = element;
                this._createTime = Time.time;
                this._delay = delay;
            }

            public Command(float delay = 0)
            {
                _createTime = Time.time;
                this._delay = delay;
            }

            public Command()
            {
                _createTime = Time.time;
            }

            public void AddSubCommands(params Command[] subs)
            {
                _subCommands ??= new List<Command>();
                _subCommands.AddRange(subs);
            }

            public void Execute()
            {
                if (_subCommands != null)
                    for (int i = 0; i < _subCommands.Count; i++) _subCommands[i].Execute();
                Execute_();
            }
            protected abstract void Execute_();
            #region GetterSetter
            public float RemaniderTime { get { return Delay - (Time.time - _createTime); } }
            public float Delay { get => _delay; set => _delay = value; }
            public bool IsReady { get { return Time.time - (_createTime) >= Delay; } }
            public virtual bool IsTerminated { get => _isTerminated; protected set => _isTerminated = value; }
            public bool WaitUntilTerminated { get => _waitUntilTerminated; set => _waitUntilTerminated = value; }
            #endregion
            public int CompareTo(Command other)
            {
                if (this.RemaniderTime > other.RemaniderTime) return +1;
                else if (this.RemaniderTime < other.RemaniderTime) return -1;
                else return 0;
            }


        }

        private class OpenUIElementCommand : Command
        {
            public OpenUIElementCommand(UI_Element element, float delay = 0) : base(element, delay)
            {
            }

            protected override void Execute_()
            {
                if (_element == null || _element.Status != UIElementStatus.Closed)
                {
                    IsTerminated = true;
                    return;
                }
                _element.Open();
            }

            public override bool IsTerminated
            {
                get
                {
                    return IsTerminated
                        || _element.Status == UIElementStatus.Opened;
                }
            }
        }

        private class CloseUIElementCommand : Command
        {
            public CloseUIElementCommand(UI_Element element, float delay = 0) : base(element, delay)
            {
            }

            protected override void Execute_()
            {
                if (_element == null || _element.Status != UIElementStatus.Opened)
                {
                    IsTerminated = true;
                    return;
                }
                _element.Close();
            }

            public override bool IsTerminated
            {
                get
                {
                    return IsTerminated
                        || _element.Status == UIElementStatus.Closed;
                }
            }

        }

        private class QuitAppCommand : Command
        {

            public QuitAppCommand(float delay) : base(delay)
            {
            }

            protected override void Execute_()
            {
                Application.Quit();
            }

        }

        private class LoadSceneCommand : Command
        {
            private string _sceneName;

            public LoadSceneCommand(string sceneName, float delay) : base(delay)
            {
                this._sceneName = sceneName;
            }

            protected override void Execute_()
            {
                SceneManager.LoadScene(_sceneName);
            }
        }


        #endregion


    }
    public static class UI_ManagerExtensions
    {

        public static void OpenUIElement(this string name, float delay = 0)
        {
            UI_Element element = name.NameToUiElement<UI_Element>();
            UI_Manager.Instance.OpenUIElement(element, delay);
        }

        public static void OpenUIElement(this UI_Element element, float delay = 0)
        {
            UI_Manager.Instance.OpenUIElement(element, delay);
        }

        public static void CloseUIElement(this string name, float delay = 0)
        {
            UI_Element element = name.NameToUiElement<UI_Element>();
            UI_Manager.Instance.CloseUIElement(element, delay);
        }

        public static void CloseUIElement(this UI_Element element, float delay = 0)
        {
            UI_Manager.Instance.CloseUIElement(element, delay);
        }

        public static void LoadScene(this string sceneName, float delay = 0)
        {
            UI_Manager.Instance.LoadScene(sceneName, delay);
        }


        public static void SetExtendedText(this string name, string text)
        {
            ExtendedText et = name.NameToUiElement<ExtendedText>();
            et.Text = text;
        }

        public static void SetExtendedTextMethod(this string name, TextMethod textMethod)
        {
            ExtendedText et = name.NameToUiElement<ExtendedText>();
            et.TextMethod = textMethod;
        }


        public static E NameToUiElement<E>(this string name) where E : UI_Element
        {
            return UI_Manager.Instance.GetElement<E>(name);
        }


    }


}

