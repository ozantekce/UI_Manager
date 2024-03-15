using System.Collections.Generic;

public static class TEXT_FORMATS
{

    private static Dictionary<string, TextFormat> NameToFormat = new Dictionary<string, TextFormat>()
    {
        {"Gold"     ,       new TextFormat("G:{0}") },
        {"Gold2"    ,       new TextFormat("Gold({0})") },
        {"MAX"      ,       new TextFormat("MAX") },
        {"Time"     ,       new TextFormat("H:{0} M:{1} S:{2}") },

    };

    public static string ExecuteFormat(this string formatName, params string[] texts)
    {
        if (string.IsNullOrEmpty(formatName))
        {
            string result = string.Empty;
            foreach (string text in texts)
            {
                result += text;
            }
            return result;
        }

        return NameToFormat[formatName].ExecuteFormat(texts);
    }


    public static void AddFormat(string formatName, string format)
    {
        NameToFormat.Add(formatName, new TextFormat(format));
    }


    private class TextFormat
    {

        private string _format;

        public TextFormat(string format)
        {
            _format = format;
        }

        public virtual string ExecuteFormat(params string[] texts)
        {
            return string.Format(this._format, texts);
        }

    }


}
