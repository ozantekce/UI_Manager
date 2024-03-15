
namespace UIManager
{
    public static class UIManagerExtensions
    {

        public static void OpenElement(this string name, float delay = 0f, int animIndex = 0)
        {
            UIElement element = name.GetUIElement<UIElement>();
            UIManager manager = UIManager.Instance;
            if(manager != null && element != null)
            {
                manager.OpenUIElement(element, delay, animIndex);
            }
            
        }

        public static void OpenElement(this UIElement element, float delay = 0f, int animIndex = 0)
        {
            UIManager manager = UIManager.Instance;
            if(manager != null && element != null)
            {
                manager.OpenUIElement(element, delay, animIndex);
            }
            
        }

        public static void CloseElement(this string name, float delay = 0f, int animIndex = 0)
        {
            UIElement element = name.GetUIElement<UIElement>();
            UIManager manager = UIManager.Instance;
            if(manager != null && element != null)
            {
                manager.CloseUIElement(element, delay, animIndex);
            }
            
        }

        public static void CloseElement(this UIElement element, float delay = 0f, int animIndex = 0)
        {
            UIManager manager = UIManager.Instance;
            if(manager != null && element != null)
            {
                manager.CloseUIElement(element, delay, animIndex);
            }
            
        }

        public static void LoadScene(this string sceneName, float delay = 0)
        {
            UIManager manager = UIManager.Instance;
            if(manager != null)
            {
                manager.LoadScene(sceneName, delay);
            }
        }


        public static void SetExtendedText(this string name, string text)
        {
            UIExtendedText extendedText = name.GetUIElement<UIExtendedText>();
            if(extendedText != null)
            {
                extendedText.Text = text;
            }
            
        }

        public static void SetExtendedTextMethod(this string name, TextMethod textMethod)
        {
            UIExtendedText extendedText = name.GetUIElement<UIExtendedText>();
            if(extendedText == null)
            {
                extendedText.UpdateTextMethod(textMethod);
            }
        }


        public static E GetUIElement<E>(this string name) where E : UIElement
        {
            UIManager manager = UIManager.Instance;
            if(manager != null)
            {
                return manager.GetElement<E>(name);
            }
            else
            {
                return null;
            }
        }

        public static bool TryGetUIElement<E>(this string name, out E element) where E : UIElement
        {
            UIManager manager = UIManager.Instance;
            if(manager != null)
            {
                return manager.TryGetElement(name, out element);
            }
            else
            {
                element = null;
                return false;
            }
            
        }


    }

}
