
namespace UI_Manager
{
    public static class UI_ManagerExtensions
    {

        public static void OpenUIElement(this string name, float delay, int animIndex)
        {
            UI_Element element = name.NameToUiElement<UI_Element>();
            UI_Manager.Instance.OpenUIElement(element, delay, animIndex);
        }

        public static void OpenUIElement(this UI_Element element, float delay, int animIndex)
        {
            UI_Manager.Instance.OpenUIElement(element, delay, animIndex);
        }

        public static void CloseUIElement(this string name, float delay, int animIndex)
        {
            UI_Element element = name.NameToUiElement<UI_Element>();
            UI_Manager.Instance.CloseUIElement(element, delay, animIndex);
        }

        public static void CloseUIElement(this UI_Element element, float delay, int animIndex)
        {
            UI_Manager.Instance.CloseUIElement(element, delay, animIndex);
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
