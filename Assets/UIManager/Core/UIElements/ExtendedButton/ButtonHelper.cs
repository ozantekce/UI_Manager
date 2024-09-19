using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIManager
{
    public static class ButtonHelper
    {

        private static Dictionary<ButtonOperationType, Action<OnClickData>> TypeMethodPairs = new Dictionary<ButtonOperationType, Action<OnClickData>>() 
        {
            {ButtonOperationType.None ,               None },
            {ButtonOperationType.SwapElements ,       SwapElements },
            {ButtonOperationType.OpenElement  ,       OpenElement },
            {ButtonOperationType.CloseElement ,       CloseElement },
            {ButtonOperationType.QuitApplication ,    QuitApplication },
            {ButtonOperationType.LoadScene ,          LoadScene }
        };


        public static void Execute(OnClickData data)
        {
            if (data.NextClickTime > Time.time)
                return;
            data.OnClickEvent?.Invoke();
            data.NextClickTime = Time.time + data.WaitToReuse;
            TypeMethodPairs[data.Type](data);
        }

        private static void SwapElements(OnClickData data)
        {
            UIManager.Instance.SwapUIElements(data.OpenElement, data.OpenAnimIndex, data.CloseElement, data.CloseAnimIndex, data.WaitToExecute);
        }


        private static void OpenElement(OnClickData data)
        {
            data.Element.OpenElement(data.WaitToExecute, data.AnimIndex);
        }

        private static void CloseElement(OnClickData data)
        {
            data.Element.CloseElement(data.WaitToExecute, data.AnimIndex);
        }

        private static void QuitApplication(OnClickData data)
        {
            UIManager.Instance.QuitApplication();
        }

        private static void LoadScene(OnClickData data)
        {
            data.SceneName.LoadScene(data.WaitToExecute);
        }
        private static void None(OnClickData data)
        {

        }

    }

}