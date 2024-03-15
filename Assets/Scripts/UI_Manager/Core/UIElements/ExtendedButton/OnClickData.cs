using UnityEngine.Events;

namespace UIManager
{

    [System.Serializable]
    public class OnClickData
    {

        public ButtonOperationType type;
        [ShowInEnum(nameof(type), nameof(ButtonOperationType.OpenElement), nameof(ButtonOperationType.CloseElement))]
        public string elementName;
        [ShowInEnum(nameof(type), nameof(ButtonOperationType.OpenElement), nameof(ButtonOperationType.CloseElement))]
        public UIElement element;
        [ShowInEnum(nameof(type), nameof(ButtonOperationType.LoadScene))]
        public string sceneName;
        [ShowInEnum(nameof(type), nameof(ButtonOperationType.OpenElement), nameof(ButtonOperationType.CloseElement))]
        public int animIndex = 0;

        public float waitToExecute;
        public float waitToReuse;

        public UnityEvent onClickEvent;

        private float _nextClickTime = -1f;

        public string ElementName
        {
            get
            {
                if (type == ButtonOperationType.LoadScene) return sceneName;
                return elementName;
            }
        }

        public UIElement Element
        {
            get
            {
                if (type == ButtonOperationType.LoadScene) return null;
                if (element != null) return element;
                return elementName.GetUIElement<UIElement>();
            }

        }


        public float NextClickTime { get => _nextClickTime; set => _nextClickTime = value; }

    }


}