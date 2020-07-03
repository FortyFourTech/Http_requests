using System;

namespace API
{
    public enum ERequestParameter
    {
        key,        // API key
        lang,       // translation direction
        hint,       // list of likely text languages
        ui,         // language code to output language names in
        text,       // text to translate
    }

    static public class RequestParametersExtension
    {
        public static string defaultValue(this ERequestParameter param)
        {
            string result;
            switch (param)
            {
                case ERequestParameter.key:
                    result = "trnsl.1.1.20180214T073632Z.4f581d8e34c840c7.317d9bb16828c60047dc8e0bfc2616d6de76958d";
                    break;
                case ERequestParameter.lang:
                    result = "ru-en";
                    break;
                case ERequestParameter.hint:
                    result = "en";
                    break;
                case ERequestParameter.ui:
                    result = "ru";
                    break;
                case ERequestParameter.text:
                    throw new Exception("request parameter '" + param.ToString() + "' doesn't have default value. You must define it explicitly.");
                default:
                    throw new Exception("defaultValue for request parameter '" + param.ToString() + "' is not defined");
            }

            return result;
        }
    }
}
