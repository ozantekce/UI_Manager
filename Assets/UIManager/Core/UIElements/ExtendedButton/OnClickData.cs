using EditorTools;
using UnityEngine;
using UnityEngine.Events;

namespace UIManager
{

    [System.Serializable]
    public class OnClickData
    {

        public ButtonOperationType Type;


        [ShowInEnum(nameof(Type), nameof(ButtonOperationType.OpenElement), nameof(ButtonOperationType.CloseElement))]
        public UIElement Element;
        [ShowInEnum(nameof(Type), nameof(ButtonOperationType.LoadScene))]
        public string SceneName;
        [ShowInEnum(nameof(Type), nameof(ButtonOperationType.OpenElement), nameof(ButtonOperationType.CloseElement))]
        public int AnimIndex = 0;


        [ShowInEnum(nameof(Type), nameof(ButtonOperationType.SwapElements))]
        public UIElement OpenElement;
        [ShowInEnum(nameof(Type), nameof(ButtonOperationType.SwapElements))]
        public int OpenAnimIndex;

        [ShowInEnum(nameof(Type), nameof(ButtonOperationType.SwapElements))]
        public UIElement CloseElement;
        [ShowInEnum(nameof(Type), nameof(ButtonOperationType.SwapElements))]
        public int CloseAnimIndex;


        [field: SerializeField] public float WaitToExecute { get; private set; }
        [field: SerializeField] public float WaitToReuse { get; private set; }

        [field: SerializeField] public UnityEvent OnClickEvent { get; private set; }

        public float NextClickTime { get; set; }

    }


}