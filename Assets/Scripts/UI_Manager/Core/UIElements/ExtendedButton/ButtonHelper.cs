using System.Collections.Generic;
using UnityEngine;

namespace UIManager
{
    public static class ButtonHelper
    {

        private static Dictionary<ButtonOperationType, OperationMethod> TypeMethodPairs = new Dictionary<ButtonOperationType, OperationMethod>() 
        {
            {ButtonOperationType.None ,               None },
            {ButtonOperationType.OpenElement  ,       OpenElement },
            {ButtonOperationType.CloseElement ,       CloseElement },
            {ButtonOperationType.QuitApplication ,    QuitApplication },
            {ButtonOperationType.LoadScene ,          LoadScene }
        };


        public static void Execute(OnClickData data)
        {
            if (data.NextClickTime > Time.time) return;
            data.onClickEvent?.Invoke();
            data.NextClickTime = Time.time + data.waitToReuse;
            TypeMethodPairs[data.type](data);
        }


        private static void OpenElement(OnClickData data)
        {
            data.Element.OpenElement(data.waitToExecute, data.animIndex);
        }

        private static void CloseElement(OnClickData data)
        {
            data.Element.CloseElement(data.waitToExecute, data.animIndex);
        }

        private static void QuitApplication(OnClickData data)
        {
            UIManager.Instance.QuitApplication();
        }

        private static void LoadScene(OnClickData data)
        {
            data.ElementName.LoadScene(data.waitToExecute);
        }
        private static void None(OnClickData data)
        {

        }

        private delegate void OperationMethod(OnClickData data);


    }

}