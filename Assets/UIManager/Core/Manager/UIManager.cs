using System.Collections.Generic;
using UnityEngine;

namespace UIManager
{

    [DefaultExecutionOrder(-1000)]
    public partial class UIManager : MonoBehaviour
    {

        public static UIManager Instance { get; private set; }


        private HashSet<UIElement> _elements;
        private Dictionary<string, UIElement> _nameToElement;


        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(this);
                return;
            }

            Instance = this;

            var elements = GameObject.FindObjectsOfType<UIElement>(true);

            _elements = new HashSet<UIElement>();
            _nameToElement = new Dictionary<string, UIElement>();

            foreach (UIElement element in elements)
            {
                RegisterElement(element);
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

        internal void RegisterElement(UIElement element)
        {
            if (_elements.Contains(element))
                return;

            if (!string.IsNullOrEmpty(element.Name))
            {
                _nameToElement[element.Name] = element;
            }
            _elements.Add(element);

            if (!element.Initialized)
            {
                element.Initialize();
            }
        }


        internal void UnregisterElement(UIElement uiElement)
        {
            if (!_elements.Contains(uiElement))
                return;

            _elements.Remove(uiElement);
            if (_nameToElement.ContainsKey(uiElement.Name))
            {
                _nameToElement.Remove(uiElement.Name);
            }
        }


        public E GetElement<E>(string name) where E : UIElement
        {
            if (ContainsElement(name))
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

    }


}

