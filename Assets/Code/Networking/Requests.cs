using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
using UnityEngine.Networking;
// using UnityEngine.Assertions;

namespace API {
    public enum Request {
        /// <summary>
        /// https://translate.yandex.net/api/v1.5/tr.json/getLangs
        /// ? [key=<API-ключ>]
        /// & [ui=<код языка>]
        /// & [callback=<имя callback-функции>]
        /// </summary>
        languageList,
        /// <summary>
        /// https://translate.yandex.net/api/v1.5/tr.json/detect
        /// ? [key=<API-ключ>]
        /// & text=<текст>
        /// & [hint=<список вероятных языков текста>]
        /// & [callback=<имя callback-функции>]
        /// </summary>
        detectLanguage,
        /// <summary>
        /// https://translate.yandex.net/api/v1.5/tr.json/translate
        ///  ? [key=<API-ключ>]
        ///  & [text=<переводимый текст>]
        ///  & [lang=<направление перевода>]
        ///  & [format=<формат текста>]
        ///  & [options=<опции перевода>]
        ///  & [callback=<имя callback-функции>]
        /// </summary>
        translate,
    }

    static public class RequestExtension {
        public static string url(this Request rq) {
            string result = NetworkService.BASE_URL;
            switch (rq) {
                case Request.languageList:
                    result += "/getLangs";
                    break;
                case Request.detectLanguage:
                    result += "/detect";
                    break;
                case Request.translate:
                    result += "/translate";
                    break;
                default:
                    throw new Exception("url for request '" + rq.ToString() + "' is not defined");
            }

            return result;
        }

        public static RequestParameter[] getParameters(this Request rq) {
            RequestParameter[] result;
            switch (rq) {
                case Request.languageList:
                    result = new RequestParameter[] {
                        RequestParameter.key,
                        RequestParameter.ui,
                    };
                    break;
                case Request.detectLanguage:
                    result = new RequestParameter[] {
                        RequestParameter.key,
                        RequestParameter.hint,
                    };
                    break;
                case Request.translate:
                    result = new RequestParameter[] {
                        RequestParameter.key,
                        RequestParameter.lang,
                    };
                    break;
                default:
                    throw new Exception("method for request '" + rq.ToString() + "' is not defined");
            }

            return result;
        }

        public static RequestParameter[] postParameters(this Request rq) {
            RequestParameter[] result;
            switch (rq) {
                case Request.languageList:
                    result = new RequestParameter[] { };
                    break;
                case Request.detectLanguage:
                    result = new RequestParameter[] {
                        RequestParameter.text,
                    };
                    break;
                case Request.translate:
                    result = new RequestParameter[] {
                        RequestParameter.text,
                    };
                    break;
                default:
                    throw new Exception("method for request '" + rq.ToString() + "' is not defined");
            }

            return result;
        }
    }
}
