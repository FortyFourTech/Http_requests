using System;

namespace API {
    public enum RequestParameter {
        key,
        lang,
        hint,
        ui,
        text,
        callback,
    }

    static public class RequestParametersExtension {
        public static string defaultValue(this RequestParameter param) {
            string result;
            switch (param) {
                case RequestParameter.key:
                    result = "trnsl.1.1.20180214T073632Z.4f581d8e34c840c7.317d9bb16828c60047dc8e0bfc2616d6de76958d";
                    break;
                case RequestParameter.lang:
                    result = "ru-en";
                    break;
                case RequestParameter.hint:
                    result = "en";
                    break;
                case RequestParameter.ui:
                    result = "ru";
                    break;
                case RequestParameter.callback:
                case RequestParameter.text:
                    throw new Exception("request parameter '" + param.ToString() + "' doesn't have default value. You must define it explicitly.");
                default:
                    throw new Exception("defaultValue for request parameter '" + param.ToString() + "' is not defined");
            }

            return result;
        }
    }
}
